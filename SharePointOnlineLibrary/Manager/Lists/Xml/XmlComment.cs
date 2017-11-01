using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using ExtensionsLibrary.Extensions;

namespace SharePointOnlineLibrary.Manager.Lists.Xml {
	/// <summary>コメントリストのXMLデータ形式です。</summary>
	[XmlType(AnonymousType = true)]
	[XmlRoot("Comments", Namespace = "", IsNullable = false)]
	public partial class XmlComments {
		#region プロパティ

		/// <summary>コメントリスト</summary>
		[XmlElement("Comment")]
		public List<XmlCommentsItem> Items { get; set; }

		/// <summary>
		/// コメントのログ情報の文字列を取得します。
		/// </summary>
		[XmlIgnore]
		public string Log {
			get {
				return this.Items
					.Select(i => i.ToString())
					.Join(Environment.NewLine);
			}
		}

		#endregion

		#region メソッド

		/// <summary>
		/// XmlField を文字列に変換します。
		/// </summary>
		/// <returns>XmlField を表す文字列を返します。</returns>
		public override string ToString() {
			return this.Log;
		}

		#endregion
	}

	/// <summary></summary>
	[XmlType(AnonymousType = true)]
	[XmlRoot("Comment", Namespace = "", IsNullable = false)]
	public partial class XmlCommentsItem {
		#region DateTime

		/// <summary></summary>
		[XmlIgnore]
		public DateTime DateTime { get; set; }

		/// <summary></summary>
		[XmlAttribute("DateTime")]
		public string DateTimeStr {
			get { return this.DateTime.ToString("yyyy/MM/dd HH:mm:ss"); }
			set { this.DateTime = DateTime.Parse(value); }
		}

		#endregion

		/// <summary>ユーザー名</summary>
		[XmlAttribute]
		public string UserName { get; set; }

		/// <summary>カテゴリー</summary>
		[XmlAttribute]
		public string Category { get; set; }

		/// <summary>ショートメッセージ</summary>
		[XmlAttribute]
		public string ShortMsg { get; set; }

		/// <summary>複数行テキスト</summary>
		[XmlText]
		public string Text { get; set; }

		#region メソッド

		/// <summary>
		/// XmlField を文字列に変換します。
		/// </summary>
		/// <returns>XmlField を表す文字列を返します。</returns>
		public override string ToString() {
			var sb = new StringBuilder();
			sb.Append(this.DateTime.ToString("yyyy/MM/dd hh:mm "));
			if (!this.Category.IsEmpty()) {
				sb.AppendFormat("[{0}]", this.Category);
			}
			sb.AppendLine(this.UserName);
			if (!this.ShortMsg.IsEmpty()) {
				sb.AppendLine(this.ShortMsg);
			}
			sb.AppendLine(this.Text);

			return sb.ToString();
		}

		#endregion
	}
}
