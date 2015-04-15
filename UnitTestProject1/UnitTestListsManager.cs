using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharePointInfoManager.Manager.Lists;
using SharePointInfoManager.Manager.Lists.Xml;

namespace UnitTestProject1 {
	[TestClass]
	public class UnitTestListsManager {
#if false
		private string url = @"https://NissayLeasing.sharepoint.com/";
		private string username = @"nlcadmin@NissayLeasing.onmicrosoft.com";
		private string password = @"!QAZ2wsx";
#elif true
		private string url = @"https://kariverification03.sharepoint.com";
		private string username = @"root@KariVerification03.onmicrosoft.com";
		private string password = @"!QAZ2wsx";
#else
		private string url = @"https://mscenter.sharepoint.com/eigyou/demo/";
		private string username = @"aoshima@mscenter.onmicrosoft.com";
		private string password = @"P@ssword!";
#endif

		#region サイトコンテンツ管理

		[TestMethod]
		[Owner("サイトコンテンツ管理")]
		[TestCategory("取得")]
		public void タイトル一覧取得() {
			var m = new ListCollectionManager(url, username, password);
			m.ThrowException += (sender, e) => {
				throw new Exception(e.ErrorMessage + " : " + e.ServerStackTrace);
			};

			var title = "サイトのページ";
			Assert.IsTrue(m.Titles.Any(s => s == title));
		}

		[TestMethod]
		[Owner("サイトコンテンツ管理")]
		[TestCategory("作成")]
		public void リスト作成() {
			var title = "カスタムリスト-日報";

			var m = new ListCollectionManager(url, username, password);
			m.ThrowException += (sender, e) => {
				throw new Exception(e.ErrorMessage + " : " + e.ServerStackTrace);
			};

			var description = "日報システムで作成した日報の一覧です。";
			m.Create(title, description, ListTemplateType.GenericList);

			var ret = m.Titles;
			m.Reload();
			Assert.IsTrue(ret.Any(s => s == title));
		}

		[TestMethod]
		[Owner("サイトコンテンツ管理")]
		[TestCategory("削除")]
		public void リスト削除() {
			var title = "カスタムリスト-テスト";

			var m = new ListCollectionManager(url, username, password);
			m.ThrowException += (sender, e) => {
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

			var m = new ListManager(url, username, password, title, false);
			m.ThrowException += (sender, e) => {
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

			var m = new ListManager(url, username, password, title);
			m.ThrowException += (sender, e) => {
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
		[TestCategory("更新")]
		public void ファイル添付() {
			var title = "カスタムリスト-テスト";

			var m = new ListManager(url, username, password, title);
			m.ThrowException += (sender, e) => {
				throw new Exception(e.ErrorMessage + " : " + e.ServerStackTrace);
			};

			var file = new FileInfo(@"C:\Users\ishikawm\Documents\dummy256MB.file");
#if false
			using (var f = file.Open(FileMode.Open)) {
				m.UpdateListItem(1, i => {
					i.AddAttachmentFile(f);
				});
			}
#else
			m.AddAttachmentFile(1, file);
#endif

			var ret = m.Titles;
			Assert.IsTrue(ret.Any(s => s == title));
		}

		[TestMethod]
		[Owner("リスト管理")]
		[TestCategory("処理")]
		public void 例外処理() {
			var title = "カスタムリスト-例外処理判定";

			var m = new ListManager(url, username, password, title);
			m.ThrowException += (sender, e) => {
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

			var ret = m.Titles;
			Assert.IsTrue(ret.Any(s => s == title));
		}

		[TestMethod]
		[Owner("リスト管理")]
		[TestCategory("参照")]
		public void アイテム参照() {
			//var title = "みんなの掲示板 One for All ,All for One";
			var title = "カスタムリスト-テスト";

			var m = new ListManager(url, username, password, title);
			m.ThrowException += (sender, e) => {
				throw new Exception(e.ErrorMessage + " : " + e.ServerStackTrace);
			};

			var ret1 = m.Load(cn => {
				var list = cn.Web.Lists.GetByTitle(title);
#if true
				var query = CreateAllItemsQuery(
					100
					, xml => {
						xml.AddQueryItems<QueryItemsAnd>(o => {
							//o.AddOperatorItem<QueryOperatorIsNotNull>("Place");
							o.AddOperatorItem<QueryOperatorGeq>("ID", "Number", "1");
							o.AddOperatorItem<QueryOperatorLt>("ID", "Number", "25");
							//o.AddOperatorItem<QueryOperatorEq>("ID", "Number", "1");
						});
						//xml.AddQueryItem<QueryOperatorEq>("Title", "Text", "アイテム1");
					}
					, "ID"
					, "Title"
					, "Field_Text"
					, "Field_Number"
					, "Field_DateTime"
					, "Field_Note"
					, "Place"
				);

				var items = list.GetItems(query);
#else
				var query = CamlQuery.CreateAllItemsQuery(limit);
				var items = list.GetItems(query);
#endif

				return items;
			}).ToDictionary(i => i.Id, i => i.FieldValues);

			var ret2 = m.Load(cn => {
				var list = cn.Web.Lists.GetByTitle(title);
				var query = CamlQuery.CreateAllItemsQuery(100);
				var items = list.GetItems(query);

				return items.Include(
					i => i.Id
					, i => i.AttachmentFiles.Include(
						f => f.FileName
						, f => f.ServerRelativeUrl
					)
				);
			}).ToDictionary(i => i.Id, i => i.AttachmentFiles);

			var ret = m.Titles;
			Assert.IsTrue(ret.Any(s => s == title));
		}

		private static CamlQuery CreateAllItemsQuery(int limit, Action<XmlView> action, params string[] viewFields) {
			var xml = new XmlView(limit, viewFields);

			if (action != null) {
				action(xml);
			}

			var query = new CamlQuery() {
				ViewXml = xml.ToString(),
			};

			return query;
		}

		[TestMethod]
		[Owner("リスト管理")]
		[TestCategory("更新")]
		public void XML文字列確認() {//XmlField
			var xml = new XmlField() {
				DisplayName = "テキスト",
				Type = "Text",
			};

			var ret = xml.ToString();

			Assert.IsNotNull(ret);
		}
	}
}
