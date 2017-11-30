using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ExtensionsLibrary.Extensions;
using Microsoft.SharePoint;
using ObjectAnalysisProject.Extensions;
using SharePointAddItem.Model;
using SharePointLibrary.Extensions;
using SharepointServerInsertApp.Forms.Primitives;
using SharepointServerInsertApp.Properties;
using XmlLibrary.Extensions;

namespace SharepointServerInsertApp {
	/// <summary>
	/// フォーム
	/// </summary>
	public partial class FormServerInsert : FormFetchFileDataByServer {
		#region フィールド

		#endregion

		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public FormServerInsert() : base() {
			this.InitializeComponent();

			this.Url = Settings.Default.URL;
			this.ListName = Settings.Default.ListName;
			this.LogRowLimit = Settings.Default.LogRowLimit;
		}

		#endregion

		#region プロパティ

		#endregion

		#region メソッド

		#region 準備

		private void InitializeComponent() {
			this.Name = nameof(FormServerInsert);
			this.Text = "リストアイテム追加";
		}

		#endregion

		#region 実行

		/// <summary>
		/// 実行処理のメインです。
		/// </summary>
		protected override void RunCore() {
			var tbl = this.TableData;
			var items = tbl.GetItems();
			AddItems(items);
		}

		#endregion

		#region 追加

		/// <summary>
		/// リストにアイテムを追加します。
		/// </summary>
		/// <param name="items"></param>
		public virtual void AddItems(IEnumerable<Dictionary<string, object>> items) {
			var requestUrl = this.Url;
			var listInternalName = this.ListName;

			var cnt = AddItems(requestUrl, listInternalName, items);

			this.WriteLineMessage($"{listInternalName} リストにアイテムを追加しました。: {cnt} 件");
		}

		/// <summary>
		/// リストにアイテムを追加します。
		/// </summary>
		/// <param name="requestUrl">サイトURL</param>
		/// <param name="listInternalName">リスト内部名</param>
		/// <param name="items">追加するアイテムのコレクション</param>
		/// <returns>追加した件数を返します。</returns>
		protected virtual int AddItems(string requestUrl, string listInternalName, IEnumerable<Dictionary<string, object>> items) {
			var sb = new StringBuilder();
			try {
				using (var site = new SPSite(requestUrl))
				using (var web = site.OpenWeb()) {
					var list = web.GetListByInternalName(listInternalName);
					var results = list.AddItemsByBatch(items.ToArray()).ToList();
					results.ForEach(r => sb.AppendLine(r));
					var ret = (
						from r in results
						select r.DeserializeFromXml<Results>() into rr
						from r in rr.Result
						select r
					);
					var cnt = ret?.Count(r => r.Success);

					return cnt.HasValue ? cnt.Value : 0;
				}
			} catch (ArgumentException ex) when (ex.Source == nameof(XmlLibrary)) {
				var result = sb.ToString();
				result = result.OmitGreaterThan(80);
				throw new ApplicationException($"追加処理を終了しました。Results={result}");
			}
		}

		#endregion

		#endregion
	}

	public static partial class DataTableExtension {
		/// <summary>
		/// デーブルからアイテムを取得します。
		/// </summary>
		/// <param name="this">DataTable</param>
		/// <returns>アイテムのコレクションを返します。</returns>
		public static IEnumerable<Dictionary<string, object>> GetItems(this DataTable @this) {
			var cols = @this.GetColumns();
			var items = (
				from r in @this
				select cols.ToDictionary(c => c.ColumnName, c => r[c.ColumnName])
			);
			return items;
		}
	}
}
