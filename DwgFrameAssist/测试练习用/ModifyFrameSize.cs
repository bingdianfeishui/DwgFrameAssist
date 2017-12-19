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
    public partial class ModifyFrameSize : UserControl
    {
        public ModifyFrameSize()
        {
            InitializeComponent();
        }

        private void btnUpdateFrameSize_Click(object sender, EventArgs e)
        {
            Acad.Document acCurDoc = Acad.Application.DocumentManager.MdiActiveDocument;
            try
            {

                if (cmbDirection.SelectedIndex == -1 || cmbFrameSize.SelectedIndex == -1 || cmbItemType.SelectedIndex == -1)
                    Acad.Application.ShowAlertDialog("类型、方向、图幅参数必须选择！");
      
                DwgFrameTools dft = new DwgFrameTools(acCurDoc, XmlUtil.getXmlValue("markArea", "value") == "true");

                DwgInfo info = dft.DwgInformation;
                dft.ReplaceFrameBlock(dft.GenerateStandardDwgFileName(
                    cmbItemType.SelectedIndex == 1,   //1——零件
                    cmbDirection.SelectedIndex == 0,    //0——横向
                    cmbFrameSize.Text));
                DwgFrameTools dft2 = new DwgFrameTools(acCurDoc, XmlUtil.getXmlValue("markArea", "value") == "true");
                //info.Name += "new";
                //info.DwgNO = "new Dwg no";

                dft2.UpdateDwgInfo(info);
            }
            catch (System.Exception ex)
            {
                acCurDoc.Editor.WriteMessage(ex.ToString());
            }
        }

        private void ModifyFrameSize_Load(object sender, EventArgs e)
        {

        }
    }
}
