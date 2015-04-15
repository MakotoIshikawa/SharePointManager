using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ObjectAnalysisProject.Extensions;
using SharePointManager.Extensions;
using SharePointManager.Manager;

namespace SharePointGroupMngApp {
	/// <summary>
	/// フォーム
	/// </summary>
	public partial class FormGroupMng : System.Windows.Forms.Form {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public FormGroupMng() {
			this.InitializeComponent();

			this.textBoxUrl.Text = Properties.Settings.Default.URL;
			this.textBoxUser.Text = Properties.Settings.Default.User;
			this.textBoxPassword.Text = Properties.Settings.Default.Password;
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
			try {
				this.Enabled = false;

				var url = this.textBoxUrl.Text;
				var username = this.textBoxUser.Text;
				var password = this.textBoxPassword.Text;

				var gm = new GroupManager(url, username, password);
				gm.ThrowException += (_sender, _e) => {
					throw new Exception(_e.ErrorMessage);
				};

				var tbl = this.gridCsv.ToDataTable();
				var gls = tbl.Rows.Cast<DataRow>()
				.Select(r => new {
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

				MessageBox.Show(sb.ToString());
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			} finally {
				this.Enabled = true;
			}
		}

		/// <summary>
		/// [ユーザー追加]ボタンのクリックイベントです。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void buttonAddUser_Click(object sender, EventArgs e) {
			try {
				this.Enabled = false;

				var url = this.textBoxUrl.Text;
				var username = this.textBoxUser.Text;
				var password = this.textBoxPassword.Text;

				var gm = new GroupManager(url, username, password, false);
				gm.ThrowException += (_sender, _e) => {
					throw new Exception(_e.ErrorMessage);
				};

				var tbl = this.gridCsv.ToDataTable();
				var gls = tbl.Rows.Cast<DataRow>()
				.Select(r => new {
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
					sb.AppendLine("グループ名:" + groupName);

					gg.ToList().ForEach(u1 => {
						var title = u1.表示名;
						var loginName = u1.ログイン名;
						var mail = u1.メールアドレス;
						sb.Append("  ユーザー名:").AppendFormat("{0}({1})", title, mail);

						try {
							gm.AddUser(groupName, title, loginName, mail);
							sb.Append("を追加しました。").AppendLine();
						} catch (Exception ex) {
							sb.Append("の登録に失敗しました。")
							.AppendLine(ex.Message);
						}
					});
				});

				MessageBox.Show(sb.ToString());
			} catch (Exception ex) {
				MessageBox.Show(ex.ToString());
			} finally {
				this.Enabled = true;
			}
		}

		/// <summary>
		/// [メンバー登録]ボタンのクリックイベントです。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void buttonAddMember_Click(object sender, EventArgs e) {
			try {
				this.Enabled = false;

				var url = this.textBoxUrl.Text;
				var username = this.textBoxUser.Text;
				var password = this.textBoxPassword.Text;

				var gm = new GroupManager(url, username, password);
				gm.ThrowException += (_sender, _e) => {
					throw new Exception(_e.ErrorMessage);
				};

				var userLs = gm.SiteUsers.ToDictionary(u => u.Title);

				var tbl = this.gridCsv.ToDataTable();
				var gls =tbl.Rows.Cast<DataRow>()
				.Select(r => new {
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

				MessageBox.Show(sb.ToString());
			} catch (Exception ex) {
				MessageBox.Show(ex.ToString());
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
			switch (this.openFileDialog.ShowDialog()) {
			case DialogResult.OK:
				this.textBoxFilePath.Text = openFileDialog.FileName;
				break;
			default:
				break;
			}
		}

		/// <summary>
		/// [取得]ボタンのクリックイベントです。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void buttonGetGroups_Click(object sender, EventArgs e) {
			try {
				this.Enabled = false;

				var url = this.textBoxUrl.Text;
				var username = this.textBoxUser.Text;
				var password = this.textBoxPassword.Text;

				var gm = new GroupManager(url, username, password);
				gm.ThrowException += (_sender, _e) => {
					throw new Exception(_e.ErrorMessage);
				};

				var tbl = gm.SiteGroups.Select(g => new {
					g.Id,
					g.Title,
					g.Description,
				}).OrderBy(g => g.Id).ToDataTable();
				this.gridCsv.DataSource = tbl;
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			} finally {
				this.Enabled = true;
			}
		}

		/// <summary>
		/// [取得]ボタンのクリックイベントです。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void buttonGetUsers_Click(object sender, EventArgs e) {
			try {
				this.Enabled = false;

				var url = this.textBoxUrl.Text;
				var username = this.textBoxUser.Text;
				var password = this.textBoxPassword.Text;

				var gm = new GroupManager(url, username, password);
				gm.ThrowException += (_sender, _e) => {
					throw new Exception(_e.ErrorMessage);
				};

				var tbl = gm.SiteUsers.Select(g => new {
					g.Id,
					g.Title,
					g.LoginName,
					g.Email,
					g.PrincipalType,
					g.IsSiteAdmin,
				}).OrderBy(g => g.Id).ToDataTable();
				this.gridCsv.DataSource = tbl;
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			} finally {
				this.Enabled = true;
			}
		}

		#endregion

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
				this.buttonAddGroup.Enabled = file.Exists;
				this.buttonAddMember.Enabled = file.Exists;

				var table = file.LoadCsvData();
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

		#endregion

		#region メソッド

		#endregion
	}
}
