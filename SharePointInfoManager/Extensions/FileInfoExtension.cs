using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.IO;
using ExtensionsLibrary.Extensions;

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
#if true
			// 接続文字列取得
			var connectionString = @this.GetConnectionStringOfCsvByOleDb();

			var tbl = new DataTable();
			using (var cn = new OleDbConnection(connectionString)) {
				if (@this.Extension.HasString("xls")) {
					var cmd = @this.GetSelectCommandTextOfExcel();
					using (var adapter = new OleDbDataAdapter(cmd, cn)) {
						adapter.Fill(tbl);
					}
				} else {
					var cmd = @this.GetSelectCommandTextOfCSV();
					using (var adapter = new OleDbDataAdapter(cmd, cn)) {
						adapter.Fill(tbl);
					}
				}
			}

			return tbl;
#else
			// 接続文字列取得
			var connectionString = @this.GetConnectionStringOfCsvByOdbc();

			var tbl = new DataTable();
			using (var cn = new OdbcConnection(connectionString)) {
				var cmd = @this.GetSelectCommandTextOfCSV();
				using (var adapter = new OdbcDataAdapter(cmd, cn)) {
					adapter.Fill(tbl);
				}
			}

			return tbl;
#endif
		}

		/// <summary>
		/// CSVファイル形式の接続文字列を取得します。(ODBC)
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <returns>CSVファイル形式の接続文字列を返します。</returns>
		private static string GetConnectionStringOfCsvByOdbc(this FileInfo @this) {
			var connectionString = "Driver={Microsoft Text Driver (*.txt; *.csv)};"
			+ "Dbq=" + @this.DirectoryName + ";"
			+ "Extensions=asc,csv,tab,txt;";
			return connectionString;
		}

		/// <summary>
		/// CSVファイル形式の接続文字列を取得します。(OLE DB)
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="hdr">かどうかを表す値</param>
		/// <param name="imex">imex</param>
		/// <returns>CSVファイル形式の接続文字列を返します。</returns>
		private static string GetConnectionStringOfCsvByOleDb(this FileInfo @this, bool hdr = true, EImex? imex = null) {
			var cmd = new DbConnectionStringBuilder();
			//cmd["Provider"] = "Microsoft.Jet.OLEDB.4.0";
			cmd["Provider"] = "Microsoft.ACE.OLEDB.12.0";

			var sb = new DbConnectionStringBuilder();
			sb["HDR"] = hdr ? "Yes" : "No";

			var properties = string.Empty;
			if (@this.Extension.HasString("xls")) {
				cmd["Data Source"] = @this.FullName;

				var fileKind = "Excel 8.0";
				properties = fileKind + ";";
			} else {
				cmd["Data Source"] = @this.DirectoryName;

				var fileKind = "text";
				properties = fileKind + ";";
				sb["FMT"] = "Delimited";
			}

			if (imex.HasValue) {
				sb["IMEX"] = (short)imex;
			}

			properties += sb.ToString();

			cmd["Extended Properties"] = properties;

			var str = cmd.ToString();
			return str;
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

		/// <summary>
		/// Excelファイル形式のSQL SELECT ステートメント文字列を取得します。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <returns>SQL SELECT ステートメント文字列を返します。</returns>
		/// <remarks>ファイル名と同名のシートのデータを取得します。</remarks>
		private static string GetSelectCommandTextOfExcel(this FileInfo @this) {
			var selectCommandText = "SELECT * FROM [" + @this.Name.CommentOut(@this.Extension) + "$]";
			return selectCommandText;
		}
	}

	/// <summary>
	/// IMEXに関する列挙型です。
	/// </summary>
	public enum EImex : short {
		/// <summary>エクスポート</summary>
		Export = 0,
		/// <summary>インポート</summary>
		Import = 1,
		/// <summary>リンク(完全な更新が可能)</summary>
		Link = 2,
	}
}
