using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CommonFeaturesLibrary.Providers.Csv;
using ExtensionsLibrary.Extensions;

namespace CommonFeaturesLibrary.Extensions {
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

		/// <summary>
		/// 文字列を書き込み、続けて行終端記号を書き込みます。[非同期]
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="message">メッセージ</param>
		public static async Task WriteLineAsync(this FileInfo @this, string message) {
			using (var sw = @this.AppendText()) {
				await sw.WriteLineAsync(message);
			}
		}

		#endregion

		#region Clear

		/// <summary>
		/// ファイルのデータを初期化します。
		/// </summary>
		/// <param name="this">FileInfo</param>
		public static void Clear(this FileInfo @this) {
			var fileName = @this.FullName;
			using (var sw = new StreamWriter(fileName, false)) {
			}
		}

		#endregion

		#region GetVersionName

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

			var dir = @this.DirectoryName;
			var ext = @this.Extension;
			var name = @this.Name.CommentOut(@this.Extension);
			return $@"{dir}\{name} ({version}){ext}";
		}

		#endregion

		#region ChangeExtension

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

		/// <summary>
		/// 指定した Stream の内容をコピーします。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="stream">Stream</param>
		public static async Task CopyAsync(this FileInfo @this, Stream stream) {
			using (var fs = @this.OpenWrite()) {
				await stream.CopyToAsync(fs);
			}
		}

		/// <summary>
		/// 追加方法とエンコーディングを指定して、
		/// 新しいテキスト ファイルに書き込みを行う StreamWriter を作成します。
		/// </summary>
		/// <param name="tmpFile"></param>
		/// <param name="append">
		/// データをファイルに追加する場合は true、ファイルを上書きする場合は false。
		/// <para>指定されたファイルが存在しない場合、このパラメーターは無効であり、コンストラクターは新しいファイルを作成します。</para>
		/// </param>
		/// <param name="encoding">使用する文字エンコーディング</param>
		/// <returns>新しい StreamWriter を返します。</returns>
		public static StreamWriter CreateText(this FileInfo tmpFile, bool append, Encoding encoding) {
			return new StreamWriter(tmpFile.FullName, append, encoding);
		}

		/// <summary>
		/// 拡張子なしのファイル名を取得します。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <returns>拡張子なしのファイル名を返します。</returns>
		public static string GetNameWithoutExtension(this FileInfo @this) {
			return @this.Name.Remove(@this.Name.IndexOf(@this.Extension), @this.Extension.Length);
		}

		/// <summary>
		/// CSV ファイルからデータを読み込みます。
		/// </summary>
		/// <param name="file"></param>
		/// <returns>読み込んだデータテーブルを返します。</returns>
		public static DataTable LoadDataTable(this FileInfo file) {
			var csv = new CsvConnection(file);

			var table = new DataTable();
			csv.Connect(a => a.Fill(table));
			return table;
		}

		#endregion
	}
}
