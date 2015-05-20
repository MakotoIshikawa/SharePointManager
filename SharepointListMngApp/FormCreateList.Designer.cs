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
			this.components = new System.ComponentModel.Container();
			this.buttonCreate = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.labelListName = new System.Windows.Forms.Label();
			this.textBoxListName = new System.Windows.Forms.TextBox();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.labelDescription = new System.Windows.Forms.Label();
			this.textBoxDescription = new System.Windows.Forms.TextBox();
			this.labelListUrl = new System.Windows.Forms.Label();
			this.textBoxListUrl = new System.Windows.Forms.TextBox();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonCreate
			// 
			this.buttonCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonCreate.Location = new System.Drawing.Point(206, 103);
			this.buttonCreate.Name = "buttonCreate";
			this.buttonCreate.Size = new System.Drawing.Size(75, 23);
			this.buttonCreate.TabIndex = 3;
			this.buttonCreate.Text = "作成";
			this.buttonCreate.UseVisualStyleBackColor = true;
			this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "CSVファイル(*.csv;*.txt)|*.csv;*.txt|すべてのファイル(*.*)|*.*";
			this.openFileDialog.InitialDirectory = "C:\\PowerShell";
			// 
			// labelListName
			// 
			this.labelListName.AutoSize = true;
			this.labelListName.Location = new System.Drawing.Point(12, 15);
			this.labelListName.Name = "labelListName";
			this.labelListName.Size = new System.Drawing.Size(41, 12);
			this.labelListName.TabIndex = 5;
			this.labelListName.Text = "リスト名";
			// 
			// textBoxListName
			// 
			this.textBoxListName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxListName.Location = new System.Drawing.Point(80, 12);
			this.textBoxListName.Name = "textBoxListName";
			this.textBoxListName.Size = new System.Drawing.Size(259, 19);
			this.textBoxListName.TabIndex = 0;
			this.textBoxListName.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxListName_Validating);
			this.textBoxListName.Validated += new System.EventHandler(this.TextBoxValidated);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(287, 103);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 4;
			this.buttonCancel.Text = "キャンセル";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// labelDescription
			// 
			this.labelDescription.AutoSize = true;
			this.labelDescription.Location = new System.Drawing.Point(12, 65);
			this.labelDescription.Name = "labelDescription";
			this.labelDescription.Size = new System.Drawing.Size(29, 12);
			this.labelDescription.TabIndex = 7;
			this.labelDescription.Text = "説明";
			// 
			// textBoxDescription
			// 
			this.textBoxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxDescription.Location = new System.Drawing.Point(80, 62);
			this.textBoxDescription.Multiline = true;
			this.textBoxDescription.Name = "textBoxDescription";
			this.textBoxDescription.Size = new System.Drawing.Size(259, 35);
			this.textBoxDescription.TabIndex = 2;
			// 
			// labelListUrl
			// 
			this.labelListUrl.AutoSize = true;
			this.labelListUrl.Location = new System.Drawing.Point(12, 40);
			this.labelListUrl.Name = "labelListUrl";
			this.labelListUrl.Size = new System.Drawing.Size(51, 12);
			this.labelListUrl.TabIndex = 6;
			this.labelListUrl.Text = "リストURL";
			// 
			// textBoxListUrl
			// 
			this.textBoxListUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxListUrl.Location = new System.Drawing.Point(80, 37);
			this.textBoxListUrl.Name = "textBoxListUrl";
			this.textBoxListUrl.Size = new System.Drawing.Size(259, 19);
			this.textBoxListUrl.TabIndex = 1;
			this.textBoxListUrl.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxListUrl_Validating);
			this.textBoxListUrl.Validated += new System.EventHandler(this.TextBoxValidated);
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// FormCreateList
			// 
			this.AcceptButton = this.buttonCreate;
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(374, 138);
			this.Controls.Add(this.labelListUrl);
			this.Controls.Add(this.textBoxListUrl);
			this.Controls.Add(this.labelDescription);
			this.Controls.Add(this.textBoxDescription);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.labelListName);
			this.Controls.Add(this.textBoxListName);
			this.Controls.Add(this.buttonCreate);
			this.MinimumSize = new System.Drawing.Size(390, 176);
			this.Name = "FormCreateList";
			this.Text = "新規作成";
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonCreate;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Label labelListName;
		private System.Windows.Forms.TextBox textBoxListName;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label labelDescription;
		private System.Windows.Forms.TextBox textBoxDescription;
		private System.Windows.Forms.Label labelListUrl;
		private System.Windows.Forms.TextBox textBoxListUrl;
		private System.Windows.Forms.ErrorProvider errorProvider;
	}
}

