namespace SharepointListMngApp {
	partial class FormCreateList {
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
			this.buttonCreate = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.textBoxFilePath = new System.Windows.Forms.TextBox();
			this.buttonReference = new System.Windows.Forms.Button();
			this.textBoxUrl = new System.Windows.Forms.TextBox();
			this.textBoxUser = new System.Windows.Forms.TextBox();
			this.textBoxPassword = new System.Windows.Forms.TextBox();
			this.labelUrl = new System.Windows.Forms.Label();
			this.labelUser = new System.Windows.Forms.Label();
			this.labelPassword = new System.Windows.Forms.Label();
			this.gridCsv = new System.Windows.Forms.DataGridView();
			this.labelListName = new System.Windows.Forms.Label();
			this.textBoxListName = new System.Windows.Forms.TextBox();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.labelDescription = new System.Windows.Forms.Label();
			this.textBoxDescription = new System.Windows.Forms.TextBox();
			this.labelListUrl = new System.Windows.Forms.Label();
			this.textBoxListUrl = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.gridCsv)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonCreate
			// 
			this.buttonCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonCreate.Enabled = false;
			this.buttonCreate.Location = new System.Drawing.Point(712, 404);
			this.buttonCreate.Name = "buttonCreate";
			this.buttonCreate.Size = new System.Drawing.Size(75, 23);
			this.buttonCreate.TabIndex = 0;
			this.buttonCreate.Text = "作成";
			this.buttonCreate.UseVisualStyleBackColor = true;
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
			this.textBoxUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxUrl.Enabled = false;
			this.textBoxUrl.Location = new System.Drawing.Point(80, 38);
			this.textBoxUrl.Name = "textBoxUrl";
			this.textBoxUrl.Size = new System.Drawing.Size(662, 19);
			this.textBoxUrl.TabIndex = 3;
			// 
			// textBoxUser
			// 
			this.textBoxUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxUser.Enabled = false;
			this.textBoxUser.Location = new System.Drawing.Point(80, 64);
			this.textBoxUser.Name = "textBoxUser";
			this.textBoxUser.Size = new System.Drawing.Size(662, 19);
			this.textBoxUser.TabIndex = 4;
			// 
			// textBoxPassword
			// 
			this.textBoxPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxPassword.Enabled = false;
			this.textBoxPassword.Location = new System.Drawing.Point(80, 90);
			this.textBoxPassword.Name = "textBoxPassword";
			this.textBoxPassword.Size = new System.Drawing.Size(662, 19);
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
			// gridCsv
			// 
			this.gridCsv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridCsv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridCsv.Location = new System.Drawing.Point(14, 115);
			this.gridCsv.Name = "gridCsv";
			this.gridCsv.RowTemplate.Height = 21;
			this.gridCsv.Size = new System.Drawing.Size(773, 230);
			this.gridCsv.TabIndex = 11;
			// 
			// labelListName
			// 
			this.labelListName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelListName.AutoSize = true;
			this.labelListName.Location = new System.Drawing.Point(12, 354);
			this.labelListName.Name = "labelListName";
			this.labelListName.Size = new System.Drawing.Size(41, 12);
			this.labelListName.TabIndex = 13;
			this.labelListName.Text = "リスト名";
			// 
			// textBoxListName
			// 
			this.textBoxListName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxListName.Location = new System.Drawing.Point(80, 351);
			this.textBoxListName.Name = "textBoxListName";
			this.textBoxListName.Size = new System.Drawing.Size(707, 19);
			this.textBoxListName.TabIndex = 12;
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(712, 429);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 14;
			this.buttonCancel.Text = "キャンセル";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// labelDescription
			// 
			this.labelDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelDescription.AutoSize = true;
			this.labelDescription.Location = new System.Drawing.Point(12, 404);
			this.labelDescription.Name = "labelDescription";
			this.labelDescription.Size = new System.Drawing.Size(41, 12);
			this.labelDescription.TabIndex = 16;
			this.labelDescription.Text = "説明文";
			// 
			// textBoxDescription
			// 
			this.textBoxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxDescription.Location = new System.Drawing.Point(80, 401);
			this.textBoxDescription.Multiline = true;
			this.textBoxDescription.Name = "textBoxDescription";
			this.textBoxDescription.Size = new System.Drawing.Size(626, 51);
			this.textBoxDescription.TabIndex = 15;
			// 
			// labelListUrl
			// 
			this.labelListUrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelListUrl.AutoSize = true;
			this.labelListUrl.Location = new System.Drawing.Point(12, 379);
			this.labelListUrl.Name = "labelListUrl";
			this.labelListUrl.Size = new System.Drawing.Size(51, 12);
			this.labelListUrl.TabIndex = 18;
			this.labelListUrl.Text = "リストURL";
			// 
			// textBoxListUrl
			// 
			this.textBoxListUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxListUrl.Location = new System.Drawing.Point(80, 376);
			this.textBoxListUrl.Name = "textBoxListUrl";
			this.textBoxListUrl.Size = new System.Drawing.Size(707, 19);
			this.textBoxListUrl.TabIndex = 17;
			// 
			// FormCreateList
			// 
			this.AcceptButton = this.buttonCreate;
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(799, 464);
			this.Controls.Add(this.labelListUrl);
			this.Controls.Add(this.textBoxListUrl);
			this.Controls.Add(this.labelDescription);
			this.Controls.Add(this.textBoxDescription);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.labelListName);
			this.Controls.Add(this.textBoxListName);
			this.Controls.Add(this.gridCsv);
			this.Controls.Add(this.labelPassword);
			this.Controls.Add(this.labelUser);
			this.Controls.Add(this.labelUrl);
			this.Controls.Add(this.textBoxPassword);
			this.Controls.Add(this.textBoxUser);
			this.Controls.Add(this.textBoxUrl);
			this.Controls.Add(this.buttonReference);
			this.Controls.Add(this.textBoxFilePath);
			this.Controls.Add(this.buttonCreate);
			this.Name = "FormCreateList";
			this.Text = "新規作成";
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.obj_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.obj_DragEnter);
			((System.ComponentModel.ISupportInitialize)(this.gridCsv)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonCreate;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.TextBox textBoxFilePath;
		private System.Windows.Forms.Button buttonReference;
		private System.Windows.Forms.TextBox textBoxUrl;
		private System.Windows.Forms.TextBox textBoxUser;
		private System.Windows.Forms.TextBox textBoxPassword;
		private System.Windows.Forms.Label labelUrl;
		private System.Windows.Forms.Label labelUser;
		private System.Windows.Forms.Label labelPassword;
		private System.Windows.Forms.DataGridView gridCsv;
		private System.Windows.Forms.Label labelListName;
		private System.Windows.Forms.TextBox textBoxListName;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label labelDescription;
		private System.Windows.Forms.TextBox textBoxDescription;
		private System.Windows.Forms.Label labelListUrl;
		private System.Windows.Forms.TextBox textBoxListUrl;
	}
}

