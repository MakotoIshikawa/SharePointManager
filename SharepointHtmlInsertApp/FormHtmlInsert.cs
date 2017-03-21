using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommonFeaturesLibrary;
using ExtensionsLibrary.Extensions;
using ObjectAnalysisProject.Extensions;
using SharepointHtmlInsertApp.Properties;
using SharePointManager.Interface;
using SharePointManager.Manager.Lists;
using WindowsFormsLibrary.Extensions;
using WindowsFormsLibrary.Forms.Primitives;

namespace SharepointHtmlInsertApp {
	/// <summary>
	/// フォーム
	/// </summary>
	public partial class FormHtmlInsert : FormEditText, IListEdit {
		#region フィールド

		/// <summary>ログ出力</summary>
		private Logger _log = new Logger();

		#endregion

		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public FormHtmlInsert() {
			this.InitializeComponent();
#if true
			this.Url = Settings.Default.URL;
			this.UserName = Settings.Default.User;
			this.Password = Settings.Default.Password;
			this.ListName = Settings.Default.ListName;

			this.LogRowLimit = Settings.Default.LogRowLimit;
			this.UniqueKey = Settings.Default.UniqueKey;
			this.Replace = Settings.Default.Replace;
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

		/// <summary>
		/// リストパス
		/// </summary>
		public string ListName {
			get { return this.textBoxListName.Text.Trim(); }
			set { this.textBoxListName.Text = value; }
		}

		/// <summary>表示ログ最大行数</summary>
		public int LogRowLimit { get; protected set; }

		/// <summary>
		/// ID 列以外の固有キーとなる列名を取得します。
		/// </summary>
		public string UniqueKey { get; protected set; }

		/// <summary>
		/// 置換処理を行うかどうかを取得します。
		/// </summary>
		public bool Replace { get; protected set; }

		#endregion

		#region イベントハンドラ

		#region クリックイベント

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
			var dlg = this.folderBrowserDialog;
			switch (dlg.ShowDialog()) {
			case DialogResult.OK:
				this.textBoxFilePath.Text = dlg.SelectedPath;
				break;
			default:
				break;
			}
		}

		#endregion

		#region ダブルクリックイベント

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

		#region テキストチェンジイベント

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

				var table = (
					from d in directory.EnumerateDirectories()
					let files = d.GetFileInfos(true)
					let cnt = files.Count()
					where files.Any()
					select new {
						Name = d.Name,
						FullName = d.FullName,
						FileCount = cnt
					}
				).ToDataTable();

				this.gridDirectories.DataSource = table;
			} catch (Exception ex) {
				this.gridDirectories.DataSource = null;

				this.WriteLineMessage(ex.Message);
			}
		}

		#endregion

		#region ドラックイベント

		/// <summary>
		/// オブジェクトがコントロールの境界内にドラッグされると発生します。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void obj_DragEnter(object sender, DragEventArgs e) {
			try {
				// ドラッグ中のファイルやディレクトリの取得
				var infos = e.Data.GetDirectories();
				if (!infos.Any()) {
					throw new DirectoryNotFoundException();
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
			// ドラッグ中のファイルやディレクトリの取得
			var infos = e.Data.GetDirectories();
			this.textBoxFilePath.Text = infos.Any() ? infos.First().FullName : string.Empty;
		}

		#endregion

		#endregion

		#region メソッド

		/// <summary>
		/// 実行処理
		/// </summary>
		public void Run() {
			if (this.ListName.IsEmpty()) {
				this.WriteLineMessage("リスト名を入力して下さい。");
				return;
			}

			try {
				this.Enabled = false;

				var url = this.Url;
				var username = this.UserName;
				var password = this.Password;

				var listName = this.ListName;

				var uniqueKey = this.UniqueKey;
				var m = new ListManager(url, username, password, listName) {
					UniqueKey = uniqueKey,
				};
				m.ThrowException += (s, e) => this.WriteException(e.Value);
				m.UpdatedItem += (s, e) => this.WriteLineMessage($"{e.Message} 列名=[{e.Value.Keys.Join(", ")}]");

				var rows = this.gridDirectories.GetSelectedRows();
				var dirs = (
					from row in rows
					let fullPath = row.Cells["FullName"].Value.ToString()
					select new DirectoryInfo(fullPath)
				);

				dirs.ForEach(dir => {
					try {
						this.WriteLineMessage($"ディレクトリ名 : {dir.Name}");
						m.UpdateRichText(dir, this.Replace);
					} catch (Exception ex) {
						this.WriteLineMessage($"HTML の取込に失敗しました。: {ex.Message}");
					}
				});
			} catch (Exception ex) {
				this.WriteException(ex);
			} finally {
				this.Enabled = true;
			}
		}

		#region メッセージ出力

		/// <summary>
		/// 例外をログに残します。
		/// </summary>
		/// <param name="ex">例外</param>
		protected void WriteException(Exception ex) {
			this._log.WriteLog(ex.ToString());
		}

		/// <summary>
		/// メッセージ書込
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <remarks>
		/// ユーザインターフェイスにメッセージを書き込む</remarks>
		protected void WriteLineMessage(string message) {
			try {
				// 時刻ログ取得
				var msg = message.GetTimeLog();

				// リストにログ追加
				this.listBoxMessage.AddMessage(msg, this.LogRowLimit);

				// ログファイルに出力
				this._log.WriteLog(message);
#if true
				// バルーン表示
				if (this.notifyIcon.Visible) {
					this.notifyIcon.ShowBalloonTip(500, "情報", msg, ToolTipIcon.Info);
				}
#endif
			} catch (Exception ex) {
				this._log.WriteLog(ex.ToString());
			}
		}

		#endregion

		#endregion
	}
}
