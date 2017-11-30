using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SharePointAddItem.Model {
	/// <summary>
	/// バッチ実行結果
	/// </summary>
	[Serializable()]
	[DesignerCategory("code")]
	[XmlType(AnonymousType = true)]
	[XmlRoot(Namespace = "", IsNullable = false)]
	public partial class Results {
		/// <summary>
		/// 処理結果のコレクション
		/// </summary>
		[XmlElement("Result")]
		public ResultsResult[] Result { get; set; }
	}

	/// <summary>
	/// 処理結果
	/// </summary>
	[Serializable()]
	[DesignerCategory("code")]
	[XmlType(AnonymousType = true)]
	public partial class ResultsResult {
		/// <summary>
		/// ID
		/// </summary>
		[XmlElement(IsNullable = false)]
		public ushort ID { get; set; }

		/// <summary>
		/// エラーテキスト
		/// </summary>
		[XmlElement(IsNullable = false)]
		public string ErrorText { get; set; }

		/// <summary>
		/// メソッドID
		/// </summary>
		[XmlAttribute("ID")]
		public ushort MethodID { get; set; }

		/// <summary>
		/// 終了コード
		/// </summary>
		[XmlAttribute]
		public byte Code { get; set; }

		/// <summary>
		/// 成功しているかどうかを取得します。
		/// </summary>
		[XmlIgnore]
		public bool Success => Code == 0;
	}
}
