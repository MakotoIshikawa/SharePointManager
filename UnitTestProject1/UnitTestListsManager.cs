using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ExtensionsLibrary.Extensions;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectAnalysisProject.Extensions;
using SharePointManager.Manager.Extensions;
using SharePointManager.Manager.Lists;
using SharePointManager.Manager.Lists.Xml;

namespace UnitTestProject {
	[TestClass]
	public class UnitTestListsManager {
#if false
		private string _url = @"https://NissayLeasing.sharepoint.com/";
		private string _user = @"nlcadmin@NissayLeasing.onmicrosoft.com";
		private string _password = @"!QAZ2wsx";
#elif false
		private string _url = @"https://kariverification03.sharepoint.com";
		private string _user = @"root@KariVerification03.onmicrosoft.com";
		private string _password = @"!QAZ2wsx";
#elif true
		private string _url = @"https://mur365.sharepoint.com/";
		private string _user = @"fuji-s@mur365.onmicrosoft.com";
		private string _password = @"Muromachi_Fsi!";
#else
		private string _url = @"https://mscenter.sharepoint.com/eigyou/demo/";
		private string _user = @"aoshima@mscenter.onmicrosoft.com";
		private string _password = @"P@ssword!";
#endif

		#region サイトコンテンツ管理

		[TestMethod]
		[Owner("サイトコンテンツ管理")]
		[TestCategory("取得")]
		public void タイトル一覧取得() {
			var m = new ListCollectionManager(_url, _user, _password);
			m.ThrowSharePointException += (sender, e) => {
				throw new Exception(e.ErrorMessage + " : " + e.ServerStackTrace);
			};

			var title = "サイトのページ";
			Assert.IsTrue(m.Titles.Any(s => s == title));
		}

		[TestMethod]
		[Owner("サイトコンテンツ管理")]
		[TestCategory("作成")]
		public void リスト作成() {
			var title = "カスタムリスト-過去日報";
			var url = "CustomListOldDailyReport";

			var m = new ListCollectionManager(_url, _user, _password);
			m.ThrowSharePointException += (sender, e) => {
				throw new Exception(e.ErrorMessage + " : " + e.ServerStackTrace);
			};

			var description = "過去に作成した日報の一覧です。";
			m.Create(title, url, description, ListTemplateType.GenericList);

			var ret = m.Titles;
			m.Reload();
			Assert.IsTrue(ret.Any(s => s == title));
		}

		[TestMethod]
		[Owner("サイトコンテンツ管理")]
		[TestCategory("削除")]
		public void リスト削除() {
			var title = "カスタムリスト-テスト";

			var m = new ListCollectionManager(_url, _user, _password);
			m.ThrowSharePointException += (sender, e) => {
				throw new Exception(e.ErrorMessage + " : " + e.ServerStackTrace);
			};

			m.DeleteByTitle(title);

			var ret = m.Titles;
			m.Reload();
			Assert.IsFalse(ret.Any(s => s == title));
		}

		#endregion

		[TestMethod]
		[Owner("リスト管理")]
		[TestCategory("追加")]
		public void フィールド拡張() {
			var title = "カスタムリスト-テスト";

			var m = new ListManager(_url, _user, _password, title, false);
			m.ThrowSharePointException += (sender, e) => {
				throw new Exception(e.ErrorMessage + " : " + e.ServerStackTrace);
			};

			m.AddField<FieldUrl>("UrlPath", "サイトURL");

			var ret = m.GetField<FieldUrl>("UrlPath");

			m.Reload();
			Assert.IsNotNull(ret);
		}

		[TestMethod]
		[Owner("リスト管理")]
		[TestCategory("追加")]
		public void アイテム追加() {
			var title = "カスタムリスト-テスト";

			var m = new ListManager(_url, _user, _password, title);
			m.ThrowSharePointException += (sender, e) => {
				throw new Exception(e.ErrorMessage + " : " + e.ServerStackTrace);
			};

			var num = m.ItemCount + 1;
			var row = new Dictionary<string, object>();
			row["タイトル"] = "Item" + num.ToString("000");
			row["Field_Text"] = num.ToString();
			row["Field_Number"] = num;
			row["Field_DateTime"] = DateTime.Now.ToString();
			row["Field_Note"] = @"<div>1行目</div><div>2行目</div><div>3行目</div>";
			row["場所"] = "場所" + num.ToString("000");
			row["サイトURL"] = @"/Shared Documents/ツール実施手順.txt";

			m.AddListItem(row);
#if false
			var file = new FileInfo(@"C:\Users\ishikawm\Documents\Works\link.txt");
			using (var f = file.Open(FileMode.Open)) {
				m.AddListItem(row, i => {
					i.AddAttachmentFile(f);
				});
			}
#endif
			m.Reload();

			var expected = num;
			var actual = m.ItemCount;
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[Owner("リスト管理")]
		[TestCategory("変換")]
		public void 行データ変換() {
			var title = "カスタムリスト-テスト";

			var m = new ListManager(_url, _user, _password, title);
			var fs = m.Fields;

			var num = m.ItemCount + 1;
			var row = new Dictionary<string, object>();
			row["Title"] = "見出し" + num.ToString("000");
			row["タイトル"] = "Item" + num.ToString("000");
			row["Field_Text"] = num.ToString();
			row["Field_Number"] = num;
			row["Field_DateTime"] = DateTime.Now;
			row["Field_Note"] = @"<div>1行目</div><div>2行目</div><div>3行目</div>";
			row["場所"] = "場所" + num.ToString("000");
			row["サイトURL"] = @"/Shared Documents/ツール実施手順.txt";
			row["Created"] = DateTime.Now;
			row["登録日時"] = DateTime.MinValue;

			var items = row.ConvertRowData(fs);

			var expected = row.Count;
			var actual = items.Count;
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[Owner("リスト管理")]
		[TestCategory("更新")]
		public void ファイル添付() {
			var title = "カスタムリスト-テスト";

			var m = new ListManager(_url, _user, _password, title);
			m.ThrowSharePointException += (sender, e) => {
				throw new Exception(e.ErrorMessage + " : " + e.ServerStackTrace);
			};

#if false
			var file = new FileInfo(@"C:\Users\ishikawm\Documents\dummy256MB.file");
			using (var f = file.Open(FileMode.Open)) {
				m.UpdateListItem(1, i => {
					i.AddAttachmentFile(f);
				});
			}
#else
			var file1 = new FileInfo(@"C:\Users\ishikawm\Documents\L.txt");
			var file2 = new FileInfo(@"C:\Users\ishikawm\Documents\R.txt");
			//var file3 = new FileInfo(@"C:\Users\ishikawm\Documents\dummy200MB.file");
			m.AddAttachmentFile(28, new[] { file1, file2 });
#endif
			//TODO: 検証内容検討
			var ret = m.ListName;
			Assert.AreEqual(ret, title);
		}

		[TestMethod]
		[Owner("リスト管理")]
		[TestCategory("処理")]
		public void 例外処理() {
			var title = "カスタムリスト-例外処理判定";

			var m = new ListManager(_url, _user, _password, title);
			m.ThrowSharePointException += (sender, e) => {
				var msg = e.ErrorMessage + " : " + e.ServerStackTrace;
				throw new Exception(msg);
			};

			m.TryExecute(cn => {// Try
				var l = cn.Web.Lists.GetByTitle(title);
				l.Description = "既存のリストです。";
				l.Update();
			}, cn => {// Catch
				var listCreateInfo = new ListCreationInformation() {
					Title = title,
					Description = "見つからないので作成しました。",
					TemplateType = (int)ListTemplateType.GenericList,
				};
				var l = cn.Web.Lists.Add(listCreateInfo);
			}, cn => {// Finally
				var l = cn.Web.Lists.GetByTitle(title);
				l.EnableFolderCreation = false;
				l.Update();
			});

			//TODO: 検証内容検討
			var ret = m.ListName;
			Assert.AreEqual(ret, title);
		}

		[TestMethod]
		[Owner("リスト管理")]
		[TestCategory("参照")]
		public void アイテム参照() {
			//var title = "みんなの掲示板 One for All ,All for One";
			var title = "カスタムリスト-テスト";

			var m = new ListManager(_url, _user, _password, title);
			m.ThrowSharePointException += (sender, e) => {
				throw new Exception(e.ErrorMessage + " : " + e.ServerStackTrace);
			};

			{
				var row = m.GetAllItemsValues(title, 1000);

				var expected = "お試し";
				var actual = row[1]["Title"];
				Assert.AreEqual(expected, actual);
			}
			{
				int limit = 100;
				var row = m.GetItemsValues(title, xml => {
					xml.AddQueryItems<QueryItemsAnd>(o => {
						//o.AddOperatorItem<QueryOperatorIsNotNull>("Place");
						o.AddOperatorItem<QueryOperatorGeq>("ID", "Number", "1");
						o.AddOperatorItem<QueryOperatorLt>("ID", "Number", "25");
						//o.AddOperatorItem<QueryOperatorEq>("ID", "Number", "1");
					});
					//xml.AddQueryItem<QueryOperatorEq>("Title", "Text", "アイテム1");
				}, limit, "ID", "Title", "Field_Text", "Field_Number", "Field_DateTime", "Field_Note", "Place");
				var expected = "お試し";
				var actual = row[1]["Title"];
				Assert.AreEqual(expected, actual);
			}
			{
				var key = "UniqueKey";
				var val = "201504161100";
				var id = m.GetID(key, val);

				var expected = 1;
				var actual = id;
				Assert.AreEqual(expected, actual);
			}
			{
				var id = 1;
				var index = 0;
				var ret1 = m.GetAttachmentFiles(title, id).ToList();
				var ret2 = m.GetAttachmentFilesDictionary(title);
				var expected = ret1[index].FileName;
				var actual = ret2[id][index].FileName;
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod]
		[Owner("リスト管理")]
		[TestCategory("更新")]
		public void XML文字列確認() {
			var xml = new XmlField() {
				DisplayName = "テキスト",
				Type = "Text",
			};

			var sb = new StringBuilder();
			sb.Append(@"<Comment DateTime=""2015-04-23 12:30:00"" UserName=""ユーザー1"">コメント1行目です。
コメント2行目です。
コメント3行目です。</Comment>");
			sb.Append(@"<Comment DateTime=""2015/04/23 13:00"" UserName=""ユーザー1"">
コメント1行目です。
コメント2行目です。
コメント3行目です。
</Comment>");

			var content = sb.ToString();

			var str1 = content.ConvertXmlString<XmlComments>(c => c.GetLog());

			var expected = string.Empty;
			var actual = str1;
			Assert.AreNotEqual(expected, actual);
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
	}
}
