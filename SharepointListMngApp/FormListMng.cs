using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ExtensionsLibrary.Extensions;
using ObjectAnalysisProject.Extensions;
using SharePointManager.Extensions;
using SharePointManager.Interface;
using SharePointManager.Manager.Extensions;
using SharePointManager.Manager.Lists;
using SharePointManager.Manager.Lists.Xml;

namespace SharepointListMngApp {
	/// <summary>
	/// フォーム
	/// </summary>
	public partial class FormListMng : Form, IListEdit {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public FormListMng() {
			this.InitializeComponent();
#if true
			this.Url = Properties.Settings.Default.URL;
			this.UserName = Properties.Settings.Default.User;
			this.Password = Properties.Settings.Default.Password;
			this.ListName = Properties.Settings.Default.ListName;
#endif
		}

		#endregion

		#region プロパティ

		/// <summary>SharePoint サイト URL</summary>
		public string Url {
			get { return this.textBoxUrl.Text.Trim(); }
			set { this.textBoxUrl.Text = value; }
		}

		/// <summary>ユーザー</summary>
		public string UserName {
			get { return this.textBoxUser.Text.Trim(); }
			set { this.textBoxUser.Text = value; }
		}

		/// <summary>パスワード</summary>
		public string Password {
			get { return this.textBoxPassword.Text.Trim(); }
			set { this.textBoxPassword.Text = value; }
		}

		/// <summary>リスト名</summary>
		public string ListName {
			get { return this.textBoxListName.Text.Trim(); }
			set { this.textBoxListName.Text = value; }
		}

		#endregion

		#region イベントハンドラ

		/// <summary>
		/// [実行]ボタンのクリックイベントです。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void buttonRun_Click(object sender, EventArgs e) {
			this.Run();
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
				this.buttonRun.Enabled = file.Exists;

				var table = file.LoadCsvData();
				this.gridCsv.DataSource = table;
			} catch (Exception ex) {
				this.gridCsv.DataSource = null;

				MessageBox.Show(ex.Message);
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

			this.textBoxFilePath.Text = files.FirstOrDefault();
		}

		#region メニュー

		/// <summary>
		/// 新規作成
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void NewToolStripMenuItem_Click(object sender, EventArgs e) {
			this.CreateList();
		}

		private void CreateList() {
			var f = new FormCreateList(this.Url, this.UserName, this.Password);
			var ret = f.ShowDialog(this);
			switch (ret) {
			case DialogResult.OK:
				f.Run();
				break;
			case DialogResult.Cancel:
				MessageBox.Show("作成しませんでした。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
				break;
			}
		}

		/// <summary>
		/// 開く
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void OpenToolStripMenuItem_Click(object sender, EventArgs e) {
			this.buttonReference_Click(sender, e);
		}

		#endregion

		#endregion

		#region メソッド

		/// <summary>
		/// 実行
		/// </summary>
		public void Run() {
			if (this.ListName.IsEmpty()) {
				MessageBox.Show("リスト名を入力して下さい。");
				return;
			}

			try {
				this.Enabled = false;

				var url = this.Url;
				var username = this.UserName;
				var password = this.Password;
				var listName = this.ListName;

				var lm = new ListManager(url, username, password, listName);
				lm.ThrowException += (s, ea) => {
					throw new Exception(ea.ErrorMessage);
				};

				var tbl = this.gridCsv.ToDataTable();
				var ls = tbl.ToDictionaryList();
				ls.ForEach(r => {
					var key = "コメント";
					if (r.ContainsKey(key)) {
						try {
							var content = r[key].ToString();
							var log = content.ConvertXmlString<XmlComments>(c => c.GetLog());
							r[key] = log;
						} catch {
						}
					}

					lm.AddListItem(r);
				});

				var msg = "[" + listName + "] にアイテムを登録しました。";
				MessageBox.Show(msg);
			} catch (Exception ex) {
				MessageBox.Show(ex.ToString());
			} finally {
				this.Enabled = true;
			}
		}

		#endregion
	}
}
