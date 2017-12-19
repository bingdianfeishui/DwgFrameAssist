namespace DwgFrameAssist
{
    partial class ModifyFrameSize
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
            this.label5 = new System.Windows.Forms.Label();
            this.cmbFrameSize = new System.Windows.Forms.ComboBox();
            this.cmbDirection = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbItemType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnUpdateFrameSize = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 30;
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
            this.cmbFrameSize.Location = new System.Drawing.Point(44, 63);
            this.cmbFrameSize.Name = "cmbFrameSize";
            this.cmbFrameSize.Size = new System.Drawing.Size(161, 20);
            this.cmbFrameSize.TabIndex = 27;
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
            this.cmbDirection.Location = new System.Drawing.Point(43, 36);
            this.cmbDirection.Name = "cmbDirection";
            this.cmbDirection.Size = new System.Drawing.Size(162, 20);
            this.cmbDirection.TabIndex = 26;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 28;
            this.label3.Text = "类别";
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
            this.cmbItemType.Location = new System.Drawing.Point(43, 11);
            this.cmbItemType.Name = "cmbItemType";
            this.cmbItemType.Size = new System.Drawing.Size(162, 20);
            this.cmbItemType.TabIndex = 25;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 29;
            this.label4.Text = "方向";
            // 
            // btnUpdateFrameSize
            // 
            this.btnUpdateFrameSize.Location = new System.Drawing.Point(130, 89);
            this.btnUpdateFrameSize.Name = "btnUpdateFrameSize";
            this.btnUpdateFrameSize.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateFrameSize.TabIndex = 31;
            this.btnUpdateFrameSize.Text = "更新图框";
            this.btnUpdateFrameSize.UseVisualStyleBackColor = true;
            this.btnUpdateFrameSize.Click += new System.EventHandler(this.btnUpdateFrameSize_Click);
            // 
            // ModifyFrameSize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnUpdateFrameSize);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbFrameSize);
            this.Controls.Add(this.cmbDirection);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbItemType);
            this.Controls.Add(this.label4);
            this.Name = "ModifyFrameSize";
            this.Size = new System.Drawing.Size(225, 120);
            this.Load += new System.EventHandler(this.ModifyFrameSize_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbFrameSize;
        private System.Windows.Forms.ComboBox cmbDirection;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbItemType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnUpdateFrameSize;
    }
}
