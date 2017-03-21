using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// FileInfo を拡張するメソッドを提供します。
	/// </summary>
	public static partial class FileInfoExtension {
		#region メソッド

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
				".pcx"
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

		#endregion
	}
}
