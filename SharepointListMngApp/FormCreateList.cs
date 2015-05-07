using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExtensionsLibrary.Extensions;
using ObjectAnalysisProject.Extensions;
using SharePointManager.Extensions;
using SharePointManager.Interface;
using SharePointManager.Manager.Lists;
using SP = Microsoft.SharePoint.Client;

namespace SharepointListMngApp {
	/// <summary>
	/// フォーム
	/// </summary>
	public partial class FormCreateList : Form, IListEdit {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="url">ユーザー</param>
		/// <param name="user">ユーザー</param>
		/// <param name="password">パスワード</param>
		public FormCreateList(string url, string user, string password) {
			this.InitializeComponent();
#if true
			this.Url = url;
			this.UserName = user;
			this.Password = password;
#endif
		}

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
				this.buttonCreate.Enabled = file.Exists;

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

			var filePath = files.FirstOrDefault();
			var fileInfo = new FileInfo(filePath);
			this.textBoxFilePath.Text = fileInfo.Exists ? fileInfo.FullName : string.Empty;
			this.ListName = fileInfo.Exists ? fileInfo.Name.CommentOut(fileInfo.Extension) : string.Empty;
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

		/// <summary>SharePoint リスト URL</summary>
		public string ListUrl {
			get { return this.textBoxListUrl.Text.Trim(); }
			set { this.textBoxListUrl.Text = value; }
		}

		/// <summary>リスト説明</summary>
		public string Description {
			get { return this.textBoxDescription.Text; }
			set { this.textBoxDescription.Text = value; }
		}

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

				// リスト作成
				var msg = this.CreateList(url, username, password, listName);
				var ret = MessageBox.Show(msg, "確認"
					, MessageBoxButtons.YesNo
					, MessageBoxIcon.Question
					, MessageBoxDefaultButton.Button2
				);

				switch (ret) {
				case DialogResult.Yes:
					// フィールド拡張
					this.AddField(url, username, password, listName);
					break;
				case DialogResult.No:
				default:
					return;
				}
			} catch (SP.ServerException ex) {
				MessageBox.Show(ex.Message);
			} catch (SP.PropertyOrFieldNotInitializedException ex) {
				MessageBox.Show(ex.Message);
			} catch (Exception ex) {
				MessageBox.Show(ex.ToString());
			} finally {
				this.Enabled = true;
			}
		}

		/// <summary>
		/// リスト作成
		/// </summary>
		/// <param name="url">ユーザー</param>
		/// <param name="user">ユーザー</param>
		/// <param name="password">パスワード</param>
		/// <param name="listName">リスト名</param>
		/// <returns></returns>
		private string CreateList(string url, string username, string password, string listName) {
			var m = new ListCollectionManager(url, username, password);
			m.ThrowException += (s, ea) => {
				throw new Exception(ea.ErrorMessage);
			};

			var sb = new StringBuilder();
			if (m.Titles.Any(t => t == listName)) {
				sb.AppendFormat("[{0}] は既に存在します。", listName).AppendLine();
			} else {
				var listUrl = this.ListUrl;
				var description = this.Description;

				m.Create(listName, listUrl, description, SP.ListTemplateType.GenericList);
				sb.AppendFormat("[{0}] を作成しました。", listName).AppendLine();
			}

			sb.AppendLine("リストに列を追加しますか？");
			return sb.ToString();
		}

		/// <summary>
		/// フィールド拡張
		/// </summary>
		/// <param name="url">ユーザー</param>
		/// <param name="user">ユーザー</param>
		/// <param name="password">パスワード</param>
		/// <param name="listName">リスト名</param>
		private void AddField(string url, string username, string password, string listName) {
			var m = new ListManager(url, username, password, listName);
			m.ThrowException += (s, ea) => {
				throw new Exception(ea.ErrorMessage);
			};

			var tbl = this.gridCsv.ToDataTable();
			var msg = m.SetFields(tbl);
			MessageBox.Show(msg);
		}

		#endregion
	}
}
