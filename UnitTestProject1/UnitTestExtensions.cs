using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CommonFeaturesLibrary.Extensions;
using ExtensionsLibrary.Extensions;
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

			var expected = @"C:\Users\ishikawm\Documents\L (1).txt";
			var actual = fname1;
			Assert.AreNotEqual(expected, actual);
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

		[TestMethod]
		[Owner("その他")]
		[TestCategory("確認")]
		public void ディレクトリ内ファイル取得() {
			var fullPath = $@"\\10.25.250.70\fileserver\H25\557_MS事業部\010_事業部共有\ＭＳ部資産\Personal\01_社員(L以上)\karikomi(刈込)\20160201_イデア\70_User\yamada\20170313 移行ツール用htmlファイル\ＳＣＭ規程管理ＤＢ\documents";
			var dir = new DirectoryInfo(fullPath);
			var files = dir.GetFileInfos(true, ".htm", ".html");

			Assert.IsTrue(files.Any());
			Assert.IsFalse(files.Any(f => f.Extension == ".htm"));
			Assert.IsFalse(files.Any(f => f.Extension == ".html"));
		}

		[TestMethod]
		[Owner("その他")]
		[TestCategory("判定")]
		public void ファイル種別判定() {
			Assert.IsTrue(new FileInfo("aaaaa.JPG").IsSharePointIcon());
			Assert.IsTrue(new FileInfo("aaaaa.jpg").IsSharePointIcon());
			Assert.IsTrue(new FileInfo("aaaaa.JPEG").IsSharePointIcon());
			Assert.IsTrue(new FileInfo("aaaaa.jpeg").IsSharePointIcon());
			Assert.IsTrue(new FileInfo("aaaaa.xls").IsSharePointIcon());
			Assert.IsTrue(new FileInfo("aaaaa.doc").IsSharePointIcon());
			Assert.IsTrue(new FileInfo("aaaaa.xlsm").IsSharePointIcon());
			Assert.IsTrue(new FileInfo("aaaaa.docm").IsSharePointIcon());
			Assert.IsTrue(new FileInfo("aaaaa.htm").IsSharePointIcon());
			Assert.IsFalse(new FileInfo("aaaaa.html").IsSharePointIcon());
		}
	}
}
