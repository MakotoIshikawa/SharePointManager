using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharePointManager.Manager;

namespace UnitTestProject {
	[TestClass]
	public class UnitTestGroupManager {
#if false
		private string url = @"https://NissayLeasing.sharepoint.com/";
		private string username = @"nlcadmin@NissayLeasing.onmicrosoft.com";
		private string password = @"!QAZ2wsx";
#else
		private const int _ver = 12;
		private string _rootUrl = String.Format(@"https://kariverification{0:00}.sharepoint.com", _ver);
		private string _user = String.Format(@"root@KariVerification{0:00}.onmicrosoft.com", _ver);
		private string _password = @"!QAZ2wsx";
#endif

		[TestMethod]
		[Owner("サイトグループ管理")]
		[TestCategory("正常系")]
		[Priority(1)]
		public void サイトグループ情報取得() {
			var m = new GroupManager(this._rootUrl, this._user, this._password);

			Assert.IsTrue(m.SiteGroups.Any());
		}

		[TestMethod]
		[Owner("サイトグループ管理")]
		[TestCategory("異常系")]
		[ExpectedException(typeof(Exception))]	// 例外が発生すれば成功
		[Priority(2)]
		public void 例外発生パターン() {
		}
	}
}
