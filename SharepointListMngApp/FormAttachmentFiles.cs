using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommonFeaturesLibrary;
using ExtensionsLibrary.Extensions;
using ObjectAnalysisProject.Extensions;
using SharePointManager.Extensions;
using SharePointManager.Manager.Extensions;
using SharePointManager.Manager.Lists;
using SharePointManager.Manager.Lists.Xml;

namespace SharepointListMngApp {
	/// <summary>
	/// フォーム
	/// </summary>
	public partial class FormAttachmentFiles : Form {
		#region フィールド

		/// <summary>ログ出力</summary>
		private LogOutputter _log = new LogOutputter();

		#endregion

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

			this.LogRowLimit = Properties.Settings.Default.LogRowLimit;
			this.UniqueKey = Properties.Settings.Default.UniqueKey;
#endif
		}

		#endregion

		#region プロパティ

		/// <summary>表示ログ最大行数</summary>
		public int LogRowLimit { get; protected set; }

		/// <summary>固有キー</summary>
		public string UniqueKey { get; protected set; }

		#endregion

		#region イベントハンドラ

		/// <summary>
		/// [実行]ボタンのクリックイベントです。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void buttonRun_Click(object sender, EventArgs e) {
			if (this.textBoxListName.Text.IsEmpty()) {
				this.WriteLineMessage("リスト名を入力して下さい。");
				return;
			}

			try {
				this.Enabled = false;

				var url = this.textBoxUrl.Text;
				var username = this.textBoxUser.Text;
				var password = this.textBoxPassword.Text;
				var listName = this.textBoxListName.Text;

				var m = new ListManager(url, username, password, listName);
				m.ThrowException += (s, ea) => {
					throw new Exception(ea.ErrorMessage);
				};

				var key = this.UniqueKey;
				foreach (var row in this.gridDirectories.SelectedRows.Cast<DataGridViewRow>()) {
					var fullPath = row.Cells["FullName"].Value.ToString();

					var dir = new DirectoryInfo(fullPath);
					this.WriteLineMessage("ファイルを添付します。 : " + dir.FullName);
					var files = dir.EnumerateFiles();

					var id = m.GetID(key, dir.Name);
					m.AddAttachmentFile(id, files);

					files.Select(f => f.Name).ToList()
					.ForEach(f => {
						this.WriteLineMessage(string.Format("ファイル名 : {0}", f));
					});
				}
			} catch (Exception ex) {
				this.WriteLineMessage(ex.ToString());
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

				this.WriteLineMessage(ex.Message);
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

		/// <summary>
		/// ダブルクリックイベント
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		/// <remarks>
		/// ダブルクリックされたときに発生します。</remarks>
		private void listBoxMessage_DoubleClick(object sender, EventArgs e) {
			var ret = MessageBox.Show("ログをクリアしますか？", "確認"
				, MessageBoxButtons.YesNo
				, MessageBoxIcon.Question
				, MessageBoxDefaultButton.Button2
			);

			switch (ret) {
			case DialogResult.Yes:
				this.listBoxMessage.Items.Clear();
				break;
			default:
				break;
			}
		}

		#endregion

		#region メソッド

		/// <summary>
		/// メッセージ書込
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <remarks>
		/// ユーザインターフェイスにメッセージを書き込む</remarks>
		private void WriteLineMessage(string message) {
			try {
				// 時刻ログ取得
				var msg = message.GetTimeLog();

				// リストにログ追加
				this.listBoxMessage.AddMessage(msg, this.LogRowLimit);

				// ログファイルに出力
				this._log.WriteLog(message);
#if true
				// バルーン表示
				if (this.notifyIcon1.Visible) {
					this.notifyIcon1.ShowBalloonTip(500, "情報", msg, ToolTipIcon.Info);
				}
#endif
			} catch (Exception ex) {
				this._log.WriteLog(ex.ToString());
			}
		}

		#endregion
	}

}
