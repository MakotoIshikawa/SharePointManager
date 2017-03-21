using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ExtensionsLibrary.Extensions;
using SharePointManager.Interface;
using SharePointManager.Manager.Lists;
using WindowsFormsLibrary.Extensions;
using WindowsFormsLibrary.Forms.Primitives;

namespace SharepointListMngApp.Forms {
	/// <summary>
	/// フォーム
	/// </summary>
	public partial class FormCreateList : FormEditText, IListEdit {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="url">ユーザー</param>
		/// <param name="user">ユーザー</param>
		/// <param name="password">パスワード</param>
		public FormCreateList(string url, string user, string password) {
			this.InitializeComponent();

			this.Url = url;
			this.UserName = user;
			this.Password = password;

			this.Manager = new ListCollectionManager(url, user, password);
			this.Manager.Success += (s, e) => Debug.WriteLine(e.Message);
		}

		#endregion

		#region イベントハンドラ

		/// <summary>
		/// [作成]ボタンのクリックイベントです。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void buttonCreate_Click(object sender, EventArgs e) {
			if (!this.IsValidatedListName(this.textBoxListName)) {
				this.DialogResult = DialogResult.None;
				return;
			}

			if (!this.IsValidatedListUrl(this.textBoxListUrl)) {
				this.DialogResult = DialogResult.None;
				return;
			}
		}

		#region 検証イベント

		/// <summary>
		/// [リスト名]コントロールが検証を行っているときに呼び出されます。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void textBoxListName_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
			var tb = (sender as TextBox);
			if (tb == null) {
				return;
			}

			if (!this.IsValidatedListName(tb)) {
				e.Cancel = true;
				return;
			}
		}

		/// <summary>
		/// [リストURL]コントロールが検証を行っているときに呼び出されます。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void textBoxListUrl_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
			var tb = (sender as TextBox);
			if (tb == null) {
				return;
			}

			if (!this.IsValidatedListUrl(tb)) {
				e.Cancel = true;
				return;
			}
		}

		/// <summary>
		/// [共通]コントロールの検証が終了すると呼び出されます。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void TextBoxValidated(object sender, EventArgs e) {
			this.errorProvider.Clear();
		}

		#endregion

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

		/// <summary>管理オブジェクト</summary>
		public ListCollectionManager Manager { get; protected set; }

		#endregion

		#region メソッド

		/// <summary>
		/// 実行
		/// </summary>
		public void Run() {
			try {
				this.Enabled = false;

				// リスト作成
				this.CreateCustomList();
			} finally {
				this.Enabled = true;
			}
		}

		/// <summary>
		/// カスタムリスト作成
		/// </summary>
		private void CreateCustomList() {
			var listName = this.ListName;
			var listUrl = this.ListUrl;
			var description = this.Description;

			this.Manager.Create(listName, listUrl, description);
		}

		#region 検証

		/// <summary>
		/// [リスト名]入力値判定
		/// </summary>
		/// <param name="tb">TextBox</param>
		/// <returns>有効かどうかを返します。</returns>
		private bool IsValidatedListName(TextBox tb) {
			return tb.IsValidated(this.errorProvider, s => {
				if (s.IsEmpty()) {
					var sb = new StringBuilder()
						.AppendFormat("{0}は必須項目です。", this.labelListName.Text).AppendLine()
						.AppendLine("入力して下さい。");

					throw new Exception(sb.ToString());
				}
			});
		}

		/// <summary>
		/// [リストURL]入力値判定
		/// </summary>
		/// <param name="tb">TextBox</param>
		/// <returns>有効かどうかを返します。</returns>
		private bool IsValidatedListUrl(TextBox tb) {
			return tb.IsValidated(this.errorProvider, s => {
				if (s.IsEmpty()) {
					var sb = new StringBuilder()
						.AppendFormat("{0}は必須項目です。", this.labelListUrl.Text).AppendLine()
						.AppendLine("入力して下さい。");

					throw new Exception(sb.ToString());
				}

				var rex = new Regex(@"[0-9A-Za-z_]");
				if (!rex.IsMatch(s)) {
					var sb = new StringBuilder()
						.AppendLine("無効な文字が含まれています。");

					throw new Exception(sb.ToString());
				}
			});
		}

		#endregion

		#endregion
	}
}
