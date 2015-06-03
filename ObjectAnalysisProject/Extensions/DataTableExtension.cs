using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ExtensionsLibrary.Extensions;

namespace ObjectAnalysisProject.Extensions {
	/// <summary>
	/// DataTable を拡張するメソッドを提供します。
	/// </summary>
	public static partial class DataTableExtension {
		#region メソッド

		#region データ追加

		/// <summary>
		/// DataGridView の列コレクションを指定して、
		/// データテーブルに列を追加します。</summary>
		/// <param name="this">データテーブル</param>
		/// <param name="columns">列コレクション</param>
		public static void AddColumns(this DataTable @this, DataGridViewColumnCollection columns) {
			foreach (var col in columns.Cast<DataGridViewColumn>()) {
				@this.Columns.Add(col.Name, col.ValueType != null ? col.ValueType : typeof(Object));
			}
		}

		/// <summary>
		/// DataGridView の行コレクションを指定して、
		/// データテーブルに行データを追加します。</summary>
		/// <param name="this">データテーブル</param>
		/// <param name="rows">行コレクション</param>
		public static void AddRows(this DataTable @this, DataGridViewRowCollection rows) {
			foreach (var row in rows.Cast<DataGridViewRow>()) {
				try {
					var r = (DataRowView)row.DataBoundItem;
					@this.Rows.Add(r.Row);
				} catch (Exception) {
					continue;
				}
			}
		}

		#endregion

		#region Select

		/// <summary>
		/// データテーブルのデータ行を新しいフォームに射影します。
		/// </summary>
		/// <typeparam name="TResult">selector によって返される値の型。</typeparam>
		/// <param name="table">データテーブル</param>
		/// <param name="selector">各要素に適用する変換関数。</param>
		/// <returns>source の各要素に対して変換関数を呼び出した結果として得られる要素を含む IEnumerable(T)</returns>
		public static IEnumerable<TResult> Select<TResult>(this DataTable table, Func<DataRow, int, TResult> selector) {
			return table.Select().Select(selector);
		}

		/// <summary>
		/// データテーブルのデータ行を新しいフォームに射影します。
		/// </summary>
		/// <typeparam name="TResult">selector によって返される値の型。</typeparam>
		/// <param name="table">データテーブル</param>
		/// <param name="selector">各要素に適用する変換関数。</param>
		/// <returns>source の各要素に対して変換関数を呼び出した結果として得られる要素を含む IEnumerable(T)</returns>
		public static IEnumerable<TResult> Select<TResult>(this DataTable table, Func<DataRow, TResult> selector) {
			return table.Select().Select(selector);
		}

		#endregion

		#region Where

		/// <summary>
		/// 述語に基づいて値のデータテーブルのデータ行をフィルター処理します。
		/// </summary>
		/// <param name="table">データテーブル</param>
		/// <param name="predicate">各要素が条件を満たしているかどうかをテストする関数。</param>
		/// <returns>条件を満たす、入力シーケンスの要素を含む IEnumerable(DataRow)</returns>
		public static IEnumerable<DataRow> Where(this DataTable table, Func<DataRow, bool> predicate) {
			return table.Select().Where(predicate);
		}

		/// <summary>
		/// 述語に基づいて値のデータテーブルのデータ行をフィルター処理します。
		/// </summary>
		/// <param name="table">データテーブル</param>
		/// <param name="predicate">各要素が条件を満たしているかどうかをテストする関数。</param>
		/// <returns>条件を満たす、入力シーケンスの要素を含む IEnumerable(DataRow)</returns>
		public static IEnumerable<DataRow> Where(this DataTable table, Func<DataRow, int, bool> predicate) {
			return table.Select().Where(predicate);
		}

		#endregion

		#region 列データ列挙取得	GetColumns(+2)

		/// <summary>
		/// 列インデックスを指定して、
		/// データテーブルから列データの列挙を取得します。</summary>
		/// <param name="table">データテーブル</param>
		/// <param name="columnIndex">列インデックス</param>
		/// <returns>列データの列挙を返します。</returns>
		public static IEnumerable<Object> GetColumns(this DataTable table, int columnIndex) {
			return table.Select(row => row[columnIndex]);
		}

		/// <summary>
		/// 列の名前を指定して、
		/// データテーブルから列データの列挙を取得します。</summary>
		/// <param name="table">データテーブル</param>
		/// <param name="columnName">列名</param>
		/// <returns>列データの列挙を返します。</returns>
		public static IEnumerable<Object> GetColumns(this DataTable table, string columnName) {
			return table.Select(row => row[columnName]);
		}

		/// <summary>
		/// 列スキーマを指定して、
		/// データテーブルから列データの列挙を取得します。</summary>
		/// <param name="table">データテーブル</param>
		/// <param name="column">列スキーマ</param>
		/// <returns>列データの列挙を返します。</returns>
		public static IEnumerable<Object> GetColumns(this DataTable table, DataColumn column) {
			return table.Select(row => row[column]);
		}

		#endregion

		#region 細断処理

		/// <summary>
		/// オブジェクトを細断処理します。
		/// オブジェクトの配列からデータテーブルにデータをロードします。</summary>
		/// <param name="this">データテーブル</param>
		/// <param name="source">オブジェクトの順序は、データテーブルにロードします。</param>
		/// <param name="options">指定ソース配列からの値がテーブル内の既存の行に適用されますか。</param>
		/// <returns>ソースシーケンスから作成されたデータテーブル。</returns>
		public static void Shred<T>(this DataTable @this, IEnumerable<T> source, LoadOption? options) {
			if (@this == null) {
				throw new ArgumentNullException("@this", "データテーブルが null です。");
			}

			if (typeof(T).IsPrimitive) {
				// Tがプリミティブ型である場合、スカラ配列からテーブルをロードします。
				@this.ShredPrimitive(source, options);
			} else {
				@this.ShredNotPrimitive(source, options);
			}
		}

		#region プリミティブ型細断処理

		/// <summary>
		/// プリミティブ型オブジェクトの細断処理</summary>
		/// <param name="table">データテーブル</param>
		/// <param name="source">元になる列挙子</param>
		/// <param name="options">
		/// 配列値を既存の行にある対応する値に適用する方法を決定するために使用します。
		/// null を指定できます。</param>
		private static void ShredPrimitive<T>(this DataTable table, IEnumerable<T> source, LoadOption? options = null) {
			// 列コレクション内の列の位置インデックスを取得
			var col = table.AddColumn("Value", typeof(T));
			var ordinal = col.Ordinal;

			var length = table.Columns.Count;

			// ソースシーケンスを列挙し、スカラー値をロードします。StoredArray
			table.LoadData(source.Select(v => v.ConvertInsertedArray(length, ordinal)), options);
		}

		#endregion

		#region オブジェクトの細断処理

		/// <summary>
		/// オブジェクトの細断処理</summary>
		/// <typeparam name="T">データ型</typeparam>
		/// <param name="table">データテーブル</param>
		/// <param name="source">元になる列挙子</param>
		/// <param name="options">
		/// 配列値を既存の行にある対応する値に適用する方法を決定するために使用します。
		/// null を指定できます。</param>
		private static void ShredNotPrimitive<T>(this DataTable table, IEnumerable<T> source, LoadOption? options = null) {
			// インスタンスのメンバ情報でテーブルのスキーマを拡張する。
			table.ExtendSchema(source);

			// ソースシーケンスを列挙し、オブジェクトの値をロードします。
			table.LoadData(source.Select(v => v.ConvertInsertedArray(table.Columns)), options);
		}

		#endregion

		#endregion

		#region データ読込

		/// <summary>
		/// テーブルにデータを読み込みます。</summary>
		/// <param name="table">データテーブル</param>
		/// <param name="dataRows">行データ</param>
		/// <param name="options">
		/// 配列値を既存の行にある対応する値に適用する方法を決定するために使用します。
		/// null を指定できます。</param>
		public static void LoadData(this DataTable table, IEnumerable<object[]> dataRows, LoadOption? options = null) {
			if (table == null) {
				throw new ArgumentNullException("table", "テーブルが null です。");
			}

			try {
				table.BeginLoadData();

				foreach (var values in dataRows) {
					table.LoadDataRow(values, options);
				}
			} finally {
				table.EndLoadData();
			}
		}

		/// <summary>
		/// 特定の行を検索し、更新します。
		/// 一致する行が見つからない場合は、指定した値を使用して新しい行が作成されます。</summary>
		/// <param name="table"></param>
		/// <param name="values">新しい行の作成に使用する値の配列。</param>
		/// <param name="options">配列値を既存の行にある対応する値に適用する方法を決定するために使用します。</param>
		/// <returns>新しい System.Data.DataRow。</returns>
		public static DataRow LoadDataRow(this DataTable table, object[] values, LoadOption? options = null) {
			if (table == null) {
				throw new ArgumentNullException("table", "テーブルが null です。");
			}

			if (options.HasValue) {
				return table.LoadDataRow(values, options.Value);
			} else {
				return table.LoadDataRow(values, true);
			}
		}

		#endregion

		#region スキーマ拡張	ExtendSchema(+1)

		/// <summary>
		/// テーブルのスキーマを拡張します。</summary>
		/// <typeparam name="T">データ型</typeparam>
		/// <param name="table">データテーブル</param>
		/// <param name="source">列挙子</param>
		private static void ExtendSchema<T>(this DataTable table, IEnumerable<T> source) {
			foreach (var instance in source) {
				table.ExtendSchema(instance);
			}
		}

		/// <summary>
		/// インスタンスのメンバ(フィールド、プロパティ)情報でテーブルのスキーマを拡張します。</summary>
		/// <typeparam name="T">データ型</typeparam>
		/// <param name="table">データテーブル</param>
		/// <param name="instance">インスタンス</param>
		private static void ExtendSchema<T>(this DataTable table, T instance) {
			var members = instance.GetMembers()
			.Select(f => new { Name = f.Item1, Type = f.Item2 })
			.ToList();

			// メンバ情報でテーブルのスキーマを拡張する。
			members.ForEach(m => {
				table.AddColumn(m.Name, m.Type);
			});
		}

		/// <summary>
		/// パブリックなフィールドとプロパティの情報を取得します。
		/// </summary>
		/// <typeparam name="T">インスタンスの型</typeparam>
		/// <param name="this">this</param>
		/// <returns>メンバー情報を返します。</returns>
		public static IEnumerable<Tuple<string, Type, object>> GetMembers<T>(this T @this) {
			// インスタンスが派生している場合は派生型を、それ以外はT型
			var instanceType = (@this.GetType() != typeof(T)) ? @this.GetType() : typeof(T);
			var fields = instanceType.GetFields();
			var properties = instanceType.GetProperties();

			var member =
				fields.Select(f => new { f.Name, Type = f.FieldType, Value = f.GetValue(@this), })
				.Union(properties.Select(p => new { p.Name, Type = p.PropertyType, Value = p.GetValue(@this, null), }));

			return member.Select(m => Tuple.Create(m.Name, m.Type, m.Value));
		}

		#endregion

		/// <summary>
		/// 名前と型を指定して、列を追加します。
		/// </summary>
		/// <param name="this">this</param>
		/// <param name="name">列の名前(System.Data.DataColumn.ColumnName)</param>
		/// <param name="type">列の型(System.Data.DataColumn.DataType)</param>
		/// <returns>
		/// 新たに作成した列を返します。
		/// 列コレクション内に同名の列が既に存在する場合は、その列を返します。
		/// </returns>
		private static DataColumn AddColumn(this DataTable @this, string name, Type type = null) {
			if (!@this.Columns.Contains(name)) {
				// 入力テーブルに Value 列が存在しない場合、新しい列を追加します。
				if (type == null) {
					return @this.Columns.Add(name);
				} else {
					return @this.Columns.Add(name, type);
				}
			}

			return @this.Columns[name];
		}

		#region ConvertInsertedArray(+1)

		/// <summary>
		/// 要素数と位置インデックスを指定して、
		/// 配列に変換します。
		/// </summary>
		/// <typeparam name="T">型</typeparam>
		/// <param name="value">値</param>
		/// <param name="length">配列の要素数</param>
		/// <param name="ordinal">列の位置インデックス</param>
		/// <returns>配列</returns>
		private static object[] ConvertInsertedArray<T>(this T value, int length, int ordinal) {
			var values = new object[length];
			values[ordinal] = value;
			return values;
		}

		/// <summary>
		/// 列コレクションを指定して、
		/// 列インデックスをキーとするインスタンス値の配列を取得します。
		/// </summary>
		/// <typeparam name="T">データ型</typeparam>
		/// <param name="instance">インスタンス</param>
		/// <param name="columns">列コレクション</param>
		/// <returns>列をキーとするインスタンス値の配列を返します。</returns>
		private static object[] ConvertInsertedArray<T>(this T instance, DataColumnCollection columns) {
			var length = columns.Count;
			var values = new object[length];

			var members = instance.GetMembers()
			.Select(m => new {
				Index = columns[m.Item1].Ordinal,
				Value = m.Item3
			}).ToList();

			members.ForEach(m => {
				values[m.Index] = m.Value;
			});

			return values;
		}

		#endregion

		#endregion
	}
}
