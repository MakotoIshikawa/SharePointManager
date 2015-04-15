using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharePointInfoManager.Manager;

namespace UnitTestProject {
	[TestClass]
	public class UnitTestGroupManager {
#if false
		private string url = @"https://NissayLeasing.sharepoint.com/";
		private string username = @"nlcadmin@NissayLeasing.onmicrosoft.com";
		private string password = @"!QAZ2wsx";
#else
		private string url = @"https://kariverification03.sharepoint.com";
		private string username = @"root@KariVerification03.onmicrosoft.com";
		private string password = @"!QAZ2wsx";
#endif

		[TestMethod]
		[Owner("サイトグループ管理")]
		[TestCategory("正常系")]
		[Priority(1)]
		public void サイトグループ情報取得() {
			var m = new GroupManager(url, username, password);

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
