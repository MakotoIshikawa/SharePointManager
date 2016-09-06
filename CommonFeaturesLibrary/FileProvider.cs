using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.IO;
using System.Text;
using ExtensionsLibrary.Extensions;

namespace CommonFeaturesLibrary {
	/// <summary>
	/// ファイルにアクセスするメソッドを提供します。
	/// </summary>
	public static partial class FileProvider {
		#region 読込

		/// <summary>
		/// CSVファイルのデータを読込みます。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="cols">SELECT する列の配列</param>
		/// <returns>CSVファイルのデータを格納した DataTable を返します。</returns>
		/// <exception cref="System.IO.FileNotFoundException">ファイルが存在しません。</exception>
		public static DataTable LoadDataTable(this FileInfo @this, params string[] cols) {
			if (!@this.Exists) {
				throw new FileNotFoundException("ファイルが存在しません。");
			}
#if true
			// 接続文字列取得
			var connectionString = @this.GetConnectionStringByOleDb();

			var tbl = new DataTable();
			using (var cn = new OleDbConnection(connectionString)) {
				var cmd = (
					@this.Extension.HasString("md")
					|| @this.Extension.HasString("accd")
					|| @this.Extension.HasString("xls")
				) ? @this.GetSelectCommandTextOfExcel(string.Empty, cols)
				: @this.GetSelectCommandTextOfCSV(cols);

				using (var adapter = new OleDbDataAdapter(cmd, cn)) {
					adapter.Fill(tbl);
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
		/// 文字列の列挙から Select 句を生成します。
		/// </summary>
		/// <param name="cols">列名の列挙</param>
		/// <returns>生成した Select 句の文字列を返します。</returns>
		private static string CreateSelectPhrase(IEnumerable<string> cols) {
			return (cols == null || !cols.Any())
				? "*"
				: cols.Select(c => string.Format("[{0}]", c)).Join(", ");
		}

		#endregion

		#region 接続文字列取得

		#region ODBC

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

		#endregion

		#region OLE DB

		/// <summary>
		/// CSVファイル形式の接続文字列を取得します。(OLE DB)
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="hdr">かどうかを表す値</param>
		/// <param name="imex">imex</param>
		/// <returns>CSVファイル形式の接続文字列を返します。</returns>
		private static string GetConnectionStringByOleDb(this FileInfo @this, bool hdr = true, EImex? imex = null) {
			var cmd = new DbConnectionStringBuilder();
			//cmd["Provider"] = "Microsoft.Jet.OLEDB.4.0";
			cmd["Provider"] = "Microsoft.ACE.OLEDB.12.0";

			var sb = new DbConnectionStringBuilder();

			var properties = new StringBuilder();
			if (@this.Extension.HasString("md")
			|| @this.Extension.HasString("accd")
			) {
				cmd["Data Source"] = @this.FullName;
			} else if (@this.Extension.HasString("xls")) {
				cmd["Data Source"] = @this.FullName;
				sb["HDR"] = hdr ? "Yes" : "No";

				var fileKind = "Excel 8.0";
				properties.Append(fileKind).Append(";");
			} else {
				cmd["Data Source"] = @this.DirectoryName;
				sb["HDR"] = hdr ? "Yes" : "No";

				var fileKind = "text";
				properties.Append(fileKind).Append(";");
				sb["FMT"] = "Delimited";
			}

			if (imex.HasValue) {
				sb["IMEX"] = (short)imex;
			}

			properties.Append(sb.ToString());

			if (!properties.ToString().IsEmpty()) {
				cmd["Extended Properties"] = properties.ToString();
			}

			var str = cmd.ToString();
			return str;
		}

		#endregion

		#endregion

		#region 抽出条件文取得

		/// <summary>
		/// CSVファイル形式のSQL SELECT ステートメント文字列を取得します。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="cols">SELECT する列の配列</param>
		/// <returns>SQL SELECT ステートメント文字列を返します。</returns>
		private static string GetSelectCommandTextOfCSV(this FileInfo @this, params string[] cols) {
			var select = CreateSelectPhrase(cols);
			var sb = new StringBuilder();
			sb.AppendFormat("SELECT {1} FROM [{0}]", @this.Name, select);
			return sb.ToString();
		}

		/// <summary>
		/// Excelファイル形式のSQL SELECT ステートメント文字列を取得します。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="tableName">テーブル名</param>
		/// <param name="cols">SELECT する列の配列</param>
		/// <returns>SQL SELECT ステートメント文字列を返します。</returns>
		/// <remarks>
		/// <para>指定したテーブル名のデータを取得します。</para>
		/// <para>テーブル名の指定がない場合は、ファイル名と同名のシートのデータを取得します。</para>
		/// </remarks>
		private static string GetSelectCommandTextOfExcel(this FileInfo @this, string tableName, params string[] cols) {
			var name = tableName.IsEmpty() ? @this.Name.CommentOut(@this.Extension) : tableName;

			var select = CreateSelectPhrase(cols);
			var sb = new StringBuilder();
			if (@this.Extension.HasString("xls")) {
				sb.AppendFormat("SELECT {1} FROM [{0}$]", name, select);
			} else {
				sb.AppendFormat("SELECT {1} FROM [{0}]", name, select);
			}

			return sb.ToString();
		}

		#endregion
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
