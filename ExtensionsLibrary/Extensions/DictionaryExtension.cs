using System.Collections.Generic;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// Dictionary を拡張するメソッドを提供します。
	/// </summary>
	public static class DictionaryExtension {
		#region GetValueOrDefault

		/// <summary>
		/// 値を取得します。存在しないKeyの場合は default 値を返します。
		/// </summary>
		/// <param name="this">マップ</param>
		/// <param name="key">キー</param>
		/// <returns>値を返します。</returns>
		public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> @this, TKey key) {
			return @this.GetValueOrDefault(key, default(TValue));
		}

		/// <summary>
		/// 値を取得します。存在しないKeyの場合は default 値を返します。
		/// </summary>
		/// <param name="this">マップ</param>
		/// <param name="key">キー</param>
		/// <param name="defaultValue">default 値</param>
		/// <returns>値を返します。</returns>
		public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> @this, TKey key, TValue defaultValue) {
			return @this.ContainsKey(key) ? @this[key] : defaultValue;
		}

		#endregion
	}
}