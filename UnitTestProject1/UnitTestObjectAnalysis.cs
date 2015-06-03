using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectAnalysisProject.Extensions;
using SharePointManager.Manager.Lists.Xml;
using ExtensionsLibrary.Extensions;
using System.Reflection;
using System.Data;

namespace UnitTestProject {
	[TestClass]
	public class UnitTestObjectAnalysis {
		[TestMethod]
		[Owner("オブジェクト分析")]
		[TestCategory("情報取得")]
		public void メンバー取得() {
			var data = new {
				Num = 1u,
				DisplayName = "テキスト",
				Type = "Text",
			};

			var member1 = data.GetMembers()
				.Select(m => new { Name = m.Item1, Type = m.Item2, Value = m.Item3 });

			var fields = data.GetFields();
			var properties = data.GetProperties();
			var member2 =
				fields.Select(f => new { Name = f.Name, Type = f.FieldType })
				.Union(properties.Select(p => new { Name = p.Name, Type = p.PropertyType }));

			Assert.IsTrue(member1.Select(m => m.Name).SequenceEqual(member2.Select(m => m.Name)));
			Assert.IsTrue(member1.Select(m => m.Type).SequenceEqual(member2.Select(m => m.Type)));
		}

		[TestMethod]
		[Owner("オブジェクト分析")]
		[TestCategory("情報取得")]
		public void 型情報取得() {
			IEnumerable<string> val1 = new List<string>();
			var val2 = new List<string>();

			var t1 = val1.GetType();
			var t2 = val2.GetType();

			Assert.AreEqual(t1, t2);
		}

		[TestMethod]
		[Owner("オブジェクト分析")]
		[TestCategory("情報取得")]
		public void コレクション情報取得() {
			var mes1 = typeof(DataTable).GetIndexers();
			var mes2 = typeof(DataRow).GetIndexers();

			var source1 = new List<string>();
			source1.Add("内容1");
			source1.Add("内容2");
			source1.Add("内容3");
			source1.Add("内容4");
			source1.Add("内容5");
			source1.Add("内容6");

			var tbl1 = source1.ToDataTable();

			Assert.IsNotNull(tbl1);

			var source = new List<int>();
			var i = 0;
			source.Add(i++);
			source.Add(i++);
			source.Add(i++);
			source.Add(i++);
			source.Add(i++);
			source.Add(i++);

			var tbl2 = source.ToDataTable();

			Assert.IsNotNull(tbl2);
		}
	}
}
