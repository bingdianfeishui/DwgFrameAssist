namespace DwgFrameAssist
{
    partial class SingleDwgInfo
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ckForceUpdateFrame = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbFrameSize = new System.Windows.Forms.ComboBox();
            this.cmbDirection = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbItemType = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtPage = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTotalPages = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnModify = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.txtScale = new System.Windows.Forms.TextBox();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.cmbSecret = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDwgName = new System.Windows.Forms.TextBox();
            this.txtDwgNO = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ckForceUpdateFrame);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.cmbFrameSize);
            this.panel1.Controls.Add(this.cmbDirection);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cmbItemType);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.txtPage);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtTotalPages);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btnModify);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.txtScale);
            this.panel1.Controls.Add(this.txtWeight);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtVersion);
            this.panel1.Controls.Add(this.cmbSecret);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtDwgName);
            this.panel1.Controls.Add(this.txtDwgNO);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(225, 284);
            this.panel1.TabIndex = 41;
            // 
            // ckForceUpdateFrame
            // 
            this.ckForceUpdateFrame.AutoSize = true;
            this.ckForceUpdateFrame.Location = new System.Drawing.Point(120, 227);
            this.ckForceUpdateFrame.Name = "ckForceUpdateFrame";
            this.ckForceUpdateFrame.Size = new System.Drawing.Size(96, 16);
            this.ckForceUpdateFrame.TabIndex = 42;
            this.ckForceUpdateFrame.Text = "强制更新图框";
            this.toolTip1.SetToolTip(this.ckForceUpdateFrame, "默认未更改图框参数则不更新图框。勾选时强制更新，可用于升级旧版图框。");
            this.ckForceUpdateFrame.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 65;
            this.label5.Text = "图幅";
            // 
            // cmbFrameSize
            // 
            this.cmbFrameSize.AutoCompleteCustomSource.AddRange(new string[] {
            "A4",
            "A3",
            "A2",
            "A1",
            "A0"});
            this.cmbFrameSize.DisplayMember = "0";
            this.cmbFrameSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFrameSize.FormattingEnabled = true;
            this.cmbFrameSize.Items.AddRange(new object[] {
            "A4",
            "A3",
            "A2",
            "A1",
            "A0"});
            this.cmbFrameSize.Location = new System.Drawing.Point(53, 63);
            this.cmbFrameSize.Name = "cmbFrameSize";
            this.cmbFrameSize.Size = new System.Drawing.Size(157, 20);
            this.cmbFrameSize.TabIndex = 62;
            // 
            // cmbDirection
            // 
            this.cmbDirection.AutoCompleteCustomSource.AddRange(new string[] {
            "横向",
            "纵向"});
            this.cmbDirection.DisplayMember = "0";
            this.cmbDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDirection.FormattingEnabled = true;
            this.cmbDirection.Items.AddRange(new object[] {
            "横向",
            "纵向"});
            this.cmbDirection.Location = new System.Drawing.Point(52, 36);
            this.cmbDirection.Name = "cmbDirection";
            this.cmbDirection.Size = new System.Drawing.Size(158, 20);
            this.cmbDirection.TabIndex = 61;
            this.cmbDirection.SelectedIndexChanged += new System.EventHandler(this.cmbDirection_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 63;
            this.label4.Text = "类别";
            // 
            // cmbItemType
            // 
            this.cmbItemType.AutoCompleteCustomSource.AddRange(new string[] {
            "部套",
            "零件"});
            this.cmbItemType.DisplayMember = "0";
            this.cmbItemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbItemType.FormattingEnabled = true;
            this.cmbItemType.Items.AddRange(new object[] {
            "部套",
            "零件"});
            this.cmbItemType.Location = new System.Drawing.Point(52, 11);
            this.cmbItemType.Name = "cmbItemType";
            this.cmbItemType.Size = new System.Drawing.Size(158, 20);
            this.cmbItemType.TabIndex = 60;
            this.cmbItemType.SelectedIndexChanged += new System.EventHandler(this.cmbItemType_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 40);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 12);
            this.label11.TabIndex = 64;
            this.label11.Text = "方向";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(18, 248);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(79, 23);
            this.btnRefresh.TabIndex = 58;
            this.btnRefresh.Text = "刷新";
            this.toolTip1.SetToolTip(this.btnRefresh, "重新载入当前图纸信息。");
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // txtPage
            // 
            this.txtPage.Location = new System.Drawing.Point(169, 171);
            this.txtPage.Name = "txtPage";
            this.txtPage.Size = new System.Drawing.Size(42, 21);
            this.txtPage.TabIndex = 55;
            this.txtPage.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(128, 175);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 57;
            this.label1.Text = "页数";
            // 
            // txtTotalPages
            // 
            this.txtTotalPages.Location = new System.Drawing.Point(54, 171);
            this.txtTotalPages.Name = "txtTotalPages";
            this.txtTotalPages.Size = new System.Drawing.Size(42, 21);
            this.txtTotalPages.TabIndex = 54;
            this.txtTotalPages.Text = "1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 175);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 56;
            this.label3.Text = "总页";
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(131, 248);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(80, 23);
            this.btnModify.TabIndex = 53;
            this.btnModify.Text = "修改";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 150);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 12);
            this.label10.TabIndex = 52;
            this.label10.Text = "密级";
            // 
            // txtScale
            // 
            this.txtScale.Location = new System.Drawing.Point(53, 222);
            this.txtScale.Name = "txtScale";
            this.txtScale.Size = new System.Drawing.Size(42, 21);
            this.txtScale.TabIndex = 45;
            this.txtScale.Text = "1:1";
            // 
            // txtWeight
            // 
            this.txtWeight.Location = new System.Drawing.Point(168, 197);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.Size = new System.Drawing.Size(42, 21);
            this.txtWeight.TabIndex = 44;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 226);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 51;
            this.label9.Text = "比例";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(128, 201);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 50;
            this.label8.Text = "重量";
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(53, 197);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(42, 21);
            this.txtVersion.TabIndex = 43;
            this.txtVersion.Text = "R00";
            // 
            // cmbSecret
            // 
            this.cmbSecret.AutoCompleteCustomSource.AddRange(new string[] {
            "普通商密",
            "核心机密"});
            this.cmbSecret.DisplayMember = "0";
            this.cmbSecret.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSecret.FormattingEnabled = true;
            this.cmbSecret.Items.AddRange(new object[] {
            "普通商密",
            "核心机密"});
            this.cmbSecret.Location = new System.Drawing.Point(53, 146);
            this.cmbSecret.Name = "cmbSecret";
            this.cmbSecret.Size = new System.Drawing.Size(157, 20);
            this.cmbSecret.TabIndex = 46;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 201);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 49;
            this.label6.Text = "版本";
            // 
            // txtDwgName
            // 
            this.txtDwgName.Location = new System.Drawing.Point(53, 119);
            this.txtDwgName.Name = "txtDwgName";
            this.txtDwgName.Size = new System.Drawing.Size(157, 21);
            this.txtDwgName.TabIndex = 42;
            this.txtDwgName.Text = "测试图档";
            // 
            // txtDwgNO
            // 
            this.txtDwgNO.Location = new System.Drawing.Point(53, 92);
            this.txtDwgNO.Name = "txtDwgNO";
            this.txtDwgNO.Size = new System.Drawing.Size(157, 21);
            this.txtDwgNO.TabIndex = 41;
            this.txtDwgNO.Text = "A012B-000000A";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 123);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 47;
            this.label7.Text = "名称";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 48;
            this.label2.Text = "图号";
            // 
            // SingleDwgInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "SingleDwgInfo";
            this.Size = new System.Drawing.Size(225, 284);
            this.Load += new System.EventHandler(this.SingleDwgInfo_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtScale;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.ComboBox cmbSecret;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDwgName;
        private System.Windows.Forms.TextBox txtDwgNO;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTotalPages;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbFrameSize;
        private System.Windows.Forms.ComboBox cmbDirection;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbItemType;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.CheckBox ckForceUpdateFrame;
        private System.Windows.Forms.ToolTip toolTip1;

    }
}
