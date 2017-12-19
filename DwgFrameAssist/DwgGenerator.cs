using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Autodesk.AutoCAD.Runtime;
using Acad = Autodesk.AutoCAD.ApplicationServices;
using AcDb = Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Windows;
using System.Threading;
using System.Reflection;

namespace DwgFrameAssist
{
    public partial class DwgGenerator : Form
    {
        DataTable dt = new DataTable();
        Dictionary<String, Dictionary<String, List<String>>> types;

        public DwgGenerator()
        {
            InitializeComponent();

            types = XmlUtil.GetDwgTypeValues();

            //部套、零件
            cmbItemType.Items.Clear();
            cmbItemType.Items.AddRange(new List<string>(types.Keys).ToArray());
            cmbItemType.SelectedIndex = 0;

            //横向 纵向
            cmbDirection.Items.Clear();
            cmbDirection.Items.AddRange(new List<string>(types[cmbItemType.SelectedItem.ToString()].Keys).ToArray());
            cmbDirection.SelectedIndex = 0;

            //图幅
            cmbFrameSize.Items.Clear();
            cmbFrameSize.Items.AddRange(types[cmbItemType.SelectedItem.ToString()][cmbDirection.SelectedItem.ToString()].ToArray());
            cmbFrameSize.SelectedIndex = 0;

            //cmbDirection.SelectedIndex = 0;
            //cmbFrameSize.SelectedIndex = 0;
            //cmbItemType.SelectedIndex = 0;
            cmbSecret.SelectedIndex = 0;

            dt.Columns.Add("类别");
            dt.Columns.Add("文件名");
            dt.Columns.Add("图号");
            dt.Columns.Add("名称");
            dt.Columns.Add("图幅");
            dt.Columns.Add("版本");
            dt.Columns.Add("比例");
            dt.Columns.Add("重量");
            dt.Columns.Add("密级");
            dt.Columns.Add("张数");

            numTotalPage_ValueChanged(null, null);
            dgv.DataSource = dt;
            dgv.Columns["图号"].Visible = false;
            dgv.Columns["版本"].Visible = false;
            dgv.Columns["张数"].Visible = false;

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                if(col.Name != "名称")
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                else
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            dgv.Columns["类别"].ReadOnly = true;
            dgv.Columns["文件名"].ReadOnly = true;
            dgv.Columns["图幅"].ReadOnly = true;
            dgv.Columns["密级"].ReadOnly = true;

            StringBuilder sb = new StringBuilder();
            sb.Append(XmlUtil.getXmlValue("help", "value"));
            //sb.AppendLine("离线刷图工具 v0.9 beta");
            //sb.AppendLine();
            //sb.AppendLine("离线生成标准图框，可设置的参数分为【基本属性】、【公共属性】和【私有属性】，");
            //sb.AppendLine("见左侧属性设置部分。");
            //sb.AppendLine("公共属性更改时，所有表格行都会更改。");
            //sb.AppendLine("私有属性可针对所选择的单行或多行进行设置。");
            //sb.AppendLine("另外，公共属性【图纸名称】及私有属性【比例】、【重量】，可在表格中单独修改，");
            //sb.AppendLine("双击相应单元格或点击单");
            //sb.AppendLine("元格后按F2后可更改，更改完按Enter或点击其他单元格以保存。");
            //sb.AppendLine();
            //sb.AppendLine("☆☆ 注意：");
            //sb.AppendLine("☆☆ 1、手动更改公共属性【图纸名称】单元格后，不要在左侧属性设置框再次更改");
            //sb.AppendLine("该公共属性，否则会刷新所有行的该属性。");
            //sb.AppendLine("☆☆ 2、图纸名称支持双行名称（即主标题和副标题)，两行标题间用'^'分隔开即可。");
            //sb.AppendLine("☆☆ 如：某行图纸名称为  第一行名称^第二行名称");
            //sb.AppendLine("☆☆ 最后生成到图纸上的名称格式为：");
            //sb.AppendLine("☆☆     第一行名称       (7号字，居中)");
            //sb.AppendLine("☆☆    (第二行名称)      (5号字，居中)");
            //sb.AppendLine("☆☆ (副标题首尾的\"()\"字符串可在config.xml中修改)");
            //sb.AppendLine();
            //sb.AppendLine("点击左侧'帮助'标签可显示或隐藏该帮助内容。");

            //sb.AppendLineLine(rtextHelp.Text);
            //sb.AppendLineLine();
            //sb.AppendLineLine("离线生成标准图框，可设置的参数分为基本属性、公共属性和私有属性。");
            //sb.AppendLineLine("公共属性更改时，所有表格行都会更改。");
            //sb.AppendLineLine("私有属性可针对所选择的单行或多行进行设置。");
            //sb.AppendLineLine("部分私有属性，如图纸名称，可在表格中单独修改，双击相应单元格\r\n或点击单元格后按F2后可更改，更改完按Enter或点击其他单元格以保存。");
            //sb.AppendLineLine("注意： 手动更改公共属性的单元格后，不要再更改公共属性，否则会刷新所有行该属性。");
            //sb.AppendLineLine("☆☆ 图纸名称支持两行（即主标题和副标题)，两行标题间用'^'分隔开即可。\r\n如：某行图纸名称为  第一行名称^第二行名称");
            //sb.AppendLineLine("☆☆ 最后生成到图纸上的名称格式为：");
            //sb.AppendLineLine("☆☆     第一行名称       (7号字)");
            //sb.AppendLineLine("☆☆    (第二行名称)      (5号字，收尾的括号字符串可在config.xml中修改)");
            //sb.AppendLineLine();
            //sb.AppendLineLine("点击左侧'帮助'标签可显示或隐藏该帮助内容。");

            rtextHelp.Text = sb.ToString();
            if (XmlUtil.getXmlValue("showHelp", "value") == "true")
            {
                XmlUtil.setXmlValue("showHelp", "value", "false");
                rtextHelp.Visible = true;

            }
            else
                rtextHelp.Visible = false;

            ckbOverride.Checked = XmlUtil.getXmlValue("override", "value") == "true";

        }

        private void btnCreateDwg_Click(object sender, EventArgs e)
        {
            try
            {
                bool overwrite = ckbOverride.Checked;
                string dllDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                string sourcePath = Path.Combine(dllDir, XmlUtil.getXmlValue("sourceFolder", "value") + cmbItemType.Text);
                string destPath = Path.Combine(XmlUtil.getXmlValue("dwgFolder", "value"), GetFolderName(txtDwgNO.Text));

                if (!Directory.Exists(destPath))
                    Directory.CreateDirectory(destPath);

                var sb = new StringBuilder();
                Dictionary<String, DataRow> dict = new Dictionary<string, DataRow>();
                for (int i = 0; i < (int)numTotalPage.Value; i++)
                {
                    DataRow row = dt.Rows[i];
                    string sourceFile = Path.Combine(sourcePath, row["类别"] + "-" + row["图幅"] + ".dwg");
                    string destFile = Path.Combine(destPath, GenerateFileName(row) + ".dwg");
                    dict.Add(destFile, row);

                    var s = CopyFile(sourceFile, destFile, overwrite);
                    if (s != null) sb.AppendLine(s);

                }
                if (sb.Length > 0)
                    MessageBox.Show(sb.ToString(), "复制图纸出错！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    ModifyDwgs(dict);

                Process.Start(destPath);
            }
            catch
            {
                throw;
            }
            this.Focus();
        }

        private void ModifyDwgs(Dictionary<String, DataRow> dict)
        {

            try
            {
                foreach (var entry in dict)
                {
                    if (!File.Exists(entry.Key)) continue;   //文件不存在则跳过

#if R18
                    Acad.Document doc = Acad.Application.DocumentManager.Open(entry.Key, true);
#elif R19
                    Acad.Document doc = Acad.DocumentCollectionExtension.Open(Acad.Application.DocumentManager, entry.Key, true, "");
#elif R20
                    Acad.Document doc = Acad.DocumentCollectionExtension.Open(Acad.Application.DocumentManager, entry.Key, true, "");
#endif
                    //Thread.Sleep(500);
                    bool drawMarkLine = XmlUtil.getXmlValue("markArea", "value") == "true";
                    var dft = new DwgFrameTools(doc, drawMarkLine);

                    Dictionary<DwgInfoTypeEnum, String> info = new Dictionary<DwgInfoTypeEnum, string>();

                    info.Add(DwgInfoTypeEnum.图号, entry.Value["图号"] as string);
                    info.Add(DwgInfoTypeEnum.名称, entry.Value["名称"] as string);
                    info.Add(DwgInfoTypeEnum.版本, entry.Value["版本"] as string);
                    info.Add(DwgInfoTypeEnum.比例, entry.Value["比例"] as string);
                    info.Add(DwgInfoTypeEnum.密级, entry.Value["密级"] as string);
                    info.Add(DwgInfoTypeEnum.总张数, (entry.Value["张数"] as string).Split('-')[0]);
                    info.Add(DwgInfoTypeEnum.张数, (entry.Value["张数"] as string).Split('-')[1]);
                    info.Add(DwgInfoTypeEnum.重量, entry.Value["重量"] as string);
                    dft.UpdateDwgInfo(info);
                    //doc.CloseAndSave(doc.Name);
                    doc.Database.SaveAs(doc.Name, false, AcDb.DwgVersion.Current, doc.Database.SecurityParameters);

#if R18
                    doc.CloseAndDiscard();
#elif R19
                    Acad.DocumentExtension.CloseAndDiscard(doc);
#elif R20
                    Acad.DocumentExtension.CloseAndDiscard(doc);
#endif

                    //doc.CloseAndDiscard();
                     
                }
            }
            catch
            {
                
                throw;
            }
            
        }

        private String CopyFile(string sourceFile, string destFile, bool overwrite)
        {
            string str = null;
            try
            {
                Debug.Print("复制文件：");
                Debug.Print("模板文件："+sourceFile);
                Debug.Print("目标文件：" + destFile);
                
               File.Copy(sourceFile, destFile, overwrite);
            }
            catch (System.Exception e)
            {
                str = string.Format("复制文件{0}出错:\r\n{1}。\r\n", new FileInfo(destFile).Name, e.Message);
            }
            return str;
        }

        private string GetFolderName(string s)
        {
            string regx = @"(\w+-)(\d{3})\d{3}([A-Za-z]{1,3})\w*";
            var match = new Regex(regx).Match(s);
            if (match == null) 
                return s;

            foreach (Capture  item in match.Groups)
            {
                Debug.Print(item.Value);
            }

            return match.Groups[1].Value + match.Groups[2].Value + "000" + match.Groups[3].Value;
            //throw new InvalidEnumArgumentException("图号不符合要求");
        }

        private void numTotalPage_ValueChanged(object sender, EventArgs e)
        {
            int num = (int)numTotalPage.Value;
            int count = dt.Rows.Count;
            if (count > num)
            {
                for (int i = 0; i < count - num; i++)
                {
                    dt.Rows.RemoveAt(count - 1 - i);
                }
            }
            else
            {
                for (int i = 0; i < num - count; i++)
                {
                    dt.Rows.Add(dt.NewRow());
                }
            }

            UpdateAllRows(true);
            RefreshDgv();
        }

        private void RefreshDgv()
        {
            
            dgv.Update();
            
        }

        private void UpdateAllRows(bool isInit)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                //Debug.Print(row.ItemArray.ToList<String>().ToString());

                dt.Rows[i]["类别"] = cmbItemType.Text;
                dt.Rows[i]["图号"] = txtDwgNO.Text;
                dt.Rows[i]["名称"] = txtDwgName.Text;
                dt.Rows[i]["版本"] = txtVersion.Text;
                dt.Rows[i]["张数"] = ((int)numTotalPage.Value) + "-" + (i + 1).ToString();
                if (isInit)
                {
                    dt.Rows[i]["密级"] = cmbSecret.Text;
                    dt.Rows[i]["重量"] = txtWeight.Text;
                    dt.Rows[i]["比例"] = txtScale.Text;
                    dt.Rows[i]["图幅"] = cmbDirection.Text + "-" + cmbFrameSize.Text;
                }

                dt.Rows[i]["文件名"] = GenerateFileName(dt.Rows[i]);

            }
            
            RefreshDgv();
            //throw new NotImplementedException();
        }

        private String GenerateFileName(DataRow row)
        {
            return row["图号"] + "_" + row["版本"] + "_" + row["张数"];
        }

        private void dgv_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(this.dgv.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(System.Globalization.CultureInfo.CurrentUICulture), 
                this.dgv.DefaultCellStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }

        private void cmbItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbDirection.Items.Clear();
            cmbDirection.Items.AddRange(types[cmbItemType.SelectedItem.ToString()].Keys.ToArray());
            cmbDirection.SelectedIndex = 0;
            cmbDirection.Refresh();

            UpdateAllRows(false);
        }

        private void txtDwgNO_TextChanged(object sender, EventArgs e)
        {
            UpdateAllRows(false);
        }

        private void txtVersion_TextChanged(object sender, EventArgs e)
        {
            UpdateAllRows(false);
        }

        private void cmbDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbFrameSize.Items.Clear();
            cmbFrameSize.Items.AddRange(types[cmbItemType.SelectedItem.ToString()][cmbDirection.SelectedItem.ToString()].ToArray());
            cmbFrameSize.SelectedIndex = 0;
            cmbFrameSize.Refresh();

            if (dgv.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgv.SelectedRows)
                {
                     (row.DataBoundItem as DataRowView).Row["图幅"] = cmbDirection.Text + "-" + cmbFrameSize.Text;
                }

                //(dgv.SelectedRows[0].DataBoundItem as DataRowView).Row["图幅"] = cmbDirection.Text + "-" + cmbFrameSize.Text;
                RefreshDgv();
            }
        }

        private void cmbFrameSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgv.SelectedRows)
                {
                    (row.DataBoundItem as DataRowView).Row["图幅"] = cmbDirection.Text + "-" + cmbFrameSize.Text;
                }
                //(dgv.SelectedRows[0].DataBoundItem as DataRowView).Row["图幅"] = cmbDirection.Text + "-" + cmbFrameSize.Text;
                RefreshDgv();
            }
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {

            if (dgv.SelectedRows.Count > 0)
            {
                DataRowView row = dgv.SelectedRows[0].DataBoundItem as DataRowView;
                string str = row.Row["图幅"] as string;
                txtScale.Text = row.Row["比例"] as string;
                txtWeight.Text = row.Row["重量"] as string;
                cmbSecret.Text = row.Row["密级"] as string;
                //Debug.Print("dgv_SelectionChanged: " + str);
                if (str != null && str != "")
                {
                    var arr = str.Split('-');
                    cmbDirection.SelectedItem = arr[0];
                    cmbFrameSize.SelectedItem = arr[1];
                }
            }
        }

        private void txtDwgName_TextChanged(object sender, EventArgs e)
        {
            UpdateAllRows(false);
        }

        private void cmbSecret_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgv.SelectedRows)
                    (row.DataBoundItem as DataRowView).Row["密级"] = cmbSecret.Text;
                RefreshDgv();
            }
        }

        private void txtScale_TextChanged(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {

                foreach (DataGridViewRow row in dgv.SelectedRows)
                    (row.DataBoundItem as DataRowView).Row["比例"] = txtScale.Text;
                RefreshDgv();
            }
        }

        private void txtWeight_TextChanged(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgv.SelectedRows)
                    (row.DataBoundItem as DataRowView).Row["重量"] = txtWeight.Text;
                RefreshDgv();
            }
        }

        private void DwgGenerator_Load(object sender, EventArgs e)
        {
            
            dgv.Columns[dgv.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        }

        private void btnOpenDir_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(XmlUtil.getXmlValue("dwgFolder", "value"));

            string destPath = Path.Combine(XmlUtil.getXmlValue("dwgFolder", "value"), GetFolderName(txtDwgNO.Text));

            if (!Directory.Exists(destPath))
                Directory.CreateDirectory(destPath);

            Process.Start(destPath);
                    
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            rtextHelp.Visible = !rtextHelp.Visible;
        }

        private void ckbOverride_CheckedChanged(object sender, EventArgs e)
        {
            XmlUtil.setXmlValue("override", "value", ckbOverride.Checked.ToString().ToLower());
        }


    }
}
