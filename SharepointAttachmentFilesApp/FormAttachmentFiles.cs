using System;
using System.IO;
using System.Linq;
using ExtensionsLibrary.Extensions;
using ObjectAnalysisProject.Extensions;
using SharepointAttachmentFilesApp.Properties;
using SharePointOnlineLibrary.Manager.Lists;
using WindowsFormsLibrary;

namespace SharepointAttachmentFilesApp {
	/// <summary>
	/// フォーム
	/// </summary>
	public partial class FormAttachmentFiles : FormDirectoryBase {
		#region フィールド

		#endregion

		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public FormAttachmentFiles() : base() {
			this.InitializeComponent();

			this.Url = Settings.Default.URL;
			this.UserName = Settings.Default.User;
			this.Password = Settings.Default.Password;
			this.ListName = Settings.Default.ListName;

			this.LogRowLimit = Settings.Default.LogRowLimit;
			this.UniqueKey = Settings.Default.UniqueKey;
			this.Excludes = Settings.Default.Excludes?.Split(',')?.Select(e => $".{e}")?.ToArray();
		}

		#endregion

		#region プロパティ

		/// <summary>
		/// ID 列以外の固有キーとなる列名を取得します。
		/// </summary>
		public string UniqueKey { get; protected set; }

		/// <summary>
		/// 除外する拡張子の配列を取得します。
		/// </summary>
		public string[] Excludes { get; protected set; }

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
			m.Success += (s, e) => this.WriteLineMessage(e.Message);

			var rows = this.SelectedRows;
			var dirs = (
				from row in rows
				let fullPath = row.Cells["FullName"].Value.ToString()
				select new DirectoryInfo(fullPath)
			);

			dirs.ForEach(dir => {
				try {
					this.WriteLineMessage($"ファイルを添付します。 : {dir.FullName}");
					m.AddAttachmentFiles(dir, this.Excludes);
				} catch (ApplicationException ex) {
					this.WriteLineMessage(ex.Message);
				} catch (Exception ex) {
					this.WriteLineMessage($"ファイルの添付に失敗しました。: {ex.Message}");
				}
			});
		}

		#endregion
	}
}
