using System;
using SharePointManager.Interface;
using SharePointManager.Manager.Lists;

namespace SharepointListMngApp.Forms {
	/// <summary>
	/// リストビュー用のフォームです。
	/// </summary>
	public partial class FormListView : FormEditText {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="url">ユーザー</param>
		/// <param name="user">ユーザー</param>
		/// <param name="password">パスワード</param>
		/// <param name="listName">リスト名</param>
		public FormListView(string url, string user, string password, string listName) {
			this.InitializeComponent();

			this.ListName = listName;
			this.Manager = new ListManager(url, user, password, listName);

			var tb = this.Manager.ItemsTable;
			this.gridCsv.DataSource = tb;
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

		/// <summary>リスト名</summary>
		public string ListName {
			get { return this.textLabelListName.Text.Trim(); }
			set { this.textLabelListName.Text = value; }
		}

		/// <summary>管理オブジェクト</summary>
		public ListManager Manager { get; protected set; }

		#endregion

		#region イベントハンドラ

		#region メニュー

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
	}
}
