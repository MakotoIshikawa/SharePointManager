using System;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.IO;
using System.Text;
using ExtensionsLibrary.Extensions;
using SharePointManager.Properties;

namespace SharePointManager.Extensions {
	/// <summary>
	/// FileInfo を拡張するメソッドを提供します。
	/// </summary>
	public static partial class FileInfoExtension {
		/// <summary>
		/// ファイルのデータを読込みます。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="select">SELECT句を表す文字列</param>
		/// <returns>ファイルのデータを格納した DataTable を返します。</returns>
		public static DataTable LoadDataTable(this FileInfo @this, string select = null) {
			return @this.LoadCsvData(select);
		}

		/// <summary>
		/// CSVファイルのデータを読込みます。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="select">SELECT句を表す文字列</param>
		/// <returns>CSVファイルのデータを格納した DataTable を返します。</returns>
		/// <exception cref="System.IO.FileNotFoundException">ファイルが存在しません。</exception>
		private static DataTable LoadCsvData(this FileInfo @this, string select = null) {
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
				) ? @this.GetSelectCommandTextOfExcel(select, string.Empty)
				: @this.GetSelectCommandTextOfCSV(select);

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
		/// CSVファイルのデータを変更する
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="modify">データを変更するメソッド</param>
		/// <exception cref="System.IO.FileNotFoundException">ファイルが存在しません。</exception>
		/// <exception cref="System.ArgumentNullException">データを変更するメソッドが指定されていません。</exception>
		public static void SaveCsvData(this FileInfo @this, Action<DataTable> modify) {
			if (!@this.Exists) {
				throw new FileNotFoundException("ファイルが存在しません。");
			}

			if (modify == null) {
				throw new ArgumentNullException("データを変更するメソッドが指定されていません。");
			}

			// 接続文字列取得
			var connectionString = @this.GetConnectionStringByOleDb();

			var tbl = new DataTable();
			using (var cn = new OleDbConnection(connectionString)) {
				cn.Open();

				var cmd = (
					@this.Extension.HasString("md")
					|| @this.Extension.HasString("accd")
					|| @this.Extension.HasString("xls")
				) ? @this.GetSelectCommandTextOfExcel(null, string.Empty)
				: @this.GetSelectCommandTextOfCSV(null);

				using (var ad = new OleDbDataAdapter(cmd, cn)) {
					ad.Fill(tbl);

					modify(tbl);

					ad.UpdateCommand = cn.CreateCommand();
					ad.UpdateCommand.CommandText = @"";
					ad.Update(tbl);
				}
			}
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

		#region OLE DB

		private static string GetConnectionStringByOleDb(this FileInfo @this) {
			var connectionString = @this.GetConnectionStringOfCsvByOleDb();
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

		/// <summary>
		/// CSVファイル形式のSQL SELECT ステートメント文字列を取得します。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <returns>SQL SELECT ステートメント文字列を返します。</returns>
		private static string GetSelectCommandTextOfCSV(this FileInfo @this, string select) {
			var sb = new StringBuilder();
			sb.AppendFormat("SELECT {1} FROM [{0}]", @this.Name, select.IsEmpty() ? "*" : select);
			return sb.ToString();
		}

		/// <summary>
		/// Excelファイル形式のSQL SELECT ステートメント文字列を取得します。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="tableName">テーブル名</param>
		/// <returns>SQL SELECT ステートメント文字列を返します。</returns>
		/// <remarks>
		/// <para>指定したテーブル名のデータを取得します。</para>
		/// <para>テーブル名の指定がない場合は、ファイル名と同名のシートのデータを取得します。</para>
		/// </remarks>
		private static string GetSelectCommandTextOfExcel(this FileInfo @this, string select, string tableName) {
			var name = tableName.IsEmpty() ? @this.Name.CommentOut(@this.Extension) : tableName;

			var sb = new StringBuilder();
			if (@this.Extension.HasString("xls")) {
				sb.AppendFormat("SELECT {1} FROM [{0}$]", name, select.IsEmpty() ? "*" : select);
			} else {
				sb.AppendFormat("SELECT {1} FROM [{0}]", name, select.IsEmpty() ? "*" : select);
			}

			return sb.ToString();
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
