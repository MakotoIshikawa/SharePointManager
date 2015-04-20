using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharePointManager.Manager.Lists;
using SharePointManager.Manager.Lists.Xml;
using ExtensionsLibrary.Extensions;
using SharePointManager.Manager.Extensions;
using System.Linq.Expressions;

namespace UnitTestProject {
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
		[TestCategory("変換")]
		public void 行データ変換() {
			var title = "カスタムリスト-テスト";

			var m = new ListManager(url, username, password, title);
			m.ThrowException += (sender, e) => {
				throw new Exception(e.ErrorMessage + " : " + e.ServerStackTrace);
			};

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

			var ns = m.GetInternalNames(row.Keys).ToList();
			var items = m.ConvertRowData(row);

			var expected = row.Count;
			var actual = items.Count;
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

			var retrievals = new Expression<Func<ListItem, object>>[] {
				i => i.Id
				, i => i.AttachmentFiles.Include(
					f => f.FileName
					, f => f.ServerRelativeUrl
				)
			};
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
		public void XML文字列確認() {//XmlField
			var xml = new XmlField() {
				DisplayName = "テキスト",
				Type = "Text",
			};

			var ret = xml.ToString();

			Assert.IsNotNull(ret);
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
