namespace DwgFrameAssist
{
    partial class DwgGenerator
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DwgGenerator));
            this.btnCreateDwg = new System.Windows.Forms.Button();
            this.ckbOverride = new System.Windows.Forms.CheckBox();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbItemType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numTotalPage = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDwgName = new System.Windows.Forms.TextBox();
            this.txtDwgNO = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cmbSecret = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtScale = new System.Windows.Forms.TextBox();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbFrameSize = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbDirection = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOpenDir = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.rtextHelp = new System.Windows.Forms.RichTextBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalPage)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCreateDwg
            // 
            this.btnCreateDwg.Location = new System.Drawing.Point(128, 340);
            this.btnCreateDwg.Name = "btnCreateDwg";
            this.btnCreateDwg.Size = new System.Drawing.Size(90, 30);
            this.btnCreateDwg.TabIndex = 12;
            this.btnCreateDwg.Text = "生成图纸";
            this.btnCreateDwg.UseVisualStyleBackColor = true;
            this.btnCreateDwg.Click += new System.EventHandler(this.btnCreateDwg_Click);
            // 
            // ckbOverride
            // 
            this.ckbOverride.AutoSize = true;
            this.ckbOverride.Location = new System.Drawing.Point(16, 318);
            this.ckbOverride.Name = "ckbOverride";
            this.ckbOverride.Size = new System.Drawing.Size(72, 16);
            this.ckbOverride.TabIndex = 11;
            this.ckbOverride.Text = "强制覆盖";
            this.ckbOverride.UseVisualStyleBackColor = true;
            this.ckbOverride.CheckedChanged += new System.EventHandler(this.ckbOverride_CheckedChanged);
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv.BackgroundColor = System.Drawing.SystemColors.ControlDark;
            this.dgv.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv.Location = new System.Drawing.Point(232, 12);
            this.dgv.Margin = new System.Windows.Forms.Padding(10);
            this.dgv.Name = "dgv";
            this.dgv.RowTemplate.Height = 23;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(542, 355);
            this.dgv.TabIndex = 9;
            this.toolTip1.SetToolTip(this.dgv, "双击个别单元格或点击后按F2，可编辑单元格的值。");
            this.dgv.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgv_RowPostPaint);
            this.dgv.SelectionChanged += new System.EventHandler(this.dgv_SelectionChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbItemType);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numTotalPage);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(2, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(222, 45);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "基本设置";
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
            this.cmbItemType.Location = new System.Drawing.Point(44, 17);
            this.cmbItemType.Name = "cmbItemType";
            this.cmbItemType.Size = new System.Drawing.Size(55, 20);
            this.cmbItemType.TabIndex = 1;
            this.cmbItemType.SelectedIndexChanged += new System.EventHandler(this.cmbItemType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 22;
            this.label3.Text = "类别";
            // 
            // numTotalPage
            // 
            this.numTotalPage.Location = new System.Drawing.Point(152, 17);
            this.numTotalPage.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTotalPage.Name = "numTotalPage";
            this.numTotalPage.Size = new System.Drawing.Size(64, 21);
            this.numTotalPage.TabIndex = 2;
            this.numTotalPage.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTotalPage.ValueChanged += new System.EventHandler(this.numTotalPage_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(117, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 19;
            this.label1.Text = "张数";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtVersion);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtDwgName);
            this.groupBox2.Controls.Add(this.txtDwgNO);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(2, 63);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(222, 115);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "公共属性";
            this.toolTip1.SetToolTip(this.groupBox2, "更改公共属性为更改所有行。");
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(44, 84);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(172, 21);
            this.txtVersion.TabIndex = 5;
            this.txtVersion.Text = "R00";
            this.txtVersion.TextChanged += new System.EventHandler(this.txtVersion_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 18;
            this.label6.Text = "版本";
            // 
            // txtDwgName
            // 
            this.txtDwgName.Location = new System.Drawing.Point(44, 52);
            this.txtDwgName.Name = "txtDwgName";
            this.txtDwgName.Size = new System.Drawing.Size(172, 21);
            this.txtDwgName.TabIndex = 4;
            this.txtDwgName.Text = "测试图档";
            this.txtDwgName.TextChanged += new System.EventHandler(this.txtDwgName_TextChanged);
            // 
            // txtDwgNO
            // 
            this.txtDwgNO.Location = new System.Drawing.Point(44, 20);
            this.txtDwgNO.Name = "txtDwgNO";
            this.txtDwgNO.Size = new System.Drawing.Size(172, 21);
            this.txtDwgNO.TabIndex = 3;
            this.txtDwgNO.Text = "A012B-000000A";
            this.txtDwgNO.TextChanged += new System.EventHandler(this.txtDwgNO_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "名称";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 16;
            this.label2.Text = "图号";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cmbSecret);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.txtScale);
            this.groupBox3.Controls.Add(this.txtWeight);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.cmbFrameSize);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.cmbDirection);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(3, 184);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(221, 122);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "私有属性";
            this.toolTip1.SetToolTip(this.groupBox3, "私有属性可单独设置，可选择多行进行批量设置。");
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
            this.cmbSecret.Location = new System.Drawing.Point(41, 91);
            this.cmbSecret.Name = "cmbSecret";
            this.cmbSecret.Size = new System.Drawing.Size(174, 20);
            this.cmbSecret.TabIndex = 10;
            this.cmbSecret.SelectedIndexChanged += new System.EventHandler(this.cmbSecret_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 95);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 12);
            this.label10.TabIndex = 27;
            this.label10.Text = "密级";
            // 
            // txtScale
            // 
            this.txtScale.Location = new System.Drawing.Point(139, 55);
            this.txtScale.Name = "txtScale";
            this.txtScale.Size = new System.Drawing.Size(76, 21);
            this.txtScale.TabIndex = 9;
            this.txtScale.Text = "1:1";
            this.txtScale.TextChanged += new System.EventHandler(this.txtScale_TextChanged);
            // 
            // txtWeight
            // 
            this.txtWeight.Location = new System.Drawing.Point(41, 55);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.Size = new System.Drawing.Size(57, 21);
            this.txtWeight.TabIndex = 8;
            this.txtWeight.TextChanged += new System.EventHandler(this.txtWeight_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(106, 58);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 24;
            this.label9.Text = "比例";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(106, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 24;
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
            this.cmbFrameSize.Location = new System.Drawing.Point(139, 20);
            this.cmbFrameSize.Name = "cmbFrameSize";
            this.cmbFrameSize.Size = new System.Drawing.Size(76, 20);
            this.cmbFrameSize.TabIndex = 7;
            this.cmbFrameSize.SelectedIndexChanged += new System.EventHandler(this.cmbFrameSize_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 58);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 23;
            this.label8.Text = "重量";
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
            this.cmbDirection.Location = new System.Drawing.Point(43, 20);
            this.cmbDirection.Name = "cmbDirection";
            this.cmbDirection.Size = new System.Drawing.Size(55, 20);
            this.cmbDirection.TabIndex = 6;
            this.cmbDirection.SelectedIndexChanged += new System.EventHandler(this.cmbDirection_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 23;
            this.label4.Text = "方向";
            // 
            // btnOpenDir
            // 
            this.btnOpenDir.Location = new System.Drawing.Point(12, 340);
            this.btnOpenDir.Name = "btnOpenDir";
            this.btnOpenDir.Size = new System.Drawing.Size(94, 30);
            this.btnOpenDir.TabIndex = 22;
            this.btnOpenDir.Text = "打开生成目录";
            this.btnOpenDir.UseVisualStyleBackColor = true;
            this.btnOpenDir.Click += new System.EventHandler(this.btnOpenDir_Click);
            // 
            // rtextHelp
            // 
            this.rtextHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtextHelp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtextHelp.Location = new System.Drawing.Point(232, 63);
            this.rtextHelp.Name = "rtextHelp";
            this.rtextHelp.ReadOnly = true;
            this.rtextHelp.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtextHelp.Size = new System.Drawing.Size(542, 304);
            this.rtextHelp.TabIndex = 23;
            this.rtextHelp.Text = "离线刷图工具";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(180, 318);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(29, 12);
            this.linkLabel1.TabIndex = 24;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "帮助";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // DwgGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 379);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.rtextHelp);
            this.Controls.Add(this.btnOpenDir);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.ckbOverride);
            this.Controls.Add(this.btnCreateDwg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "DwgGenerator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "离线刷图工具 beta by LeeY";
            this.Load += new System.EventHandler(this.DwgGenerator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalPage)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreateDwg;
        private System.Windows.Forms.CheckBox ckbOverride;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cmbItemType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numTotalPage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDwgName;
        private System.Windows.Forms.TextBox txtDwgNO;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbFrameSize;
        private System.Windows.Forms.ComboBox cmbDirection;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbSecret;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtScale;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnOpenDir;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.RichTextBox rtextHelp;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}
