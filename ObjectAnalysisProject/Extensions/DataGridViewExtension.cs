using System.Data;
using System.Windows.Forms;

namespace ObjectAnalysisProject.Extensions {
	/// <summary>
	/// DataGridView を拡張するメソッドを提供します。
	/// </summary>
	public static partial class DataGridViewExtension {
		#region データテーブル変換

		/// <summary>
		/// DataGridView のデータソースを、
		/// データテーブルに変換します。</summary>
		/// <param name="this">DataGridView</param>
		/// <returns>データテーブルのインスタンスを返します。</returns>
		public static DataTable ToDataTable(this DataGridView @this) {
			var dt = @this.DataSource as DataTable;
			if (dt != null) {
				dt.AcceptChanges();
				return dt;
			}

			dt = new DataTable();

			// 列追加
			dt.AddColumns(@this.Columns);

			// 行データ追加
			dt.AddRows(@this.Rows);

			return dt;
		}

		#endregion
	}
}
