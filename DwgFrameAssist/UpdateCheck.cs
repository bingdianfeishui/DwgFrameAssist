using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace DwgFrameAssist
{

    class UpdateCenter {
        public readonly string Dir = XmlUtil.getXmlValue("updatePath","value");

        public bool StatusOK { get; private set; }
        public string CurVer { get; private set; }
        public string RemoteVer { get; private set; }
        public string Message { get; private set; }
        public bool HasNewVersion { get { return CheckNewVersion(); } }
        public UpdateCenter()
        {
            try
            {
                var ass = Assembly.GetExecutingAssembly();
                var name = ass.GetName();
                CurVer = FileVersionInfo.GetVersionInfo(ass.Location).FileVersion;


                var remoteFile = Directory.GetFiles(Dir, "v_*")[0];

                var file = Path.Combine(Dir, remoteFile);
                RemoteVer = remoteFile.Split('_')[1];//FileVersionInfo.GetVersionInfo(file).FileVersion;

                Message = CheckUpdate();
                StatusOK = true;
            }
            catch (FileNotFoundException ex)
            {
                StatusOK = false;
                Message = "检查更新时发生错误:  文件不存在\r\n" + ex.Message + "\r\n";
            }
            catch (System.Exception ex)
            {
                StatusOK = false;
                Message = "检查更新时发生错误: " + ex.Message + "\r\n";
            }
        }

        private bool CheckNewVersion()
        {
            var curVer = long.Parse(CurVer.Replace(".", ""));
            var remoteVer = long.Parse(RemoteVer.Replace(".", ""));
            return remoteVer > curVer;
        }

        private string CheckUpdate()
        {
            if (HasNewVersion)
                return string.Format("========================检查更新=================\r\n远程目录：{0}\r\n远程文件版本：{1}\r\n当前文件版本：{2}\r\n请及时更新！\r\n", Dir, CurVer, RemoteVer);
            else
                return "当前文件为最新版本，无需更新!\r\n";
        }

        internal void OpenUpdatePath()
        {
            if (StatusOK && HasNewVersion && Directory.Exists(Dir))
                Process.Start(Dir);
        }

        internal void OpenInstallDir()
        {
            string installDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (StatusOK && HasNewVersion && Directory.Exists(installDir))                
                Process.Start(installDir);
        }
    }
}


