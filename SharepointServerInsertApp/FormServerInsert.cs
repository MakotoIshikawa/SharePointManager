using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExtensionsLibrary.Extensions;
using Microsoft.SharePoint;
using ObjectAnalysisProject.Extensions;
using SharePointAddItem.Model;
using SharePointLibrary.Extensions;
using SharepointServerInsertApp.Extensions;
using SharepointServerInsertApp.Forms.Primitives;
using SharepointServerInsertApp.Properties;
using WindowsFormsLibrary.Extensions;
using XmlLibrary.Extensions;

namespace SharepointServerInsertApp {
	/// <summary>
	/// フォーム
	/// </summary>
	public partial class FormServerInsert : FormFetchFileDataByServer {
		#region フィールド

		private Button buttonDelete;

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
			this.buttonDelete = new Button {
				Anchor = (AnchorStyles.Bottom | AnchorStyles.Right),
				Location = new Point(400, 407),
				Name = nameof(buttonDelete),
				Size = new Size(75, 23),
				TabIndex = 1,
				Text = "全件削除",
				UseVisualStyleBackColor = true,
			};

			this.buttonDelete.Click += (sender, e) => {
				var listTitle = this.ListName;
				var result = this.ShowMessageBox($"[{listTitle}] リストのアイテムを一括削除しますか？", "確認", MessageBoxButtons.YesNo);
				switch (result) {
				case DialogResult.OK:
				case DialogResult.Retry:
				case DialogResult.Yes:
					this.DeleteAll();
					break;
				}
			};

			this.Controls.Add(this.buttonDelete);

			this.TextBoxListName.Width -= 75;

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

		#region 削除

		/// <summary>
		/// 全てのアイテムを削除します。
		/// </summary>
		/// <returns>削除したアイテム数を返します。</returns>
		public virtual void DeleteAll() {
			var requestUrl = this.Url;
			var listInternalName = this.ListName;

			var cnt = DeleteAll(requestUrl, listInternalName);

			this.WriteLineMessage($"{listInternalName} リストのアイテムを全て削除しました。: {cnt} 件");
		}

		/// <summary>
		/// 全てのアイテムを削除します。
		/// </summary>
		/// <param name="requestUrl">サイトURL</param>
		/// <param name="listInternalName">リスト内部名</param>
		/// <returns>削除したアイテムの個数</returns>
		protected virtual int DeleteAll(string requestUrl, string listInternalName) {
			var sb = new StringBuilder();
			try {
				using (var site = new SPSite(requestUrl))
				using (var web = site.OpenWeb()) {
					var list = web.GetListByInternalName(listInternalName);
					var results = list.DeleteAllByBatch();
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
				throw new ApplicationException($"削除処理を終了します。Results={result}");
			}
		}

		#endregion

		#endregion
	}
}
