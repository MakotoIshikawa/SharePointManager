namespace TermStoreMngApp {
	partial class FormTermStoreInfo {
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent() {
			this.buttonRun = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.textBoxFilePath = new System.Windows.Forms.TextBox();
			this.buttonReference = new System.Windows.Forms.Button();
			this.textBoxUrl = new System.Windows.Forms.TextBox();
			this.textBoxUser = new System.Windows.Forms.TextBox();
			this.textBoxPassword = new System.Windows.Forms.TextBox();
			this.labelUrl = new System.Windows.Forms.Label();
			this.labelUser = new System.Windows.Forms.Label();
			this.labelPassword = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxGroup = new System.Windows.Forms.TextBox();
			this.gridCsv = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)(this.gridCsv)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonRun
			// 
			this.buttonRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonRun.Enabled = false;
			this.buttonRun.Location = new System.Drawing.Point(712, 412);
			this.buttonRun.Name = "buttonRun";
			this.buttonRun.Size = new System.Drawing.Size(75, 23);
			this.buttonRun.TabIndex = 0;
			this.buttonRun.Text = "実行";
			this.buttonRun.UseVisualStyleBackColor = true;
			this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "CSVファイル(*.csv;*.txt)|*.csv;*.txt|すべてのファイル(*.*)|*.*";
			this.openFileDialog.InitialDirectory = "C:\\PowerShell";
			// 
			// textBoxFilePath
			// 
			this.textBoxFilePath.AllowDrop = true;
			this.textBoxFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxFilePath.Location = new System.Drawing.Point(12, 12);
			this.textBoxFilePath.Name = "textBoxFilePath";
			this.textBoxFilePath.Size = new System.Drawing.Size(730, 19);
			this.textBoxFilePath.TabIndex = 1;
			this.textBoxFilePath.TextChanged += new System.EventHandler(this.textBoxFilePath_TextChanged);
			this.textBoxFilePath.DragDrop += new System.Windows.Forms.DragEventHandler(this.obj_DragDrop);
			this.textBoxFilePath.DragEnter += new System.Windows.Forms.DragEventHandler(this.obj_DragEnter);
			// 
			// buttonReference
			// 
			this.buttonReference.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonReference.AutoSize = true;
			this.buttonReference.Location = new System.Drawing.Point(748, 10);
			this.buttonReference.Name = "buttonReference";
			this.buttonReference.Size = new System.Drawing.Size(39, 23);
			this.buttonReference.TabIndex = 2;
			this.buttonReference.Text = "参照";
			this.buttonReference.UseVisualStyleBackColor = true;
			this.buttonReference.Click += new System.EventHandler(this.buttonReference_Click);
			// 
			// textBoxUrl
			// 
			this.textBoxUrl.Location = new System.Drawing.Point(80, 38);
			this.textBoxUrl.Name = "textBoxUrl";
			this.textBoxUrl.Size = new System.Drawing.Size(358, 19);
			this.textBoxUrl.TabIndex = 3;
			// 
			// textBoxUser
			// 
			this.textBoxUser.Location = new System.Drawing.Point(80, 64);
			this.textBoxUser.Name = "textBoxUser";
			this.textBoxUser.Size = new System.Drawing.Size(358, 19);
			this.textBoxUser.TabIndex = 4;
			// 
			// textBoxPassword
			// 
			this.textBoxPassword.Location = new System.Drawing.Point(80, 90);
			this.textBoxPassword.Name = "textBoxPassword";
			this.textBoxPassword.Size = new System.Drawing.Size(358, 19);
			this.textBoxPassword.TabIndex = 5;
			// 
			// labelUrl
			// 
			this.labelUrl.AutoSize = true;
			this.labelUrl.Location = new System.Drawing.Point(12, 41);
			this.labelUrl.Name = "labelUrl";
			this.labelUrl.Size = new System.Drawing.Size(27, 12);
			this.labelUrl.TabIndex = 6;
			this.labelUrl.Text = "URL";
			// 
			// labelUser
			// 
			this.labelUser.AutoSize = true;
			this.labelUser.Location = new System.Drawing.Point(12, 67);
			this.labelUser.Name = "labelUser";
			this.labelUser.Size = new System.Drawing.Size(46, 12);
			this.labelUser.TabIndex = 7;
			this.labelUser.Text = "ユーザID";
			// 
			// labelPassword
			// 
			this.labelPassword.AutoSize = true;
			this.labelPassword.Location = new System.Drawing.Point(12, 93);
			this.labelPassword.Name = "labelPassword";
			this.labelPassword.Size = new System.Drawing.Size(52, 12);
			this.labelPassword.TabIndex = 8;
			this.labelPassword.Text = "パスワード";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 118);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(55, 12);
			this.label1.TabIndex = 10;
			this.label1.Text = "グループ名";
			// 
			// textBoxGroup
			// 
			this.textBoxGroup.Location = new System.Drawing.Point(80, 115);
			this.textBoxGroup.Name = "textBoxGroup";
			this.textBoxGroup.Size = new System.Drawing.Size(358, 19);
			this.textBoxGroup.TabIndex = 9;
			// 
			// gridCsv
			// 
			this.gridCsv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridCsv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridCsv.Location = new System.Drawing.Point(14, 140);
			this.gridCsv.Name = "gridCsv";
			this.gridCsv.RowTemplate.Height = 21;
			this.gridCsv.Size = new System.Drawing.Size(773, 266);
			this.gridCsv.TabIndex = 11;
			// 
			// FormTermStoreInfo
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(799, 447);
			this.Controls.Add(this.gridCsv);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBoxGroup);
			this.Controls.Add(this.labelPassword);
			this.Controls.Add(this.labelUser);
			this.Controls.Add(this.labelUrl);
			this.Controls.Add(this.textBoxPassword);
			this.Controls.Add(this.textBoxUser);
			this.Controls.Add(this.textBoxUrl);
			this.Controls.Add(this.buttonReference);
			this.Controls.Add(this.textBoxFilePath);
			this.Controls.Add(this.buttonRun);
			this.Name = "FormTermStoreInfo";
			this.Text = "用語セット管理";
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.obj_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.obj_DragEnter);
			((System.ComponentModel.ISupportInitialize)(this.gridCsv)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonRun;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.TextBox textBoxFilePath;
		private System.Windows.Forms.Button buttonReference;
		private System.Windows.Forms.TextBox textBoxUrl;
		private System.Windows.Forms.TextBox textBoxUser;
		private System.Windows.Forms.TextBox textBoxPassword;
		private System.Windows.Forms.Label labelUrl;
		private System.Windows.Forms.Label labelUser;
		private System.Windows.Forms.Label labelPassword;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxGroup;
		private System.Windows.Forms.DataGridView gridCsv;
	}
}

