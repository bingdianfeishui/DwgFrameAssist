// (C) Copyright 2017 by Sky123.Org 
//
using System;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using System.Reflection;
using System.Text;
using System.IO;

// This line is not mandatory, but improves loading performances
[assembly: ExtensionApplication(typeof(DwgFrameAssist.MyPlugin))]

namespace DwgFrameAssist
{

    // This class is instantiated by AutoCAD once and kept alive for the 
    // duration of the session. If you don't do any one time initialization 
    // then you should remove this class.
    public class MyPlugin : IExtensionApplication
    {

        void IExtensionApplication.Initialize()
        {
            // Add one time initialization here
            // One common scenario is to setup a callback function here that 
            // unmanaged code can call. 
            // To do this:
            // 1. Export a function from unmanaged code that takes a function
            //    pointer and stores the passed in value in a global variable.
            // 2. Call this exported function in this function passing delegate.
            // 3. When unmanaged code needs the services of this managed module
            //    you simply call acrxLoadApp() and by the time acrxLoadApp 
            //    returns  global function pointer is initialized to point to
            //    the C# delegate.
            // For more DwgInformation see: 
            // http://msdn2.microsoft.com/en-US/library/5zwkzwf4(VS.80).aspx
            // http://msdn2.microsoft.com/en-us/library/44ey4b32(VS.80).aspx
            // http://msdn2.microsoft.com/en-US/library/7esfatk4.aspx
            // as well as some of the existing AutoCAD managed apps.

            // Initialize your plug-in application here
            var ass = Assembly.GetExecutingAssembly();
            var verInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(ass.Location);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆");
            sb.AppendLine("     图框小助手 v{0}");
            sb.AppendLine("                 by LeeY");
            sb.AppendLine();
            sb.AppendLine("    命令调用：");
            sb.AppendLine("    EDWG——图框修改器");
            sb.AppendLine("    GDWG——离线刷图工具");
            sb.AppendLine();
            sb.AppendLine("    祝 使用愉快");
            sb.AppendLine("☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆");

            var info = string.Format(sb.ToString(), verInfo.FileVersion);

            var ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage(info);

            if (XmlUtil.getXmlValue("checkUpdateOnLoad","value") == "true")
            {
                var update = new UpdateCenter();
                ed.WriteMessage(update.Message);
                if (update.HasNewVersion)
                {
                    var res = System.Windows.Forms.MessageBox.Show("发现新版本，是否更新？\r\n  更新方法:选择\"是\"，将打开远程目录和本地目录；关闭CAD，下载远程目录中的新文件覆盖本机的文件。", "DwgFrameAssist更新器", System.Windows.Forms.MessageBoxButtons.YesNo);
                    if (res == System.Windows.Forms.DialogResult.Yes)
                    {
                        update.OpenInstallDir();
                        update.OpenUpdatePath();
                        //Application.Quit();
                        //System.Environment.Exit(0);
                    }
                }

            }
        }

        void IExtensionApplication.Terminate()
        {
            // Do plug-in application clean up here
        }

    }

}
