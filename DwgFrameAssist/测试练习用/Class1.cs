//using Autodesk.AutoCAD.ApplicationServices;
//using Autodesk.AutoCAD.DatabaseServices;
//using Autodesk.AutoCAD.EditorInput;
//using Autodesk.AutoCAD.Runtime;
//using Autodesk.AutoCAD.Geometry;
//using System.Collections.Generic;
//using System.Linq;
//using System;

//namespace DwgFrameAssist
//{
//    public partial class MyCommands
//    {
//        [CommandMethod("MER", CommandFlags.UsePickSet)]
//        public void MinimumEnclosingRectangle()
//        {
//            MinimumEnclosingBoundary(false);
//        }

//        [CommandMethod("MEC", CommandFlags.UsePickSet)]
//        public void MinimumEnclosingCircle()
//        {
//            MinimumEnclosingBoundary();
//        }

//        public void MinimumEnclosingBoundary(
//          bool circularBoundary = true
//        )
//        {
//            Document doc =
//                Application.DocumentManager.MdiActiveDocument;
//            Database db = doc.Database;
//            Editor ed = doc.Editor;

//            // Ask user to select entities

//            PromptSelectionOptions pso =
//              new PromptSelectionOptions();
//            pso.MessageForAdding = "\nSelect objects to enclose: ";
//            pso.AllowDuplicates = false;
//            pso.AllowSubSelections = true;
//            pso.RejectObjectsFromNonCurrentSpace = true;
//            pso.RejectObjectsOnLockedLayers = false;

//            //PromptSelectionResult psr = ed.GetSelection(pso);
//            var psr = ed.SelectAll();
//            if (psr.Status != PromptStatus.OK)
//                return;

//            bool oneBoundPerEnt = false;

//            //if (psr.Value.Count > 1)
//            //{
//            //    PromptKeywordOptions pko =
//            //      new PromptKeywordOptions(
//            //        "\nMultiple objects selected: create " +
//            //        "individual boundaries around each one?"
//            //      );
//            //    pko.AllowNone = true;
//            //    pko.Keywords.Add("Yes");
//            //    pko.Keywords.Add("No");
//            //    pko.Keywords.Default = "No";

//            //    PromptResult pkr = ed.GetKeywords(pko);
//            //    if (pkr.Status != PromptStatus.OK)
//            //        return;

//            //    oneBoundPerEnt = (pkr.StringResult == "Yes");
//            //}

//            // There may be a SysVar defining the buffer
//            // to add to our radius

//            double buffer = 0.0;
//            try
//            {
//                object bufvar =
//                  Application.GetSystemVariable(
//                    "ENCLOSINGBOUNDARYBUFFER"
//                  );
//                if (bufvar != null)
//                {
//                    short bufval = (short)bufvar;
//                    buffer = bufval / 100.0;
//                }
//            }
//            catch
//            {
//                object bufvar =
//                  Application.GetSystemVariable("USERI1");
//                if (bufvar != null)
//                {
//                    short bufval = (short)bufvar;
//                    buffer = bufval / 100.0;
//                }
//            }

//            // Get the current UCS

//            CoordinateSystem3d ucs =
//              ed.CurrentUserCoordinateSystem.CoordinateSystem3d;

//            // Collect points on the component entities

//            Point3dCollection pts = new Point3dCollection();

//            Transaction tr =
//              db.TransactionManager.StartTransaction();
//            using (tr)
//            {
//                BlockTableRecord btr =
//                  (BlockTableRecord)tr.GetObject(
//                    db.CurrentSpaceId,
//                    OpenMode.ForWrite
//                  );

//                for (int i = 0; i < psr.Value.Count; i++)
//                {
//                    Entity ent =
//                      (Entity)tr.GetObject(
//                        psr.Value[i].ObjectId,
//                        OpenMode.ForRead
//                      );

//                    // Collect the points for each selected entity

//                    Point3dCollection entPts = CollectPoints(tr, ent);
//                    foreach (Point3d pt in entPts)
//                    {
//                        /*
//                         * Create a DBPoint, for testing purposes
//                         *
//                        DBPoint dbp = new DBPoint(pt);
//                        btr.AppendEntity(dbp);
//                        tr.AddNewlyCreatedDBObject(dbp, true);
//                         */

//                        pts.Add(pt);
//                    }

//                    // Create a boundary for each entity (if so chosen) or
//                    // just once after collecting all the points

//                    if (oneBoundPerEnt || i == psr.Value.Count - 1)
//                    {
//                        try
//                        {
//                            Entity bnd =
//                              (circularBoundary ?
//                                CircleFromPoints(pts, ucs, buffer) :
//                                RectangleFromPoints(pts, ucs, buffer)
//                              );
//                            btr.AppendEntity(bnd);
//                            tr.AddNewlyCreatedDBObject(bnd, true);
//                        }
//                        catch
//                        {
//                            ed.WriteMessage(
//                              "\nUnable to calculate enclosing boundary."
//                            );
//                        }

//                        pts.Clear();
//                    }
//                }

//                tr.Commit();
//            }
//        }

//        private Point3dCollection CollectPoints(
//          Transaction tr, Entity ent
//        )
//        {
//            // The collection of points to populate and return

//            Point3dCollection pts = new Point3dCollection();

//            // We'll start by checking a block reference for
//            // attributes, getting their bounds and adding
//            // them to the point list. We'll still explode
//            // the BlockReference later, to gather points
//            // from other geometry, it's just that approach
//            // doesn't work for attributes (we only get the
//            // AttributeDefinitions, which don't have bounds)

//            BlockReference br = ent as BlockReference;
//            if (br != null)
//            {
//                foreach (ObjectId arId in br.AttributeCollection)
//                {
//                    DBObject obj = tr.GetObject(arId, OpenMode.ForRead);
//                    if (obj is AttributeReference)
//                    {
//                        AttributeReference ar = (AttributeReference)obj;
//                        ExtractBounds(ar, pts);
//                    }
//                }
//            }

//            // If we have a curve - other than a polyline, which
//            // we will want to explode - we'll get points along
//            // its length

//            Curve cur = ent as Curve;
//            if (cur != null &&
//                !(cur is Polyline ||
//                  cur is Polyline2d ||
//                  cur is Polyline3d))
//            {
//                // Two points are enough for a line, we'll go with
//                // a higher number for other curves

//                int segs = (ent is Line ? 2 : 20);

//                double param = cur.EndParam - cur.StartParam;
//                for (int i = 0; i < segs; i++)
//                {
//                    try
//                    {
//                        Point3d pt =
//                          cur.GetPointAtParameter(
//                            cur.StartParam + (i * param / (segs - 1))
//                          );
//                        pts.Add(pt);
//                    }
//                    catch { }
//                }
//            }
//            else if (ent is DBPoint)
//            {
//                // Points are easy

//                pts.Add(((DBPoint)ent).Position);
//            }
//            else if (ent is DBText)
//            {
//                // For DBText we use the same approach as
//                // for AttributeReferences

//                ExtractBounds((DBText)ent, pts);
//            }
//            else if (ent is MText)
//            {
//                // MText is also easy - you get all four corners
//                // returned by a function. That said, the points
//                // are of the MText's box, so may well be different
//                // from the bounds of the actual contents

//                MText txt = (MText)ent;
//                Point3dCollection pts2 = txt.GetBoundingPoints();
//                foreach (Point3d pt in pts2)
//                {
//                    pts.Add(pt);
//                }
//            }
//            else if (ent is Face)
//            {
//                Face f = (Face)ent;
//                try
//                {
//                    for (short i = 0; i < 4; i++)
//                    {
//                        pts.Add(f.GetVertexAt(i));
//                    }
//                }
//                catch { }
//            }
//            else if (ent is Solid)
//            {
//                Solid sol = (Solid)ent;
//                try
//                {
//                    for (short i = 0; i < 4; i++)
//                    {
//                        pts.Add(sol.GetPointAt(i));
//                    }
//                }
//                catch { }
//            }
//            else
//            {
//                // Here's where we attempt to explode other types
//                // of object

//                DBObjectCollection oc = new DBObjectCollection();
//                try
//                {
//                    ent.Explode(oc);
//                    if (oc.Count > 0)
//                    {
//                        foreach (DBObject obj in oc)
//                        {
//                            Entity ent2 = obj as Entity;
//                            if (ent2 != null && ent2.Visible)
//                            {
//                                foreach (Point3d pt in CollectPoints(tr, ent2))
//                                {
//                                    pts.Add(pt);
//                                }
//                            }
//                            obj.Dispose();
//                        }
//                    }
//                }
//                catch { }
//            }
//            return pts;
//        }

//        private void ExtractBounds(
//          DBText txt, Point3dCollection pts
//        )
//        {
//            // We have a special approach for DBText and
//            // AttributeReference objects, as we want to get
//            // all four corners of the bounding box, even
//            // when the text or the containing block reference
//            // is rotated

//            if (txt.Bounds.HasValue && txt.Visible)
//            {
//                // Create a straight version of the text object
//                // and copy across all the relevant properties
//                // (stopped copying AlignmentPoint, as it would
//                // sometimes cause an eNotApplicable error)

//                // We'll create the text at the WCS origin
//                // with no rotation, so it's easier to use its
//                // extents

//                DBText txt2 = new DBText();
//                txt2.Normal = Vector3d.ZAxis;
//                txt2.Position = Point3d.Origin;

//                // Other properties are copied from the original

//                txt2.TextString = txt.TextString;
//                txt2.TextStyleId = txt.TextStyleId;
//                txt2.LineWeight = txt.LineWeight;
//                txt2.Thickness = txt2.Thickness;
//                txt2.HorizontalMode = txt.HorizontalMode;
//                txt2.VerticalMode = txt.VerticalMode;
//                txt2.WidthFactor = txt.WidthFactor;
//                txt2.Height = txt.Height;
//                txt2.IsMirroredInX = txt2.IsMirroredInX;
//                txt2.IsMirroredInY = txt2.IsMirroredInY;
//                txt2.Oblique = txt.Oblique;

//                // Get its bounds if it has them defined
//                // (which it should, as the original did)

//                if (txt2.Bounds.HasValue)
//                {
//                    Point3d maxPt = txt2.Bounds.Value.MaxPoint;

//                    // Place all four corners of the bounding box
//                    // in an array

//                    Point2d[] bounds =
//                      new Point2d[] {
//              Point2d.Origin,
//              new Point2d(0.0, maxPt.Y),
//              new Point2d(maxPt.X, maxPt.Y),
//              new Point2d(maxPt.X, 0.0)
//            };

//                    // We're going to get each point's WCS coordinates
//                    // using the plane the text is on

//                    Plane pl = new Plane(txt.Position, txt.Normal);

//                    // Rotate each point and add its WCS location to the
//                    // collection

//                    foreach (Point2d pt in bounds)
//                    {
//                        pts.Add(
//                          pl.EvaluatePoint(
//                            pt.RotateBy(txt.Rotation, Point2d.Origin)
//                          )
//                        );
//                    }
//                }
//            }
//        }

//        private Entity RectangleFromPoints(
//          Point3dCollection pts, CoordinateSystem3d ucs, double buffer
//        )
//        {
//            // Get the plane of the UCS

//            Plane pl = new Plane(ucs.Origin, ucs.Zaxis);

//            // We will project these (possibly 3D) points onto
//            // the plane of the current UCS, as that's where
//            // we will create our circle

//            // Project the points onto it

//            List<Point2d> pts2d = new List<Point2d>(pts.Count);
//            for (int i = 0; i < pts.Count; i++)
//            {
//                pts2d.Add(pl.ParameterOf(pts[i]));
//            }

//            // Assuming we have some points in our list...

//            if (pts.Count > 0)
//            {
//                // Set the initial min and max values from the first entry

//                double minX = pts2d[0].X,
//                       maxX = minX,
//                       minY = pts2d[0].Y,
//                       maxY = minY;

//                // Perform a single iteration to extract the min/max X and Y

//                for (int i = 1; i < pts2d.Count; i++)
//                {
//                    Point2d pt = pts2d[i];
//                    if (pt.X < minX) minX = pt.X;
//                    if (pt.X > maxX) maxX = pt.X;
//                    if (pt.Y < minY) minY = pt.Y;
//                    if (pt.Y > maxY) maxY = pt.Y;
//                }

//                // Our final buffer amount will be the percentage of the
//                // smallest of the dimensions

//                double buf =
//                  Math.Min(maxX - minX, maxY - minY) * buffer;

//                // Apply the buffer to our point ordinates

//                minX -= buf;
//                minY -= buf;
//                maxX += buf;
//                maxY += buf;

//                // Create the boundary points

//                Point2d pt0 = new Point2d(minX, minY),
//                        pt1 = new Point2d(minX, maxY),
//                        pt2 = new Point2d(maxX, maxY),
//                        pt3 = new Point2d(maxX, minY);

//                // Finally we create the polyline

//                var p = new Polyline(4);
//                p.Normal = pl.Normal;
//                p.AddVertexAt(0, pt0, 0, 0, 0);
//                p.AddVertexAt(1, pt1, 0, 0, 0);
//                p.AddVertexAt(2, pt2, 0, 0, 0);
//                p.AddVertexAt(3, pt3, 0, 0, 0);
//                p.Closed = true;

//                return p;
//            }
//            return null;
//        }

//        private Entity CircleFromPoints(
//          Point3dCollection pts, CoordinateSystem3d ucs, double buffer
//        )
//        {
//            // Get the plane of the UCS

//            Plane pl = new Plane(ucs.Origin, ucs.Zaxis);

//            // We will project these (possibly 3D) points onto
//            // the plane of the current UCS, as that's where
//            // we will create our circle

//            // Project the points onto it

//            List<Point2d> pts2d = new List<Point2d>(pts.Count);
//            for (int i = 0; i < pts.Count; i++)
//            {
//                pts2d.Add(pl.ParameterOf(pts[i]));
//            }

//            // Assuming we have some points in our list...

//            if (pts.Count > 0)
//            {
//                // We need the center and radius of our circle

//                Point2d center;
//                double radius = 0;

//                // Use our fast approximation algorithm to
//                // calculate the center and radius of our
//                // circle to within 1% (calling the function
//                // with 100 iterations gives 10%, calling it
//                // with 10K gives 1%)

//                BadoiuClarksonIteration(
//                  pts2d, 10000, out center, out radius
//                );

//                // Get our center point in WCS (on the plane
//                // of our UCS)

//                Point3d cen3d = pl.EvaluatePoint(center);

//                // Create the circle and add it to the drawing

//                return new Circle(
//                  cen3d, ucs.Zaxis, radius * (1.0 + buffer)
//                );
//            }
//            return null;
//        }

//        // Algorithm courtesy (and copyright of) Frank Nielsen
//        // http://blog.informationgeometry.org/article.php?id=164

//        public void BadoiuClarksonIteration(
//          List<Point2d> set, int iter,
//          out Point2d cen, out double rad
//        )
//        {
//            // Choose any point of the set as the initial
//            // circumcenter

//            cen = set[0];
//            rad = 0;

//            for (int i = 0; i < iter; i++)
//            {
//                int winner = 0;
//                double distmax = (cen - set[0]).Length;

//                // Maximum distance point

//                for (int j = 1; j < set.Count; j++)
//                {
//                    double dist = (cen - set[j]).Length;
//                    if (dist > distmax)
//                    {
//                        winner = j;
//                        distmax = dist;
//                    }
//                }
//                rad = distmax;

//                // Update

//                cen =
//                  new Point2d(
//                    cen.X + (1.0 / (i + 1.0)) * (set[winner].X - cen.X),
//                    cen.Y + (1.0 / (i + 1.0)) * (set[winner].Y - cen.Y)
//                  );
//            }
//        }
//    }
//}