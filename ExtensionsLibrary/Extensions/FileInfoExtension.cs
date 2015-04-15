using System.IO;

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
#if false
		/// <summary>
		/// JSON ファイルを逆シリアル化します。
		/// </summary>
		/// <typeparam name="TResult">逆シリアル化する型</typeparam>
		/// <param name="this">JSON ファイル情報</param>
		/// <returns>逆シリアル化されたオブジェクト</returns>
		public static TResult DeserializeFromJson<TResult>(this FileInfo @this) {
			// 読み込むファイルを開く
			using (var fs = @this.OpenRead()) {
				return fs.DeserializeFromJson<TResult>();
			}
		}

#endif
		#endregion

		#endregion
	}
}