using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using CommonFeaturesLibrary.Extensions;
using ExtensionsLibrary.Extensions;

namespace CommonFeaturesLibrary {
	/// <summary>
	/// ファイルバージョンを管理するメソッドを提供するクラスです。
	/// </summary>
	public static partial class FileVersionManager {
		#region	定数

		/// <summary>
		/// ファイル世代管理数</summary>
		private const int LOG_FILE_CNT = 3;

		/// <summary>
		/// 容量1[MB]</summary>
		private const int MB = (0x400 * 0x400);

		/// <summary>
		/// 容量2[MB]</summary>
		private const int MAX_CAPACITY = (MB * 0x02);

		#endregion

		#region	メソッド

		#region ログ出力

		/// <summary>
		/// ファイルバージョンを管理して、
		/// ログファイルにメッセージを書き込みます。
		/// </summary>
		/// <param name="this">ファイル情報</param>
		/// <param name="message">メッセージ</param>
		public static void WriteLog(this FileInfo @this, string message) {
			lock (@this) {
				@this.CheckCapacity((long)message.Length);
				@this.WriteLine(message.GetTimeLog());
			}
		}

		/// <summary>
		/// ファイルバージョンを管理して、
		/// ログファイルにメッセージを書き込みます。[非同期]
		/// </summary>
		/// <param name="this">ファイル情報</param>
		/// <param name="message">メッセージ</param>
		public static async Task WriteLogAsync(this FileInfo @this, string message) {
			@this.CheckCapacity((long)message.Length);
			await @this.WriteLineAsync(message.GetTimeLog());
		}

		#endregion

		#region ファイルバージョン管理

		/// <summary>
		/// ファイル容量のチェックしてファイルを世代管理します。
		/// </summary>
		/// <param name="fileInfo">ファイル情報</param>
		/// <param name="dtLength">追加するデータのサイズ</param>
		/// <param name="genMgtCnt">ファイル世代管理数</param>
		/// <param name="maxCapacity">最大容量</param>
		public static void CheckCapacity(this FileInfo fileInfo, long dtLength, uint genMgtCnt = LOG_FILE_CNT, long maxCapacity = MAX_CAPACITY) {
			try {
				if (!fileInfo.Exists) {// 指定ファイルが存在しない
					return;
				}

				var size = fileInfo.Length + dtLength;

				// 指定ファイルの容量に空がある
				if (size < maxCapacity) {
					return;
				}

				var filePath = fileInfo.FullName;

				// 世代ファイルが存在するか確認
				var version = StateSelection(filePath, genMgtCnt);
				if (version == 0) {// 世代ファイルが存在しない場合
					RenameFile(filePath, 0);	// ファイル改名
					return;
				}

				if (version >= genMgtCnt) {// 世代ファイルが管理数分ある
					var oldName = CreateVersionName(filePath, version);
					var oldInfo = new FileInfo(oldName);

					// ファイル削除
					oldInfo.Delete();

					version--;
				}

				var times = version;
				for (var i = 0; i < version; i++) {
					// ファイル改名
					RenameFile(filePath, times);
					times--;
				}
				// ファイル改名
				RenameFile(filePath, 0);
			} finally {
				// 情報更新
				fileInfo.Refresh();
			}
		}

		/// <summary>
		/// 世代ファイル管理状況
		/// </summary>
		private static uint StateSelection(string filePath, uint ver) {
			var nState = 0u;
			for (var i = 1u; i <= ver; i++) {// 世代ファイルが存在するか確認
				var fName = CreateVersionName(filePath, i);
				var fileInfo = new FileInfo(fName);
				if (!fileInfo.Exists) {// ファイルが存在しない
					return nState;
				} else {// ファイルが存在する
					nState = i;
				}
			}

			return nState;
		}

		/// <summary>
		/// バージョンファイル情報作成
		/// </summary>
		/// <param name="filePath">ファイルパス</param>
		/// <param name="version">バージョン</param>
		/// <returns>バージョン番号を付与したファイル名を返します。</returns>
		private static string CreateVersionName(string filePath, uint version) {
			var file = new FileInfo(filePath);
			return file.GetVersionName(version);
		}

		/// <summary>
		/// ファイル改名
		/// </summary>
		private static void RenameFile(string filePath, uint ver) {
			var oldName = (ver == 0) ? filePath : CreateVersionName(filePath, ver);
			var newName = CreateVersionName(filePath, ver + 1);

			if (oldName == newName) {
				// 同名なら何もしない
				return;
			}

			File.Move(oldName, newName);

			Debug.WriteLine($"{oldName}を{newName}に改名");
		}

		#endregion

		#endregion
	}
}
