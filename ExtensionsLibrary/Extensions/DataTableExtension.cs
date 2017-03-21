using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// DataTable を拡張するメソッドを提供します。
	/// </summary>
	public static partial class DataTableExtension {
		#region メソッド

		/// <summary>
		/// コレクションに変換します。
		/// </summary>
		/// <typeparam name="T">コレクションの要素の型</typeparam>
		/// <param name="this">データテーブル</param>
		/// <returns>コレクションを返します。</returns>
		public static IEnumerable<T> ConvertCollection<T>(this DataTable @this) where T : new() {
			var pros = (
				from p in typeof(T).GetProperties()
				join c in @this.GetColumns()
				  on p.Name equals c.ColumnName
				select p.Name
			);

			var rows = @this.GetRows();
			var collection = (
				from r in rows
				let dic = pros.ToDictionary(p => p, p => r[p])
				let item = new T().SetProperties(dic)
				select item
			);

			return collection;
		}

		/// <summary>
		/// 行データのコレクションを取得します。
		/// </summary>
		/// <param name="this">DataTable</param>
		/// <returns>行データのコレクションを返します。</returns>
		public static IEnumerable<DataRow> GetRows(this DataTable @this) {
			return @this.Rows.Cast<DataRow>();
		}

		/// <summary>
		/// 列データのコレクションを取得します。
		/// </summary>
		/// <param name="this">DataTable</param>
		/// <returns>列データのコレクションを返します。</returns>
		public static IEnumerable<DataColumn> GetColumns(this DataTable @this) {
			return @this.Columns.Cast<DataColumn>();
		}

		#endregion
	}
}
