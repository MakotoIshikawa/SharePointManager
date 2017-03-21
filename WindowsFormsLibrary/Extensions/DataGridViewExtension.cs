using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsLibrary.Extensions {
	/// <summary>
	/// DataGridView を拡張するメソッドを提供します。
	/// </summary>
	public static partial class DataGridViewExtension {
		#region メソッド

		/// <summary>
		/// 選択中の行コレクションを取得します。
		/// </summary>
		/// <param name="this">DataGridView</param>
		/// <returns>DataGridViewRow のコレクションを返します。</returns>
		public static IEnumerable<DataGridViewRow> GetSelectedRows(this DataGridView @this) {
			return @this.SelectedRows.ToGeneric();
		}

		/// <summary>
		/// ジェネリック型のコレクションに変換します。
		/// </summary>
		/// <param name="this">DataGridViewSelectedRowCollection</param>
		/// <returns>DataGridViewRow のコレクションを返します。</returns>
		public static IEnumerable<DataGridViewRow> ToGeneric(this DataGridViewSelectedRowCollection @this) {
			return @this.Cast<DataGridViewRow>();
		}

		#endregion
	}
}
