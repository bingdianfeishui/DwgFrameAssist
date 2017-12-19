using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DwgFrameAssist
{
    public class DwgInfo
    {
        public string DwgType { get; set; }
        public String DwgNO { get; set; }
        public String Name { get; set; }
        public String Version { get; set; }
        public String Scale { get; set; }
        public String Weight { get; set; }
        public String Secret { get; set; }
        public int TotalPages { get; set; }
        public int PageNO { get; set; }
        public String Direction { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public DwgInfo(string dwgType="部套", string dwgNO = "demo", string name="demo",
            string version = "demo", string scale = "demo", string weight = "demo",
            string secret ="普通商密", int totalPages=0, int pageNO=0,
            string direction = "demo", int width = 0, int height = 0) 
        {
            this.DwgType = dwgType;
            this.DwgNO = dwgNO;
            this.Name = name;
            this.Version = version;
            this.Scale = scale;
            this.Weight = weight;
            this.Secret = secret;
            this.TotalPages = totalPages;
            this.PageNO = pageNO;
            this.Direction = direction;
            this.Width = width;
            this.Height = height;
        }

        public override String ToString()
        {
            return DwgNO + ", " + Name + ", " + Version + ", " + Weight + ", " + Scale + ", " + Secret + ", " +
                Direction  + ", " +  Width  + ", " + Height   + ", " + TotalPages  + ", " +  PageNO;
        }

        public bool IsFrameSizeEqual(DwgInfo info)
        {
            return info.DwgType == DwgType && info.Height == Height && info.Width == Width;
        }
    }
}
