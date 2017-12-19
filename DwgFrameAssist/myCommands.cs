// (C) Copyright 2017 by Sky123.Org 
//
using System;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Windows;
using System.Collections.Generic;
using System.Threading;
using System.IO;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(DwgFrameAssist.MyCommands))]

namespace DwgFrameAssist
{

    // This class is instantiated by AutoCAD for each document when
    // a command is called by the user the first time in the context
    // of a given document. In other words, non static data in this class
    // is implicitly per-document!
    public partial class MyCommands
    {
        PaletteSet myPaletteSet;
        SingleDwgInfo control;
        //ModifyFrameSize framesize;
        //修改标题框
        [CommandMethod("edwg", CommandFlags.Session)]
        public void EditDwgTittleBlock()
        {
            //Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage("Hello AutoCAD!");
            try
            {
                string title = "图框修改器 beta";
                if (myPaletteSet == null)
                {
                    myPaletteSet = new PaletteSet("DwgFrameAssist", new System.Guid("D61D0875-A507-4b73-8B5F-9266BEACD096"));

                    control = new SingleDwgInfo();
                    myPaletteSet.Add(title, control);

                    //myPaletteSet.Size = new System.Drawing.Size(control.Size.Width, control.Size.Height+20); 

                }
                else
                {
                    if (control == null) control = new SingleDwgInfo();
                    //if (framesize == null) framesize = new ModifyFrameSize();

                    if (myPaletteSet.Count == 0)
                    {
                        myPaletteSet.Add(title, control);
                    }

                }
                //myPaletteSet.Dock = DockSides.None;
                myPaletteSet.Visible = true;
            }
            catch (System.Exception e)
            {
                Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage("\r\n发生错误：" + e.ToString());
            }
            
        }

        //加载成功测试
        [CommandMethod("dwgtest", CommandFlags.Session)]
        public void LoadTest()
        {
            
            //Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(".Net版图框小助手已成功加载！！\r\n");
            // Get the current document and database
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            // Start a transaction
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // 以只读方式打开块表   Open the Block table for read
                BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                             OpenMode.ForRead) as BlockTable;


                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl["test"], OpenMode.ForRead) as BlockTableRecord;
                if (acBlkTblRec == null)
                    throw new NullReferenceException("未找到有效的test块！");

                foreach (ObjectId id in acBlkTblRec)
                {
                    if (id.ObjectClass.Name == "AcDbText")
                    {
                        DBText acEnt = acTrans.GetObject(id, OpenMode.ForRead) as DBText;
                        
                        
                        if (acEnt != null)
                        {
                            Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(acEnt.TextString + "\r\n");
                            Point3d pos = acEnt.Position;

                            Extents3d w = (Extents3d)acEnt.Bounds;
                            int i = 0;
                            //Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(w.TextString + "\r\n");
                        }
                    }
                }


            }
        }

        //批量刷图
        [CommandMethod("gdwg", CommandFlags.Session)]
        public void GenerateDwgs()
        {
            try
            {
                DwgGenerator form = new DwgGenerator();
                form.Show();

            }
            catch (System.Exception e)
            {
                Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage("\r\n发生错误：" + e.ToString());
            }
        }



        #region 注释掉的无用测试方法
        /**
        [CommandMethod("A01")]
        public void A01()
        {
            Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage("Hello AutoCAD!");

            if (myPaletteSet == null)
            {
                myPaletteSet = new PaletteSet("My Palette", new System.Guid("D61D0875-A507-4b73-8B5F-9266BEACD596"));

                dwgManager = dwgManager ?? new DwgGenerator();
                if (myPaletteSet.Count > 0)
                {
                    for (int i = 0; myPaletteSet != null && i < myPaletteSet.Count - 1; i++)
                    {
                        myPaletteSet.Remove(i);
                    }
                }
                myPaletteSet.Add("DwgGenerator", dwgManager);
                myPaletteSet.Dock = DockSides.None;

                myPaletteSet.Size = dwgManager.Size;

            }


            myPaletteSet.Visible = true;
        }

        // Modal Command with localized name
        [CommandMethod("clmgr")]
        public void MyCommand() // This method can have any name
        {
            if (myPaletteSet != null)
            {
                myPaletteSet.Visible = false;
                myPaletteSet.Close();

            }


            if (dwgManager != null)
            {
                for (int i = 0; myPaletteSet != null && i < myPaletteSet.Count - 1; i++)
                {
                    myPaletteSet.Remove(i);
                }

                dwgManager.Dispose();
                dwgManager = null;
                myPaletteSet.Dispose();
                myPaletteSet = null;
            }

        }


        [CommandMethod("mark")]
        public void MardArea()
        {
            try
            {
                var dft = new DwgFrameTools(Application.DocumentManager.MdiActiveDocument, false);
                Dictionary<DwgInfoTypeEnum, String> info = new Dictionary<DwgInfoTypeEnum, string>();
                info.Add(DwgInfoTypeEnum.图号, "AAAA");
                info.Add(DwgInfoTypeEnum.总张数, 10 + "");
                info.Add(DwgInfoTypeEnum.张数, 10 + "");
                info.Add(DwgInfoTypeEnum.重量, 32.5876 + "");
                dft.UpdateDwgInfo(info);
            }
            catch (System.Exception e)
            {
                Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage("运行出错：" + e.Message);
            }
        }

        [CommandMethod("addc")]
        public static void AddCircle()
        {
            // Get the current document and database
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            // Start a transaction
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Open the Block table for read
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                                OpenMode.ForRead) as BlockTable;

                // Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                OpenMode.ForWrite) as BlockTableRecord;

                // Create a circle that is at 2,3 with a radius of 4.25
                using (Circle acCirc = new Circle())
                {
                    acCirc.Center = new Point3d(2, 3, 0);
                    acCirc.Radius = 4.25;

                    // Add the new object to the block table record and the transaction
                    acBlkTblRec.AppendEntity(acCirc);
                    acTrans.AddNewlyCreatedDBObject(acCirc, true);
                }

                // Save the new object to the database
                acTrans.Commit();
            }
        }

        [CommandMethod("test2")]
        public void testMethod2()
        {
            List<string> fileList = new List<string>() { 
            "D:/a01.dwg",
            "D:/a02.dwg",
            "D:/a03.dwg",
            "D:/a04.dwg"
            };
            Point3d pt3d = new Point3d(100, 100, 0);
            int i = 1;
            try
            {
                foreach (var file in fileList)
                {
                    if (!File.Exists(file)) continue;   //文件不存在则跳过
                    using (Database acCurDb = new Database(false, true))
                    {
                        acCurDb.ReadDwgFile(file, System.IO.FileShare.None, false, "");
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
                            using (Line acLine = new Line(new Point3d(0, 0, 0), pt3d * i++))
                            {

                                //acLine.SetDatabaseDefaults();

                                // 添加新对象到块表记录和事务中   Add the new object to the block table record and the transaction
                                acBlkTblRec.AppendEntity(acLine);
                                acTrans.AddNewlyCreatedDBObject(acLine, true);
                            }
                            // 保存新对象到数据库中   Save the new object to the database
                            acTrans.Commit();
                        }
                        acCurDb.SaveAs(file, true, DwgVersion.Current, acCurDb.SecurityParameters);
                        //acCurDb.Save();
                        //acCurDb.SaveAs(file, DwgVersion.Current);
                        //dwgDoc.CloseAndSave(dwgDoc.Name);
                    }
                }
            }
            catch (System.Exception e)
            {
                Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage("运行出错：" + e.ToString());
            }

        }

        [CommandMethod("test1", CommandFlags.Session)]
        public void testMethod1()
        {
            List<string> fileList = new List<string>() { 
            "D:/a01.dwg",
            "D:/a02.dwg",
            "D:/a03.dwg",
            "D:/a04.dwg"
            };
            Point3d pt3d = new Point3d(100, 0, 0);
            int i = -1;
            try
            {
                //foreach (var file in fileList)
                {
                    string file = fileList[0];
                    //if (!File.Exists(file)) continue;   //文件不存在则跳过
                    Document dwgDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.Open(file, false);
                    //Document dwgDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

                    if (Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument != dwgDoc)
                        Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument = dwgDoc;

                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(dwgDoc.Name);

                    using (dwgDoc.LockDocument())
                    using (Transaction tr = dwgDoc.TransactionManager.StartTransaction())
                    {
                        BlockTable bt = (BlockTable)tr.GetObject(dwgDoc.Database.BlockTableId, OpenMode.ForRead);
                        BlockTableRecord ms = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                        Line l = new Line(Point3d.Origin, new Point3d(1, 1, 1));
                        ms.AppendEntity(l);
                        tr.AddNewlyCreatedDBObject(l, true);
                        tr.Commit();
                        //dwgDoc.Database.SaveAs(drawingPath, DwgVersion.Current);  //<-- this line was throwing filesharingviolation error since file is opened
                    }
                    dwgDoc.CloseAndSave(file);
                   
                    //using (dwgDoc.LockDocument())
                    //{
                    //    using (Database acCurDb = dwgDoc.Database)
                    //    {

                    //        // 启动一个事务  Start a transaction
                    //        using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                    //        {
                    //            // 以只读方式打开块表   Open the Block table for read
                    //            BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                    //                                            OpenMode.ForRead) as BlockTable;

                    //            // 以写方式打开模型空间块表记录   Open the Block table record Model space for write
                    //            BlockTableRecord acBlkTblRec;
                    //            acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                    //                                            OpenMode.ForWrite) as BlockTableRecord;

                    //            // 创建一条直线
                    //            using (Line acLine = new Line(new Point3d(0, 0, 0), pt3d * i--))
                    //            {

                    //                //acLine.SetDatabaseDefaults();

                    //                // 添加新对象到块表记录和事务中   Add the new object to the block table record and the transaction
                    //                acBlkTblRec.AppendEntity(acLine);
                    //                acTrans.AddNewlyCreatedDBObject(acLine, true);
                    //            }
                    //            // 保存新对象到数据库中   Save the new object to the database
                    //            acTrans.Commit();
                    //        }
                    //    }
                        
                    //}
                    //dwgDoc.CloseAndSave(dwgDoc.Name);
                    //Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.CloseAll();
                }
                
            }
            catch (System.Exception e)
            {
                Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage("运行出错：" + e);
            }
        }

         **/
        #endregion

    }

}
