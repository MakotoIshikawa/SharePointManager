using System.Collections.Generic;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// ジェネリックの List を拡張するメソッドを提供します。
	/// </summary>
	public static class ListExtension {
		/// <summary>
		/// 先頭に要素を追加します。
		/// </summary>
		/// <typeparam name="T">要素の型</typeparam>
		/// <param name="ls">リスト</param>
		/// <param name="item">要素</param>
		public static void Prepend<T>(this List<T> ls, T item) {
			ls.Insert(0, item);
		}
	}
}
