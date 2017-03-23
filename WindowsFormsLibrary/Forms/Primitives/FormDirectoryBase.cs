using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommonFeaturesLibrary;
using ExtensionsLibrary.Extensions;
using ObjectAnalysisProject.Extensions;
using WindowsFormsLibrary.Extensions;
using WindowsFormsLibrary.Forms.Primitives;
using WindowsFormsLibrary.Interface;

namespace WindowsFormsLibrary {
	/// <summary>
	/// フォーム
	/// </summary>
	public partial class FormDirectoryBase : FormEditText, IListEdit {
		#region フィールド

		/// <summary>ログ出力</summary>
		private Logger _log = new Logger();

		#endregion

		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		protected FormDirectoryBase() {
			this.InitializeComponent();
		}

		#endregion

		#region プロパティ

		/// <summary>
		/// SharePoint サイト URL
		/// </summary>
		public string Url {
			get { return this.textBoxUrl.Text.Trim(); }
			set { this.textBoxUrl.Text = value; }
		}

		/// <summary>
		/// ユーザー
		/// </summary>
		public string UserName {
			get { return this.textBoxUser.Text.Trim(); }
			set { this.textBoxUser.Text = value; }
		}

		/// <summary>
		/// パスワード
		/// </summary>
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

		/// <summary>
		/// 表示ログ最大行数
		/// </summary>
		public int LogRowLimit { get; protected set; }

		/// <summary>
		/// 選択中の行コレクションを取得します。
		/// </summary>
		public IEnumerable<DataGridViewRow> SelectedRows => this.gridDirectories.GetSelectedRows();

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
		public virtual void Run() {
			try {
				this.Enabled = false;

				this.ValidationCheck();

				this.RunCore();
			} catch (ApplicationException ex) {
				this.WriteLineMessage(ex.Message);
			} catch (Exception ex) {
				this.WriteException(ex);
			} finally {
				this.Enabled = true;
			}
		}

		/// <summary>
		/// バリデーションチェック
		/// </summary>
		protected virtual void ValidationCheck() {
			if (this.Url.IsEmpty()) {
				throw new ApplicationException("URL を入力して下さい。");
			}

			if (this.UserName.IsEmpty()) {
				throw new ApplicationException("ユーザー名を入力して下さい。");
			}

			if (this.Password.IsEmpty()) {
				throw new ApplicationException("パスワードを入力して下さい。");
			}

			if (this.ListName.IsEmpty()) {
				throw new ApplicationException("リスト名を入力して下さい。");
			}
		}

		/// <summary>
		/// 実行処理の中核です。オーバーライドできます。
		/// </summary>
		protected virtual void RunCore() {
		}

		#region メッセージ出力

		/// <summary>
		/// 例外をログに残します。
		/// </summary>
		/// <param name="ex">例外</param>
		protected void WriteException(Exception ex) {
			try {
				this._log.WriteLog(ex.ToString());
			} catch {
			} finally {
				this.ShowMessageBox($"{ex}", nameof(MessageBoxIcon.Error), icon: MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// メッセージ書込
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <remarks>
		/// ユーザインターフェイスにメッセージを書き込む</remarks>
		protected void WriteLineMessage(string message) {
			try {
				if (message.IsEmpty()) {
					return;
				}

				// 時刻ログ取得
				var msg = message.GetTimeLog();

				// リストにログ追加
				this.listBoxMessage.AddMessage(msg, this.LogRowLimit);

				// ログファイルに出力
				this._log.WriteLog(message);

				// バルーン表示
				if (this.notifyIcon.Visible) {
					this.notifyIcon.ShowBalloonTip(500, "情報", message, ToolTipIcon.Info);
				}
			} catch (Exception ex) {
				this.WriteException(ex);
			}
		}

		#endregion

		#endregion
	}
}
