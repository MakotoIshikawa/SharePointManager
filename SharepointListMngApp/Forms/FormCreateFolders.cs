using System;
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
	/// フォルダー追加処理用のフォームです。
	/// </summary>
	public partial class FormCreateFolders : FormEditText, IListEdit {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="url">ユーザー</param>
		/// <param name="user">ユーザー</param>
		/// <param name="password">パスワード</param>
		/// <param name="listName">リスト名</param>
		public FormCreateFolders(string url, string user, string password, string listName) {
			this.InitializeComponent();

			this.ListPath = listName;
			var listPath = this.ListPath.Split('/', '\\');
			this.ListName = listPath.FirstOrDefault();

			this.FolderName = listPath.Skip(1).Join("/");

			this.Manager = new ListManager(url, user, password, this.ListName) {
				FolderName = this.FolderName,
			};
		}

		#endregion

		#region プロパティ

		/// <summary>SharePoint サイト URL</summary>
		public string Url {
			get { return this.Manager.Url; }
			set { this.Manager.Url = value; }
		}

		/// <summary>ユーザー</summary>
		public string UserName {
			get { return this.Manager.UserName; }
			set { this.Manager.UserName = value; }
		}

		/// <summary>パスワード</summary>
		public string Password {
			get { return this.Manager.Password; }
			set { this.Manager.Password = value; }
		}

		/// <summary>
		/// リストパス
		/// </summary>
		public string ListPath {
			get { return this.textLabelListName.Text.Trim(); }
			set { this.textLabelListName.Text = value; }
		}

		/// <summary>
		/// リスト名
		/// </summary>
		public string ListName { get; protected set; }

		/// <summary>
		/// フォルダ名
		/// </summary>
		public string FolderName { get; protected set; }

		/// <summary>管理オブジェクト</summary>
		public ListManager Manager { get; protected set; }

		#endregion

		#region イベントハンドラ

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

				var table = file.LoadDataTable();
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

		#region メニュー

		/// <summary>
		/// [開く(O)]クリックイベント
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void OpenToolStripMenuItem_Click(object sender, EventArgs e) {
			this.buttonReference_Click(sender, e);
		}

		/// <summary>
		/// [終了(X)]クリックイベント
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void ExitToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Close();
		}

		#endregion

		#endregion

		#region メソッド

		/// <summary>
		/// 実行
		/// </summary>
		public void Run() {
			try {
				this.Enabled = false;

				var tbl = this.gridCsv.ToDataTable();
				var ls = tbl.ToDictionaryList();

				// TODO: Title で固定している
				var titles = ls.Select(kvp => kvp["Title"].ToString());

				this.Manager.AddFolders(titles);
			} finally {
				this.Enabled = true;
			}
		}

		#endregion
	}
}
