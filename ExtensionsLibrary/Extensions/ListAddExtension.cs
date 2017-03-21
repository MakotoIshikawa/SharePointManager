using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// ジェネリックのリストに要素を追加する拡張メソッドを提供します。
	/// </summary>
	public static partial class ListAddExtension {
		#region メソッド

		#region SqlParameter

		/// <summary>
		/// SqlParameter のリストに要素を追加します。
		/// </summary>
		/// <param name="this">SqlParameter のリスト</param>
		/// <param name="parameterName">パラメーター名</param>
		/// <param name="value">値</param>
		public static void Add(this List<SqlParameter> @this, string parameterName, object value) {
			if (value == null) {
				@this.Add(new SqlParameter(parameterName, DBNull.Value));
			} else {
				@this.Add(new SqlParameter(parameterName, value));
			}
		}

		/// <summary>
		/// SqlParameter のリストに要素を追加します。
		/// </summary>
		/// <param name="this">SqlParameter のリスト</param>
		/// <param name="parameterName">パラメーター名</param>
		/// <param name="value">値</param>
		public static void Add(this List<SqlParameter> @this, string parameterName, string value) {
			if (value.IsEmpty()) {
				@this.Add(new SqlParameter(parameterName, DBNull.Value));
			} else {
				@this.Add(new SqlParameter(parameterName, value));
			}
		}

		/// <summary>
		/// SqlParameter のリストに要素を追加します。
		/// </summary>
		/// <param name="this">SqlParameter のリスト</param>
		/// <param name="parameterName">パラメーター名</param>
		/// <param name="dbType"></param>
		/// <param name="size"></param>
		/// <param name="sourceColumn"></param>
		public static void Add(this List<SqlParameter> @this, string parameterName, SqlDbType dbType, int size, string sourceColumn) {
			@this.Add(new SqlParameter(parameterName, dbType, size, sourceColumn));
		}

		/// <summary>
		/// SqlParameter のリストに要素を追加します。
		/// </summary>
		/// <param name="this">SqlParameter のリスト</param>
		/// <param name="parameterName">パラメーター名</param>
		/// <param name="dbType">DBタイプ</param>
		/// <param name="size">サイズ</param>
		/// <param name="direction">ParameterDirection</param>
		/// <param name="isNullable">NULLかどうか</param>
		/// <param name="precision">precision</param>
		/// <param name="scale">scale</param>
		/// <param name="sourceColumn">sourceColumn</param>
		/// <param name="sourceVersion">sourceVersion</param>
		/// <param name="value">値</param>
		public static void Add(this List<SqlParameter> @this, string parameterName, SqlDbType dbType, int size, ParameterDirection direction, bool isNullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value) {
			@this.Add(new SqlParameter(parameterName, dbType, size, direction, isNullable, precision, scale, sourceColumn, sourceVersion, value));
		}

		#endregion

		#endregion
	}
}
