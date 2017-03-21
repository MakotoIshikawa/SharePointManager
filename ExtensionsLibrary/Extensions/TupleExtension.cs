using System;
using System.Collections.Generic;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// Tuple クラスを拡張するメソッドを提供します。
	/// </summary>
	public static partial class TupleExtension {
		#region メソッド

		#region Add

		/// <summary>
		/// リストの末尾にオブジェクトを追加します。
		/// </summary>
		/// <typeparam name="T1">組の 1 番目のコンポーネントの型</typeparam>
		/// <param name="this">1 つの要素で構成される組のリスト</param>
		/// <param name="item1">リストの末尾に追加する 1 番目のオブジェクト。</param>
		public static void Add<T1>(this List<Tuple<T1>> @this, T1 item1)
			=> @this.Add(Tuple.Create(item1));

		/// <summary>
		/// リストの末尾にオブジェクトを追加します。
		/// </summary>
		/// <typeparam name="T1">組の 1 番目のコンポーネントの型</typeparam>
		/// <typeparam name="T2">組の 2 番目のコンポーネントの型</typeparam>
		/// <param name="this">2 つの要素で構成される組のリスト</param>
		/// <param name="item1">リストの末尾に追加する 1 番目のオブジェクト。</param>
		/// <param name="item2">リストの末尾に追加する 2 番目のオブジェクト。</param>
		public static void Add<T1, T2>(this List<Tuple<T1, T2>> @this, T1 item1, T2 item2)
			=> @this.Add(Tuple.Create(item1, item2));

		/// <summary>
		/// リストの末尾にオブジェクトを追加します。
		/// </summary>
		/// <typeparam name="T1">組の 1 番目のコンポーネントの型</typeparam>
		/// <typeparam name="T2">組の 2 番目のコンポーネントの型</typeparam>
		/// <typeparam name="T3">組の 3 番目のコンポーネントの型</typeparam>
		/// <param name="this">3 つの要素で構成される組のリスト</param>
		/// <param name="item1">リストの末尾に追加する 1 番目のオブジェクト。</param>
		/// <param name="item2">リストの末尾に追加する 2 番目のオブジェクト。</param>
		/// <param name="item3">リストの末尾に追加する 3 番目のオブジェクト。</param>
		public static void Add<T1, T2, T3>(this List<Tuple<T1, T2, T3>> @this, T1 item1, T2 item2, T3 item3)
			=> @this.Add(Tuple.Create(item1, item2, item3));

		/// <summary>
		/// リストの末尾にオブジェクトを追加します。
		/// </summary>
		/// <typeparam name="T1">組の 1 番目のコンポーネントの型</typeparam>
		/// <typeparam name="T2">組の 2 番目のコンポーネントの型</typeparam>
		/// <typeparam name="T3">組の 3 番目のコンポーネントの型</typeparam>
		/// <typeparam name="T4">組の 4 番目のコンポーネントの型</typeparam>
		/// <param name="this">4 つの要素で構成される組のリスト</param>
		/// <param name="item1">リストの末尾に追加する 1 番目のオブジェクト。</param>
		/// <param name="item2">リストの末尾に追加する 2 番目のオブジェクト。</param>
		/// <param name="item3">リストの末尾に追加する 3 番目のオブジェクト。</param>
		/// <param name="item4">リストの末尾に追加する 4 番目のオブジェクト。</param>
		public static void Add<T1, T2, T3, T4>(this List<Tuple<T1, T2, T3, T4>> @this, T1 item1, T2 item2, T3 item3, T4 item4)
			=> @this.Add(Tuple.Create(item1, item2, item3, item4));

		/// <summary>
		/// リストの末尾にオブジェクトを追加します。
		/// </summary>
		/// <typeparam name="T1">組の 1 番目のコンポーネントの型</typeparam>
		/// <typeparam name="T2">組の 2 番目のコンポーネントの型</typeparam>
		/// <typeparam name="T3">組の 3 番目のコンポーネントの型</typeparam>
		/// <typeparam name="T4">組の 4 番目のコンポーネントの型</typeparam>
		/// <typeparam name="T5">組の 5 番目のコンポーネントの型</typeparam>
		/// <param name="this">5 つの要素で構成される組のリスト</param>
		/// <param name="item1">リストの末尾に追加する 1 番目のオブジェクト。</param>
		/// <param name="item2">リストの末尾に追加する 2 番目のオブジェクト。</param>
		/// <param name="item3">リストの末尾に追加する 3 番目のオブジェクト。</param>
		/// <param name="item4">リストの末尾に追加する 4 番目のオブジェクト。</param>
		/// <param name="item5">リストの末尾に追加する 5 番目のオブジェクト。</param>
		public static void Add<T1, T2, T3, T4, T5>(this List<Tuple<T1, T2, T3, T4, T5>> @this, T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
			=> @this.Add(Tuple.Create(item1, item2, item3, item4, item5));

		/// <summary>
		/// リストの末尾にオブジェクトを追加します。
		/// </summary>
		/// <typeparam name="T1">組の 1 番目のコンポーネントの型</typeparam>
		/// <typeparam name="T2">組の 2 番目のコンポーネントの型</typeparam>
		/// <typeparam name="T3">組の 3 番目のコンポーネントの型</typeparam>
		/// <typeparam name="T4">組の 4 番目のコンポーネントの型</typeparam>
		/// <typeparam name="T5">組の 5 番目のコンポーネントの型</typeparam>
		/// <typeparam name="T6">組の 6 番目のコンポーネントの型</typeparam>
		/// <param name="this">6 つの要素で構成される組のリスト</param>
		/// <param name="item1">リストの末尾に追加する 1 番目のオブジェクト。</param>
		/// <param name="item2">リストの末尾に追加する 2 番目のオブジェクト。</param>
		/// <param name="item3">リストの末尾に追加する 3 番目のオブジェクト。</param>
		/// <param name="item4">リストの末尾に追加する 4 番目のオブジェクト。</param>
		/// <param name="item5">リストの末尾に追加する 5 番目のオブジェクト。</param>
		/// <param name="item6">リストの末尾に追加する 6 番目のオブジェクト。</param>
		public static void Add<T1, T2, T3, T4, T5, T6>(this List<Tuple<T1, T2, T3, T4, T5, T6>> @this, T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
			=> @this.Add(Tuple.Create(item1, item2, item3, item4, item5, item6));

		/// <summary>
		/// リストの末尾にオブジェクトを追加します。
		/// </summary>
		/// <typeparam name="T1">組の 1 番目のコンポーネントの型</typeparam>
		/// <typeparam name="T2">組の 2 番目のコンポーネントの型</typeparam>
		/// <typeparam name="T3">組の 3 番目のコンポーネントの型</typeparam>
		/// <typeparam name="T4">組の 4 番目のコンポーネントの型</typeparam>
		/// <typeparam name="T5">組の 5 番目のコンポーネントの型</typeparam>
		/// <typeparam name="T6">組の 6 番目のコンポーネントの型</typeparam>
		/// <typeparam name="T7">組の 7 番目のコンポーネントの型</typeparam>
		/// <param name="this">7 つの要素で構成される組のリスト</param>
		/// <param name="item1">リストの末尾に追加する 1 番目のオブジェクト。</param>
		/// <param name="item2">リストの末尾に追加する 2 番目のオブジェクト。</param>
		/// <param name="item3">リストの末尾に追加する 3 番目のオブジェクト。</param>
		/// <param name="item4">リストの末尾に追加する 4 番目のオブジェクト。</param>
		/// <param name="item5">リストの末尾に追加する 5 番目のオブジェクト。</param>
		/// <param name="item6">リストの末尾に追加する 6 番目のオブジェクト。</param>
		/// <param name="item7">リストの末尾に追加する 7 番目のオブジェクト。</param>
		public static void Add<T1, T2, T3, T4, T5, T6, T7>(this List<Tuple<T1, T2, T3, T4, T5, T6, T7>> @this, T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
			=> @this.Add(Tuple.Create(item1, item2, item3, item4, item5, item6, item7));

		#endregion

		#endregion
	}
}
