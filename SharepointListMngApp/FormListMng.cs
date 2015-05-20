using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
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
		/// <param name="url">ユーザー</param>
		/// <param name="user">ユーザー</param>
		/// <param name="password">パスワード</param>
		/// <param name="listName">リスト名</param>
		public FormListMng(string url, string user, string password, string listName) {
			this.InitializeComponent();
			//TODO: タブオーダー検討

			this.Url = url;
			this.UserName = user;
			this.Password = password;
			this.ListName = listName;

			this.Manager = new ListManager(url, user, password, listName);
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
			get { return this.textLabelListName.Text.Trim(); }
			set { this.textLabelListName.Text = value; }
		}

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
				this.Manager.AddItems(tbl, CnvComments);
			} finally {
				this.Enabled = true;
			}
		}

		/// <summary>
		/// [室町ビル]コメント変換処理
		/// </summary>
		/// <param name="row"></param>
		private static void CnvComments(Dictionary<string, object> row) {
			var key = "コメント";
			if (row.ContainsKey(key)) {
				try {
					var content = row[key].ToString();
					var log = content.ConvertXmlString<XmlComments>(c => c.GetLog());
					row[key] = log;
				} catch {
				}
			}
		}

		#endregion
	}
}
