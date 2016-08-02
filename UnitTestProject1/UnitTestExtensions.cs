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
using SharePointManager.Extensions;

namespace UnitTestProject {
	[TestClass]
	public class UnitTestExtensions {
		[TestMethod]
		[Owner("ファイル管理")]
		[TestCategory("更新")]
		public void ファイル更新() {
			var file = new FileInfo(@"C:\csv\test101.csv");
			file.SaveCsvData(t => {
				t.Rows[1][3] = "変更";
			});
		}
	}
}
