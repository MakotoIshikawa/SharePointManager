using System;
using System.IO;
using System.Windows.Forms;
using ExtensionsLibrary.Extensions;

namespace CommonFeaturesLibrary {
	/// ---------------------------------------------------------------------------------------
	/// <summary>
	/// ログ出力クラス</summary>
	/// ---------------------------------------------------------------------------------------
	public class OutputLog {
		/// <summary>
		/// ファイルパス</summary>
		private String m_filePath;

		/// ---------------------------------------------------------------------------------------
		/// <summary>
		/// コンストラクタ</summary>
		/// <remarks>
		/// オブジェクトを生成します。</remarks>
		/// ---------------------------------------------------------------------------------------
		public OutputLog()
			: this(GetAppLogFilePath()) {
		}

		/// ---------------------------------------------------------------------------------------
		/// <summary>
		/// コンストラクタ</summary>
		/// <param name="filePath">ファイルパス</param>
		/// <remarks>
		/// オブジェクトを生成します。</remarks>
		/// ---------------------------------------------------------------------------------------
		public OutputLog(String filePath) {
			this.m_filePath = filePath;
		}

		/// ---------------------------------------------------------------------------------------
		/// <summary>
		/// デストラクタ</summary>
		/// <remarks>
		/// オブジェクトの消去
		/// 使用中のリソースをすべてクリーンアップします</remarks>
		/// ---------------------------------------------------------------------------------------
		~OutputLog() {
		}

		#region	ログ書込

		/// ---------------------------------------------------------------------------------------
		/// <summary>
		/// ログ書込</summary>
		/// <param name="message">メッセージ</param>
		/// <remarks>
		/// ログファイルにログを書き込む</remarks>
		/// ---------------------------------------------------------------------------------------
		public void WriteLog(String message) {
			lock (this.m_filePath) {
				var fgm = new FileGenMgt(this.m_filePath, (long)message.Length, 5);
				OutputLog.WriteLog(this.m_filePath, message);
			}
		}

		/// ---------------------------------------------------------------------------------------
		/// <summary>
		/// ログ書込</summary>
		/// <param name="filePath">ファイルパス</param>
		/// <param name="message">メッセージ</param>
		/// <remarks>
		/// ログファイルにログを書き込む</remarks>
		/// ---------------------------------------------------------------------------------------
		private static void WriteLog(String filePath, String message) {
			using (var sw = new StreamWriter(filePath, true)) {
				sw.WriteLine(message.GetTimeLog());

				sw.Close();
			}
		}

		#endregion

		#region	アプリログファイルパス取得

		/// ---------------------------------------------------------------------------------------
		/// <summary>
		/// アプリログファイルパス取得</summary>
		/// <remarks>
		/// アプリログファイルパスを取得する</remarks>
		/// ---------------------------------------------------------------------------------------
		private static String GetAppLogFilePath() {
			return Path.ChangeExtension(Application.ExecutablePath, ".log");
		}

		#endregion

		#region	ファイル内容クリア

		/// ---------------------------------------------------------------------------------------
		/// <summary>
		/// ファイル内容クリア</summary>
		/// <remarks>
		/// ファイル内容をクリアする</remarks>
		/// ---------------------------------------------------------------------------------------
		public void FileClear() {
			using (var sw = new StreamWriter(this.m_filePath, false)) {
			}
		}

		#endregion
	}
}
