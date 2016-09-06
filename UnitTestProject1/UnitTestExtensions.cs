using System;
using System.IO;
using System.Text;
using CommonFeaturesLibrary.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharePointManager.Manager.Extensions;
using SharePointManager.Manager.Lists.Xml;

namespace UnitTestProject {
	[TestClass]
	public class UnitTestExtensions {
		[TestMethod]
		[Owner("ファイル管理")]
		[TestCategory("更新")]
		public void ファイル更新() {
			// TODO: 更新処理未実装
			throw new NotImplementedException("更新処理未実装");
		}

		[TestMethod]
		[Owner("その他")]
		[TestCategory("変更")]
		public void ファイル名変更() {
			var file = new FileInfo(@"C:\Users\ishikawm\Documents\L.txt");
			var fname0 = file.GetVersionName(0);
			var fname1 = file.GetVersionName(1);

			Assert.IsNotNull(fname0);
		}

		[TestMethod]
		[Owner("その他")]
		[TestCategory("確認")]
		public void XML文字列確認() {
			var xml = new XmlField() {
				DisplayName = "テキスト",
				Type = "Text",
			};

			var sb = new StringBuilder();
			sb.Append(@"<Comment DateTime=""2015-04-23 12:30:00"" UserName=""ユーザー1"">コメント1行目です。
コメント2行目です。
コメント3行目です。</Comment>");
			sb.Append(@"<Comment DateTime=""2015/04/15 21:55"" UserName=""さくら情報ﾃｽﾄ"" Category=""指示"" ShortMsg=""了解"">コメント１
コメント２
コメント３</Comment>");

			var content = sb.ToString();

			var str1 = content.ConvertXmlString<XmlComments>(c => c.ToString());

			var expected = string.Empty;
			var actual = str1;
			Assert.AreNotEqual(expected, actual);
		}
	}
}
