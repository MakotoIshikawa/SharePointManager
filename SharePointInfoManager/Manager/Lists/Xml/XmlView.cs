using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ExtensionsLibrary.Extensions;

namespace SharePointInfoManager.Manager.Lists.Xml {
	/// <summary>
	/// CAML Query 用のXML構造です。
	/// </summary>
	[XmlType(AnonymousType = true)]
	[XmlRoot("View", Namespace = "", IsNullable = false)]
	public partial class XmlView {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public XmlView() { }

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="rowLimit">最大行数</param>
		/// <param name="viewFields">参照フィールド名配列</param>
		public XmlView(int rowLimit, params string[] viewFields) {
			if (rowLimit != 0) {
				this.RowLimit = rowLimit;
			}

			this.AddFields(viewFields);
		}

		#endregion

		/// <summary></summary>
		[XmlArray("ViewFields", IsNullable = false)]
		[XmlArrayItem("FieldRef", IsNullable = false)]
		public List<FieldRef> Fields { get; set; }

		/// <summary></summary>
		public ViewQuery Query { get; set; }

		/// <summary></summary>
		public int RowLimit { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool RowLimitSpecified { get { return this.RowLimit > 0; } }

		#region メソッド

		/// <summary>
		/// 参照フィールド追加
		/// </summary>
		/// <param name="name">フィールド名</param>
		protected void AddField(string name) {
			if (this.Fields == null) {
				this.Fields = new List<FieldRef>();
			}

			this.Fields.Add(new FieldRef() { Name = name });
		}

		/// <summary>
		/// 参照フィールド追加
		/// </summary>
		/// <param name="viewFields">参照フィールド名配列</param>
		public void AddFields(params string[] viewFields) {
			foreach (var name in viewFields) {
				this.AddField(name);
			}
		}

		#region AddQueryItem

		/// <summary>
		/// クエリ項目追加
		/// </summary>
		/// <typeparam name="TOperator">条件型</typeparam>
		/// <param name="name">参照フィールド名</param>
		public void AddQueryItem<TOperator>(string name) where TOperator : QueryOperator, new() {
			this.AddQueryItem(new TOperator() {
				FieldRef = new FieldRef() { Name = name },
			});
		}

		/// <summary>
		/// クエリ項目追加
		/// </summary>
		/// <typeparam name="TOperator">条件型</typeparam>
		/// <param name="name">参照フィールド名</param>
		/// <param name="type">比較値型</param>
		/// <param name="value">比較値</param>
		public void AddQueryItem<TOperator>(string name, string type, string value) where TOperator : QueryOperator, new() {
			this.AddQueryItem(new TOperator() {
				FieldRef = new FieldRef() { Name = name },
				Value = new ViewValue() { Type = type, Value = value },
			});
		}

		/// <summary>
		/// クエリ項目追加
		/// </summary>
		/// <param name="item">クエリ項目</param>
		protected void AddQueryItem(object item) {
			if (this.Query == null) {
				this.Query = new ViewQuery();
			}
			if (this.Query.Items == null) {
				this.Query.Items = new List<object>();
			}

			this.Query.Items.Add(item);
		}

		/// <summary>
		/// クエリ項目追加
		/// </summary>
		/// <typeparam name="TQueryItems"></typeparam>
		/// <param name="action"></param>
		public void AddQueryItems<TQueryItems>(Action<TQueryItems> action) where TQueryItems : QueryItems, new() {
			var items = new TQueryItems();
			action(items);

			this.AddQueryItem(items);
		}

		#endregion

		/// <summary>
		/// XmlField を文字列に変換します。。
		/// </summary>
		/// <returns>XmlField を表す文字列を返します。</returns>
		public override string ToString() {
			try {
				var str = this.ToXmlString(false);
				var index = str.IndexOf("<View");
				if (index < 0) {
					throw new Exception();
				}

				var ret = str.Substring(index);
				return ret;
			} catch (Exception) {
				return string.Empty;
			}
		}

		#endregion
	}

	/// <summary></summary>
	[XmlType(AnonymousType = true)]
	public partial class ViewQuery {
		/// <summary></summary>
		[XmlArray("Where", IsNullable = false)]
		[XmlArrayItem("And", typeof(QueryItemsAnd))
		, XmlArrayItem("Or", typeof(QueryItemsOr))
		, XmlArrayItem("Eq", typeof(QueryOperatorEq))
		, XmlArrayItem("Neq", typeof(QueryOperatorNeq))
		, XmlArrayItem("Gt", typeof(QueryOperatorGt))
		, XmlArrayItem("Lt", typeof(QueryOperatorLt))
		, XmlArrayItem("Geq", typeof(QueryOperatorGeq))
		, XmlArrayItem("Leq", typeof(QueryOperatorLeq))
		, XmlArrayItem("IsNull", typeof(QueryOperatorIsNull))
		, XmlArrayItem("IsNotNull", typeof(QueryOperatorIsNotNull))
		, XmlArrayItem("Contains", typeof(QueryOperatorContains))
		, XmlArrayItem("BeginsWith", typeof(QueryOperatorBeginsWith))
		, XmlArrayItem("DateRangesOverlap", typeof(QueryOperatorDateRangesOverlap))]
		public List<object> Items { get; set; }
	}

	/// <summary></summary>
	[XmlType(AnonymousType = true)]
	public abstract partial class QueryItems {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ(protected)
		/// </summary>
		protected QueryItems() { }

		#endregion

		/// <summary></summary>
		[XmlElement("Eq", typeof(QueryOperatorEq))]
		[XmlElement("Neq", typeof(QueryOperatorNeq))]
		[XmlElement("Gt", typeof(QueryOperatorGt))]
		[XmlElement("Lt", typeof(QueryOperatorLt))]
		[XmlElement("Geq", typeof(QueryOperatorGeq))]
		[XmlElement("Leq", typeof(QueryOperatorLeq))]
		[XmlElement("IsNull", typeof(QueryOperatorIsNull))]
		[XmlElement("IsNotNull", typeof(QueryOperatorIsNotNull))]
		[XmlElement("Contains", typeof(QueryOperatorContains))]
		[XmlElement("BeginsWith", typeof(QueryOperatorBeginsWith))]
		[XmlElement("DateRangesOverlap", typeof(QueryOperatorDateRangesOverlap))]
		public List<QueryOperator> Items { get; set; }

		#region メソッド

		/// <summary>
		/// 条件項目追加
		/// </summary>
		/// <typeparam name="TOperator">条件型</typeparam>
		/// <param name="name">参照フィールド名</param>
		public void AddOperatorItem<TOperator>(string name) where TOperator : QueryOperator, new() {
			this.AddOperatorItem(new TOperator() {
				FieldRef = new FieldRef() { Name = name },
			});
		}

		/// <summary>
		/// 条件項目追加
		/// </summary>
		/// <typeparam name="TOperator">条件型</typeparam>
		/// <param name="name">参照フィールド名</param>
		/// <param name="type">比較値型</param>
		/// <param name="value">比較値</param>
		public void AddOperatorItem<TOperator>(string name, string type, string value) where TOperator : QueryOperator, new() {
			this.AddOperatorItem(new TOperator() {
				FieldRef = new FieldRef() { Name = name },
				Value = new ViewValue() { Type = type, Value = value },
			});
		}

		/// <summary>
		/// 条件項目追加
		/// </summary>
		/// <param name="item">条件項目</param>
		protected void AddOperatorItem(QueryOperator item) {
			if (this.Items == null) {
				this.Items = new List<QueryOperator>();
			}

			this.Items.Add(item);
		}

		#endregion
	}

	/// <summary></summary>
	[XmlType(AnonymousType = true)]
	public partial class QueryItemsAnd : QueryItems { }

	/// <summary></summary>
	[XmlType(AnonymousType = true)]
	public partial class QueryItemsOr : QueryItems { }

	/// <summary></summary>
	[XmlType(AnonymousType = true)]
	public abstract partial class QueryOperator {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ(protected)
		/// </summary>
		protected QueryOperator() { }

		#endregion

		/// <summary></summary>
		public FieldRef FieldRef { get; set; }

		/// <summary></summary>
		[XmlElement("Value", IsNullable = false)]
		public ViewValue Value { get; set; }
	}

	/// <summary></summary>
	[XmlType(AnonymousType = true)]
	public partial class FieldRef {
		/// <summary></summary>
		[XmlAttribute]
		public string Name { get; set; }
	}

	/// <summary></summary>
	[XmlType(AnonymousType = true)]
	public partial class ViewValue {
		/// <summary></summary>
		[XmlAttribute]
		public string Type { get; set; }

		/// <summary></summary>
		[XmlText]
		public string Value { get; set; }
	}

	/// <summary></summary>
	[XmlType(AnonymousType = true)]
	public partial class QueryOperatorEq : QueryOperator { }

	/// <summary></summary>
	[XmlType(AnonymousType = true)]
	public partial class QueryOperatorNeq : QueryOperator { }

	/// <summary></summary>
	[XmlType(AnonymousType = true)]
	public partial class QueryOperatorGt : QueryOperator { }

	/// <summary></summary>
	[XmlType(AnonymousType = true)]
	public partial class QueryOperatorLt : QueryOperator { }

	/// <summary></summary>
	[XmlType(AnonymousType = true)]
	public partial class QueryOperatorGeq : QueryOperator { }

	/// <summary></summary>
	[XmlType(AnonymousType = true)]
	public partial class QueryOperatorLeq : QueryOperator { }

	/// <summary></summary>
	[XmlType(AnonymousType = true)]
	public partial class QueryOperatorIsNull : QueryOperator { }

	/// <summary></summary>
	[XmlType(AnonymousType = true)]
	public partial class QueryOperatorIsNotNull : QueryOperator { }

	/// <summary></summary>
	[XmlType(AnonymousType = true)]
	public partial class QueryOperatorContains : QueryOperator { }

	/// <summary></summary>
	[XmlType(AnonymousType = true)]
	public partial class QueryOperatorBeginsWith : QueryOperator { }

	/// <summary></summary>
	[XmlType(AnonymousType = true)]
	public partial class QueryOperatorDateRangesOverlap : QueryOperator { }
}
