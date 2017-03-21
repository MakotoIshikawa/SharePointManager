using System;
using System.IO;
using System.Reflection;
using ExtensionsLibrary.Extensions;

namespace CommonFeaturesLibrary {
	/// <summary>
	/// ログ出力クラス
	/// </summary>
	public class Logger {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <remarks>
		/// オブジェクトを生成します。</remarks>
		public Logger()
			: this(AppLogFilePath) {
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="filePath">ファイルパス</param>
		/// <remarks>
		/// オブジェクトを生成します。</remarks>
		public Logger(string filePath) {
			this.FileInfo = new FileInfo(filePath);
		}

		#endregion

		#region	プロパティ

		/// <summary>ファイル情報</summary>
		public FileInfo FileInfo { get; protected set; }

		/// <summary>アプリログファイルパス</summary>
		public static string AppLogFilePath {
			get {
				try {
					var assembly = Assembly.GetEntryAssembly();
					var path = assembly.Location;
					var info = new FileInfo(path);
					return info.ChangeExtension(".log");
				} catch (Exception) {
					return $"{AppDomain.CurrentDomain.BaseDirectory}.log";
				}
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
		public void WriteLog(string message) {
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
