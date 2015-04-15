using System;
using System.Collections.Generic;
using System.Data;

namespace ObjectAnalysisProject.Extensions {
	/// <summary>
	/// Enumerable を拡張メソッドを提供します。
	/// </summary>
	public static partial class EnumerableExtension {
		#region テーブル変換	ToDataTable(+1)

		/// <summary>
		/// 指定した入力 IEnumerable オブジェクトに応じて、
		/// オブジェクトのコピーを格納する DataTableを返します。
		/// </summary>
		/// <typeparam name="T">ソース シーケンス内のオブジェクトの型。</typeparam>
		/// <param name="source">ソース IEnumerable シーケンス。</param>
		/// <param name="options">DataTable 読み込みオプションを指定する LoadOption 列挙体。</param>
		/// <returns>オブジェクト型の入力シーケンスを格納する DataTable。</returns>
		public static DataTable ToDataTable<T>(this IEnumerable<T> source, LoadOption? options = null) {
#if true
			var table = new DataTable(typeof(T).Name);
			table.Shred(source, options);
			return table;
#else
			var sh = new ObjectShredder<T>();
			sh.Shred(source, options);
			return sh.Table;
#endif
		}

		/// <summary>
		/// 指定した入力 IEnumerable オブジェクトに応じて、
		/// オブジェクトのコピーを格納する DataTableを返します。
		/// </summary>
		/// <typeparam name="T">ソース シーケンス内のオブジェクトの型。</typeparam>
		/// <param name="source">ソース IEnumerable シーケンス。</param>
		/// <param name="table">元になる DataTable。</param>
		/// <param name="options">DataTable 読み込みオプションを指定する LoadOption 列挙体。</param>
		/// <returns>オブジェクト型の入力シーケンスを格納する DataTable。</returns>
		public static DataTable ToDataTable<T>(this IEnumerable<T> source, DataTable table, LoadOption? options = null) {
#if true
			table.Shred(source, options);
			return table;
#else
			var sh = new ObjectShredder<T>(table);
			sh.Shred(source, options);
			return sh.Table;
#endif
		}

		#endregion
	}
}
