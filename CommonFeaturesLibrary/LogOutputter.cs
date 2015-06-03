using System;
using System.IO;
using System.Windows.Forms;
using ExtensionsLibrary.Extensions;

namespace CommonFeaturesLibrary {
	/// <summary>
	/// ログ出力クラス
	/// </summary>
	public class LogOutputter {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <remarks>
		/// オブジェクトを生成します。</remarks>
		public LogOutputter()
			: this(AppLogFilePath) {
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="filePath">ファイルパス</param>
		/// <remarks>
		/// オブジェクトを生成します。</remarks>
		public LogOutputter(String filePath) {
			this.FileInfo = new FileInfo(filePath);
		}

		#endregion

		#region	プロパティ

		/// <summary>ファイル情報</summary>
		public FileInfo FileInfo { get; protected set; }

		/// <summary>アプリログファイルパス</summary>
		public static String AppLogFilePath {
			get {
				var info = new FileInfo(Application.ExecutablePath);
				return info.ChangeExtension(".log");
			}
		}

		#endregion

		#region メソッド

		#region	ログ書込

		/// <summary>
		/// ログ書込
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <remarks>
		/// ログファイルにログを書き込む</remarks>
		public void WriteLog(String message) {
			this.FileInfo.WriteLog(message);
		}

		#endregion

		#region	ファイル内容クリア

		/// <summary>
		/// ログファイルの内容をクリアします。
		/// </summary>
		public void Clear() {
			this.FileInfo.Clear();
		}

		#endregion

		#endregion
	}
}
