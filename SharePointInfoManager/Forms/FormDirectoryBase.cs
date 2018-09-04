using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommonFeaturesLibrary;
using ExtensionsLibrary.Extensions;
using ObjectAnalysisProject.Extensions;
using SharePointManager.Interface;
using WindowsFormsLibrary.Extensions;
using WindowsFormsLibrary.Forms.Primitives;

namespace SharePointManager.Forms {
	/// <summary>
	/// <see cref="FormEditText"/> クラスを継承した、ディレクトリ編集フォームクラスです。
	/// </summary>
	public partial class FormDirectoryBase : FormEditText, IListEdit, ISignInInfo {
		#region フィールド

		/// <summary>ログ出力</summary>
		private Logger _log = new Logger();

		#endregion

		#region コンストラクタ

		/// <summary>
		/// <see cref="FormDirectoryBase"/> クラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <remarks>継承クラスのみ呼び出すことができます。</remarks>
		protected FormDirectoryBase() : base() {
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
		public IEnumerable<DataGridViewRow> SelectedRows {
			get { return this.gridDirectories.GetSelectedRows(); }
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
			var folderBrowserDialog = this.folderBrowserDialog;
			var dialogResult = folderBrowserDialog.ShowDialog();
			if (dialogResult == DialogResult.OK) {
				this.textBoxFilePath.Text = folderBrowserDialog.SelectedPath;
			}
		}

		/// <summary>
		/// ダブルクリックイベント
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		/// <remarks>
		/// ダブルクリックされたときに発生します。</remarks>
		private void listBoxMessage_DoubleClick(object sender, EventArgs e) {
			var dialogResult = MessageBox.Show("ログをクリアしますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
			if (dialogResult == DialogResult.Yes) {
				this.listBoxMessage.Items.Clear();
			}
		}

		/// <summary>
		/// [ファイルパス]テキストボックスの変更イベントです。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void textBoxFilePath_TextChanged(object sender, EventArgs e) {
			var textBox = sender as TextBox;
			if (textBox == null) {
				return;
			}
			try {
				var directoryInfo = new DirectoryInfo(textBox.Text);
				this.buttonRun.Enabled = directoryInfo.Exists;
				var dataSource = (
					from d in directoryInfo.EnumerateDirectories()
					let files = d.GetFileInfos(true)
					let cnt = files.Count()
					where files.Any()
					select new {
						Name = d.Name,
						FullName = d.FullName,
						FileCount = cnt
					}
				).ToDataTable(null);
				this.gridDirectories.DataSource = dataSource;
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
			try {
				if (!e.Data.GetDirectories().Any()) {
					throw new DirectoryNotFoundException();
				}
				e.Effect = DragDropEffects.Copy;
			} catch (Exception) {
			}
		}

		/// <summary>
		/// ドラッグ アンド ドロップ操作が完了したときに発生します。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void obj_DragDrop(object sender, DragEventArgs e) {
			var directories = e.Data.GetDirectories();
			this.textBoxFilePath.Text = (directories.Any() ? directories.First().FullName : string.Empty);
		}

		#endregion

		#region メソッド

		/// <summary>
		/// 実行処理
		/// </summary>
		public virtual void Run() {
			try {
				base.Enabled = false;
				this.ValidationCheck();
				this.RunCore();
			} catch (ApplicationException ex) {
				this.WriteLineMessage(ex.Message);
			} catch (Exception ex2) {
				this.WriteException(ex2);
			} finally {
				base.Enabled = true;
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

		/// <summary>
		/// 例外をログに残します。
		/// </summary>
		/// <param name="ex">例外</param>
		protected void WriteException(Exception ex) {
			try {
				this._log.WriteLog(ex.ToString());
			} catch {
			} finally {
				this.ShowMessageBox(string.Format("{0}", ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
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
				if (!message.IsEmpty()) {
					var timeLog = message.GetTimeLog();
					this.listBoxMessage.AddMessage(timeLog, this.LogRowLimit);
					this._log.WriteLog(message);
					if (this.notifyIcon.Visible) {
						this.notifyIcon.ShowBalloonTip(500, "情報", message, ToolTipIcon.Info);
					}
				}
			} catch (Exception ex) {
				this.WriteException(ex);
			}
		}

		#endregion
	}
}
