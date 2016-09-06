using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommonFeaturesLibrary.Extensions;
using ExtensionsLibrary.Extensions;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectAnalysisProject.Extensions;
using SharePointManager.Manager.Extensions;
using SharePointManager.Manager.Lists;
using SharePointManager.Manager.Lists.Xml;
using SP = Microsoft.SharePoint.Client;

namespace UnitTestProject {
	[TestClass]
	public class UnitTestListsManager {
#if false
		private string _rootUrl = String.Format(@"https://devorix.sharepoint.com");
		private string _user = String.Format(@"daisuke_karikomi@devorix.onmicrosoft.com");
		private string _password = @"!QAZ2wsx";
		private ListManager _mng = null;
#else
		private const int _ver = 12;
		private static string _rootUrl = String.Format(@"https://kariverification{0:00}.sharepoint.com", _ver);
		private static string _user = String.Format(@"root@KariVerification{0:00}.onmicrosoft.com", _ver);
		private static string _password = @"!QAZ2wsx";
#endif
		private static string _siteUrl = _rootUrl + @"/sites/IDEA";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public UnitTestListsManager() {
		}

		#region メソッド

		private ListManager CreateListManager(string title) {
			var m = new ListManager(_siteUrl, _user, _password, title.Trim());
			m.ThrowException += (sender, e) => {
				throw e.Value;
			};
			m.ThrowSharePointException += (sender, e) => {
				throw new Exception(e.ErrorMessage + " : " + e.ServerStackTrace);
			};
			return m;
		}

		#endregion

		#region サイトコンテンツ管理

		[TestMethod]
		[Owner("サイトコンテンツ管理")]
		[TestCategory("取得")]
		public void タイトル一覧取得() {
			var m = new ListCollectionManager(_rootUrl, _user, _password);
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

			var m = new ListCollectionManager(_rootUrl, _user, _password);
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

			var m = new ListCollectionManager(_rootUrl, _user, _password);
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
			var listName = "CustomList";
			var m = CreateListManager(listName);

			m.AddField<FieldUrl>("UrlPath", "サイトURL");

			var ret = m.GetField<FieldUrl>("UrlPath");

			m.Reload();
			Assert.IsNotNull(ret);
		}

		[TestMethod]
		[Owner("リスト管理")]
		[TestCategory("追加")]
		public void アイテム追加() {
			var listName = "CustomList";
			var m = CreateListManager(listName);

			var num = m.ItemCount + 1;
			var row = new Dictionary<string, object>();
			row["タイトル"] = "Item" + num.ToString("000");
			row["本文"] = string.Format("本文です : {0:000}", num);

			m.AddListItem(row);

			m.Reload();

			var expected = num;
			var actual = m.ItemCount;
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[Owner("リスト管理")]
		[TestCategory("追加")]
		public void ディスカッション項目追加() {
			var listName = "DiscussionBBS";
			var m = CreateListManager(listName);

			var num = m.ItemCount;
			num++;
			var titel = string.Format("項目{0:000}", num);
			m.AddListItem(new Dictionary<string, object> {
				{ "件名", string.Format("討論 {0:000}", num) },
				{ "本文", string.Format("討論の内容です。 : {0}", DateTime.Now) },
				{ "質問", true },
			}, isFolder: true);
			m.Reload();

			{// アイテム数確認
				var expected = num;
				var actual = m.ItemCount;
				Assert.AreEqual(expected, actual);
			}

			num++;
			m.AddListItem(new Dictionary<string, object> {
				{ "件名", string.Format("返信 {0:000}", num) },
				{ "本文", string.Format("討論への返信です。 : {0}", DateTime.Now) },
			}, titel);

			m.Reload();

			{// アイテム数確認
				var expected = num;
				var actual = m.ItemCount;
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod]
		[Owner("リスト管理")]
		[TestCategory("追加")]
		public void 項目追加テスト() {
			var listName = "DiscussionBBS";
			var m = CreateListManager(listName);

			var num = m.ItemCount + 1;
			m.AddListItem(new Dictionary<string, object> {
				//{ "Title", string.Format("返信 {0:000}", num) },
				{ "Title", "返信" },
				{ "本文", string.Format("討論への返信です。 : {0}", DateTime.Now) },
				//{ "ContentTypeId", new Guid("CBEE9C472F3D864B9FFDCE275891BF73") },
			}, "フォルダ008");

			m.Reload();

			var expected = num;
			var actual = m.ItemCount;
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[Owner("リスト管理")]
		[TestCategory("追加")]
		public void フォルダ追加() {
			var listName = "CustomList";
			var m = CreateListManager(listName);
			m.FolderName = "フォルダ008/フォルダ012";

			var num = m.ItemCount;
			var count = 3;
			m.AddFolders(Enumerable.Range(num + 1, count)
				.Select(r => string.Format("フォルダ{0:000}", r)));

			m.Reload();

			var expected = num + count;
			var actual = m.ItemCount;
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[Owner("リスト管理")]
		[TestCategory("変換")]
		public void 行データ変換() {
			var listName = "CustomList";
			var m = CreateListManager(listName);

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
		[TestCategory("添付ファイル")]
		public void 添付ファイル追加() {
			var listName = "CustomList";
			var m = CreateListManager(listName);

			var files = (new[] {
				@"C:\Users\ishikawm\Documents\L.txt",
				@"C:\Users\ishikawm\Documents\R.txt",
			}).Select(s => new FileInfo(s)).ToList();

			var id = 1;
			m.AddAttachmentFile(id, files);

			var attachmentFiles = m.GetAttachmentFiles(id).ToList();

			{// ファイル数確認
				var expected = files.Count;
				var actual = attachmentFiles.Count;
				Assert.AreEqual(expected, actual);
			}
			{// ファイル名比較
				var condition = files.Select(f => f.Name)
					.SequenceEqual(attachmentFiles.Select(f => f.FileName));
				Assert.IsTrue(condition);
			}
		}

		[TestMethod]
		[Owner("リスト管理")]
		[TestCategory("処理")]
		public void 例外処理() {
			var title = "カスタムリスト-例外処理判定";
			var url = _rootUrl + @"/sites/test_template";

			var m = new ListManager(url, _user, _password, title);
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

			// TODO: 検証内容検討
			var ret = m.ListName;
			Assert.AreEqual(ret, title);
		}

		[TestMethod]
		[Owner("リスト管理")]
		[TestCategory("参照")]
		public void アイテム参照() {
			var listName = "CustomList";
			var m = CreateListManager(listName);

			{
				var rows = m.GetAllItemsValues(1000);

				var expected = "Title00609";
				var actual = rows.First(row => Convert.ToInt32(row["ID"]) == 4)["Title"];
				Assert.AreEqual(expected, actual);
			}
			{// ID
				int limit = 100;
				var rows = m.GetItemsValues(xml => 
					xml.AddQueryItems<QueryItemsAnd>(a => {
						a.AddQuery<QueryOperatorGeq>("ID", 1);
						a.AddQuery<QueryOperatorLt>("ID", 25);
						//a.AddQuery<QueryOperatorIsNotNull>("Place");
					})
				, limit, "ID", "Title");
				var expected = "Title00609";
				var actual = rows.First(row => Convert.ToInt32(row["ID"]) == 4)["Title"];
				Assert.AreEqual(expected, actual);
			}
			{// 承認状況
				int limit = 100;
				var rows = m.GetItemsValues(xml =>
					xml.AddQuery<QueryOperatorNeq>("_ModerationStatus", "承認済み")
				, limit, "ID", "Title");
				var expected = "Title00615";
				var actual = rows.First(row => Convert.ToInt32(row["ID"]) == 10)["Title"];
				Assert.AreEqual(expected, actual);
			}
			{// 更新日
				int limit = 5000;
				var rows = m.GetItemsValues(xml =>
					xml.AddQueryItems<QueryItemsAnd>(a => {
						a.AddQuery<QueryOperatorGeq>("Modified", new DateTime(2016, 04, 05));
						a.AddQuery<QueryOperatorLt>("Modified", new DateTime(2016, 04, 30));
					})
				, limit);
				var expected = 3;
				var actual = rows.Count();
				//Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod]
		[Owner("リスト管理")]
		[TestCategory("参照")]
		public void アイテムID参照() {
			var listName = "CustomList";
			var m = CreateListManager(listName);

			var key = "DocUniID";
			{
				var val = "10000001";
				var id = m.GetID(key, val);

				var expected = 1;
				var actual = id;
				Assert.AreEqual(expected, actual);
			}
			{
				var val = "10000008";
				var id = m.GetID(key, val);

				var expected = 8;
				var actual = id;
				Assert.AreEqual(expected, actual);
			}
			{
				var val = "10000012";
				var id = m.GetID(key, val);

				var expected = 12;
				var actual = id;
				Assert.AreEqual(expected, actual);
			}
			{
				var val = "10000031";
				var id = m.GetID(key, val);

				var expected = 31;
				var actual = id;
				Assert.AreEqual(expected, actual);
			}
			{
				var val = "10000035";
				var id = m.GetID(key, val);

				var expected = 35;
				var actual = id;
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod]
		[Owner("リスト管理")]
		[TestCategory("参照")]
		public void 予定表参照() {
			var listName = "CustomList";
			var m = CreateListManager(listName);

			{
				var rows = m.GetAllItemsValues(1000);

				var expected = "・建物物価の灯油価格の連絡";
				var actual = rows.First(row => Convert.ToInt32(row["ID"]) == 4)["Title"];
				Assert.AreEqual(expected, actual);
			}
			{// 終了時刻
				int limit = 5000;
				var rows = m.GetItemsValues(xml =>
					xml.AddQueryItems<QueryItemsAnd>(a => {
						a.AddQuery<QueryOperatorGeq>("EndDate", new DateTime(2016, 4, 1, 0, 0, 0));
						a.AddQuery<QueryOperatorLt>("EndDate", new DateTime(2016, 5, 1, 0, 0, 0));
					})
				, limit);
				var expected = 10;
				var actual = rows.Count();
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod]
		[Owner("リスト管理")]
		[TestCategory("参照")]
		public void アイテム添付ファイル参照() {
			var listName = "CustomList";
			var m = CreateListManager(listName);

			var id = 1;
			var index = 0;
			var ret1 = m.GetAttachmentFiles(id).ToList();
			var ret2 = m.GetAttachmentFilesDictionary();
			var expected = ret1[index].FileName;
			var actual = ret2[id][index].FileName;
			Assert.AreEqual(expected, actual);
		}
	}
}
