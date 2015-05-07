using System;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// Object を拡張するメソッドを提供します。
	/// </summary>
	public static partial class ObjectExtension {
		/// <summary>
		/// Boolean 型に変換します。
		/// </summary>
		/// <param name="this">object</param>
		/// <returns>変換した Boolean 値を返します。</returns>
		public static bool ToBoolean(this object @this) {
			var result = false;
			bool.TryParse(@this.ToString(), out result);

			return result;
		}

	}
}
