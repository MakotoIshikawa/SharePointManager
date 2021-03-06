﻿using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommonFeaturesLibrary.Extensions;
using ExtensionsLibrary.Extensions;
using ObjectAnalysisProject.Extensions;
using SharePointManager.Manager.TermStore;
using WindowsFormsLibrary.Extensions;

namespace TermStoreMngApp {
	/// <summary>
	/// フォーム
	/// </summary>
	public partial class FormTermStoreInfo : Form {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public FormTermStoreInfo() {
			this.InitializeComponent();
#if true
			this.textBoxUrl.Text = Properties.Settings.Default.URL;
			this.textBoxUser.Text = Properties.Settings.Default.User;
			this.textBoxPassword.Text = Properties.Settings.Default.Password;
			this.textBoxGroup.Text = Properties.Settings.Default.Group;
#endif
		}

		#endregion

		#region イベントハンドラ

		/// <summary>
		/// [実行]ボタンのクリックイベントです。
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベントデータ</param>
		private void buttonRun_Click(object sender, EventArgs e) {
			this.RunAction(this.Run);
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
			this.RunAction<TextBox>(sender, tx => {
				var file = new FileInfo(tx.Text);
				this.buttonRun.Enabled = file.Exists;

				var table = GetCsvTable(file);
				this.gridCsv.DataSource = table;
			}, () => {
				this.gridCsv.DataSource = null;
				this.buttonRun.Enabled = false;
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

		#endregion

		#region メソッド

		private void Run() {
			var url = this.textBoxUrl.Text;
			var username = this.textBoxUser.Text;
			var password = this.textBoxPassword.Text;
			var groupName = this.textBoxGroup.Text;

			var tm = new TermStoreManager(url, username, password, groupName) {
				DefaultLcID = 1041,
			};

			var tbl = this.gridCsv.ToDataTable();
			tbl.Select(r => new {
				セット名 = r[0].ToString().Trim(),
				セット説明 = r[1].ToString().Trim(),
				公開 = r[2].ToBoolean(),
				タグ = r[3].ToBoolean(),
				用語 = r[4].ToString().Trim(),
				説明 = r[5].ToString().Trim(),
			}).GroupBy(r => new {
				r.セット名,
				r.セット説明,
				r.公開,
			}).ToList().ForEach(v => {
				var row = v.Select(x => new TermInfo(x.用語) {
					IsAvailableForTagging = x.タグ,
					Description = x.説明,
				}).ToArray();

				var info = new TermSetInfo(v.Key.セット名, row) {
					IsOpenForTermCreation = v.Key.公開,
					Description = v.Key.セット説明,
				};

				tm.AddTerm(info);
			});

			this.ShowMessageBox("用語セットを登録しました。");
		}

		/// <summary>
		/// CSVファイルのデータを加工してテーブルに格納します。
		/// </summary>
		/// <param name="file">ファイル情報</param>
		/// <returns>データテーブル</returns>
		protected static DataTable GetCsvTable(FileInfo file) {
			var csv = file.LoadDataTable();

			var setName = string.Empty;
			var setDesc = string.Empty;
			var isOpen = false;
			var table = csv.Where(row => !string.IsNullOrWhiteSpace(row[4].ToString()))
			.Select(row => {
				var row0 = row[0].ToString();
				if (!string.IsNullOrWhiteSpace(row0)) {
					setName = row0;
				}
				var row1 = row[1].ToString();
				if (!string.IsNullOrWhiteSpace(row1)) {
					setDesc = row1;
				}
				var row2 = row[2].ToString();
				if (!string.IsNullOrWhiteSpace(row2)) {
					isOpen = (row2 == "公開");
				}

				var flg = false;
				bool.TryParse(row[3].ToString(), out flg);

				return new {
					セット名 = setName,
					セット説明 = setDesc,
					公開 = isOpen,
					タグ = flg,
					用語 = row[4].ToString(),
					説明 = row[5].ToString(),
				};
			}).ToDataTable();

			return table;
		}

		#endregion
	}
}
