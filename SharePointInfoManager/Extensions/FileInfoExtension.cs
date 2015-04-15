using System.Data;
using System.Data.Odbc;
using System.IO;

namespace SharePointManager.Extensions {
	/// <summary>
	/// FileInfo を拡張するメソッドを提供します。
	/// </summary>
	public static partial class FileInfoExtension {
		/// <summary>
		/// CSVファイルのデータを読込みます。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <returns>CSVファイルのデータを格納した DataTable を返します。</returns>
		public static DataTable LoadCsvData(this FileInfo @this) {
			if (!@this.Exists) {
				throw new FileNotFoundException("ファイルが存在しません。");
			}

			// 接続文字列取得
			var connectionString = @this.GetConnectionStringOfCSV();

			var tbl = new DataTable();
			using (var cn = new OdbcConnection(connectionString)) {
				var cmd = @this.GetSelectCommandTextOfCSV();
				using (var adapter = new OdbcDataAdapter(cmd, cn)) {
					adapter.Fill(tbl);
				}
			}

			return tbl;
		}

		/// <summary>
		/// CSVファイル形式の接続文字列を取得します。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <returns>CSVファイル形式の接続文字列を返します。</returns>
		private static string GetConnectionStringOfCSV(this FileInfo @this) {
			var connectionString = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq="
				+ @this.DirectoryName + ";Extensions=asc,csv,tab,txt;";
			return connectionString;
		}

		/// <summary>
		/// CSVファイル形式のSQL SELECT ステートメント文字列を取得します。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <returns>SQL SELECT ステートメント文字列を返します。</returns>
		private static string GetSelectCommandTextOfCSV(this FileInfo @this) {
			var selectCommandText = "SELECT * FROM [" + @this.Name + "]";
			return selectCommandText;
		}
	}
}
