using System;
using System.IO;
using System.Text;
using ExtensionsLibrary.Extensions;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// FileInfo を拡張するメソッドを提供します。
	/// </summary>
	public static partial class FileInfoExtension {
		#region メソッド

		#region 逆シリアル化

		/// <summary>
		/// XML ファイルを逆シリアル化します。
		/// </summary>
		/// <typeparam name="TResult">逆シリアル化する型</typeparam>
		/// <param name="this">XML ファイル情報</param>
		/// <returns>逆シリアル化されたオブジェクト</returns>
		public static TResult DeSerializeFromXml<TResult>(this FileInfo @this) {
			// 読み込むファイルを開く
			using (var fs = @this.OpenRead()) {
				return fs.DeserializeFromXml<TResult>();
			}
		}

		#endregion

		#region WriteLine

		/// <summary>
		/// 文字列を書き込み、続けて行終端記号を書き込みます。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="message">メッセージ</param>
		public static void WriteLine(this FileInfo @this, string message) {
			@this.WriteLine(message, Encoding.UTF8);
		}

		/// <summary>
		/// 文字列を書き込み、続けて行終端記号を書き込みます。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="message">メッセージ</param>
		/// <param name="encod">エンコーディング</param>
		public static void WriteLine(this FileInfo @this, string message, Encoding encod) {
			using (var fs = @this.Open(FileMode.Append, FileAccess.Write)) {
				using (var sw = new StreamWriter(fs, encod)) {
					sw.WriteLine(message);
					sw.Close();
				}
			}
		}

		#endregion

		/// <summary>
		/// ファイルのデータを初期化します。
		/// </summary>
		/// <param name="this">FileInfo</param>
		public static void Clear(this FileInfo @this) {
			var fileName = @this.FullName;
			using (var sw = new StreamWriter(fileName, false)) {
			}
		}

		/// <summary>
		/// バージョン番号を付与したファイル名を取得します。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="version">バージョン番号</param>
		/// <returns>バージョン番号を付与したファイル名を返します。</returns>
		public static string GetVersionName(this FileInfo @this, uint version) {
			if (version <= 0) {
				return @this.FullName;
			}

			var name = @this.Name.CommentOut(@this.Extension);
			return string.Format(@"{0}\{1} ({2}){3}", @this.DirectoryName, name, version, @this.Extension);
		}

		/// <summary>
		/// ファイルパスの拡張子を変更します。
		/// </summary>
		/// <param name="this">ファイル情報</param>
		/// <param name="extension">新しい拡張子</param>
		/// <returns>拡張子を変更したファイルパスを返します。</returns>
		public static string ChangeExtension(this FileInfo @this, string extension) {
			var path = @this.FullName;
			var changePath = Path.ChangeExtension(path, extension);
			return changePath;
		}

		#endregion
	}
}