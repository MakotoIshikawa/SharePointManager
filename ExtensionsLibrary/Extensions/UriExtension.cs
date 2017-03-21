using System;
using System.IO;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// Uri クラスを拡張するメソッドを提供します。
	/// </summary>
	public static partial class UriExtension {
		#region メソッド

		/// <summary>
		/// URI パス文字列のファイル名と拡張子を返します。
		/// </summary>
		/// <param name="this">Uri</param>
		/// <returns>URI の最後のディレクトリ文字の後ろの文字を返します。</returns>
		public static string GetFileName(this Uri @this) {
			return Path.GetFileName(@this.OriginalString);
		}

		/// <summary>
		/// URI パス文字列のファイル名を拡張子を付けずに返します。
		/// </summary>
		/// <param name="this">Uri</param>
		/// <returns>URI の最後のディレクトリ文字の後ろの拡張子を除く文字を返します。</returns>
		public static string GetFileNameWithoutExtension(this Uri @this) {
			return Path.GetFileNameWithoutExtension(@this.OriginalString);
		}

		#endregion
	}
}
