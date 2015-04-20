using System;
using System.IO;
using System.Windows.Forms;
using ExtensionsLibrary.Extensions;

namespace CommonFeaturesLibrary {
	/// <summary>
	/// ログ出力クラス
	/// </summary>
	public class OutputLog {
		#region フィールド

		/// <summary>ファイル情報</summary>
		private FileInfo m_fileInfo;

		#endregion

		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <remarks>
		/// オブジェクトを生成します。</remarks>
		public OutputLog()
			: this(GetAppLogFilePath()) {
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="filePath">ファイルパス</param>
		/// <remarks>
		/// オブジェクトを生成します。</remarks>
		public OutputLog(String filePath) {
			this.m_fileInfo = new FileInfo(filePath);
		}

		#endregion

		#region	ログ書込

		/// <summary>
		/// ログ書込</summary>
		/// <param name="message">メッセージ</param>
		/// <remarks>
		/// ログファイルにログを書き込む</remarks>
		public void WriteLog(String message) {
			lock (this.m_fileInfo) {
				var fileName = this.m_fileInfo.FullName;
				this.m_fileInfo.CheckCapacity((long)message.Length);
				this.m_fileInfo.WriteLine(message.GetTimeLog());
			}
		}

		#endregion

		#region	ファイル内容クリア

		/// <summary>
		/// ログファイルの内容をクリアします。
		/// </summary>
		public void LogClear() {
			this.m_fileInfo.Clear();
		}

		#endregion

		#region	アプリログファイルパス取得

		/// <summary>
		/// アプリログファイルパス取得
		/// </summary>
		/// <remarks>
		/// アプリログファイルパスを取得する</remarks>
		private static String GetAppLogFilePath() {
			return Path.ChangeExtension(Application.ExecutablePath, ".log");
		}

		#endregion
	}
}
