//using Autodesk.AutoCAD.Runtime;
//using Acad = Autodesk.AutoCAD.ApplicationServices;
//using Autodesk.AutoCAD.DatabaseServices;
//using Autodesk.AutoCAD.Geometry;
//using Autodesk.AutoCAD.EditorInput;
//using Autodesk.AutoCAD.Windows;
//using System.Collections.Generic;
//using System.Linq;
//using System;
//using Autodesk.AutoCAD.Customization;

//namespace DwgFrameAssist
//{
//    public partial class MyCommands
//    {
//        [CommandMethod("sync", CommandFlags.Session)]
//        public void Synchronize()
//        {
//            try
//            {
//                DwgFrameTools dft = new DwgFrameTools(Acad.Application.DocumentManager.MdiActiveDocument, true);
//                DwgInfo info = dft.DwgInformation;
//                dft.ReplaceFrameBlock(dft.GenerateStandardDwgFileName(false, true, "A2"));
//                DwgFrameTools dft2 = new DwgFrameTools(Acad.Application.DocumentManager.MdiActiveDocument, true);
//                info.Name += "new";
//                info.DwgNO = "new Dwg no";

//                dft2.UpdateDwgInfo(info);
//            }
//            catch (System.Exception e)
//            {
//                Acad.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(e.ToString());
//            }
            
//        }

//        [CommandMethod("up1", CommandFlags.Session)]
//        public void UpdateBlock1()
//        {
//            Acad.Document dwgDoc = Acad.Application.DocumentManager.MdiActiveDocument;
//            Database acCurDb = dwgDoc.Database;
//            using (dwgDoc.LockDocument())
//            {
//                // 启动一个事务  Start a transaction
//                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
//                {
//                    // 以只读方式打开块表   Open the Block table for read
//                    BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
//                                                 OpenMode.ForRead) as BlockTable;

//                    // 以写方式打开模型空间块表记录   Open the Block table record Model space for write
//                    BlockTableRecord acBlkTblRec;
//                    acBlkTblRec = acTrans.GetObject(acBlkTbl["test"],
//                                                    OpenMode.ForWrite) as BlockTableRecord;

//                    foreach (ObjectId id in acBlkTblRec)
//                    {
//                        DBObject obj = acTrans.GetObject(id, OpenMode.ForWrite) as DBObject;
//                        if (obj is DBText)
//                        {
//                            DBText txt = (DBText)obj ;
//                            if (txt.TextString.Contains("delete"))
//                            {
//                                txt.Erase(true);
//                            }
//                            else if (txt.TextString.Contains("old"))
//                            {
//                                txt.UpgradeOpen();
//                                txt.TextString.Replace("old", "upgrated");
//                            }
//                        }

                        
//                    }

//                    // 创建一条直线
//                    Line acLine = new Line(Point3d.Origin, new Point3d(100, 100, 0));

//                    acLine.SetDatabaseDefaults();

//                    // 添加新对象到块表记录和事务中   Add the new object to the block table record and the transaction
//                    acBlkTblRec.AppendEntity(acLine);
//                    acTrans.AddNewlyCreatedDBObject(acLine, true);

//                    foreach (ObjectId id in acBlkTblRec.GetBlockReferenceIds(false, true))
//                    {
//                        BlockReference acBlkRef = acTrans.GetObject(id, OpenMode.ForWrite) as BlockReference;
//                        acBlkRef.RecordGraphicsModified(true);
//                    }

//                    // 保存新对象到数据库中   Save the new object to the database
//                    acTrans.Commit();
//                }
//            }
//            dwgDoc.Editor.Regen();
//        }


//        [CommandMethod("up2", CommandFlags.Session)]
//        public void UpdateBlock2()
//        {
//            string blkFile = @"D:\test2.dwg";
//            Acad.Document dwgDoc = Acad.Application.DocumentManager.MdiActiveDocument;
//            Database acCurDb = dwgDoc.Database;
//            using (dwgDoc.LockDocument())
//            {
//                // 启动一个事务  Start a transaction
//                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
//                {
                    
//                    using(Database db = new Database(false, true))
//                    {
//                        db.ReadDwgFile(blkFile, System.IO.FileShare.ReadWrite, true, "");
                        
//                        //acCurDb.Insert("test1", db, true);//整张图纸作为指定块名的块插入。如有同名块，不会覆盖。


//                        //复制
//                        ObjectIdCollection ids = new ObjectIdCollection();
//                        using (Transaction tr = db.TransactionManager.StartTransaction())
//                        {
//                            BlockTable bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
//                            if (bt.Has("test"))
//                                ids.Add(bt["test"]);

//                            tr.Commit();
//                        }
//                        if (ids.Count > 0)
//                        {
//                            acCurDb.WblockCloneObjects(ids, acCurDb.BlockTableId, new IdMapping(), DuplicateRecordCloning.Replace, false);
//                        }
                        
//                    }

//                    //foreach (ObjectId id in acBlkTblRec.GetBlockReferenceIds(false, true))
//                    //{
//                    //    BlockReference acBlkRef = acTrans.GetObject(id, OpenMode.ForWrite) as BlockReference;
//                    //    acBlkRef.RecordGraphicsModified(true);
//                    //}

//                    // 保存新对象到数据库中   Save the new object to the database
//                    acTrans.Commit();
//                }
//            }
//            dwgDoc.Editor.Regen();
//        }

//        [CommandMethod("up3", CommandFlags.Session)]
//        public void UpdateBlock3()
//        {
//            string blkFile = @"D:\test2.dwg";
//            Acad.Document dwgDoc = Acad.Application.DocumentManager.MdiActiveDocument;
//            Database acCurDb = dwgDoc.Database;
//            using (dwgDoc.LockDocument())
//            {
//                SymbolUtilityServices.GetBlockModelSpaceId(acCurDb);
//                // 启动一个事务  Start a transaction
//                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
//                {

//                    using (Database db = new Database(false, true))
//                    {
//                        db.ReadDwgFile(blkFile, System.IO.FileShare.ReadWrite, true, "");

//                        //acCurDb.Insert("test1", db, true);//整张图纸作为指定块名的块插入。如有同名块，不会覆盖。


//                        //复制
//                        ObjectIdCollection ids = new ObjectIdCollection();
//                        using (Transaction tr = db.TransactionManager.StartTransaction())
//                        {
//                            BlockTable bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
//                            if (bt.Has("hh"))
//                                ids.Add(bt["hh"]);

//                            tr.Commit();
//                        }
//                        if (ids.Count > 0)
//                        {
//                            acCurDb.WblockCloneObjects(ids, acCurDb.BlockTableId, new IdMapping(), DuplicateRecordCloning.Ignore, false);
//                        }

//                    }

//                    //foreach (ObjectId id in acBlkTblRec.GetBlockReferenceIds(false, true))
//                    //{
//                    //    BlockReference acBlkRef = acTrans.GetObject(id, OpenMode.ForWrite) as BlockReference;
//                    //    acBlkRef.RecordGraphicsModified(true);
//                    //}

//                    // 保存新对象到数据库中   Save the new object to the database
//                    acTrans.Commit();
//                }
//            }
//            dwgDoc.Editor.Regen();
//        }
    
    
//        [CommandMethod("ntb")]
//        public void ToolBarTest()
//        {
//            try
//            {
//                Editor ed = Acad.Application.DocumentManager.MdiActiveDocument.Editor;

//                string mainCuiFile = (string)Acad.Application.GetSystemVariable("MENUNAME");
//                mainCuiFile += ".cuix";
//                CustomizationSection cs
//                   = new CustomizationSection(mainCuiFile);


//                if (cs.MenuGroup.PopMenus.IsNameFree("Custom Menu"))
//                {
//                    System.Collections.Specialized.StringCollection pmAliases = new System.Collections.Specialized.StringCollection();
//                    PopMenu pm = new PopMenu("Custom Menu", pmAliases, "Custom Menu Tag", cs.MenuGroup);

//                    PopMenuItem pmi = new PopMenuItem(pm, -1);
//                    pmi.MacroID = "up2";
//                    pmi.Name = "Autodesk User Group International";
//                    pmi = new PopMenuItem(pm, -1);
//                    pmi = new PopMenuItem(pm, -1);
//                    pmi.MacroID = "ID_CustomSafe";
//                    pmi.Name = "Online Developer Center";


//                    foreach (Workspace wk in cs.Workspaces)
//                    {
//                        WorkspacePopMenu wkpm = new WorkspacePopMenu(wk, pm);
//                        wkpm.Display = 1;
//                    }
//                }
//                else
//                    ed.WriteMessage("Custom Menu already Exists\n");

//                if(cs.MenuGroup.Toolbars.IsNameFree("xxx"))
//                {
//                    Toolbar newTb = new Toolbar("New Toolbar", cs.MenuGroup);
//                    ToolbarControl newControl = new ToolbarControl(ControlType.NamedViewControl, newTb, -1);
//                    ToolbarFlyout newFlyout = new ToolbarFlyout(newTb, -1);
//                    newFlyout.ToolbarReference = "DIMENSION";

//                    ToolbarButton newButton = new ToolbarButton(newTb, -1);
//                    //newButton.MacroID = "up1";

//                }
//                //string curWorkspace = (string)Acad.Application.GetSystemVariable("WSCURRENT");
//                //Workspace ws = cs.getWorkspace(curWorkspace);

//                //Toolbar tbDraw = cs.MenuGroup.Toolbars.FindToolbarWithName("Drawxxx");
//                //WorkspaceToolbar wsTbDraw = ws.WorkspaceToolbars.FindWorkspaceToolbar(tbDraw.ElementID, cs.MenuGroup.Name);
//                //if (wsTbDraw == null)
//                //{
//                //    wsTbDraw = new WorkspaceToolbar(ws, tbDraw);
//                //    ws.WorkspaceToolbars.Add(wsTbDraw);
//                //}
//                //wsTbDraw.Display = 1;
//                //wsTbDraw.ToolbarOrient = ToolbarOrient.left;
//                //wsTbDraw.DockRow = 0;
//                //wsTbDraw.DockColumn = 1;


//                ////Toolbar newTb = new Toolbar("New Toolbar", cs.MenuGroup);
//                ////newTb.ElementID = "EID_NewToolbar";
//                ////newTb.ToolbarOrient = ToolbarOrient.floating;
//                ////newTb.ToolbarVisible = ToolbarVisible.show;

//                //ToolbarButton newButton = new ToolbarButton(tbDraw, -1);
//                //newButton.MacroID = "ID_Pline";
//                //ToolbarButton newButton2 = new ToolbarButton(tbDraw, -1);
//                //newButton2.MacroID = "up2";
//                //ToolbarControl newControl = new ToolbarControl(ControlType.NamedViewControl, tbDraw, -1);
//                //ToolbarFlyout newFlyout = new ToolbarFlyout(tbDraw, -1);
//                //newFlyout.ToolbarReference = "DIMENSION";
//            }
//            catch(System.Exception e)
//            {
//                throw e;
//            }
//        }
//    }
//}
