namespace SharePointGroupMngApp {
	partial class FormGroupMng {
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
			this.buttonAddMember = new System.Windows.Forms.Button();
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
			this.buttonAddGroup = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabAddGroup = new System.Windows.Forms.TabPage();
			this.labelAddGroup = new System.Windows.Forms.Label();
			this.tabAddUser = new System.Windows.Forms.TabPage();
			this.label1 = new System.Windows.Forms.Label();
			this.buttonAddUser = new System.Windows.Forms.Button();
			this.tabAddMember = new System.Windows.Forms.TabPage();
			this.label2 = new System.Windows.Forms.Label();
			this.tabGetGroups = new System.Windows.Forms.TabPage();
			this.tabGetUsers = new System.Windows.Forms.TabPage();
			this.buttonGetGroups = new System.Windows.Forms.Button();
			this.buttonGetUsers = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.gridCsv)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tabAddGroup.SuspendLayout();
			this.tabAddUser.SuspendLayout();
			this.tabAddMember.SuspendLayout();
			this.tabGetGroups.SuspendLayout();
			this.tabGetUsers.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonAddMember
			// 
			this.buttonAddMember.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonAddMember.AutoSize = true;
			this.buttonAddMember.Enabled = false;
			this.buttonAddMember.Location = new System.Drawing.Point(704, 3);
			this.buttonAddMember.Name = "buttonAddMember";
			this.buttonAddMember.Size = new System.Drawing.Size(79, 23);
			this.buttonAddMember.TabIndex = 0;
			this.buttonAddMember.Text = "メンバー登録";
			this.buttonAddMember.UseVisualStyleBackColor = true;
			this.buttonAddMember.Click += new System.EventHandler(this.buttonAddMember_Click);
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
			this.textBoxUrl.Location = new System.Drawing.Point(80, 38);
			this.textBoxUrl.Name = "textBoxUrl";
			this.textBoxUrl.Size = new System.Drawing.Size(662, 19);
			this.textBoxUrl.TabIndex = 3;
			// 
			// textBoxUser
			// 
			this.textBoxUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxUser.Location = new System.Drawing.Point(80, 64);
			this.textBoxUser.Name = "textBoxUser";
			this.textBoxUser.Size = new System.Drawing.Size(662, 19);
			this.textBoxUser.TabIndex = 4;
			// 
			// textBoxPassword
			// 
			this.textBoxPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxPassword.Location = new System.Drawing.Point(80, 90);
			this.textBoxPassword.Name = "textBoxPassword";
			this.textBoxPassword.PasswordChar = '*';
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
			this.gridCsv.Size = new System.Drawing.Size(773, 271);
			this.gridCsv.TabIndex = 11;
			// 
			// buttonAddGroup
			// 
			this.buttonAddGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonAddGroup.AutoSize = true;
			this.buttonAddGroup.Enabled = false;
			this.buttonAddGroup.Location = new System.Drawing.Point(708, 3);
			this.buttonAddGroup.Name = "buttonAddGroup";
			this.buttonAddGroup.Size = new System.Drawing.Size(77, 23);
			this.buttonAddGroup.TabIndex = 12;
			this.buttonAddGroup.Text = "グループ追加";
			this.buttonAddGroup.UseVisualStyleBackColor = true;
			this.buttonAddGroup.Click += new System.EventHandler(this.buttonAddGroup_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabAddGroup);
			this.tabControl1.Controls.Add(this.tabAddMember);
			this.tabControl1.Controls.Add(this.tabAddUser);
			this.tabControl1.Controls.Add(this.tabGetGroups);
			this.tabControl1.Controls.Add(this.tabGetUsers);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tabControl1.Location = new System.Drawing.Point(0, 392);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(799, 55);
			this.tabControl1.TabIndex = 13;
			// 
			// tabAddGroup
			// 
			this.tabAddGroup.Controls.Add(this.labelAddGroup);
			this.tabAddGroup.Controls.Add(this.buttonAddGroup);
			this.tabAddGroup.Location = new System.Drawing.Point(4, 22);
			this.tabAddGroup.Name = "tabAddGroup";
			this.tabAddGroup.Padding = new System.Windows.Forms.Padding(3);
			this.tabAddGroup.Size = new System.Drawing.Size(791, 29);
			this.tabAddGroup.TabIndex = 0;
			this.tabAddGroup.Text = "グループ新規追加";
			this.tabAddGroup.UseVisualStyleBackColor = true;
			// 
			// labelAddGroup
			// 
			this.labelAddGroup.AutoSize = true;
			this.labelAddGroup.Location = new System.Drawing.Point(6, 8);
			this.labelAddGroup.Name = "labelAddGroup";
			this.labelAddGroup.Size = new System.Drawing.Size(199, 12);
			this.labelAddGroup.TabIndex = 13;
			this.labelAddGroup.Text = "必須項目は、[グループ名], [説明] です。";
			// 
			// tabAddUser
			// 
			this.tabAddUser.Controls.Add(this.label1);
			this.tabAddUser.Controls.Add(this.buttonAddUser);
			this.tabAddUser.Location = new System.Drawing.Point(4, 22);
			this.tabAddUser.Name = "tabAddUser";
			this.tabAddUser.Padding = new System.Windows.Forms.Padding(3);
			this.tabAddUser.Size = new System.Drawing.Size(791, 29);
			this.tabAddUser.TabIndex = 1;
			this.tabAddUser.Text = "ユーザー新規追加";
			this.tabAddUser.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(351, 12);
			this.label1.TabIndex = 14;
			this.label1.Text = "必須項目は、[グループ名], [表示名], [ログイン名], [メールアドレス] です。";
			// 
			// buttonAddUser
			// 
			this.buttonAddUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonAddUser.AutoSize = true;
			this.buttonAddUser.Enabled = false;
			this.buttonAddUser.Location = new System.Drawing.Point(704, 3);
			this.buttonAddUser.Name = "buttonAddUser";
			this.buttonAddUser.Size = new System.Drawing.Size(79, 23);
			this.buttonAddUser.TabIndex = 1;
			this.buttonAddUser.Text = "ユーザー追加";
			this.buttonAddUser.UseVisualStyleBackColor = true;
			this.buttonAddUser.Click += new System.EventHandler(this.buttonAddUser_Click);
			// 
			// tabAddMember
			// 
			this.tabAddMember.Controls.Add(this.label2);
			this.tabAddMember.Controls.Add(this.buttonAddMember);
			this.tabAddMember.Location = new System.Drawing.Point(4, 22);
			this.tabAddMember.Name = "tabAddMember";
			this.tabAddMember.Padding = new System.Windows.Forms.Padding(3);
			this.tabAddMember.Size = new System.Drawing.Size(791, 29);
			this.tabAddMember.TabIndex = 2;
			this.tabAddMember.Text = "メンバー関連付け";
			this.tabAddMember.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(224, 12);
			this.label2.TabIndex = 14;
			this.label2.Text = "必須項目は、[グループ名], [メンバー名] です。";
			// 
			// tabGetGroups
			// 
			this.tabGetGroups.Controls.Add(this.buttonGetGroups);
			this.tabGetGroups.Location = new System.Drawing.Point(4, 22);
			this.tabGetGroups.Name = "tabGetGroups";
			this.tabGetGroups.Size = new System.Drawing.Size(791, 29);
			this.tabGetGroups.TabIndex = 3;
			this.tabGetGroups.Text = "グループ情報参照";
			this.tabGetGroups.UseVisualStyleBackColor = true;
			// 
			// tabGetUsers
			// 
			this.tabGetUsers.Controls.Add(this.buttonGetUsers);
			this.tabGetUsers.Location = new System.Drawing.Point(4, 22);
			this.tabGetUsers.Name = "tabGetUsers";
			this.tabGetUsers.Size = new System.Drawing.Size(791, 29);
			this.tabGetUsers.TabIndex = 4;
			this.tabGetUsers.Text = "ユーザー情報参照";
			this.tabGetUsers.UseVisualStyleBackColor = true;
			// 
			// buttonGetGroups
			// 
			this.buttonGetGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonGetGroups.AutoSize = true;
			this.buttonGetGroups.Location = new System.Drawing.Point(708, 3);
			this.buttonGetGroups.Name = "buttonGetGroups";
			this.buttonGetGroups.Size = new System.Drawing.Size(75, 23);
			this.buttonGetGroups.TabIndex = 0;
			this.buttonGetGroups.Text = "取得";
			this.buttonGetGroups.UseVisualStyleBackColor = true;
			this.buttonGetGroups.Click += new System.EventHandler(this.buttonGetGroups_Click);
			// 
			// buttonGetUsers
			// 
			this.buttonGetUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonGetUsers.AutoSize = true;
			this.buttonGetUsers.Location = new System.Drawing.Point(708, 3);
			this.buttonGetUsers.Name = "buttonGetUsers";
			this.buttonGetUsers.Size = new System.Drawing.Size(75, 23);
			this.buttonGetUsers.TabIndex = 1;
			this.buttonGetUsers.Text = "取得";
			this.buttonGetUsers.UseVisualStyleBackColor = true;
			this.buttonGetUsers.Click += new System.EventHandler(this.buttonGetUsers_Click);
			// 
			// FormGroupMng
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(799, 447);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.gridCsv);
			this.Controls.Add(this.labelPassword);
			this.Controls.Add(this.labelUser);
			this.Controls.Add(this.labelUrl);
			this.Controls.Add(this.textBoxPassword);
			this.Controls.Add(this.textBoxUser);
			this.Controls.Add(this.textBoxUrl);
			this.Controls.Add(this.buttonReference);
			this.Controls.Add(this.textBoxFilePath);
			this.Name = "FormGroupMng";
			this.Text = "権限グループ管理";
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.obj_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.obj_DragEnter);
			((System.ComponentModel.ISupportInitialize)(this.gridCsv)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tabAddGroup.ResumeLayout(false);
			this.tabAddGroup.PerformLayout();
			this.tabAddUser.ResumeLayout(false);
			this.tabAddUser.PerformLayout();
			this.tabAddMember.ResumeLayout(false);
			this.tabAddMember.PerformLayout();
			this.tabGetGroups.ResumeLayout(false);
			this.tabGetGroups.PerformLayout();
			this.tabGetUsers.ResumeLayout(false);
			this.tabGetUsers.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonAddMember;
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
		private System.Windows.Forms.Button buttonAddGroup;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabAddGroup;
		private System.Windows.Forms.TabPage tabAddUser;
		private System.Windows.Forms.TabPage tabAddMember;
		private System.Windows.Forms.Button buttonAddUser;
		private System.Windows.Forms.Label labelAddGroup;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TabPage tabGetGroups;
		private System.Windows.Forms.Button buttonGetGroups;
		private System.Windows.Forms.TabPage tabGetUsers;
		private System.Windows.Forms.Button buttonGetUsers;
	}
}

