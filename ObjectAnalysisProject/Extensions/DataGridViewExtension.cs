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

			// データテーブルの雛形作成
			dt = @this.CreateTable();

			// 行データ追加
			dt.AddRows(@this.Rows);

			return dt;
		}

		/// <summary>
		/// DataGridView の列構成を利用して、
		/// 新しい DataTable のインスタンスを作成します。</summary>
		/// <param name="this">DataGridView</param>
		/// <returns>データテーブルの雛形を返します。</returns>
		private static DataTable CreateTable(this DataGridView @this) {
			var dt = new DataTable();

			// 列追加
			dt.AddColumns(@this.Columns);

			return dt;
		}

		#endregion
	}
}
