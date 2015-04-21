using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

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
		public static void ShredPrimitive<T>(this DataTable table, IEnumerable<T> source, LoadOption? options = null) {
			// テーブルがnullの場合、例外を投げます。
			if (table == null) {
				throw new ArgumentNullException("table", "テーブルが null です。");
			}

			// テーブルのスキーマを拡張する。
			var index = table.ExtendSchema("Value", typeof(T));

			var count = table.Columns.Count;

			// ソースシーケンスを列挙し、スカラー値をロードします。
			table.LoadData(source.Select(value => value.ToArray(count, index)), options);
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
		public static void ShredNotPrimitive<T>(this DataTable table, IEnumerable<T> source, LoadOption? options = null) {
			// テーブルがnullの場合、例外を投げます。
			if (table == null) {
				throw new ArgumentNullException("table", "テーブルが null です。");
			}

			// インスタンスのメンバ情報でテーブルのスキーマを拡張する。
			table.ExtendSchema(source);

			var count = table.Columns.Count;

			// ソースシーケンスを列挙し、オブジェクトの値をロードします。
			table.LoadData(source.Select(instance => table.GetMemberInfo(instance).ToArray(count)), options);
		}

		/// <summary>
		/// データテーブルの列をキーとするインスタンス値の列挙子を取得します。</summary>
		/// <typeparam name="T">データ型</typeparam>
		/// <param name="table">データテーブル</param>
		/// <param name="instance">インスタンス</param>
		/// <returns>データテーブルの列をキーとするインスタンス値の列挙子</returns>
		private static IEnumerable<KeyValuePair<int, object>> GetMemberInfo<T>(this DataTable table, T instance) {
			// インスタンスが派生している場合は派生型を、それ以外はT型
			var instanceType = (instance.GetType() != typeof(T)) ? instance.GetType() : typeof(T);

			var fields = instanceType.GetFields();
			var properties = instanceType.GetProperties();

			var member =
				fields.Select(f => new { Index = table.Columns[f.Name].Ordinal, Value = f.GetValue(instance) })
				.Union(properties.Select(p => new { Index = table.Columns[p.Name].Ordinal, Value = p.GetValue(instance, null) }));

			return member.Select(info => new KeyValuePair<int, object>(info.Index, info.Value));
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

			table.BeginLoadData();

			foreach (var values in dataRows) {
				table.LoadDataRow(values, options);
			}

			table.EndLoadData();
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

		#region スキーマ拡張	ExtendSchema(+2)

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
			// インスタンスが派生している場合は派生型を、それ以外はT型
			var instanceType = (instance.GetType() != typeof(T)) ? instance.GetType() : typeof(T);

			var fields = instanceType.GetFields();
			var properties = instanceType.GetProperties();

			var member =
				fields.Select(f => new { Name = f.Name, Type = f.FieldType })
				.Union(properties.Select(p => new { Name = p.Name, Type = p.PropertyType }));

			// メンバ情報でテーブルのスキーマを拡張する。
			foreach (var info in member) {
				table.ExtendSchema(info.Name, info.Type);
			}
		}

		/// <summary>
		/// 名前と型を指定して、テーブルのスキーマを拡張します。</summary>
		/// <param name="table">データテーブル</param>
		/// <param name="name">名前</param>
		/// <param name="type">型</param>
		/// <returns>位置</returns>
		private static int ExtendSchema(this DataTable table, string name, Type type) {
			// 入力テーブルに Value 列が存在しない場合、新しい列を追加します。
			if (!table.Columns.Contains(name)) {
				table.Columns.Add(name, type);
			}

			return table.Columns[name].Ordinal;
		}

		#endregion

		#region 配列変換	ToArray(+1)

		/// <summary>
		/// 配列変換</summary>
		/// <typeparam name="T">型</typeparam>
		/// <param name="value">値</param>
		/// <param name="count">要素数</param>
		/// <param name="index">インデックス</param>
		/// <returns>配列</returns>
		private static object[] ToArray<T>(this T value, int count, int index) {
			var values = new object[count];
			values[index] = value;
			return values;
		}

		/// <summary>
		/// 配列変換</summary>
		/// <typeparam name="T">型</typeparam>
		/// <param name="info">情報</param>
		/// <param name="count">要素数</param>
		/// <returns>配列</returns>
		private static object[] ToArray<T>(this IEnumerable<KeyValuePair<int, T>> info, int count) {
			var values = new object[count];

			foreach (var kvp in info) {
				values[kvp.Key] = kvp.Value;
			}

			return values;
		}

		#endregion

		#endregion
	}
}
