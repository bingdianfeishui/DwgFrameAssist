
using Acad = Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using System.IO;
using System.Xml;

namespace DwgFrameAssist
{
    public enum DwgInfoTypeEnum { 图号, 名称, 图标, 版本, 重量, 比例, 总张数, 张数, 密级, 左上图号 }

    class DwgFrameTools
    {
        private Acad.Document dwgDoc;
        private List<Point2d> cornerPts;
        private bool isPartDwg;
        private bool isExploded;
        private string tittleBlockName = "FitPDM_DWGFrame";
        private string layerName = "PDM_Title";


        public DwgInfo DwgInformation { get; private set; }
        public Dictionary<DwgInfoTypeEnum, ObjectId> DwgInfoTextDict = new Dictionary<DwgInfoTypeEnum, ObjectId>();
        public Dictionary<DwgInfoTypeEnum, List<ObjectId>> DwgInfoIds = new Dictionary<DwgInfoTypeEnum, List<ObjectId>>();
        //初始化
        public DwgFrameTools(Acad.Document doc, bool markArea = false)
        {
            try
            {
                this.dwgDoc = doc;
                this.cornerPts = GetFrameCornerPoint(GetFrameSelection(), markArea);
                if (cornerPts == null) throw new ArgumentNullException("空白图纸，无法处理。");
                this.isExploded = CheckIsExploded();
                this.isPartDwg = CheckIsPartDwg();

                InitDwgInfoObjIds();
                AnalysisDwgInfos();

                DwgInformation.DwgType = isPartDwg ? "零件" : "部套";

                if (markArea)
                {
                    foreach (var item in Enum.GetValues(typeof(DwgInfoTypeEnum)))
                    {
                        MarkDwgInfoArea(GetDwgInfoArea((DwgInfoTypeEnum)item, isPartDwg));
                    }
                    dwgDoc.Editor.Regen();

                }
                dwgDoc.Editor.WriteMessage("\r\n当前文件名：" + dwgDoc.Database.Filename + "\r\n");
                dwgDoc.Editor.WriteMessage("解析结果：" + this.DwgInformation.ToString() + "\r\n");
                dwgDoc.Editor.WriteMessage("是否炸开: " + isExploded + "\r\n");
                string tb = XmlUtil.getXmlValue("tittleBlockName", "value").Trim();
                if (!string.IsNullOrEmpty(tb))
                    tittleBlockName = tb;

                string tl = XmlUtil.getXmlValue("tittleLayerName", "value").Trim();
                if (!string.IsNullOrEmpty(tl))
                    layerName = tl;
            }
            catch
            {
                throw;
            }
        }


        //选择用来生成图框大小的元素。优先选择图框块，没选到则选择所有图元。
        private PromptSelectionResult GetFrameSelection()
        {
            Editor ed = dwgDoc.Editor;

            //选择图框块
            TypedValue[] acTypValAr = new TypedValue[1];
            acTypValAr.SetValue(new TypedValue((int)DxfCode.BlockName, tittleBlockName), 0);
            SelectionFilter acSelFilter = new SelectionFilter(acTypValAr);
            PromptSelectionResult acSSPrompt = ed.SelectAll(acSelFilter);

            //未找到图框块，则选择所有
            if (acSSPrompt.Status != PromptStatus.OK || acSSPrompt.Value.Count == 0)
                acSSPrompt = ed.SelectAll();

            return acSSPrompt;
        }

        //标题栏图层锁定与解锁工具
        private bool TitleLayerLockUtil(bool isLocked = false)
        {
            bool oldValue = false;
            Database acCurDb = dwgDoc.Database;
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {

                LayerTable acLyrTbl;
                acLyrTbl = acTrans.GetObject(acCurDb.LayerTableId,
                                                OpenMode.ForRead) as LayerTable;
                                
                if (acLyrTbl.Has(layerName))
                {
                    LayerTableRecord acLyrTblRec = acTrans.GetObject(acLyrTbl[layerName],
                                                    OpenMode.ForWrite) as LayerTableRecord;

                    oldValue = acLyrTblRec.IsLocked;
                    acLyrTblRec.IsLocked = isLocked;
                }

                acTrans.Commit();
            }
            return oldValue;
        }

        /**
         * 扫描获取图纸信息的DBText的ObjectId 
         */
        private void InitDwgInfoObjIds()
        {
            foreach (DwgInfoTypeEnum e in Enum.GetValues(typeof(DwgInfoTypeEnum)))
            {
                if (e == DwgInfoTypeEnum.图标) continue;
                DwgInfoTextDict.Add(e, GetDwgInfoTextId(e));
                DwgInfoIds.Add(e, GetDwgInfoTextIds(e));
            }
        }

        private List<ObjectId> GetDwgInfoTextIds(DwgInfoTypeEnum dwgInfoEnum)
        {
            Database acCurDb = dwgDoc.Database;
            List<ObjectId> list = new List<ObjectId>();
            using (dwgDoc.LockDocument())
            {
                // 启动一个事务  Start a transaction
                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    var pts = GetDwgInfoArea(dwgInfoEnum, isPartDwg);

                    // 以只读方式打开块表   Open the Block table for read
                    BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                                 OpenMode.ForRead) as BlockTable;


                    BlockTableRecord acBlkTblRec;

                    //炸开
                    if (isExploded)
                    {
                        acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                        TypedValue[] acTypValAr = new TypedValue[1];
                        acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "Text"), 0);
                        SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);
                        PromptSelectionResult acSSPrompt = this.dwgDoc.Editor.SelectWindow(new Point3d(pts[0].X, pts[0].Y, 0), new Point3d(pts[1].X, pts[1].Y, 0), acSelFtr);

                        // 如果提示状态是 OK，对象就被选择了    If the prompt status is OK, objects were selected
                        if (acSSPrompt.Status == PromptStatus.OK)
                        {
                            SelectionSet acSSet = acSSPrompt.Value;

                            foreach (SelectedObject acSSObj in acSSet)
                            {
                                if (acSSObj != null)
                                {
                                    DBText acEnt = acTrans.GetObject(acSSObj.ObjectId, OpenMode.ForRead) as DBText;

                                    if (acEnt != null && !acEnt.TextString.Contains("张"))
                                    {
                                        //dictInfos.Add(dwgInfoEnum, acEnt.ObjectId);
                                        if(acEnt.TextString != "")
                                            list.Add(acSSObj.ObjectId);
                                    }
                                }
                            }
                        }
                        
                    }
                    else    //图框未炸开
                    {
                        acBlkTblRec = acTrans.GetObject(acBlkTbl[tittleBlockName], OpenMode.ForWrite) as BlockTableRecord;
                        if (acBlkTblRec == null)
                            throw new NullReferenceException("未找到有效的标题块，请确认是否为标准图库！");

                        foreach (ObjectId id in acBlkTblRec)
                        {
                            if (id.ObjectClass.Name == "AcDbText")
                            {                               
                                DBText acEnt = acTrans.GetObject(id, OpenMode.ForRead) as DBText;
                                Point2d[] infoPts = GetDwgInfoArea(dwgInfoEnum, isPartDwg);     //获取dwgInfo所在的矩形区域
                                if (dwgInfoEnum == DwgInfoTypeEnum.名称)
                                    Console.WriteLine("");
                                Extents3d? bounds = acEnt.Bounds; ;
                                Point3d pos;
                                if (bounds.HasValue)
                                    pos = new Point3d((bounds.Value.MinPoint.X + bounds.Value.MaxPoint.X) / 2,
                                    (bounds.Value.MinPoint.Y + bounds.Value.MaxPoint.Y) / 2, 0);
                                else
                                    pos = acEnt.Position;

                                if (acEnt != null && !acEnt.TextString.Contains("张") && IsPointInArea(pos, infoPts))
                                {
                                    if(acEnt.TextString != "")
                                        list.Add(id);
                                }                           
                            }
                        }
                    }

                    //若未找到，则新建一个文本
                    if(list.Count == 0)
                        using (DBText acText = new DBText())
                        {
                            Point2d[] areaPts = GetDwgInfoArea(dwgInfoEnum, isPartDwg);
                            Point3d center = new Point3d((areaPts[0].X + areaPts[1].X) / 2, (areaPts[0].Y + areaPts[1].Y) / 2, 0);
                            double height = 5;
                            switch (dwgInfoEnum)
                            {
                                case DwgInfoTypeEnum.名称:
                                    height = 7;break;
                                case DwgInfoTypeEnum.图号:
                                    height = 6; break;
                                case DwgInfoTypeEnum.左上图号:
                                    height = 8; break;
                                case DwgInfoTypeEnum.版本:
                                case DwgInfoTypeEnum.重量:
                                case DwgInfoTypeEnum.比例:
                                    height = 3.5; break;
                                case DwgInfoTypeEnum.张数:
                                case DwgInfoTypeEnum.总张数:
                                    height = 2; break;
                            }                        
                        
                            acText.Height = height;
                            //acText.TextStyleName = "HZ";
                            acText.VerticalMode = TextVerticalMode.TextVerticalMid;
                            acText.HorizontalMode = TextHorizontalMode.TextCenter;
                            acText.AlignmentPoint = center;
                            //acText.Position = center;
                            acText.TextString = " ";//dwgInfoEnum.ToString();
                            acBlkTblRec.AppendEntity(acText);
                            acTrans.AddNewlyCreatedDBObject(acText, true);

                            acTrans.Commit();
                            list.Add(acText.ObjectId);
                        }

                    return list;
                }
            }
        }

        public void UpdateDwgInfo(DwgInfo info)
        {
            Dictionary<DwgInfoTypeEnum, String> dict = new Dictionary<DwgInfoTypeEnum, string>();

            dict.Add(DwgInfoTypeEnum.图号, info.DwgNO);
            dict.Add(DwgInfoTypeEnum.名称, info.Name);
            dict.Add(DwgInfoTypeEnum.版本, info.Version);
            dict.Add(DwgInfoTypeEnum.比例, info.Scale);
            dict.Add(DwgInfoTypeEnum.密级, info.Secret);
            dict.Add(DwgInfoTypeEnum.总张数, info.TotalPages.ToString());
            dict.Add(DwgInfoTypeEnum.张数, info.PageNO.ToString());
            dict.Add(DwgInfoTypeEnum.重量, info.Weight);
            UpdateDwgInfo(dict);
        }

        public void UpdateDwgInfo(Dictionary<DwgInfoTypeEnum, String> info)
        {

            Database acCurDb = dwgDoc.Database;
            using (dwgDoc.LockDocument())
            {
                bool oldValue = TitleLayerLockUtil(false);
                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    // 以只读方式打开块表   Open the Block table for read
                    BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                                 OpenMode.ForRead) as BlockTable;


                    BlockTableRecord acBlkTblRec;
                    if (isExploded)
                        acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
                    else
                        acBlkTblRec = acTrans.GetObject(acBlkTbl[tittleBlockName], OpenMode.ForRead) as BlockTableRecord;
                    if (acBlkTblRec == null)
                        throw new NullReferenceException("未找到有效的标题块，请确认是否为标准图库！");

                    foreach (var entry in info)
                    {
                        if (String.IsNullOrEmpty(entry.Value))
                            continue;
                        try
                        {
                            DBText txt = (acTrans.GetObject(DwgInfoTextDict[entry.Key], OpenMode.ForWrite) as DBText);
                            if (txt == null) continue;
                            if (entry.Key == DwgInfoTypeEnum.名称)
                            {
                                if (entry.Value.Contains("^"))
                                {
                                    string[] nameLines = entry.Value.Split(XmlUtil.getXmlValue("separator", "value")[0]);
                                    if (nameLines.Length > 2)
                                        dwgDoc.Editor.WriteMessage("警告：图纸名称行数过多，仅处理前两行！！\r\n");
                                    if (txt != null)
                                    {
                                        GenerateMultiLineName(nameLines, txt);
                                    }
                                }
                                else
                                {
                                    txt.TextString = entry.Value;
                                }
                                //Acad.Application.ShowAlertDialog(txt.WidthFactor.ToString());
                                var pts = GetDwgInfoArea(DwgInfoTypeEnum.名称);
                                var width = Math.Abs(pts[0].X - pts[1].X);  //名称自适应宽度
                                double factor = (width - 2) / txt.TextString.Length / txt.Height;
                                //Acad.Application.ShowAlertDialog(factor.ToString());
                                //if (txt.WidthFactor > factor || factor - txt.WidthFactor > 0.1)
                                txt.WidthFactor = Math.Min(0.8,factor);
                                
                                //Acad.Application.ShowAlertDialog(txt.WidthFactor.ToString());
                            }
                            else if (entry.Key == DwgInfoTypeEnum.图号)
                            {
                                //Acad.Application.ShowAlertDialog(txt.WidthFactor.ToString());
                                var pts = GetDwgInfoArea(DwgInfoTypeEnum.图号);
                                var width = Math.Abs(pts[0].X - pts[1].X);  //图号自适应宽度
                                double factor = (width-2) / entry.Value.Length / txt.Height;
                                if (txt.WidthFactor > factor)
                                    txt.WidthFactor = factor;
                                //Acad.Application.ShowAlertDialog(txt.WidthFactor.ToString());

                                txt.TextString = entry.Value;

                                txt = (acTrans.GetObject(DwgInfoTextDict[DwgInfoTypeEnum.左上图号], OpenMode.ForWrite) as DBText);
                                if (txt != null)
                                {
                                    txt.TextString = entry.Value;
                                    
                                }
                            }
                            else
                            {
                                txt.TextString = entry.Value;
                            }
                        }
                        catch(System.Exception e)
                        {
                            throw e;
                        }
                    }
                    acTrans.Commit();
                }
                TitleLayerLockUtil(oldValue);
            }
            dwgDoc.Editor.Regen();
        }

        //多行标题：单行文字方式
        private void GenerateMultiLineName(string[] nameLines, DBText txt)
        {
            txt.TextString = nameLines[0];
            

            Database acCurDb = dwgDoc.Database;
            using (dwgDoc.LockDocument())
            {
                // 启动一个事务  Start a transaction
                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    // 以只读方式打开块表   Open the Block table for read
                    BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                                 OpenMode.ForRead) as BlockTable;

                    // 以写方式打开模型空间块表记录   Open the Block table record Model space for write
                    BlockTableRecord acBlkTblRec;
                    acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                    OpenMode.ForWrite) as BlockTableRecord;

                    using (DBText t = new DBText())
                    {
                        int h = 5;
                        int.TryParse(XmlUtil.getXmlValue("h2Height", "value"), out h);
                        t.Height = h;
                        t.TextStyleId = txt.TextStyleId;
                        t.HorizontalMode = txt.HorizontalMode;
                        t.Color = txt.Color;
                        var pt = txt.AlignmentPoint;
                        t.AlignmentPoint = new Point3d(pt.X, pt.Y - (txt.Height + h) , pt.Z);
                        t.WidthFactor = 0.667;

                        t.TextString = XmlUtil.getXmlValue("prefix", "value") + nameLines[1] + XmlUtil.getXmlValue("suffix", "value");
                        // 添加新对象到块表记录和事务中   Add the new object to the block table record and the transaction
                        acBlkTblRec.AppendEntity(t);
                        acTrans.AddNewlyCreatedDBObject(t, true);
                    }



                    // 保存新对象到数据库中   Save the new object to the database
                    acTrans.Commit();
                }
            }
        }

        //多行标题：多行文字方式
        private void GenerateMultiLineName(string name)
        {
            Database acCurDb = dwgDoc.Database;
            using (dwgDoc.LockDocument())
            {
                // 启动一个事务  Start a transaction
                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    // 以只读方式打开块表   Open the Block table for read
                    BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                                 OpenMode.ForRead) as BlockTable;

                    // 以写方式打开模型空间块表记录   Open the Block table record Model space for write
                    BlockTableRecord acBlkTblRec;
                    acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                    OpenMode.ForWrite) as BlockTableRecord;

                    using (MText mtxt = new MText())
                    {
                        var pts =GetDwgInfoArea(DwgInfoTypeEnum.名称, isPartDwg);
                        mtxt.TextHeight = 7;
                        mtxt.Layer = "0";
                        mtxt.Location = new Point3d((pts[0].X + pts[1].X) / 2, (pts[0].Y + pts[1].Y) / 2, 0);
                        mtxt.Attachment = AttachmentPoint.MiddleCenter;

                        mtxt.Contents = name.Replace("^", "\r\n");

                        // 添加新对象到块表记录和事务中   Add the new object to the block table record and the transaction
                        acBlkTblRec.AppendEntity(mtxt);
                        acTrans.AddNewlyCreatedDBObject(mtxt, true);
                    }

                    

                    // 保存新对象到数据库中   Save the new object to the database
                    acTrans.Commit();
                }
            }
            //throw new NotImplementedException("还没实现");
        }

        /**
         * 检查图框是否炸开
         */
        private bool CheckIsExploded()
        {
            Database acCurDb = dwgDoc.Database;

            // 启动一个事务  Start a transaction
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // 以只读方式打开块表   Open the Block table for read
                BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                             OpenMode.ForRead) as BlockTable;

                // 以写方式打开模型空间块表记录   Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                OpenMode.ForRead) as BlockTableRecord;

                TypedValue[] acTypValAr = new TypedValue[2];
                acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "Text"), 0);
                acTypValAr.SetValue(new TypedValue((int)DxfCode.Text, "标准审查"), 1);
                SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);
                PromptSelectionResult acSSPrompt;
                acSSPrompt = this.dwgDoc.Editor.SelectAll(acSelFtr);

                // 如果提示状态是 OK，对象就被选择了    If the prompt status is OK, objects were selected
                if (acSSPrompt.Status == PromptStatus.OK)
                {
                    SelectionSet acSSet = acSSPrompt.Value;

                    if (acSSet.Count > 0)
                        return true;
                }

            }

            return false;
        }

        private void AnalysisDwgInfos()
        {
            DwgInformation = new DwgInfo();
            Database acCurDb = dwgDoc.Database;
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // 以只读方式打开块表   Open the Block table for read
                BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                             OpenMode.ForRead) as BlockTable;


                BlockTableRecord acBlkTblRec;
                if (isExploded)
                    acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
                else
                    acBlkTblRec = acTrans.GetObject(acBlkTbl[tittleBlockName], OpenMode.ForRead) as BlockTableRecord;
                if (acBlkTblRec == null)
                    throw new NullReferenceException("未找到有效的标题块，请确认是否为标准图库！");

                DBText txt = (acTrans.GetObject(DwgInfoTextDict[DwgInfoTypeEnum.图号], OpenMode.ForRead) as DBText);
                if (txt != null)
                    DwgInformation.DwgNO = txt.TextString;

                txt = (acTrans.GetObject(DwgInfoTextDict[DwgInfoTypeEnum.名称], OpenMode.ForRead) as DBText);
                if (txt != null)
                    DwgInformation.Name = txt.TextString;

                txt = (acTrans.GetObject(DwgInfoTextDict[DwgInfoTypeEnum.版本], OpenMode.ForRead) as DBText);
                if (txt != null)
                    DwgInformation.Version = txt.TextString;

                txt = (acTrans.GetObject(DwgInfoTextDict[DwgInfoTypeEnum.重量], OpenMode.ForRead) as DBText);
                if (txt != null)
                    DwgInformation.Weight = txt.TextString;

                txt = (acTrans.GetObject(DwgInfoTextDict[DwgInfoTypeEnum.比例], OpenMode.ForRead) as DBText);
                if (txt != null)
                    DwgInformation.Scale = txt.TextString;

                txt = (acTrans.GetObject(DwgInfoTextDict[DwgInfoTypeEnum.密级], OpenMode.ForRead) as DBText);
                if (txt != null)
                    DwgInformation.Secret = txt.TextString;


                int totalPages = 0, page = 0;
                txt = (acTrans.GetObject(DwgInfoTextDict[DwgInfoTypeEnum.总张数], OpenMode.ForRead) as DBText);
                if (txt != null)
                    Int32.TryParse(txt.TextString, out totalPages);
                DwgInformation.TotalPages = totalPages;

                txt = (acTrans.GetObject(DwgInfoTextDict[DwgInfoTypeEnum.张数], OpenMode.ForRead) as DBText);
                if (txt != null)
                    Int32.TryParse(txt.TextString, out page);
                DwgInformation.PageNO = page;

                DwgInformation.Direction = (cornerPts[1].Y < cornerPts[1].X) ? "横向" : "纵向";

                DwgInformation.Width = (int)(cornerPts[1].X - cornerPts[0].X);
                DwgInformation.Height = (int)(cornerPts[1].Y - cornerPts[0].Y);
            }


        }

        /**
         * 获取dwgInfoEnum对应的Text对象的ObjectId
         * */
        private ObjectId GetDwgInfoTextId(DwgInfoTypeEnum dwgInfoEnum)
        {
            Database acCurDb = dwgDoc.Database;
            using (dwgDoc.LockDocument())
            {
                // 启动一个事务  Start a transaction
                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    var pts = GetDwgInfoArea(dwgInfoEnum, isPartDwg);

                    // 以只读方式打开块表   Open the Block table for read
                    BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                                 OpenMode.ForRead) as BlockTable;


                    BlockTableRecord acBlkTblRec;

                    //炸开
                    if (isExploded)
                    {
                        acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                        TypedValue[] acTypValAr = new TypedValue[1];
                        acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "Text"), 0);
                        SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);
                        PromptSelectionResult acSSPrompt = this.dwgDoc.Editor.SelectWindow(new Point3d(pts[0].X, pts[0].Y, 0), new Point3d(pts[1].X, pts[1].Y, 0), acSelFtr);

                        // 如果提示状态是 OK，对象就被选择了    If the prompt status is OK, objects were selected
                        if (acSSPrompt.Status == PromptStatus.OK)
                        {
                            SelectionSet acSSet = acSSPrompt.Value;

                            foreach (SelectedObject acSSObj in acSSet)
                            {
                                if (acSSObj != null)
                                {
                                    DBText acEnt = acTrans.GetObject(acSSObj.ObjectId, OpenMode.ForRead) as DBText;

                                    if (acEnt != null && !acEnt.TextString.Contains("张"))
                                    {
                                        //dictInfos.Add(dwgInfoEnum, acEnt.ObjectId);
                                        if (acEnt.TextString != "")
                                            return acSSObj.ObjectId;
                                    }
                                }
                            }
                        }
                    }
                    else    //图框未炸开
                    {
                        acBlkTblRec = acTrans.GetObject(acBlkTbl[tittleBlockName], OpenMode.ForWrite) as BlockTableRecord;
                        if (acBlkTblRec == null)
                            throw new NullReferenceException("未找到有效的标题块，请确认是否为标准图库！");

                        foreach (ObjectId id in acBlkTblRec)
                        {
                            if (id.ObjectClass.Name == "AcDbText")
                            {
                                DBText acEnt = acTrans.GetObject(id, OpenMode.ForRead) as DBText;
                                Point2d[] infoPts = GetDwgInfoArea(dwgInfoEnum, isPartDwg);     //获取dwgInfo所在的矩形区域
                                if (dwgInfoEnum == DwgInfoTypeEnum.名称)
                                    Console.WriteLine("");
                                Extents3d? bounds = acEnt.Bounds; ;
                                Point3d pos;
                                if (bounds.HasValue)
                                    pos = new Point3d((bounds.Value.MinPoint.X + bounds.Value.MaxPoint.X) / 2,
                                    (bounds.Value.MinPoint.Y + bounds.Value.MaxPoint.Y) / 2, 0);
                                else
                                    pos = acEnt.Position;

                                if (acEnt != null && !acEnt.TextString.Contains("张") && IsPointInArea(pos, infoPts))
                                {
                                    if (acEnt.TextString != "")
                                        return id;
                                }
                            }
                        }
                    }

                    //若未找到，则新建一个文本
                    using (DBText acText = new DBText())
                    {
                        Point2d[] areaPts = GetDwgInfoArea(dwgInfoEnum, isPartDwg);
                        Point3d center = new Point3d((areaPts[0].X + areaPts[1].X) / 2, (areaPts[0].Y + areaPts[1].Y) / 2, 0);
                        double height = 5;
                        switch (dwgInfoEnum)
                        {
                            case DwgInfoTypeEnum.名称:
                                height = 7;break;
                            case DwgInfoTypeEnum.图号:
                                height = 6; break;
                            case DwgInfoTypeEnum.左上图号:
                                height = 8; break;
                            case DwgInfoTypeEnum.版本:
                            case DwgInfoTypeEnum.重量:
                            case DwgInfoTypeEnum.比例:
                                height = 3.5; break;
                            case DwgInfoTypeEnum.张数:
                            case DwgInfoTypeEnum.总张数:
                                height = 2; break;
                        }                        
                        
                        acText.Height = height;
                        //acText.TextStyleId = hzStyleId;
                        acText.Layer = layerName;
                        acText.ColorIndex = 2;
                        acText.VerticalMode = TextVerticalMode.TextVerticalMid;
                        acText.HorizontalMode = TextHorizontalMode.TextCenter;
                        acText.AlignmentPoint = center;
                        //acText.Position = center;
                        acText.TextString = " ";//dwgInfoEnum.ToString();
                        acBlkTblRec.AppendEntity(acText);
                        acTrans.AddNewlyCreatedDBObject(acText, true);

                        acTrans.Commit();
                        return acText.ObjectId;
                    }
                }
            }
        }

        private bool IsPointInArea(Point3d pt3d, Point2d[] pt2d)
        {
            return IsPointInArea(pt3d, new Point3d[] { new Point3d(pt2d[0].X, pt2d[0].Y, 0), new Point3d(pt2d[1].X, pt2d[1].Y, 0) });

        }

        private bool IsPointInArea(Point3d pt3d, Point3d[] areaPt)
        {
            return pt3d.X > areaPt[0].X && pt3d.X < areaPt[1].X && pt3d.Y > areaPt[0].Y && pt3d.Y < areaPt[1].Y;
        }

        private bool CheckIsPartDwg()
        {
            try
            {
                Database acCurDb = dwgDoc.Database;
                using (dwgDoc.LockDocument())
                {
                    // 启动一个事务  Start a transaction
                    using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                    {
                        var pts = GetDwgInfoArea(DwgInfoTypeEnum.图标, false);

                        // 以只读方式打开块表   Open the Block table for read
                        BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;

                        BlockTableRecord acBlkTblRec;
                        if (isExploded)
                        {
                            // 以读方式打开模型空间块表记录
                            acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;

                            TypedValue[] acTypValAr = new TypedValue[1];
                            acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "Text"), 0);
                            SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);
                            PromptSelectionResult acSSPrompt = this.dwgDoc.Editor.SelectWindow(new Point3d(pts[0].X, pts[0].Y, 0), new Point3d(pts[1].X, pts[1].Y, 0), acSelFtr);

                            // 如果提示状态是 OK，对象就被选择了    If the prompt status is OK, objects were selected
                            if (acSSPrompt.Status == PromptStatus.OK)
                            {
                                SelectionSet acSSet = acSSPrompt.Value;

                                foreach (SelectedObject acSSObj in acSSet)
                                {
                                    if (acSSObj != null)
                                    {
                                        DBText acEnt = acTrans.GetObject(acSSObj.ObjectId, OpenMode.ForRead) as DBText;

                                        if (acEnt.TextString.Contains("张"))
                                            return true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            acBlkTblRec = acTrans.GetObject(acBlkTbl[tittleBlockName], OpenMode.ForRead) as BlockTableRecord;
                            if (acBlkTblRec == null)
                                throw new NullReferenceException("未找到有效的标题块，请确认是否为标准图库！");

                            foreach (ObjectId objId in acBlkTblRec)
                            {
                                if (objId.ObjectClass.Name == "AcDbText")
                                {
                                    DBText ent = acTrans.GetObject(objId, OpenMode.ForRead) as DBText;

                                    if (ent != null && ent.TextString.Contains("张"))
                                    {
                                        Point3d pt = ent.AlignmentPoint;
                                        if (pt.X > pts[0].X && pt.X < pts[1].X && pt.Y > pts[0].Y && pt.Y < pts[1].Y)
                                        {
                                            return true;
                                        }
                                    }
                                }

                            }
                        }

                    }
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
            return false;
        }

        #region 获取图纸绘图区域
        private List<Point2d> GetFrameCornerPoint(PromptSelectionResult psr, bool markFrame = false)
        {
            List<Point2d> listPts = new List<Point2d>();
            Database db = dwgDoc.Database;
            Editor ed = dwgDoc.Editor;

            //PromptSelectionResult psr = ed.SelectAll();

            // Get the current UCS

            CoordinateSystem3d ucs = ed.CurrentUserCoordinateSystem.CoordinateSystem3d;

            if (psr.Status != PromptStatus.OK)
                return null;

            // Collect points on the component entities

            Point3dCollection pts = new Point3dCollection();
            using (dwgDoc.LockDocument())
            {
                Transaction tr = db.TransactionManager.StartTransaction();
                using (tr)
                {
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject( db.CurrentSpaceId, OpenMode.ForWrite );

                    for (int i = 0; i < psr.Value.Count; i++)
                    {
                        Entity ent =(Entity)tr.GetObject(psr.Value[i].ObjectId,OpenMode.ForRead);

                        // Collect the points for each selected entity

                        Point3dCollection entPts = CollectPoints(tr, ent);
                        foreach (Point3d pt in entPts)
                        {
                            /*
                             * Create a DBPoint, for testing purposes
                             *
                            DBPoint dbp = new DBPoint(pt);
                            btr.AppendEntity(dbp);
                            tr.AddNewlyCreatedDBObject(dbp, true);
                             */

                            pts.Add(pt);
                        }

                        // Create a boundary for each entity (if so chosen) or
                        // just once after collecting all the points

                        if (i == psr.Value.Count - 1)
                        {
                            try
                            {
                                listPts = GetBoundaryPoints(pts, ucs, 0);

                                if (markFrame)  //是否绘制边界矩形框
                                    CreateBoundaryEntity(ucs, tr, btr, listPts);
                            }
                            catch
                            {
                                ed.WriteMessage("\n识别边界失败.");
                            }

                            pts.Clear();
                        }
                    }

                    tr.Commit();
                }
            }
            return listPts;
        }

        private void CreateBoundaryEntity(CoordinateSystem3d ucs, Transaction tr, BlockTableRecord btr, List<Point2d> pts)
        {
            double minX = pts[0].X,
                minY = pts[0].Y,
                maxX = pts[1].X,
                maxY = pts[1].Y;


            // Create the boundary points

            Point2d pt0 = new Point2d(minX, minY),
                    pt1 = new Point2d(minX, maxY),
                    pt2 = new Point2d(maxX, maxY),
                    pt3 = new Point2d(maxX, minY);

            // Finally we create the polyline

            var p = new Polyline(4);
            Plane pl = new Plane(ucs.Origin, ucs.Zaxis);
            p.Normal = pl.Normal;
            p.ColorIndex = 2;
            p.AddVertexAt(0, pt0, 0, 0, 0);
            p.AddVertexAt(1, pt1, 0, 0, 0);
            p.AddVertexAt(2, pt2, 0, 0, 0);
            p.AddVertexAt(3, pt3, 0, 0, 0);
            p.Closed = true;

            btr.AppendEntity(p);
            tr.AddNewlyCreatedDBObject(p, true);
        }

        private Point3dCollection CollectPoints(Transaction tr, Entity ent)
        {
            // The collection of points to populate and return

            Point3dCollection pts = new Point3dCollection();

            // We'll start by checking a block reference for
            // attributes, getting their bounds and adding
            // them to the point list. We'll still explode
            // the BlockReference later, to gather points
            // from other geometry, it's just that approach
            // doesn't work for attributes (we only get the
            // AttributeDefinitions, which don't have bounds)

            BlockReference br = ent as BlockReference;
            if (br != null)
            {
                foreach (ObjectId arId in br.AttributeCollection)
                {
                    DBObject obj = tr.GetObject(arId, OpenMode.ForRead);
                    if (obj is AttributeReference)
                    {
                        AttributeReference ar = (AttributeReference)obj;
                        ExtractBounds(ar, pts);
                    }
                }
            }

            // If we have a curve - other than a polyline, which
            // we will want to explode - we'll get points along
            // its length

            Curve cur = ent as Curve;
            if (cur != null &&
                !(cur is Polyline ||
                  cur is Polyline2d ||
                  cur is Polyline3d))
            {
                // Two points are enough for a line, we'll go with
                // a higher number for other curves

                int segs = (ent is Line ? 2 : 20);

                double param = cur.EndParam - cur.StartParam;
                for (int i = 0; i < segs; i++)
                {
                    try
                    {
                        Point3d pt =
                          cur.GetPointAtParameter(
                            cur.StartParam + (i * param / (segs - 1))
                          );
                        pts.Add(pt);
                    }
                    catch { }
                }
            }
            else if (ent is DBPoint)
            {
                // Points are easy

                pts.Add(((DBPoint)ent).Position);
            }
            else if (ent is DBText)
            {
                // For DBText we use the same approach as
                // for AttributeReferences

                ExtractBounds((DBText)ent, pts);
            }
            else if (ent is MText)
            {
                // MText is also easy - you get all four corners
                // returned by a function. That said, the points
                // are of the MText's box, so may well be different
                // from the bounds of the actual contents

                MText txt = (MText)ent;
                Point3dCollection pts2 = txt.GetBoundingPoints();
                foreach (Point3d pt in pts2)
                {
                    pts.Add(pt);
                }
            }
            else if (ent is Face)
            {
                Face f = (Face)ent;
                try
                {
                    for (short i = 0; i < 4; i++)
                    {
                        pts.Add(f.GetVertexAt(i));
                    }
                }
                catch { }
            }
            else if (ent is Solid)
            {
                Solid sol = (Solid)ent;
                try
                {
                    for (short i = 0; i < 4; i++)
                    {
                        pts.Add(sol.GetPointAt(i));
                    }
                }
                catch { }
            }
            else
            {
                // Here's where we attempt to explode other types
                // of object

                DBObjectCollection oc = new DBObjectCollection();
                try
                {
                    ent.Explode(oc);
                    if (oc.Count > 0)
                    {
                        foreach (DBObject obj in oc)
                        {
                            Entity ent2 = obj as Entity;
                            if (ent2 != null && ent2.Visible)
                            {
                                foreach (Point3d pt in CollectPoints(tr, ent2))
                                {
                                    pts.Add(pt);
                                }
                            }
                            obj.Dispose();
                        }
                    }
                }
                catch { }
            }
            return pts;
        }

        private void ExtractBounds(DBText txt, Point3dCollection pts)
        {
            // We have a special approach for DBText and
            // AttributeReference objects, as we want to get
            // all four corners of the bounding box, even
            // when the text or the containing block reference
            // is rotated

            if (txt.Bounds.HasValue && txt.Visible)
            {
                // Create a straight version of the text object
                // and copy across all the relevant properties
                // (stopped copying AlignmentPoint, as it would
                // sometimes cause an eNotApplicable error)

                // We'll create the text at the WCS origin
                // with no rotation, so it's easier to use its
                // extents

                DBText txt2 = new DBText();
                txt2.Normal = Vector3d.ZAxis;
                txt2.Position = Point3d.Origin;

                // Other properties are copied from the original

                txt2.TextString = txt.TextString;
                txt2.TextStyleId = txt.TextStyleId;
                txt2.LineWeight = txt.LineWeight;
                txt2.Thickness = txt2.Thickness;
                txt2.HorizontalMode = txt.HorizontalMode;
                txt2.VerticalMode = txt.VerticalMode;
                txt2.WidthFactor = txt.WidthFactor;
                txt2.Height = txt.Height;
                txt2.IsMirroredInX = txt2.IsMirroredInX;
                txt2.IsMirroredInY = txt2.IsMirroredInY;
                txt2.Oblique = txt.Oblique;

                // Get its bounds if it has them defined
                // (which it should, as the original did)

                if (txt2.Bounds.HasValue)
                {
                    Point3d maxPt = txt2.Bounds.Value.MaxPoint;

                    // Place all four corners of the bounding box
                    // in an array

                    Point2d[] bounds =
                      new Point2d[] {
              Point2d.Origin,
              new Point2d(0.0, maxPt.Y),
              new Point2d(maxPt.X, maxPt.Y),
              new Point2d(maxPt.X, 0.0)
            };

                    // We're going to get each point's WCS coordinates
                    // using the plane the text is on

                    Plane pl = new Plane(txt.Position, txt.Normal);

                    // Rotate each point and add its WCS location to the
                    // collection

                    foreach (Point2d pt in bounds)
                    {
                        pts.Add(
                          pl.EvaluatePoint(
                            pt.RotateBy(txt.Rotation, Point2d.Origin)
                          )
                        );
                    }
                }
            }
        }

        private List<Point2d> GetBoundaryPoints(Point3dCollection pts, CoordinateSystem3d ucs, double buffer)
        {
            // Get the plane of the UCS

            Plane pl = new Plane(ucs.Origin, ucs.Zaxis);

            // We will project these (possibly 3D) points onto
            // the plane of the current UCS, as that's where
            // we will create our circle

            // Project the points onto it

            List<Point2d> pts2d = new List<Point2d>(pts.Count);
            for (int i = 0; i < pts.Count; i++)
            {
                pts2d.Add(pl.ParameterOf(pts[i]));
            }

            // Assuming we have some points in our list...

            if (pts.Count > 0)
            {
                // Set the initial min and max values from the first entry

                double minX = pts2d[0].X,
                       maxX = minX,
                       minY = pts2d[0].Y,
                       maxY = minY;

                // Perform a single iteration to extract the min/max X and Y

                for (int i = 1; i < pts2d.Count; i++)
                {
                    Point2d pt = pts2d[i];
                    if (pt.X < minX) minX = pt.X;
                    if (pt.X > maxX) maxX = pt.X;
                    if (pt.Y < minY) minY = pt.Y;
                    if (pt.Y > maxY) maxY = pt.Y;
                }

                // Our final buffer amount will be the percentage of the
                // smallest of the dimensions

                double buf =
                  Math.Min(maxX - minX, maxY - minY) * buffer;

                // Apply the buffer to our point ordinates

                minX -= buf;
                minY -= buf;
                maxX += buf;
                maxY += buf;

                return new List<Point2d>() { new Point2d(minX, minY), new Point2d(maxX, maxY) };
            }
            return null;
        }

        #endregion 获取图纸绘图区域

        public Point2d[] GetDwgInfoArea(DwgInfoTypeEnum dwgInfoType, bool partDwg = false)
        {
            Point2d[] pts = new Point2d[2];

            int blank;
            if (System.Math.Abs(cornerPts[0].X - cornerPts[1].X) > 297 || System.Math.Abs(cornerPts[0].Y - cornerPts[1].Y) > 297)
                blank = 10;
            else
                blank = 5;

            switch (dwgInfoType)
            {
                //===============标题栏==============
                case DwgInfoTypeEnum.图号:
                    if (partDwg)
                    {
                        pts[0] = new Point2d(cornerPts[1].X - 60 - blank, cornerPts[0].Y + 30 + blank);
                        pts[1] = new Point2d(cornerPts[1].X - blank, cornerPts[0].Y + 43 + blank);
                    }
                    else
                    {
                        pts[0] = new Point2d(cornerPts[1].X - 60 - blank, cornerPts[0].Y + 48 + blank);
                        pts[1] = new Point2d(cornerPts[1].X - blank, cornerPts[0].Y + 68 + blank);
                    }
                    break;
                case DwgInfoTypeEnum.名称:
                    if (partDwg)
                    {
                        pts[0] = new Point2d(cornerPts[1].X - 118 - blank, cornerPts[0].Y + 18 + blank);
                        pts[1] = new Point2d(cornerPts[1].X - 60 - blank, cornerPts[0].Y + 48 + blank);
                    }
                    else
                    {
                        pts[0] = new Point2d(cornerPts[1].X - 118 - blank, cornerPts[0].Y + 20 + blank);
                        pts[1] = new Point2d(cornerPts[1].X - 60 - blank, cornerPts[0].Y + 68 + blank);
                    }
                    break;

                case DwgInfoTypeEnum.图标:
                    if (partDwg)
                    {
                        pts[0] = new Point2d(cornerPts[1].X - 60 - blank, cornerPts[0].Y + blank);
                        pts[1] = new Point2d(cornerPts[1].X - blank, cornerPts[0].Y + 13 + blank);
                    }
                    else
                    {
                        pts[0] = new Point2d(cornerPts[1].X - 60 - blank, cornerPts[0].Y + blank);
                        pts[1] = new Point2d(cornerPts[1].X - blank, cornerPts[0].Y + 20 + blank);
                    }
                    break;

                case DwgInfoTypeEnum.版本:
                    if (partDwg)
                    {
                        pts[0] = new Point2d(cornerPts[1].X - 60 - blank, cornerPts[0].Y + 18 + blank);
                        pts[1] = new Point2d(cornerPts[1].X - 40 - blank, cornerPts[0].Y + 25 + blank);
                    }
                    else
                    {
                        pts[0] = new Point2d(cornerPts[1].X - 60 - blank, cornerPts[0].Y + 28 + blank);
                        pts[1] = new Point2d(cornerPts[1].X - 40 - blank, cornerPts[0].Y + 40 + blank);
                    }
                    break;

                case DwgInfoTypeEnum.重量:
                    if (partDwg)
                    {
                        pts[0] = new Point2d(cornerPts[1].X - 40 - blank, cornerPts[0].Y + 18 + blank);
                        pts[1] = new Point2d(cornerPts[1].X - 20 - blank, cornerPts[0].Y + 25 + blank);
                    }
                    else
                    {
                        pts[0] = new Point2d(cornerPts[1].X - 40 - blank, cornerPts[0].Y + 28 + blank);
                        pts[1] = new Point2d(cornerPts[1].X - 20 - blank, cornerPts[0].Y + 40 + blank);
                    }
                    break;
                case DwgInfoTypeEnum.比例:
                    if (partDwg)
                    {
                        pts[0] = new Point2d(cornerPts[1].X - 20 - blank, cornerPts[0].Y + 18 + blank);
                        pts[1] = new Point2d(cornerPts[1].X - blank, cornerPts[0].Y + 25 + blank);
                    }
                    else
                    {
                        pts[0] = new Point2d(cornerPts[1].X - 20 - blank, cornerPts[0].Y + 28 + blank);
                        pts[1] = new Point2d(cornerPts[1].X - blank, cornerPts[0].Y + 40 + blank);
                    }
                    break;

                case DwgInfoTypeEnum.总张数:
                    if (partDwg)
                    {
                        pts[0] = new Point2d(cornerPts[1].X - 60 - blank, cornerPts[0].Y + 13 + blank);
                        pts[1] = new Point2d(cornerPts[1].X - 30 - blank, cornerPts[0].Y + 18 + blank);
                    }
                    else
                    {
                        pts[0] = new Point2d(cornerPts[1].X - 60 - blank, cornerPts[0].Y + 20 + blank);
                        pts[1] = new Point2d(cornerPts[1].X - 30 - blank, cornerPts[0].Y + 28 + blank);
                    }
                    break;

                case DwgInfoTypeEnum.张数:
                    if (partDwg)
                    {
                        pts[0] = new Point2d(cornerPts[1].X - 30 - blank, cornerPts[0].Y + 13 + blank);
                        pts[1] = new Point2d(cornerPts[1].X - blank, cornerPts[0].Y + 18 + blank);
                    }
                    else
                    {
                        pts[0] = new Point2d(cornerPts[1].X - 30 - blank, cornerPts[0].Y + 20 + blank);
                        pts[1] = new Point2d(cornerPts[1].X - blank, cornerPts[0].Y + 28 + blank);
                    }
                    break;

                //===============左上角角标==============
                case DwgInfoTypeEnum.密级:
                    pts[0] = new Point2d(cornerPts[0].X + 80, cornerPts[1].Y - 15 - blank);
                    pts[1] = new Point2d(cornerPts[0].X + 125, cornerPts[1].Y - blank);
                    break;
                case DwgInfoTypeEnum.左上图号:
                    pts[0] = new Point2d(cornerPts[0].X + 25, cornerPts[1].Y - 15 - blank);
                    pts[1] = new Point2d(cornerPts[0].X + 70, cornerPts[1].Y - blank);
                    break;
                default:
                    break;
            }
            return pts;
        }

        public void MarkDwgInfoArea(Point2d[] pts)
        {
            Database acCurDb = dwgDoc.Database;
            using (dwgDoc.LockDocument())
            {
                // 启动一个事务  Start a transaction
                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    // 以只读方式打开块表   Open the Block table for read
                    BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                                 OpenMode.ForRead) as BlockTable;

                    // 以写方式打开模型空间块表记录   Open the Block table record Model space for write
                    BlockTableRecord acBlkTblRec;
                    acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                    OpenMode.ForWrite) as BlockTableRecord;

                    // 创建一条直线
                    Line acLine = new Line(new Point3d(pts[0].X, pts[0].Y, 0),
                                           new Point3d(pts[1].X, pts[1].Y, 0));

                    acLine.SetDatabaseDefaults();

                    // 添加新对象到块表记录和事务中   Add the new object to the block table record and the transaction
                    acBlkTblRec.AppendEntity(acLine);
                    acTrans.AddNewlyCreatedDBObject(acLine, true);

                    // 保存新对象到数据库中   Save the new object to the database
                    acTrans.Commit();
                }
            }
        }

        public void ReplaceFrameBlock(string blkFile)
        {
            string frameBlockName = XmlUtil.getXmlValue("tittleBlockName","value").Trim();
            if (String.IsNullOrEmpty(frameBlockName))
                frameBlockName = "FitPDM_DWGFrame";

            if (!File.Exists(blkFile))
                throw new FileNotFoundException("目标文件不存在！", blkFile);

            Database acCurDb = dwgDoc.Database;
            using (dwgDoc.LockDocument())
            {
                bool oldValue = TitleLayerLockUtil(false);
                // 启动一个事务  Start a transaction
                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    using (Database db = new Database(false, true))
                    {
                        db.ReadDwgFile(blkFile, System.IO.FileShare.ReadWrite, true, "");

                        //acCurDb.Insert("test1", db, true);//整张图纸作为指定块名的块插入。如有同名块，不会覆盖。


                        //复制新文件中的图框到当前文件中.DuplicateRecordCloning.Replace:存在同名图块则覆盖
                        ObjectIdCollection ids = new ObjectIdCollection();
                        using (Transaction tr = db.TransactionManager.StartTransaction())
                        {
                            BlockTable bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                            if (bt.Has(frameBlockName))
                                ids.Add(bt[frameBlockName]);

                            tr.Commit();
                        }
                        if (ids.Count > 0)
                        {
                            acCurDb.WblockCloneObjects(ids, acCurDb.BlockTableId, new IdMapping(), DuplicateRecordCloning.Replace, false);
                        }

                    }
                  
                    BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                                                 
                    BlockTableRecord acBlkTblRec = acTrans.GetObject(acBlkTbl[tittleBlockName], OpenMode.ForWrite) as BlockTableRecord;

                    TitleLayerLockUtil(false);
                    foreach (ObjectId id in acBlkTblRec.GetBlockReferenceIds(false, true))
                    {
                        BlockReference acBlkRef = acTrans.GetObject(id, OpenMode.ForWrite) as BlockReference;
                        acBlkRef.RecordGraphicsModified(true);
                    }
                    acTrans.Commit();
                }
                TitleLayerLockUtil(oldValue);
            }
            dwgDoc.Editor.Regen();
        }

        public string GenerateStandardDwgFileName(bool isPartDwg, bool isHorizontal, string size)
        {
            string dwgType = isPartDwg? "零件":"部套";
            string direction = isHorizontal ? "横向" : "纵向";

            string dllDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string sourcePath = Path.Combine(dllDir, XmlUtil.getXmlValue("sourceFolder", "value") + dwgType);

            return Path.Combine(sourcePath, dwgType + "-" + direction +"-" + size + ".dwg");
        }

        

    }
}
