using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ExtensionsLibrary.Extensions;
using ObjectAnalysisProject.Extensions;
using SharePointManager.Extensions;
using SharePointManager.Manager.Extensions;
using SharePointManager.Manager.Lists;

namespace SharepointAttachmentFilesApp {
	/// <summary>
	/// フォーム
	/// </summary>
	public partial class FormAttachmentFiles : Form {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public FormAttachmentFiles() {
			this.InitializeComponent();
#if true
			this.textBoxUrl.Text = Properties.Settings.Default.URL;
			this.textBoxUser.Text = Properties.Settings.Default.User;
			this.textBoxPassword.Text = Properties.Settings.Default.Password;
			this.textBoxListName.Text = Properties.Settings.Default.ListName;
#endif
		}

		#endregion

		#region イベントハンドラ

		/// <summary>
		/// [実行]ボタンのクリックイベントです。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void buttonRun_Click(object sender, EventArgs e) {
			if (this.textBoxListName.Text.IsEmpty()) {
				MessageBox.Show("リスト名を入力して下さい。");
				return;
			}

			try {
				this.Enabled = false;

				var url = this.textBoxUrl.Text;
				var username = this.textBoxUser.Text;
				var password = this.textBoxPassword.Text;
				var listName = this.textBoxListName.Text;
#if false
				var lm = new ListManager(url, username, password, listName);
				lm.ThrowException += (s, ea) => {
					throw new Exception(ea.ErrorMessage);
				};
				var tbl = this.gridCsv.ToDataTable();
				var ls = tbl.ToDictionaryList();
				ls.ForEach(r => {
					lm.AddListItem(r);
				});

				var msg = "[" + listName + "] にアイテムを登録しました。";
				MessageBox.Show(msg);
#else	// TODO:ファイル添付処理実装
				var row = this.gridDirectories.SelectedRows[0];
				var fullPath = row.Cells["FullName"].Value.ToString();
				var srt = new DirectoryInfo(fullPath).EnumerateFiles()
				.Select(f => f.FullName).Join(", ");
				MessageBox.Show(string.Format("ファイル添付(仮): {0}", srt));
#endif
			} catch (Exception ex) {
				MessageBox.Show(ex.ToString());
			} finally {
				this.Enabled = true;
			}
		}

		/// <summary>
		/// [参照]ボタンのクリックイベントです。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void buttonReference_Click(object sender, EventArgs e) {
			var dlg = this.folderBrowserDialog;
			switch (dlg.ShowDialog()) {
			case DialogResult.OK:
				this.textBoxFilePath.Text = dlg.SelectedPath;
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
				var directory = new DirectoryInfo(tb.Text);
				this.buttonRun.Enabled = directory.Exists;

				var table = directory.EnumerateDirectories().ToDataTable();
				this.gridDirectories.DataSource = table;
			} catch (Exception ex) {
				this.gridDirectories.DataSource = null;

				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// オブジェクトがコントロールの境界内にドラッグされると発生します。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void obj_DragEnter(object sender, DragEventArgs e) {
			var format = DataFormats.FileDrop;
			if (!e.Data.GetDataPresent(format)) {
				return;
			}

			// ドラッグ中のファイルやディレクトリの取得
			var drags = (string[])e.Data.GetData(format);

			foreach (var d in drags.Select(v => new DirectoryInfo(v))) {
				if (!d.Exists) {
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
			var format = DataFormats.FileDrop;
			// ドラッグ＆ドロップされたファイルやディレクトリ
			var files = (string[])e.Data.GetData(format);

			this.textBoxFilePath.Text = files.FirstOrDefault();
		}

		#endregion

		#region メソッド

		#endregion
	}
}
