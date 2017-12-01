using System.Collections.Generic;
using System.Data;
using System.Linq;
using ExtensionsLibrary.Extensions;
using ObjectAnalysisProject.Extensions;

namespace SharepointServerInsertApp.Extensions {
	/// <summary>
	/// DataTable を拡張するメソッドを提供します。
	/// </summary>
	public static partial class DataTableExtension {
		#region メソッド

		/// <summary>
		/// デーブルからアイテムを取得します。
		/// </summary>
		/// <param name="this">DataTable</param>
		/// <returns>アイテムのコレクションを返します。</returns>
		public static IEnumerable<Dictionary<string, object>> GetItems(this DataTable @this) {
			var cols = @this.GetColumns();
			var items = (
				from r in @this
				select cols.ToDictionary(c => c.ColumnName, c => r[c.ColumnName])
			);
			return items;
		}

		#endregion
	}
}
