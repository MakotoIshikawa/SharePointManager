namespace SharepointListMngApp {
	partial class FormMain {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			this.textBoxUrl = new System.Windows.Forms.TextBox();
			this.textBoxUser = new System.Windows.Forms.TextBox();
			this.textBoxPassword = new System.Windows.Forms.TextBox();
			this.labelUrl = new System.Windows.Forms.Label();
			this.labelUser = new System.Windows.Forms.Label();
			this.labelPassword = new System.Windows.Forms.Label();
			this.gridListInfo = new System.Windows.Forms.DataGridView();
			this.labelListName = new System.Windows.Forms.Label();
			this.textBoxListName = new System.Windows.Forms.TextBox();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.listBoxMessage = new System.Windows.Forms.ListBox();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.ファイルFToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.新規作成NToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.開くOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.上書き保存SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.名前を付けて保存AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.印刷PToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.印刷プレビューVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.終了XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.編集EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.元に戻すUToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.やり直しRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.切り取りTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.コピーCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.貼り付けPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.すべて選択AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ツールTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.カスタマイズCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.オプションOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ヘルプHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.内容CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.インデックスIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.検索SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.バージョン情報AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.buttonLoad = new System.Windows.Forms.Button();
			this.インポートIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.gridListInfo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.menuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// textBoxUrl
			// 
			this.textBoxUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxUrl.Location = new System.Drawing.Point(80, 29);
			this.textBoxUrl.Name = "textBoxUrl";
			this.textBoxUrl.Size = new System.Drawing.Size(472, 19);
			this.textBoxUrl.TabIndex = 3;
			// 
			// textBoxUser
			// 
			this.textBoxUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxUser.Location = new System.Drawing.Point(80, 54);
			this.textBoxUser.Name = "textBoxUser";
			this.textBoxUser.Size = new System.Drawing.Size(472, 19);
			this.textBoxUser.TabIndex = 4;
			// 
			// textBoxPassword
			// 
			this.textBoxPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxPassword.Location = new System.Drawing.Point(80, 79);
			this.textBoxPassword.Name = "textBoxPassword";
			this.textBoxPassword.Size = new System.Drawing.Size(472, 19);
			this.textBoxPassword.TabIndex = 5;
			// 
			// labelUrl
			// 
			this.labelUrl.AutoSize = true;
			this.labelUrl.Location = new System.Drawing.Point(12, 32);
			this.labelUrl.Name = "labelUrl";
			this.labelUrl.Size = new System.Drawing.Size(27, 12);
			this.labelUrl.TabIndex = 6;
			this.labelUrl.Text = "URL";
			// 
			// labelUser
			// 
			this.labelUser.AutoSize = true;
			this.labelUser.Location = new System.Drawing.Point(12, 57);
			this.labelUser.Name = "labelUser";
			this.labelUser.Size = new System.Drawing.Size(46, 12);
			this.labelUser.TabIndex = 7;
			this.labelUser.Text = "ユーザID";
			// 
			// labelPassword
			// 
			this.labelPassword.AutoSize = true;
			this.labelPassword.Location = new System.Drawing.Point(12, 82);
			this.labelPassword.Name = "labelPassword";
			this.labelPassword.Size = new System.Drawing.Size(52, 12);
			this.labelPassword.TabIndex = 8;
			this.labelPassword.Text = "パスワード";
			// 
			// gridListInfo
			// 
			this.gridListInfo.AllowUserToAddRows = false;
			this.gridListInfo.AllowUserToDeleteRows = false;
			this.gridListInfo.AllowUserToOrderColumns = true;
			this.gridListInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridListInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridListInfo.Location = new System.Drawing.Point(0, 0);
			this.gridListInfo.Name = "gridListInfo";
			this.gridListInfo.ReadOnly = true;
			this.gridListInfo.RowTemplate.Height = 21;
			this.gridListInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridListInfo.Size = new System.Drawing.Size(540, 148);
			this.gridListInfo.TabIndex = 11;
			this.gridListInfo.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridListInfo_RowEnter);
			// 
			// labelListName
			// 
			this.labelListName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelListName.AutoSize = true;
			this.labelListName.Location = new System.Drawing.Point(12, 412);
			this.labelListName.Name = "labelListName";
			this.labelListName.Size = new System.Drawing.Size(41, 12);
			this.labelListName.TabIndex = 13;
			this.labelListName.Text = "リスト名";
			// 
			// textBoxListName
			// 
			this.textBoxListName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxListName.Location = new System.Drawing.Point(80, 409);
			this.textBoxListName.Name = "textBoxListName";
			this.textBoxListName.Size = new System.Drawing.Size(391, 19);
			this.textBoxListName.TabIndex = 12;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(12, 104);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.gridListInfo);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.listBoxMessage);
			this.splitContainer1.Size = new System.Drawing.Size(540, 297);
			this.splitContainer1.SplitterDistance = 148;
			this.splitContainer1.TabIndex = 14;
			// 
			// listBoxMessage
			// 
			this.listBoxMessage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBoxMessage.FormattingEnabled = true;
			this.listBoxMessage.ItemHeight = 12;
			this.listBoxMessage.Location = new System.Drawing.Point(0, 0);
			this.listBoxMessage.Name = "listBoxMessage";
			this.listBoxMessage.Size = new System.Drawing.Size(540, 145);
			this.listBoxMessage.TabIndex = 22;
			this.listBoxMessage.DoubleClick += new System.EventHandler(this.listBoxMessage_DoubleClick);
			// 
			// notifyIcon
			// 
			this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			this.notifyIcon.Text = "端末装置制御";
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルFToolStripMenuItem1,
            this.編集EToolStripMenuItem,
            this.ツールTToolStripMenuItem,
            this.ヘルプHToolStripMenuItem});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.menuStrip.Size = new System.Drawing.Size(564, 26);
			this.menuStrip.TabIndex = 15;
			this.menuStrip.Text = "menuStrip1";
			// 
			// ファイルFToolStripMenuItem1
			// 
			this.ファイルFToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新規作成NToolStripMenuItem,
            this.開くOToolStripMenuItem,
            this.toolStripSeparator,
            this.上書き保存SToolStripMenuItem,
            this.名前を付けて保存AToolStripMenuItem,
            this.toolStripSeparator1,
            this.印刷PToolStripMenuItem,
            this.印刷プレビューVToolStripMenuItem,
            this.toolStripSeparator2,
            this.終了XToolStripMenuItem});
			this.ファイルFToolStripMenuItem1.Name = "ファイルFToolStripMenuItem1";
			this.ファイルFToolStripMenuItem1.Size = new System.Drawing.Size(85, 22);
			this.ファイルFToolStripMenuItem1.Text = "ファイル(&F)";
			// 
			// 新規作成NToolStripMenuItem
			// 
			this.新規作成NToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("新規作成NToolStripMenuItem.Image")));
			this.新規作成NToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.新規作成NToolStripMenuItem.Name = "新規作成NToolStripMenuItem";
			this.新規作成NToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.新規作成NToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
			this.新規作成NToolStripMenuItem.Text = "新規作成(&N)";
			this.新規作成NToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
			// 
			// 開くOToolStripMenuItem
			// 
			this.開くOToolStripMenuItem.Enabled = false;
			this.開くOToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("開くOToolStripMenuItem.Image")));
			this.開くOToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.開くOToolStripMenuItem.Name = "開くOToolStripMenuItem";
			this.開くOToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.開くOToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
			this.開くOToolStripMenuItem.Text = "開く(&O)";
			// 
			// toolStripSeparator
			// 
			this.toolStripSeparator.Name = "toolStripSeparator";
			this.toolStripSeparator.Size = new System.Drawing.Size(198, 6);
			// 
			// 上書き保存SToolStripMenuItem
			// 
			this.上書き保存SToolStripMenuItem.Enabled = false;
			this.上書き保存SToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("上書き保存SToolStripMenuItem.Image")));
			this.上書き保存SToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.上書き保存SToolStripMenuItem.Name = "上書き保存SToolStripMenuItem";
			this.上書き保存SToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.上書き保存SToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
			this.上書き保存SToolStripMenuItem.Text = "上書き保存(&S)";
			// 
			// 名前を付けて保存AToolStripMenuItem
			// 
			this.名前を付けて保存AToolStripMenuItem.Enabled = false;
			this.名前を付けて保存AToolStripMenuItem.Name = "名前を付けて保存AToolStripMenuItem";
			this.名前を付けて保存AToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
			this.名前を付けて保存AToolStripMenuItem.Text = "名前を付けて保存(&A)";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(198, 6);
			// 
			// 印刷PToolStripMenuItem
			// 
			this.印刷PToolStripMenuItem.Enabled = false;
			this.印刷PToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("印刷PToolStripMenuItem.Image")));
			this.印刷PToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.印刷PToolStripMenuItem.Name = "印刷PToolStripMenuItem";
			this.印刷PToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
			this.印刷PToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
			this.印刷PToolStripMenuItem.Text = "印刷(&P)";
			// 
			// 印刷プレビューVToolStripMenuItem
			// 
			this.印刷プレビューVToolStripMenuItem.Enabled = false;
			this.印刷プレビューVToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("印刷プレビューVToolStripMenuItem.Image")));
			this.印刷プレビューVToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.印刷プレビューVToolStripMenuItem.Name = "印刷プレビューVToolStripMenuItem";
			this.印刷プレビューVToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
			this.印刷プレビューVToolStripMenuItem.Text = "印刷プレビュー(&V)";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(198, 6);
			// 
			// 終了XToolStripMenuItem
			// 
			this.終了XToolStripMenuItem.Name = "終了XToolStripMenuItem";
			this.終了XToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
			this.終了XToolStripMenuItem.Text = "終了(&X)";
			this.終了XToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
			// 
			// 編集EToolStripMenuItem
			// 
			this.編集EToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.元に戻すUToolStripMenuItem,
            this.やり直しRToolStripMenuItem,
            this.toolStripSeparator3,
            this.切り取りTToolStripMenuItem,
            this.コピーCToolStripMenuItem,
            this.貼り付けPToolStripMenuItem,
            this.toolStripSeparator4,
            this.すべて選択AToolStripMenuItem});
			this.編集EToolStripMenuItem.Enabled = false;
			this.編集EToolStripMenuItem.Name = "編集EToolStripMenuItem";
			this.編集EToolStripMenuItem.Size = new System.Drawing.Size(61, 22);
			this.編集EToolStripMenuItem.Text = "編集(&E)";
			// 
			// 元に戻すUToolStripMenuItem
			// 
			this.元に戻すUToolStripMenuItem.Name = "元に戻すUToolStripMenuItem";
			this.元に戻すUToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this.元に戻すUToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
			this.元に戻すUToolStripMenuItem.Text = "元に戻す(&U)";
			// 
			// やり直しRToolStripMenuItem
			// 
			this.やり直しRToolStripMenuItem.Name = "やり直しRToolStripMenuItem";
			this.やり直しRToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
			this.やり直しRToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
			this.やり直しRToolStripMenuItem.Text = "やり直し(&R)";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(187, 6);
			// 
			// 切り取りTToolStripMenuItem
			// 
			this.切り取りTToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("切り取りTToolStripMenuItem.Image")));
			this.切り取りTToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.切り取りTToolStripMenuItem.Name = "切り取りTToolStripMenuItem";
			this.切り取りTToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.切り取りTToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
			this.切り取りTToolStripMenuItem.Text = "切り取り(&T)";
			// 
			// コピーCToolStripMenuItem
			// 
			this.コピーCToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("コピーCToolStripMenuItem.Image")));
			this.コピーCToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.コピーCToolStripMenuItem.Name = "コピーCToolStripMenuItem";
			this.コピーCToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.コピーCToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
			this.コピーCToolStripMenuItem.Text = "コピー(&C)";
			// 
			// 貼り付けPToolStripMenuItem
			// 
			this.貼り付けPToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("貼り付けPToolStripMenuItem.Image")));
			this.貼り付けPToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.貼り付けPToolStripMenuItem.Name = "貼り付けPToolStripMenuItem";
			this.貼り付けPToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.貼り付けPToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
			this.貼り付けPToolStripMenuItem.Text = "貼り付け(&P)";
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(187, 6);
			// 
			// すべて選択AToolStripMenuItem
			// 
			this.すべて選択AToolStripMenuItem.Name = "すべて選択AToolStripMenuItem";
			this.すべて選択AToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
			this.すべて選択AToolStripMenuItem.Text = "すべて選択(&A)";
			// 
			// ツールTToolStripMenuItem
			// 
			this.ツールTToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.カスタマイズCToolStripMenuItem,
            this.インポートIToolStripMenuItem,
            this.オプションOToolStripMenuItem});
			this.ツールTToolStripMenuItem.Name = "ツールTToolStripMenuItem";
			this.ツールTToolStripMenuItem.Size = new System.Drawing.Size(74, 22);
			this.ツールTToolStripMenuItem.Text = "ツール(&T)";
			// 
			// カスタマイズCToolStripMenuItem
			// 
			this.カスタマイズCToolStripMenuItem.Name = "カスタマイズCToolStripMenuItem";
			this.カスタマイズCToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
			this.カスタマイズCToolStripMenuItem.Text = "カスタマイズ(&C)";
			this.カスタマイズCToolStripMenuItem.Click += new System.EventHandler(this.CustomizeToolStripMenuItem_Click);
			// 
			// オプションOToolStripMenuItem
			// 
			this.オプションOToolStripMenuItem.Enabled = false;
			this.オプションOToolStripMenuItem.Name = "オプションOToolStripMenuItem";
			this.オプションOToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
			this.オプションOToolStripMenuItem.Text = "オプション(&O)";
			// 
			// ヘルプHToolStripMenuItem
			// 
			this.ヘルプHToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.内容CToolStripMenuItem,
            this.インデックスIToolStripMenuItem,
            this.検索SToolStripMenuItem,
            this.toolStripSeparator5,
            this.バージョン情報AToolStripMenuItem});
			this.ヘルプHToolStripMenuItem.Enabled = false;
			this.ヘルプHToolStripMenuItem.Name = "ヘルプHToolStripMenuItem";
			this.ヘルプHToolStripMenuItem.Size = new System.Drawing.Size(75, 22);
			this.ヘルプHToolStripMenuItem.Text = "ヘルプ(&H)";
			// 
			// 内容CToolStripMenuItem
			// 
			this.内容CToolStripMenuItem.Name = "内容CToolStripMenuItem";
			this.内容CToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
			this.内容CToolStripMenuItem.Text = "内容(&C)";
			// 
			// インデックスIToolStripMenuItem
			// 
			this.インデックスIToolStripMenuItem.Name = "インデックスIToolStripMenuItem";
			this.インデックスIToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
			this.インデックスIToolStripMenuItem.Text = "インデックス(&I)";
			// 
			// 検索SToolStripMenuItem
			// 
			this.検索SToolStripMenuItem.Name = "検索SToolStripMenuItem";
			this.検索SToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
			this.検索SToolStripMenuItem.Text = "検索(&S)";
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(187, 6);
			// 
			// バージョン情報AToolStripMenuItem
			// 
			this.バージョン情報AToolStripMenuItem.Name = "バージョン情報AToolStripMenuItem";
			this.バージョン情報AToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
			this.バージョン情報AToolStripMenuItem.Text = "バージョン情報(&A)...";
			// 
			// buttonLoad
			// 
			this.buttonLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonLoad.Location = new System.Drawing.Point(477, 407);
			this.buttonLoad.Name = "buttonLoad";
			this.buttonLoad.Size = new System.Drawing.Size(75, 23);
			this.buttonLoad.TabIndex = 16;
			this.buttonLoad.Text = "読込";
			this.buttonLoad.UseVisualStyleBackColor = true;
			this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
			// 
			// インポートIToolStripMenuItem
			// 
			this.インポートIToolStripMenuItem.Name = "インポートIToolStripMenuItem";
			this.インポートIToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
			this.インポートIToolStripMenuItem.Text = "インポート(&I)";
			this.インポートIToolStripMenuItem.Click += new System.EventHandler(this.ImportToolStripMenuItem_Click);
			// 
			// FormMain
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(564, 442);
			this.Controls.Add(this.buttonLoad);
			this.Controls.Add(this.menuStrip);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.labelListName);
			this.Controls.Add(this.textBoxListName);
			this.Controls.Add(this.labelPassword);
			this.Controls.Add(this.labelUser);
			this.Controls.Add(this.labelUrl);
			this.Controls.Add(this.textBoxPassword);
			this.Controls.Add(this.textBoxUser);
			this.Controls.Add(this.textBoxUrl);
			this.Name = "FormMain";
			this.Text = "リスト管理アプリケーション";
			((System.ComponentModel.ISupportInitialize)(this.gridListInfo)).EndInit();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBoxUrl;
		private System.Windows.Forms.TextBox textBoxUser;
		private System.Windows.Forms.TextBox textBoxPassword;
		private System.Windows.Forms.Label labelUrl;
		private System.Windows.Forms.Label labelUser;
		private System.Windows.Forms.Label labelPassword;
		private System.Windows.Forms.DataGridView gridListInfo;
		private System.Windows.Forms.Label labelListName;
		private System.Windows.Forms.TextBox textBoxListName;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ListBox listBoxMessage;
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem ファイルFToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem 新規作成NToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 開くOToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
		private System.Windows.Forms.ToolStripMenuItem 上書き保存SToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 名前を付けて保存AToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem 印刷PToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 印刷プレビューVToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem 終了XToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 編集EToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 元に戻すUToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem やり直しRToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem 切り取りTToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem コピーCToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 貼り付けPToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem すべて選択AToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ツールTToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem カスタマイズCToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem オプションOToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ヘルプHToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 内容CToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem インデックスIToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 検索SToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem バージョン情報AToolStripMenuItem;
		private System.Windows.Forms.Button buttonLoad;
		private System.Windows.Forms.ToolStripMenuItem インポートIToolStripMenuItem;
	}
}

