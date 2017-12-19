using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace DwgFrameAssist
{
    class XmlUtil
    {
        static string fileName = "config.xml";
        /// <summary>  
        /// 返回XMl文件指定元素的指定属性值  
        /// </summary>  
        /// <param name="xmlElement">指定元素</param>  
        /// <param name="xmlAttribute">指定属性</param>  
        /// <returns></returns>  
        public static string getXmlValue(string xmlElement, string xmlAttribute)
        {
            XDocument xmlDoc = XDocument.Load(getXmlName());
            var results = from c in xmlDoc.Descendants(xmlElement)
                          select c;
            string s = "";
            foreach (var result in results)
            {
                s = result.Attribute(xmlAttribute).Value.ToString();
            }
            return s;
        }

        /// <summary>  
        /// 设置XMl文件指定元素的指定属性的值  
        /// </summary>  
        /// <param name="xmlElement">指定元素</param>  
        /// <param name="xmlAttribute">指定属性</param>  
        /// <param name="xmlValue">指定值</param>  
        public static void setXmlValue(string xmlElement, string xmlAttribute, string xmlValue)
        {
            XDocument xmlDoc = XDocument.Load(getXmlName());
            xmlDoc.Element("Configuration").Element(xmlElement).Attribute(xmlAttribute).SetValue(xmlValue);
            xmlDoc.Save(getXmlName());
        }

        private static string getXmlName()
        {
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return System.IO.Path.Combine(path, fileName);
        }

        public static Dictionary<string, Dictionary<string, List<string>>> GetDwgTypeValues()
        {
            var types = new Dictionary<string, Dictionary<string, List<string>>>();
            
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(getXmlName());
            var dwgtypes = xmlDoc.SelectNodes("//dwgtype"); ;
            foreach (var t in dwgtypes)//部套、零件
            {
                
                var xe = (XmlElement)t;
                string type = xe.GetAttribute("type"); 

                var dwgDict = new Dictionary<String, List<string>>(); //<方向， 图幅>
                //Console.WriteLine("DWG TYPE: " + type + ":");

                var directions = (t as XmlNode).ChildNodes;
                foreach (var dr in directions)//横向、纵向
                {
                    var direction = (dr as XmlElement).GetAttribute("direction");

                    List<string> sizes = new List<string>();
                    var dwgs = (dr as XmlNode).ChildNodes;
                    foreach (var el in dwgs)//图幅
                    {
                        sizes.Add((el as XmlElement).GetAttribute("size"));
                        //Console.WriteLine((el as XmlElement).GetAttribute("size"));
                    }
                    //Console.WriteLine((el as XmlElement).GetAttribute("size"));
                    dwgDict.Add(direction, sizes);
                }
                types.Add(type, dwgDict);
            }

            return types;
        }
    }
}