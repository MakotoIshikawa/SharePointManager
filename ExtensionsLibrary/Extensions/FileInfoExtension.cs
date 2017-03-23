using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// FileInfo を拡張するメソッドを提供します。
	/// </summary>
	public static partial class FileInfoExtension {
		#region メソッド

		#region CreateText オーバーロード

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

		#endregion

		#region ファイル名取得

		/// <summary>
		/// 拡張子なしのファイル名を取得します。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <returns>拡張子なしのファイル名を返します。</returns>
		public static string GetNameWithoutExtension(this FileInfo @this) {
			return @this.Name.Remove(@this.Name.IndexOf(@this.Extension), @this.Extension.Length);
		}

		#endregion

		#region 拡張子変更

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

		#region 拡張子判定

		/// <summary>
		/// イメージファイルかどうかを判定します。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <returns>イメージファイルであれば true を返します。</returns>
		public static bool IsImage(this FileInfo @this) {
			return @this.ContainsAtExtension(
				".gif",
				".jpg", ".jpeg", ".jpe", ".jfif",
				".png",
				".bmp", ".dib", ".rle",
				".tif", ".tiff", ".nsk",
				".cgm",
				".pct", ".pic", ".pict",
				".pcx",
				".ico"
			);
		}

		/// <summary>
		/// 拡張子を判定ます。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="exts">拡張子の配列</param>
		/// <returns>該当する拡張子があれば true を返します。それ以外は false</returns>
		public static bool ContainsAtExtension(this FileInfo @this, params string[] exts) {
			var ext = @this.Extension.ToLower();
			return exts.Contains(ext);
		}

		#endregion

		#region テキスト読込

		/// <summary>
		/// ファイルからテキストを読み込みます。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <returns>読み込んだテキストを全て返します。</returns>
		public static string ReadText(this FileInfo @this) {
			using (var sr = @this.OpenText()) {
				return sr.ReadToEnd();
			}
		}

		/// <summary>
		/// ファイルからテキストを読み込みます。[非同期]
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <returns>読み込んだテキストを全て返します。</returns>
		public static async Task<string> ReadTextAsync(this FileInfo @this) {
			using (var sr = @this.OpenText()) {
				return await sr.ReadToEndAsync();
			}
		}

		#endregion

		#region 文字列行追加

		/// <summary>
		/// 文字列を書き込み、続けて行終端記号を書き込みます。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="message">メッセージ</param>
		public static void WriteLine(this FileInfo @this, string message) {
			using (var sw = @this.AppendText()) {
				sw.WriteLine(message);
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

		#region ファイルデータ初期化

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

		#region コピー

		/// <summary>
		/// 指定した Stream の内容をコピーします。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="stream">Stream</param>
		public static void Copy(this FileInfo @this, Stream stream) {
			using (var fs = @this.OpenWrite()) {
				stream.CopyTo(fs);
			}
		}

		/// <summary>
		/// 指定した Stream の内容をコピーします。[非同期]
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="stream">Stream</param>
		public static async Task CopyAsync(this FileInfo @this, Stream stream) {
			using (var fs = @this.OpenWrite()) {
				await stream.CopyToAsync(fs);
			}
		}

		#endregion

		#endregion
	}
}
