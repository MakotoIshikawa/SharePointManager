using System.Collections.Generic;
using System.Data;
using ObjectAnalysisProject.Extensions;

namespace ObjectAnalysisProject {
	/// <summary>
	/// オブジェクトシュレッダークラス</summary>
	/// <typeparam name="T">オブジェクトのタイプ</typeparam>
	public class ObjectShredder<T> {
		#region フィールド

		private DataTable m_table = null;

		#endregion

		#region プロパティ

		/// <summary>
		/// データテーブル</summary>
		public DataTable Table {
			get { return this.m_table; }
			protected set { this.m_table = value; }
		}

		#endregion

		#region コンストラクタ

		/// <summary>
		/// ObjectShredderのコンストラクタです。</summary>
		public ObjectShredder()
			: this(null) {
		}

		/// <summary>
		/// データテーブルを指定して、ObjectShredder の新しいインスタンスを作成します。</summary>
		/// <param name="table">データテーブル</param>
		public ObjectShredder(DataTable table) {
			if (table == null) {
				// 入力テーブルがnullの場合、新しいテーブルを作成します。
				this.Table = new DataTable(typeof(T).Name);
			} else {
				this.Table = table;
			}
		}

		#endregion

		#region メソッド

		/// <summary>
		/// オブジェクトを細断処理します。
		/// オブジェクトの配列からデータテーブルにデータをロードします。</summary>
		/// <param name="source">オブジェクトの順序は、データテーブルにロードします。</param>
		/// <param name="options">指定ソース配列からの値がテーブル内の既存の行に適用されますか。</param>
		/// <returns>ソースシーケンスから作成されたデータテーブル。</returns>
		public void Shred(IEnumerable<T> source, LoadOption? options) {
			if (typeof(T).IsPrimitive) {
				// Tがプリミティブ型である場合、スカラ配列からテーブルをロードします。
				this.Table.ShredPrimitive(source, options);
			} else {
				this.Table.ShredNotPrimitive(source, options);
			}
		}

		#endregion
	}
}
