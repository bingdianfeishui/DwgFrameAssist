using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.AutoCAD.Runtime;
using Acad = Autodesk.AutoCAD.ApplicationServices;
using AcDb = Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Windows;

namespace DwgFrameAssist
{
    public partial class SingleDwgInfo : UserControl
    {
        Acad.Document acCurDoc = null;
        DwgFrameTools dft = null;
        string size="";
        Dictionary<String, Dictionary<String, List<String>>> types;

        public SingleDwgInfo()
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

        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateFrameSize();

                Dictionary<DwgInfoTypeEnum, String> info = new Dictionary<DwgInfoTypeEnum, string>();

                info.Add(DwgInfoTypeEnum.图号, txtDwgNO.Text);
                info.Add(DwgInfoTypeEnum.名称, txtDwgName.Text);
                info.Add(DwgInfoTypeEnum.版本, txtVersion.Text);
                info.Add(DwgInfoTypeEnum.比例, txtScale.Text);
                info.Add(DwgInfoTypeEnum.密级, cmbSecret.Text);
                info.Add(DwgInfoTypeEnum.总张数, txtTotalPages.Text);
                info.Add(DwgInfoTypeEnum.张数, txtPage.Text);
                info.Add(DwgInfoTypeEnum.重量, txtWeight.Text);
                new DwgFrameTools(acCurDoc, XmlUtil.getXmlValue("markArea", "value") == "true").UpdateDwgInfo(info);

                //dwgDoc.Database.SaveAs(dwgDoc.Name, false, AcDb.DwgVersion.Current, dwgDoc.Database.SecurityParameters);
                //dwgDoc.CloseAndDiscard();
            }
            catch (System.Exception ex)
            {
                Acad.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage("\r\n发生错误:" + ex.Message + "\r\n");
                throw ex;
            }

            //刷新信息
            btnRefresh_Click(null, null);

            acCurDoc.Editor.Regen();
         
        }

        private void UpdateFrameSize()
        {
            //未更改则不改图框
            if (ckForceUpdateFrame.Checked == false && this.size == cmbFrameSize.Text && cmbDirection.Text == this.dft.DwgInformation.Direction && cmbItemType.Text == this.dft.DwgInformation.DwgType)
            {
                Acad.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage("\r\n跳过更新图框。。。\r\n");
                return;
            }
            if (cmbDirection.SelectedIndex == -1 || cmbFrameSize.SelectedIndex == -1 || cmbItemType.SelectedIndex == -1)
                Acad.Application.ShowAlertDialog("类型、方向、图幅参数必须选择！");

            DwgFrameTools dft = new DwgFrameTools(acCurDoc, XmlUtil.getXmlValue("markArea", "value") == "true");

            DwgInfo info = dft.DwgInformation;
            dft.ReplaceFrameBlock(dft.GenerateStandardDwgFileName(
                cmbItemType.SelectedIndex == 1,   //1——零件
                cmbDirection.SelectedItem.ToString() == "横向",  //0——横向
                cmbFrameSize.Text));

            
        }

        private void SingleDwgInfo_Load(object sender, EventArgs e)
        {
            //acCurDoc.Editor.WriteMessage(acCurDoc.Database.Filename + "解析数据中。。。\r\n");
            try
            {
                acCurDoc = Acad.Application.DocumentManager.MdiActiveDocument;
                dft = new DwgFrameTools(acCurDoc, XmlUtil.getXmlValue("markArea", "value") == "true");
                InitParameters();

                //切换文件自动加载，容易抛异常，不用
                //Acad.Application.DocumentManager.DocumentActivated += InitParameters;
            }
            catch (System.Exception ex)
            {
                Acad.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage("\r\n发生错误:" + ex + "\r\n");
            }
        }

        public void InitParameters()//Acad.DocumentActivationChangedEventArgs e)
        {
            try
            {            
                txtDwgNO.Text = dft.DwgInformation.DwgNO;
                txtDwgName.Text = dft.DwgInformation.Name;
                txtScale.Text = dft.DwgInformation.Secret;
                txtWeight.Text = dft.DwgInformation.Weight;
                txtVersion.Text = dft.DwgInformation.Version;
                txtScale.Text = dft.DwgInformation.Scale;
                txtTotalPages.Text = dft.DwgInformation.TotalPages.ToString();
                txtPage.Text = dft.DwgInformation.PageNO.ToString();

                if (cmbSecret.Items.Contains(dft.DwgInformation.Secret))
                    cmbSecret.Text = dft.DwgInformation.Secret;
                else
                    cmbSecret.SelectedIndex = 0;

                InitFrameInfo();
            }
            catch (System.Exception)
            {
                
                throw;
            }
            
        }

        private void InitFrameInfo()
        {
            if (cmbDirection.Items.Contains(dft.DwgInformation.Direction))
                cmbDirection.Text = dft.DwgInformation.Direction;
            if (cmbItemType.Items.Contains(dft.DwgInformation.DwgType))
                cmbItemType.Text = dft.DwgInformation.DwgType;

            this.size = dft.DwgInformation.Width + "x" + dft.DwgInformation.Height;
            cmbFrameSize.Text = this.size;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            SingleDwgInfo_Load(sender, e);
        }

        private void cmbDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbFrameSize.Items.Clear();
            cmbFrameSize.Items.AddRange(types[cmbItemType.SelectedItem.ToString()][cmbDirection.SelectedItem.ToString()].ToArray());
            cmbFrameSize.SelectedIndex = 0;
            cmbFrameSize.Refresh();
        }

        private void cmbItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbDirection.Items.Clear();
            cmbDirection.Items.AddRange(types[cmbItemType.SelectedItem.ToString()].Keys.ToArray());
            cmbDirection.SelectedIndex = 0;
            cmbDirection.Refresh();
        }
    }
}
