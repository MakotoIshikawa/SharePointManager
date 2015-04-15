using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ExtensionsLibrary.Extensions;
using ObjectAnalysisProject.Extensions;
using SharePointInfoManager.Extensions;
using SharePointInfoManager.Manager.Lists;

namespace SharepointListMngApp {
	/// <summary>
	/// フォーム
	/// </summary>
	public partial class FormListMng : Form {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public FormListMng() {
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

		#endregion

		#region メソッド

		#endregion
	}
}
