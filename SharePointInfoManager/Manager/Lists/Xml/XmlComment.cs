using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using ExtensionsLibrary.Extensions;
using SharePointManager.Interface;

namespace SharePointManager.Manager.Lists.Xml {
	/// <summary></summary>
	[XmlType(AnonymousType = true)]
	[XmlRoot("Comments", Namespace = "", IsNullable = false)]
	public partial class XmlComments {
		/// <summary></summary>
		[XmlElement("Comment")]
		public List<XmlCommentsItem> Items { get; set; }

		#region メソッド

		/// <summary>
		/// XmlField を文字列に変換します。
		/// </summary>
		/// <returns>XmlField を表す文字列を返します。</returns>
		public override string ToString() {
			try {
				var str = this.ToXmlString(false);
				var elmt = XElement.Parse(str);

				return elmt.ToString();
			} catch (Exception) {
				return string.Empty;
			}
		}

		/// <summary>
		/// コメントのログ情報の文字列を取得します。
		/// </summary>
		/// <returns>コメントのログ情報の文字列を返します。</returns>
		public string GetLog() {
			var sb = new StringBuilder();
			foreach (var i in this.Items) {
				sb.AppendFormat("{0} {1}", i.DateTime, i.UserName).AppendLine();
				sb.AppendLine(i.Text).AppendLine();
			}

			return sb.ToString();
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

		/// <summary></summary>
		[XmlAttribute]
		public string UserName { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string Category { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string ShoerMsg { get; set; }

		/// <summary></summary>
		[XmlText]
		public string Text { get; set; }

		#region メソッド

		/// <summary>
		/// XmlField を文字列に変換します。
		/// </summary>
		/// <returns>XmlField を表す文字列を返します。</returns>
		public override string ToString() {
			try {
				var str = this.ToXmlString(false);
				var elmt = XElement.Parse(str);

				return elmt.ToString();
			} catch (Exception) {
				return string.Empty;
			}
		}

		#endregion
	}
}
