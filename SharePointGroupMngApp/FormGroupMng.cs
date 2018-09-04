using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonFeaturesLibrary.Extensions;
using ExtensionsLibrary.Extensions;
using ObjectAnalysisProject.Extensions;
using SharePointGroupMngApp.Properties;
using SharePointManager.Manager;
using WindowsFormsLibrary.Extensions;

namespace SharePointGroupMngApp {
	/// <summary>
	/// フォーム
	/// </summary>
	public partial class FormGroupMng : Form {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public FormGroupMng() {
			this.InitializeComponent();

			this.textBoxUrl.Text = Settings.Default.URL;
			this.textBoxUser.Text = Settings.Default.User;
			this.textBoxPassword.Text = Settings.Default.Password;
		}

		#endregion

		#region イベントハンドラ

		#region Click

		/// <summary>
		/// [グループ追加]ボタンのクリックイベントです。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void buttonAddGroup_Click(object sender, EventArgs e) {
			this.RunAction(this.AddGroup);
		}

		/// <summary>
		/// [ユーザー追加]ボタンのクリックイベントです。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void buttonAddUser_Click(object sender, EventArgs e) {
			this.RunAction(this.AddUser);
		}

		/// <summary>
		/// [メンバー登録]ボタンのクリックイベントです。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void buttonAddMember_Click(object sender, EventArgs e) {
			this.RunAction(this.AddMember);
		}

		/// <summary>
		/// [参照]ボタンのクリックイベントです。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void buttonReference_Click(object sender, EventArgs e) {
			switch (this.openFileDialog.ShowDialog()) {
			case DialogResult.OK:
				this.textBoxFilePath.Text = this.openFileDialog.FileName;
				break;
			}
		}

		/// <summary>
		/// [取得]ボタンのクリックイベントです。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void buttonGetGroups_Click(object sender, EventArgs e) {
			this.RunAction(() => {
				var tbl = GetGroups();
				this.gridCsv.DataSource = tbl;
			});
		}

		/// <summary>
		/// [取得]ボタンのクリックイベントです。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void buttonGetUsers_Click(object sender, EventArgs e) {
			this.RunAction(() => {
				var tbl = GetUsers();
				this.gridCsv.DataSource = tbl;
			});
		}

		#endregion

		/// <summary>
		/// [ファイルパス]テキストボックスの変更イベントです。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void textBoxFilePath_TextChanged(object sender, EventArgs e) {
			this.RunAction<TextBox>(sender, tx => {
				var file = new FileInfo(tx.Text);
				ChangedFilePath(file);
			}, () => {
				this.gridCsv.DataSource = null;
			});
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
			} catch {
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

		#endregion

		#region メソッド

		/// <summary>
		/// グループ追加
		/// </summary>
		public void AddGroup() {
			var url = this.textBoxUrl.Text;
			var username = this.textBoxUser.Text;
			var password = this.textBoxPassword.Text;

			var gm = new GroupManager(url, username, password);
			gm.ThrowSharePointException += (_sender, _e) => {
				throw new ApplicationException(_e.ErrorMessage);
			};

			var tbl = this.gridCsv.ToDataTable();
			var gls = tbl.Select(r => new {
				グループ名 = r["グループ名"].ToString(),
				説明 = r["説明"].ToString(),
			}).ToList();

			var addGroups = new List<string>();
			gls.ForEach(gi => {
				var name = gi.グループ名;
				var description = gi.説明;

				gm.AddGroup(name, description, g => {
					addGroups.Add(g.Title);
				});
			});

			var sb = new StringBuilder();
			if (addGroups.Any()) {
				sb.AppendLine("グループを登録しました。");
				addGroups.ForEach(title => {
					sb.AppendLine(title);
				});
			} else {
				sb.AppendLine("グループを追加しませんでした。");
			}

			this.ShowMessageBox(sb.ToString());
		}

		/// <summary>
		/// ユーザー追加
		/// </summary>
		public void AddUser() {
			var url = this.textBoxUrl.Text;
			var username = this.textBoxUser.Text;
			var password = this.textBoxPassword.Text;

			var gm = new GroupManager(url, username, password, false);
			gm.ThrowSharePointException += (_sender, _e) => {
				throw new ApplicationException(_e.ErrorMessage);
			};

			var tbl = this.gridCsv.ToDataTable();
			var gls = tbl.Select(r => new {
				グループ名 = r["グループ名"].ToString(),
				表示名 = r["表示名"].ToString(),
				ログイン名 = r["ログイン名"].ToString(),
				メールアドレス = r["メールアドレス"].ToString(),
			}).GroupBy(r => new {
				r.グループ名,
			}).ToList();

			var sb = new StringBuilder();
			gls.ForEach(gg => {
				var groupName = gg.Key.グループ名;
				sb.AppendLine($"グループ名 : {groupName}");

				gg.ToList().ForEach(u1 => {
					var title = u1.表示名;
					var loginName = u1.ログイン名;
					var mail = u1.メールアドレス;
					sb.Append($"  ユーザー名:{title}({mail})");

					try {
						gm.AddUser(groupName, title, loginName, mail);
						sb.AppendLine($"を追加しました。");
					} catch (Exception ex) {
						sb.AppendLine($"の登録に失敗しました。: {ex.Message}");
					}
				});
			});

			this.ShowMessageBox(sb.ToString());
		}

		/// <summary>
		/// メンバー登録
		/// </summary>
		public void AddMember() {
			var url = this.textBoxUrl.Text;
			var username = this.textBoxUser.Text;
			var password = this.textBoxPassword.Text;

			var gm = new GroupManager(url, username, password);
			gm.ThrowSharePointException += (_sender, _e) => {
				throw new ApplicationException(_e.ErrorMessage);
			};

			var userLs = gm.SiteUsers.ToDictionary(u => u.Title);

			var tbl = this.gridCsv.ToDataTable();
			var gls = tbl.Select(r => new {
				グループ名 = r["グループ名"].ToString(),
				メンバー名 = r["メンバー名"].ToString(),
			}).GroupBy(r => new {
				r.グループ名,
			}).ToList();

			var sb = new StringBuilder();
			gls.ForEach(gg => {
				var groupName = gg.Key.グループ名;
				sb.AppendLine("グループ名:" + groupName);

				gg.ToList().ForEach(u1 => {
					var userName = u1.メンバー名;
					sb.Append("  メンバー名:").Append(userName);

					if (userLs.ContainsKey(userName)) {
						var user = userLs[userName];
						gm.AddMember(groupName, user);
						sb.Append("を追加しました。").AppendLine();
					} else {
						sb.Append("  ※ユーザがみつかりませんでした。").AppendLine();
					}
				});
			});

			this.ShowMessageBox(sb.ToString());
		}

		/// <summary>
		/// グループ一覧取得
		/// </summary>
		/// <returns>グループ一覧のテーブル</returns>
		public DataTable GetGroups() {
			var url = this.textBoxUrl.Text;
			var username = this.textBoxUser.Text;
			var password = this.textBoxPassword.Text;

			var gm = new GroupManager(url, username, password);
			gm.ThrowSharePointException += (_sender, _e) => {
				throw new ApplicationException(_e.ErrorMessage);
			};

			var tbl = gm.SiteGroups.Select(g => new {
				g.Id,
				g.Title,
				g.Description,
			}).OrderBy(g => g.Id).ToDataTable();
			return tbl;
		}

		/// <summary>
		/// ユーザー一覧取得
		/// </summary>
		/// <returns>ユーザー一覧のテーブル</returns>
		public DataTable GetUsers() {
			var url = this.textBoxUrl.Text;
			var username = this.textBoxUser.Text;
			var password = this.textBoxPassword.Text;

			var gm = new GroupManager(url, username, password);
			gm.ThrowSharePointException += (_sender, _e) => {
				throw new ApplicationException(_e.ErrorMessage);
			};

			var tbl = gm.SiteUsers.Select(g => new {
				g.Id,
				g.Title,
				g.LoginName,
				g.Email,
				g.PrincipalType,
				g.IsSiteAdmin,
			}).OrderBy(g => g.Id).ToDataTable();
			return tbl;
		}

		#region ファイルパス変更

		private void ChangedFilePath(FileInfo file) {
			this.buttonAddGroup.Enabled = file.Exists;
			this.buttonAddMember.Enabled = file.Exists;

			var table = file.LoadDataTable();
			this.gridCsv.DataSource = table;

			var cols = table.Columns.Cast<DataColumn>();
			if (cols.Any(c => c.ColumnName == "グループ名") && cols.Any(c => c.ColumnName == "説明")) {
				this.buttonAddGroup.Enabled = true;
				this.buttonAddUser.Enabled = false;
				this.buttonAddMember.Enabled = false;
			} else if (cols.Any(c => c.ColumnName == "グループ名") && cols.Any(c => c.ColumnName == "表示名") && cols.Any(c => c.ColumnName == "ログイン名") && cols.Any(c => c.ColumnName == "メールアドレス")) {
				this.buttonAddGroup.Enabled = false;
				this.buttonAddUser.Enabled = true;
				this.buttonAddMember.Enabled = false;
			} else if (cols.Any(c => c.ColumnName == "グループ名") && cols.Any(c => c.ColumnName == "メンバー名")) {
				this.buttonAddGroup.Enabled = false;
				this.buttonAddUser.Enabled = false;
				this.buttonAddMember.Enabled = true;
			} else {
				this.buttonAddGroup.Enabled = false;
				this.buttonAddUser.Enabled = false;
				this.buttonAddMember.Enabled = false;
			}
		}

		#endregion

		#endregion
	}
}
