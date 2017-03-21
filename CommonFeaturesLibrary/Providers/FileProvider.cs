using System;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using CommonFeaturesLibrary.Providers.Enums;
using ExtensionsLibrary.Extensions;

namespace CommonFeaturesLibrary.Providers {
	/// <summary>
	/// ファイルにアクセスするメソッドを提供します。
	/// </summary>
	public static partial class FileProvider {
		#region 読込

		#region データベースアダプター

		/// <summary>
		/// データベースアダプターを提供します。(ODBC)
		/// </summary>
		/// <param name="connectionString">接続文字列</param>
		/// <param name="selectCommandText">SQL SELECT文またはストアドプロシージャである文字列</param>
		/// <param name="action">データベースアダプターを操作するメソッド</param>
		public static void ProvideDataAdapterByOdbc(string connectionString, string selectCommandText, Action<DbDataAdapter> action) {
			using (var cn = new OdbcConnection(connectionString)) {
				using (var adapter = new OdbcDataAdapter(selectCommandText, cn)) {
					action?.Invoke(adapter);
				}
			}
		}

		/// <summary>
		/// データベースアダプターを提供します。(OLE DB)
		/// </summary>
		/// <param name="connectionString">接続文字列</param>
		/// <param name="selectCommandText">SQL SELECT文またはストアドプロシージャである文字列</param>
		/// <param name="action">データベースアダプターを操作するメソッド</param>
		public static void ProvideDataAdapterByOleDb(string connectionString, string selectCommandText, Action<DbDataAdapter> action) {
			using (var cn = new OleDbConnection(connectionString)) {
				using (var adapter = new OleDbDataAdapter(selectCommandText, cn)) {
					action?.Invoke(adapter);
				}
			}
		}

		#endregion

		#region DbCommand

		/// <summary>
		/// DbCommand を提供します。(ODBC)
		/// </summary>
		/// <param name="connectionString">接続文字列</param>
		/// <param name="selectCommandText">SQL SELECT文またはストアドプロシージャである文字列</param>
		/// <param name="action">データリーダーを操作するメソッド</param>
		public static void ProvideCommandByOdbc(string connectionString, string selectCommandText, Action<DbCommand> action) {
			using (var cn = new OdbcConnection(connectionString)) {
				cn.Open();
				using (var cmd = new OdbcCommand(selectCommandText, cn)) {
					action?.Invoke(cmd);
				}
			}
		}

		/// <summary>
		/// DbCommand を提供します。(OLE DB)
		/// </summary>
		/// <param name="connectionString">接続文字列</param>
		/// <param name="selectCommandText">SQL SELECT文またはストアドプロシージャである文字列</param>
		/// <param name="action">データリーダーを操作するメソッド</param>
		public static void ProvideCommandByOleDb(string connectionString, string selectCommandText, Action<DbCommand> action) {
			using (var cn = new OleDbConnection(connectionString)) {
				cn.Open();
				using (var cmd = new OleDbCommand(selectCommandText, cn)) {
					action?.Invoke(cmd);
				}
			}
		}

		#endregion

		#endregion

		#region 接続文字列取得

		#region ODBC

		/// <summary>
		/// CSVファイル形式の接続文字列を取得します。(ODBC)
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <returns>CSVファイル形式の接続文字列を返します。</returns>
		public static string GetConnectionStringOfCsvByOdbc(this FileInfo @this) {
			var directory = @this.DirectoryName;

			var sb = new StringBuilder();
			sb.Append("Driver={Microsoft Text Driver (*.txt; *.csv)};")
			.Append("Dbq=").Append(directory).Append(";")
			.Append("Extensions=asc,csv,tab,txt;");

			return sb.ToString();
		}

		#endregion

		#region OLE DB

		/// <summary>
		/// CSV ファイル形式の接続文字列を取得します。(OLE DB)
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="hdr">最初の行が列名かどうかを表す値</param>
		/// <param name="imex">IMEX</param>
		/// <returns>CSV ファイル形式の接続文字列を返します。</returns>
		public static string GetConnectionStringOfCsvByOleDb(this FileInfo @this, bool hdr, EImex? imex) {
			var cmd = new DbConnectionStringBuilder();
			//cmd["Provider"] = "Microsoft.Jet.OLEDB.4.0";
			cmd["Provider"] = "Microsoft.ACE.OLEDB.12.0";
			cmd["Data Source"] = @this.DirectoryName;

			var properties = new StringBuilder();

			var fileKind = "text";
			properties.Append(fileKind).Append(";");

			var sb = new DbConnectionStringBuilder();
			sb["HDR"] = hdr ? "Yes" : "No";
			sb["FMT"] = "Delimited";

			if (imex.HasValue) {
				sb["IMEX"] = (short)imex;
			}

			properties.Append(sb.ToString());

			if (properties.Length != 0) {
				cmd["Extended Properties"] = properties.ToString();
			}

			return cmd.ToString();
		}

		/// <summary>
		/// Excel ファイル形式の接続文字列を取得します。(OLE DB)
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="hdr">最初の行が列名かどうかを表す値</param>
		/// <param name="imex">IMEX</param>
		/// <returns>Excel ファイル形式の接続文字列を返します。</returns>
		public static string GetConnectionStringByExcel(this FileInfo @this, bool hdr, EImex? imex) {
			var cmd = new DbConnectionStringBuilder();
			//cmd["Provider"] = "Microsoft.Jet.OLEDB.4.0";
			cmd["Provider"] = "Microsoft.ACE.OLEDB.12.0";
			cmd["Data Source"] = @this.FullName;

			var properties = new StringBuilder();
			var fileKind = "Excel 8.0";
			properties.Append(fileKind).Append(";");

			var sb = new DbConnectionStringBuilder();
			sb["HDR"] = hdr ? "Yes" : "No";

			if (imex.HasValue) {
				sb["IMEX"] = (short)imex;
			}

			properties.Append(sb.ToString());

			if (properties.Length != 0) {
				cmd["Extended Properties"] = properties.ToString();
			}

			return cmd.ToString();
		}

		/// <summary>
		/// Access ファイル形式の接続文字列を取得します。(OLE DB)
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="imex">IMEX</param>
		/// <returns>Access ファイル形式の接続文字列を返します。</returns>
		public static string GetConnectionStringByAccess(this FileInfo @this, EImex? imex) {
			var cmd = new DbConnectionStringBuilder();
			//cmd["Provider"] = "Microsoft.Jet.OLEDB.4.0";
			cmd["Provider"] = "Microsoft.ACE.OLEDB.12.0";
			cmd["Data Source"] = @this.FullName;

			var sb = new DbConnectionStringBuilder();
			if (imex.HasValue) {
				sb["IMEX"] = (short)imex;
			}

			var properties = new StringBuilder();
			properties.Append(sb.ToString());

			if (properties.Length != 0) {
				cmd["Extended Properties"] = properties.ToString();
			}

			return cmd.ToString();
		}

		#endregion

		#endregion

		#region 抽出条件文取得

		/// <summary>
		/// CSVファイル形式のSQL SELECT ステートメント文字列を取得します。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="selects">取得列</param>
		/// <returns>SQL SELECT ステートメント文字列を返します。</returns>
		public static string GetSelectCommandTextOfCsv(this FileInfo @this, params string[] selects) {
			var name = @this.Name;

			var select = GetSelectPhrase(selects);
			return $"SELECT {select} FROM [{name}]";
		}

		/// <summary>
		/// Excel ファイル形式の SQL SELECT ステートメント文字列を取得します。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="tableName">テーブル名</param>
		/// <param name="selects">取得列</param>
		/// <returns>SQL SELECT ステートメント文字列を返します。</returns>
		/// <remarks>
		/// <para>指定したテーブル名のデータを取得します。</para>
		/// <para>テーブル名の指定がない場合は、ファイル名と同名のシートのデータを取得します。</para>
		/// </remarks>
		public static string GetSelectCommandTextOfExcel(this FileInfo @this, string tableName, params string[] selects) {
			var name = tableName.IsEmpty() ? @this.Name.CommentOut(@this.Extension) : tableName;

			var select = GetSelectPhrase(selects);
			return $"SELECT {select} FROM [{name}$]";
		}

		/// <summary>
		/// Access ファイル形式の SQL SELECT ステートメント文字列を取得します。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="tableName">テーブル名</param>
		/// <param name="selects">取得列</param>
		/// <returns>SQL SELECT ステートメント文字列を返します。</returns>
		/// <remarks>
		/// <para>指定したテーブル名のデータを取得します。</para>
		/// <para>テーブル名の指定がない場合は、ファイル名と同名のシートのデータを取得します。</para>
		/// </remarks>
		public static string GetSelectCommandTextOfAccess(this FileInfo @this, string tableName, params string[] selects) {
			var name = tableName.IsEmpty() ? @this.Name.CommentOut(@this.Extension) : tableName;

			var select = GetSelectPhrase(selects);
			return $"SELECT {select} FROM [{name}]";
		}

		/// <summary>
		/// SELECT 句の文字列を取得します。
		/// </summary>
		/// <param name="selects">取得列</param>
		/// <returns>SELECT 句の文字列を返します。</returns>
		private static string GetSelectPhrase(params string[] selects) {
			var ss = selects?.Where(s => !s.IsEmpty());
			return (ss?.Any() ?? false)
				? ss.Join(", ")
				: "*";
		}

		#endregion
	}
}
