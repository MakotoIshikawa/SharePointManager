﻿using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommonFeaturesLibrary;
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
		/// <summary>ログ出力</summary>
		OutputLog m_Log = new OutputLog();

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
#endif
		}

		#endregion

		#region プロパティ

		/// <summary>表示ログ最大行数</summary>
		public int LogRowLimit { get; protected set; }

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
				this.WriteLineMessage(msg);
#else	// TODO:ファイル添付処理実装
				var row = this.gridDirectories.SelectedRows[0];
				var fullPath = row.Cells["FullName"].Value.ToString();

				var files = new DirectoryInfo(fullPath).EnumerateFiles();
				files.Select(f => f.FullName).ToList()
				.ForEach(f => {
					this.WriteLineMessage(string.Format("ファイル添付: {0}", f));
				});
#endif
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
		/// メッセージ書込</summary>
		/// <param name="message">メッセージ</param>
		/// <remarks>
		/// ユーザインターフェイスにメッセージを書き込む</remarks>
		private void WriteLineMessage(string message) {
			try {
				// 時刻ログ取得
				var msg = message.GetTimeLog();

				// リストにログ追加
				this.AddListBox(msg);

				// ログファイルに出力
				this.m_Log.WriteLog(message);
#if true
				// バルーン表示
				if (this.notifyIcon1.Visible) {
					this.notifyIcon1.ShowBalloonTip(500, "情報", msg, ToolTipIcon.Info);
				}
#endif
			} catch (Exception ex) {
				this.m_Log.WriteLog(ex.ToString());
			}
		}

		/// <summary>
		/// メッセージ追加
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <remarks>
		/// リストボックスにメッセージを書き込む</remarks>
		private void AddListBox(string message) {
			if (this.listBoxMessage.Visible == false) {
				// 非表示であれば何もしない
				return;
			}

			this.listBoxMessage.Items.Add(message);

			if (this.listBoxMessage.Items.Count > this.LogRowLimit) {
				// 表示ログ最大行数を超えていたら先頭行を削除
				this.listBoxMessage.Items.RemoveAt(0);
			}

			// 追加された行を選択します
			var index = this.listBoxMessage.Items.Count - 1;
			this.listBoxMessage.SelectedIndex = index;
		}

		#endregion
	}
}
