using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommonFeaturesLibrary;
using ExtensionsLibrary.Extensions;
using ObjectAnalysisProject.Extensions;
using SharePointManager.Interface;
using SharePointManager.Manager.Extensions;
using SharePointManager.Manager.Lists;

namespace SharepointListMngApp.Forms {
	/// <summary>
	/// フォーム
	/// </summary>
	public partial class FormCreateFields : FormEditText, IListEdit {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="url">ユーザー</param>
		/// <param name="user">ユーザー</param>
		/// <param name="password">パスワード</param>
		/// <param name="listName">リスト名</param>
		public FormCreateFields(string url, string user, string password, string listName) {
			this.InitializeComponent();

			this.Url = url;
			this.UserName = user;
			this.Password = password;
			this.ListName = listName;

			this.Manager = new ListManager(url, user, password, listName);
#if false
			var fs = this.Manager.Fields.ToList();
			this.gridCsv.DataSource = fs;
#else
			var fs = this.Manager.Fields.GetEditFields().Select(f => new {
				表示名 = f.Title,
				列名 = f.InternalName,
				型 = f.FieldTypeKind,
				説明 = f.TypeShortDescription,
				読取り専用 = f.ReadOnlyField,
				削除可能 = f.CanBeDeleted,
			}).ToList();
			this.gridCsv.DataSource = fs;
#endif
		}

		#endregion

		#region イベントハンドラ

		/// <summary>
		/// [作成]ボタンのクリックイベントです。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void buttonCreate_Click(object sender, EventArgs e) {
			if (this.ListName.IsWhiteSpace()) {
				this.DialogResult = DialogResult.Cancel;
				return;
			}
		}

		/// <summary>
		/// [参照]ボタンのクリックイベントです。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void buttonReference_Click(object sender, EventArgs e) {
			switch (this.openFileDialog.ShowDialog()) {
			case DialogResult.OK:
				this.textBoxFilePath.Text = openFileDialog.FileName;
				break;
			default:
				break;
			}
		}

		/// <summary>
		/// [ファイルパス]テキストボックスの変更イベントです。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void textBoxFilePath_TextChanged(object sender, EventArgs e) {
			var tb = (sender as TextBox);
			if (tb == null) {
				return;
			}

			try {
				var file = new FileInfo(tb.Text);
				if (!file.Exists) {
					this.buttonCreate.Enabled = false;
					throw new FileNotFoundException();
				}

				this.buttonCreate.Enabled = file.Exists;

				var table = file.LoadDataTable();
				this.gridCsv.DataSource = table;
			} catch (FileNotFoundException) {
				this.gridCsv.DataSource = null;
			} catch (Exception ex) {
				this.gridCsv.DataSource = null;

				this.ShowMessageBox(ex.Message, icon: MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// オブジェクトがコントロールの境界内にドラッグされると発生します。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void obj_DragEnter(object sender, DragEventArgs e) {
			try {
				// ドラッグ中のファイルやディレクトリの取得
				var infos = e.Data.GetFiles();
				if (!infos.Any()) {
					throw new FileNotFoundException();
				}

				e.Effect = DragDropEffects.Copy;
			} catch (Exception) {
				return;
			}
		}

		/// <summary>
		/// ドラッグ アンド ドロップ操作が完了したときに発生します。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void obj_DragDrop(object sender, DragEventArgs e) {
			// ドラッグ＆ドロップされたファイル
			var infos = e.Data.GetFiles();
			this.textBoxFilePath.Text = infos.Any() ? infos.First().FullName : string.Empty;
		}

		#endregion

		#region プロパティ

		/// <summary>SharePoint サイト URL</summary>
		public string Url { get; set; }

		/// <summary>ユーザー</summary>
		public string UserName { get; set; }

		/// <summary>パスワード</summary>
		public string Password { get; set; }

		/// <summary>リスト名</summary>
		public string ListName {
			get { return this.textLabelListName.Text.Trim(); }
			set { this.textLabelListName.Text = value.Trim(); }
		}

		/// <summary>フィールド情報テーブル</summary>
		protected DataTable FieldsTable { get { return this.gridCsv.ToDataTable(); } }

		/// <summary>管理オブジェクト</summary>
		public ListManager Manager { get; protected set; }

		#endregion

		#region メソッド

		/// <summary>
		/// 実行
		/// </summary>
		public void Run() {
			try {
				this.Enabled = false;

				// フィールド拡張
				this.AddField();
			} finally {
				this.Enabled = true;
			}
		}

		/// <summary>
		/// フィールド拡張
		/// </summary>
		private void AddField() {
			var url = this.Url;
			var username = this.UserName;
			var password = this.Password;
			var listName = this.ListName;

			var tbl = this.FieldsTable;
			this.Manager.CreateFields(tbl);
		}

		#endregion
	}
}
