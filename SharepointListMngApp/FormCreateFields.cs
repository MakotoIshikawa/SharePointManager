using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ExtensionsLibrary.Extensions;
using ObjectAnalysisProject.Extensions;
using SharePointManager.Extensions;
using SharePointManager.Interface;
using SharePointManager.Manager.Lists;
using SharePointManager.MyException;

namespace SharepointListMngApp {
	/// <summary>
	/// フォーム
	/// </summary>
	public partial class FormCreateFields : Form, IListEdit {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="url">ユーザー</param>
		/// <param name="user">ユーザー</param>
		/// <param name="password">パスワード</param>
		/// <param name="listName">リスト名</param>
		public FormCreateFields(string url, string user, string password, string listName = null) {
			this.InitializeComponent();

			this.Url = url;
			this.UserName = user;
			this.Password = password;

			if (!listName.IsWhiteSpace()) {
				this.ListName = listName;
				this.textBoxListName.Enabled = false;
			}
		}

		#endregion

		#region イベントハンドラ

		/// <summary>
		/// [作成]ボタンのクリックイベントです。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void buttonCreate_Click(object sender, EventArgs e) {
			if (!this.IsValidatedListName(this.textBoxListName)) {
				this.DialogResult = DialogResult.None;
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

		#region 検証イベント

		/// <summary>
		/// [リスト名]コントロールが検証を行っているときに呼び出されます。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void textBoxListName_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
			var tb = (sender as TextBox);
			if (tb == null) {
				return;
			}

			if (!this.IsValidatedListName(tb)) {
				e.Cancel = true;
				return;
			}
		}

		/// <summary>
		/// [共通]コントロールの検証が終了すると呼び出されます。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void TextBoxValidated(object sender, EventArgs e) {
			this.errorProvider.Clear();
		}

		#endregion

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

				var table = file.LoadCsvData();
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
			if (!e.Data.GetDataPresent(DataFormats.FileDrop)) {
				return;
			}

			// ドラッグ中のファイルやディレクトリの取得
			var drags = (string[])e.Data.GetData(DataFormats.FileDrop);

			foreach (var f in drags.Select(v => new FileInfo(v))) {
				if (!f.Exists) {
					return;
				}
			}

			e.Effect = DragDropEffects.Copy;
		}

		/// <summary>
		/// ドラッグ アンド ドロップ操作が完了したときに発生します。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void obj_DragDrop(object sender, DragEventArgs e) {
			// ドラッグ＆ドロップされたファイル
			var files = (string[])e.Data.GetData(DataFormats.FileDrop);

			var filePath = files.FirstOrDefault();
			var fileInfo = new FileInfo(filePath);
			this.textBoxFilePath.Text = fileInfo.Exists ? fileInfo.FullName : string.Empty;
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
			get { return this.textBoxListName.Text.Trim(); }
			set { this.textBoxListName.Text = value.Trim(); }
		}

		/// <summary>フィールド情報テーブル</summary>
		protected DataTable FieldsTable { get { return this.gridCsv.ToDataTable(); } }

		#endregion

		#region メソッド

		/// <summary>
		/// 実行
		/// </summary>
		public void Run() {
			try {
				this.Enabled = false;

				// フィールド拡張
				var msg = this.AddField();
				this.ShowMessageBox(msg);
			} finally {
				this.Enabled = true;
			}
		}

		/// <summary>
		/// フィールド拡張
		/// </summary>
		private string AddField() {
			var url = this.Url;
			var username = this.UserName;
			var password = this.Password;
			var listName = this.ListName;

			var m = new ListManager(url, username, password, listName);

			var tbl = this.FieldsTable;
			return m.SetFields(tbl);
		}

		#region 検証

		/// <summary>
		/// [リスト名]入力値判定
		/// </summary>
		/// <param name="tb">TextBox</param>
		/// <returns>有効かどうかを返します。</returns>
		private bool IsValidatedListName(TextBox tb) {
			return tb.IsValidated(this.errorProvider, s => {
				if (s.IsEmpty()) {
					var sb = new StringBuilder()
						.AppendFormat("{0}は必須項目です。", this.labelListName.Text).AppendLine()
						.AppendLine("入力して下さい。");

					throw new Exception(sb.ToString());
				}
			});
		}

		#endregion

		#endregion
	}
}
