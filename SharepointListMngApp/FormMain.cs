﻿using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonFeaturesLibrary;
using ExtensionsLibrary.Extensions;
using SharepointListMngApp.Forms;
using SharePointManager.Manager.Lists;
using SharePointManager.MyException;
using WindowsFormsLibrary.Extensions;
using WindowsFormsLibrary.Forms.Primitives;
using SP = Microsoft.SharePoint.Client;

namespace SharepointListMngApp {
	/// <summary>
	/// フォーム
	/// </summary>
	public partial class FormMain : FormEditText {
		#region フィールド

		/// <summary>ログ出力</summary>
		private Logger _log = new Logger();

		#endregion

		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public FormMain() {
			this.InitializeComponent();
#if true
			this.textBoxUrl.Text = Properties.Settings.Default.URL;
			this.textBoxUser.Text = Properties.Settings.Default.User;
			this.textBoxPassword.Text = Properties.Settings.Default.Password;
			this.textBoxListName.Text = Properties.Settings.Default.ListName;

			this.LogRowLimit = Properties.Settings.Default.LogRowLimit;
			this.UniqueKey = Properties.Settings.Default.UniqueKey;
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

		/// <summary>リスト名</summary>
		public string ListName {
			get { return this.textBoxListName.Text.Trim(); }
			set { this.textBoxListName.Text = value; }
		}

		/// <summary>表示ログ最大行数</summary>
		public int LogRowLimit { get; protected set; }

		/// <summary>固有キー</summary>
		public string UniqueKey { get; protected set; }

		#endregion

		#region イベントハンドラ

		#region クリックイベント

		/// <summary>
		/// [一覧取得]クリックイベント
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void buttonLoad_Click(object sender, EventArgs e) {
			this.LoadLists();
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

		#region グリッド行選択イベント

		/// <summary>
		/// グリッド行選択イベント
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void gridListInfo_RowEnter(object sender, DataGridViewCellEventArgs e) {
			var grid = sender as DataGridView;
			if (grid == null) {
				return;
			}

			var cell = grid["タイトル", e.RowIndex];
			this.ListName = cell.Value.ToString();
		}

		#endregion

		#region メニュー

		/// <summary>
		/// [新規作成(N)]クリックイベント
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void NewToolStripMenuItem_Click(object sender, EventArgs e) {
			this.CreateList();
		}

		/// <summary>
		/// [終了(X)]クリックイベント
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void ExitToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Close();
		}

		/// <summary>
		/// [カスタマイズ]クリックイベント
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void CustomizeToolStripMenuItem_Click(object sender, EventArgs e) {
			this.CreateFields(this.ListName);
		}

		/// <summary>
		/// [インポート-カスタムリスト]クリックイベント
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void CustomListToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Import(false);
		}

		/// <summary>
		/// [インポート-ディスカッション掲示板]クリックイベント
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void DiscussionBBSToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Import(true);
		}

		/// <summary>
		/// [リストビュー]クリックイベント
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void ListViewToolStripMenuItem_Click(object sender, EventArgs e) {
			this.ListView();
		}

		/// <summary>
		/// [フォルダ]クリックイベント
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void FolderToolStripMenuItem_Click(object sender, EventArgs e) {
			this.CreateFolders();
		}

		#endregion

		#endregion

		#region メソッド

		/// <summary>
		/// リストの一覧を取得する処理です。
		/// </summary>
		private void LoadLists() {
			try {
				this.Enabled = false;

				var m = new ListCollectionManager(this.Url, this.UserName, this.Password);
				var ls = m.GetLists(
					l => l.Title
					, l => l.Description
					, l => l.ItemCount
				).Select(l => new {
					タイトル = l.Title,
					説明 = l.Description,
					件数 = l.ItemCount,
				}).ToList();
				this.gridListInfo.DataSource = ls;
			} catch (Exception ex) {
				this.WriteLineMessage(ex.Message);
			} finally {
				this.Enabled = true;
			}
		}

		/// <summary>
		/// リスト作成
		/// </summary>
		private void CreateList() {
			try {
				using (var f = new FormCreateList(this.Url, this.UserName, this.Password)) {
					f.Manager.Created += (s, e) => {
						this.WriteLineMessage(e.Message);

						var sb = new StringBuilder();
						sb.AppendLine(e.Message);
						sb.AppendLine("リストに列を追加しますか？");

						var ret = this.ShowMessageBox(sb.ToString(), "確認"
							, buttons: MessageBoxButtons.YesNo
							, icon: MessageBoxIcon.Question
							, defaultButton: MessageBoxDefaultButton.Button2
						);

						// リスト名
						var listName = e.Value;

						switch (ret) {
						case DialogResult.Yes:
							// フィールド拡張
							this.CreateFields(listName);
							break;
						case DialogResult.No:
						default:
							return;
						}
					};

					switch (f.ShowDialog(this)) {
					case DialogResult.OK:
						f.Run();
						break;
					case DialogResult.Cancel:
						this.WriteLineMessage("リストを作成しませんでした。");
						break;
					}
				}
			} catch (DuplicateException ex) {
				this.WriteLineMessage(ex.Message);
			} catch (SP.ServerException ex) {
				this.WriteLineMessage(ex.Message);
			} catch (SP.PropertyOrFieldNotInitializedException ex) {
				this.WriteLineMessage(ex.Message);
			} catch (ArgumentException ex) {
				this.WriteLineMessage(ex.Message);
			} catch (Exception ex) {
				this.ShowMessageBox(ex.ToString(), icon: MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// 列の作成
		/// </summary>
		/// <param name="listName">リスト名</param>
		private void CreateFields(string listName) {
			try {
				using (var f = new FormCreateFields(this.Url, this.UserName, this.Password, listName)) {
					f.Manager.AddedField += (s, e) => this.WriteLineMessage(e.Message);
					f.Manager.Success += (s, e) => this.WriteLineMessage(e.Message);
					f.Manager.ThrowException += (s, e) => this.WriteLineMessage(e.Message);

					var ret = f.ShowDialog(this);
					switch (ret) {
					case DialogResult.OK:
						f.Run();
						break;
					case DialogResult.Cancel:
						this.WriteLineMessage("列を作成しませんでした。");
						break;
					}
				}
			} catch (SP.ServerException ex) {
				this.WriteLineMessage(ex.Message);
			} catch (SP.PropertyOrFieldNotInitializedException ex) {
				this.WriteLineMessage(ex.Message);
			} catch (ArgumentException ex) {
				this.WriteLineMessage(ex.Message);
			} catch (Exception ex) {
				this.ShowMessageBox(ex.ToString(), icon: MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// インポート
		/// </summary>
		/// <param name="isFolder">追加するアイテムがフォルダかどうか</param>
		private void Import(bool isFolder) {
			try {
				using (var f = new FormImport(this.Url, this.UserName, this.Password, this.ListName) {
					IsFolder = isFolder,
				}) {
					f.Manager.AddedItem += (s, e) => this.WriteLineMessage(e.Message);
					f.Manager.Success += (s, e) => this.WriteLineMessage(e.Message);
					f.Manager.ThrowException += (s, e) => this.WriteLineMessage(e.Message);

					var ret = f.ShowDialog(this);
					switch (ret) {
					case DialogResult.OK:
						f.Run();
						break;
					case DialogResult.Cancel:
						this.WriteLineMessage("データをインポートしませんでした。");
						break;
					}
				}
			} catch (SP.ServerException ex) {
				this.WriteLineMessage(ex.Message);
			} catch (SP.PropertyOrFieldNotInitializedException ex) {
				this.WriteLineMessage(ex.Message);
			} catch (ArgumentException ex) {
				this.WriteLineMessage(ex.Message);
			} catch (Exception ex) {
				this.ShowMessageBox(ex.ToString(), icon: MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// フォルダー生成処理
		/// </summary>
		private void CreateFolders() {
			try {
				using (var f = new FormCreateFolders(this.Url, this.UserName, this.Password, this.ListName)) {
					f.Manager.AddedItem += (s, e) => this.WriteLineMessage(e.Message);
					f.Manager.Success += (s, e) => this.WriteLineMessage(e.Message);
					f.Manager.ThrowException += (s, e) => this.WriteLineMessage(e.Message);

					var ret = f.ShowDialog(this);
					switch (ret) {
					case DialogResult.OK:
						f.Run();
						break;
					case DialogResult.Cancel:
						this.WriteLineMessage("フォルダーの作成をキャンセルしました。");
						break;
					}
				}
			} catch (SP.ServerException ex) {
				this.WriteLineMessage(ex.Message);
				this.WriteException(ex);
			} catch (SP.PropertyOrFieldNotInitializedException ex) {
				this.WriteLineMessage(ex.Message);
				this.WriteException(ex);
			} catch (ArgumentException ex) {
				this.WriteLineMessage(ex.Message);
				this.WriteException(ex);
			} catch (Exception ex) {
				this.ShowMessageBox(ex.ToString(), icon: MessageBoxIcon.Error);
				this.WriteException(ex);
			}
		}

		/// <summary>
		/// リストビュー
		/// </summary>
		private void ListView() {
			try {
				using (var f = new FormListView(this.Url, this.UserName, this.Password, this.ListName)) {
					f.Manager.AddedItem += (s, e) => this.WriteLineMessage(e.Message);
					f.Manager.Success += (s, e) => this.WriteLineMessage(e.Message);
					f.Manager.ThrowException += (s, e) => this.WriteLineMessage(e.Message);

					var ret = f.ShowDialog(this);
					switch (ret) {
					case DialogResult.OK:
					case DialogResult.Cancel:
					default:
						break;
					}
				}
			} catch (SP.ServerException ex) {
				this.WriteLineMessage(ex.Message);
			} catch (SP.PropertyOrFieldNotInitializedException ex) {
				this.WriteLineMessage(ex.Message);
			} catch (ArgumentException ex) {
				this.WriteLineMessage(ex.Message);
			} catch (Exception ex) {
				this.ShowMessageBox(ex.ToString(), icon: MessageBoxIcon.Error);
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
