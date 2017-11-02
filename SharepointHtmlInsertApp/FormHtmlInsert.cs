using System;
using System.IO;
using System.Linq;
using ExtensionsLibrary.Extensions;
using ObjectAnalysisProject.Extensions;
using SharepointHtmlInsertApp.Properties;
using SharePointOnlineLibrary.Forms.Primitives;
using SharePointOnlineLibrary.Manager.Lists;

namespace SharepointHtmlInsertApp {
	/// <summary>
	/// フォーム
	/// </summary>
	public partial class FormHtmlInsert : FormDirectoryBase {
		#region フィールド

		#endregion

		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public FormHtmlInsert() : base() {
			this.InitializeComponent();

			this.Url = Settings.Default.URL;
			this.UserName = Settings.Default.User;
			this.Password = Settings.Default.Password;
			this.ListName = Settings.Default.ListName;

			this.LogRowLimit = Settings.Default.LogRowLimit;
			this.UniqueKey = Settings.Default.UniqueKey;
			this.Replace = Settings.Default.Replace;
		}

		#endregion

		#region プロパティ

		/// <summary>
		/// ID 列以外の固有キーとなる列名を取得します。
		/// </summary>
		public string UniqueKey { get; protected set; }

		/// <summary>
		/// 置換処理を行うかどうかを取得します。
		/// </summary>
		public bool Replace { get; protected set; }

		#endregion

		#region メソッド

		/// <summary>
		/// 実行処理のメインです。
		/// </summary>
		protected override void RunCore() {
			var url = this.Url;
			var username = this.UserName;
			var password = this.Password;

			var listName = this.ListName;

			var uniqueKey = this.UniqueKey;
			var m = new ListManager(url, username, password, listName) {
				UniqueKey = uniqueKey,
			};
			m.ThrowException += (s, e) => this.WriteException(e.Value);
			//m.UpdatedItem += (s, e) => this.WriteLineMessage($"{e.Message} 列名=[{e.Value.Keys.Join(", ")}]");
			m.Success += (s, e) => this.WriteLineMessage(e.Message);

			var rows = this.SelectedRows;
			var dirs = (
				from row in rows
				let fullPath = row.Cells["FullName"].Value.ToString()
				select new DirectoryInfo(fullPath)
			);

			dirs.ForEach(dir => {
				try {
					this.WriteLineMessage($"HTML をリストアイテムに取込みます。 : {dir.Name}");
					m.UpdateRichText(dir, this.Replace);
				} catch (ApplicationException ex) {
					this.WriteLineMessage(ex.Message);
				} catch (Exception ex) {
					this.WriteLineMessage($"HTML の取込みに失敗しました。: {ex.Message}");
				}
			});
		}

		private void InitializeComponent() {
			this.Name = nameof(FormHtmlInsert);
			this.Text = "HTML 本文更新";
		}

		#endregion
	}
}
