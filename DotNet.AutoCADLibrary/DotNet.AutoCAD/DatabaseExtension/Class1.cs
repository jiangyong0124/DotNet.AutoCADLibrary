using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace DotNet.AutoCAD.DatabaseExtension
{
    public static class BlockReferenceHelper
    {
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static BlockReference AddBlockDwgEasy(this Point3d ptInsert, Database db, Transaction tr,
            BlockTable bt, BlockTableRecord btr, string sourceFileName, string newBlockName, double dAngle)
        {
            if (!File.Exists(sourceFileName)) return null;
            BlockReference bref = null;
            try
            {
                ObjectId objId = new ObjectId();
                if (bt.Has(newBlockName) && !bt[newBlockName].IsErased)
                {
                    objId = bt[newBlockName];
                }
                else
                {
                    Database databaseFromFile = Getdb(sourceFileName, System.IO.FileShare.Read, true);
                    objId = db.Insert(newBlockName, databaseFromFile, true);
                    databaseFromFile.Dispose();
                }
                bref = new BlockReference(ptInsert, objId);
                bref.Rotation = dAngle;
                ObjectId blockobj = btr.AppendEntity(bref);
                BlockTableRecord empBtr = tr.GetObject(bref.BlockTableRecord, OpenMode.ForWrite) as BlockTableRecord;
                tr.AddNewlyCreatedDBObject(bref, true);
            }
            catch (Exception ex)
            {
                if (!File.Exists(sourceFileName))
                {
                    throw new Exception("缺少文件：" + sourceFileName);
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }

            return bref;
        }

        public static BlockReference AddBlockByRefEnt(this Point3d ptInsert, ArrayList arrParam, out Point3d ptMax, out Point3d ptMin)
        {

            Database db = arrParam[0] as Database;
            Transaction tr = arrParam[1] as Transaction;
            BlockTable bt = arrParam[2] as BlockTable;
            BlockTableRecord btr = arrParam[3] as BlockTableRecord;
            string sourceFileName = arrParam[4].ToString();
            string newBlockName = arrParam[5].ToString();
            string strName = arrParam[6].ToString();
            double dAngle = Convert.ToDouble(arrParam[7]);

            ptMax = Point3d.Origin; ptMin = Point3d.Origin;
            if (!File.Exists(sourceFileName)) return null;

            Database dbNew = null;
            BlockReference bref = null;
            try
            {
                ObjectId objId = new ObjectId();
                if (bt.Has(newBlockName) && !bt[newBlockName].IsErased)
                {
                    objId = bt[newBlockName];
                }
                else
                {
                    Database databaseFromFile = Getdb(sourceFileName, System.IO.FileShare.Read, true);
                    using (Transaction trRef = databaseFromFile.TransactionManager.StartTransaction())
                    {
                        BlockTable refBt = trRef.GetObject(databaseFromFile.BlockTableId, OpenMode.ForRead) as BlockTable;
                        BlockTableRecord refBtr = trRef.GetObject(refBt[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
                        var query = from ObjectId objIdRef in refBtr
                                    let dbObj = trRef.GetObject(objIdRef, OpenMode.ForRead)
                                    where dbObj is BlockReference
                                    select dbObj;

                        List<BlockReference> lstEnt = query.ToList().ConvertAll(a => a as BlockReference);

                        var queryLst = from BlockReference refEnt in lstEnt
                                       where refEnt.Name.Equals(strName)
                                       select refEnt;
                        BlockReference refEntOld = queryLst.First();
                        ptMax = refEntOld.GeometricExtents.MaxPoint;
                        ptMin = refEntOld.GeometricExtents.MinPoint;
                        ObjectIdCollection objIds = new ObjectIdCollection();
                        objIds.Add(refEntOld.ObjectId);
                        dbNew = databaseFromFile.Wblock(objIds, refEntOld.Position);
                        objId = db.Insert(newBlockName, dbNew, true);
                        trRef.Commit();
                    }
                }
                bref = new BlockReference(ptInsert, objId)
                {
                    Rotation = dAngle
                };
                ObjectId blockobj = btr.AppendEntity(bref);
                tr.AddNewlyCreatedDBObject(bref, true);
            }
            catch (Exception ex)
            {
                if (!File.Exists(sourceFileName))
                {
                    throw new Exception("缺少文件：" + sourceFileName);
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }

            return bref;
        }

        public static BlockReference AddBlockByRefEntNoOut(this Point3d ptInsert, ArrayList arrParam)
        {

            Database db = arrParam[0] as Database;
            Transaction tr = arrParam[1] as Transaction;
            BlockTable bt = arrParam[2] as BlockTable;
            BlockTableRecord btr = arrParam[3] as BlockTableRecord;
            string sourceFileName = arrParam[4].ToString();
            string newBlockName = arrParam[5].ToString();
            string strName = arrParam[6].ToString();
            double dAngle = Convert.ToDouble(arrParam[7]);

            if (!File.Exists(sourceFileName)) return null;

            Database dbNew = null;
            BlockReference bref = null;
            try
            {
                ObjectId objId = new ObjectId();
                if (bt.Has(newBlockName) && !bt[newBlockName].IsErased)
                {
                    objId = bt[newBlockName];
                }
                else
                {
                    Database databaseFromFile = Getdb(sourceFileName, System.IO.FileShare.Read, true);
                    using (Transaction trRef = databaseFromFile.TransactionManager.StartTransaction())
                    {
                        BlockTable refBt = trRef.GetObject(databaseFromFile.BlockTableId, OpenMode.ForRead) as BlockTable;
                        BlockTableRecord refBtr = trRef.GetObject(refBt[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
                        var query = from ObjectId objIdRef in refBtr
                                    let dbObj = trRef.GetObject(objIdRef, OpenMode.ForRead)
                                    where dbObj is BlockReference
                                    select dbObj;

                        List<BlockReference> lstEnt = query.ToList().ConvertAll(a => a as BlockReference);

                        var queryLst = from BlockReference refEnt in lstEnt
                                       where refEnt.Name.Equals(strName)
                                       select refEnt;
                        BlockReference refEntOld = queryLst.First();
                        ObjectIdCollection objIds = new ObjectIdCollection();
                        objIds.Add(refEntOld.ObjectId);
                        dbNew = databaseFromFile.Wblock(objIds, refEntOld.Position);
                        objId = db.Insert(newBlockName, dbNew, true);
                        trRef.Commit();
                    }
                }
                bref = new BlockReference(ptInsert, objId)
                {
                    Rotation = dAngle
                };
                ObjectId blockobj = btr.AppendEntity(bref);
                tr.AddNewlyCreatedDBObject(bref, true);
            }
            catch (Exception ex)
            {
                if (!File.Exists(sourceFileName))
                {
                    throw new Exception("缺少文件：" + sourceFileName);
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }

            return bref;
        }
    
        public static BlockReference AddBlockByRefEnt(this Point3d ptInsert, ArrayList arrParam)
        {

            Database db = arrParam[0] as Database;
            Transaction tr = arrParam[1] as Transaction;
            BlockTable bt = arrParam[2] as BlockTable;
            BlockTableRecord btr = arrParam[3] as BlockTableRecord;
            string sourceFileName = arrParam[4].ToString();
            string newBlockName = arrParam[5].ToString();
            string strName = arrParam[6].ToString();
            double dAngle = Convert.ToDouble(arrParam[7]);

            if (!File.Exists(sourceFileName)) return null;

            Database dbNew = null;
            BlockReference bref = null;
            try
            {
                ObjectId objId = new ObjectId();
                if (bt.Has(newBlockName) && !bt[newBlockName].IsErased)
                {
                    objId = bt[newBlockName];
                }
                else
                {
                    Database databaseFromFile = Getdb(sourceFileName, System.IO.FileShare.Read, true);
                    using (Transaction trRef = databaseFromFile.TransactionManager.StartTransaction())
                    {
                        BlockTable refBt = trRef.GetObject(databaseFromFile.BlockTableId, OpenMode.ForRead) as BlockTable;
                        BlockTableRecord refBtr = trRef.GetObject(refBt[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
                        var query = from ObjectId objIdRef in refBtr
                                    let dbObj = trRef.GetObject(objIdRef, OpenMode.ForRead)
                                    where dbObj is BlockReference
                                    select dbObj;

                        List<BlockReference> lstEnt = query.ToList().ConvertAll(a => a as BlockReference);

                        var queryLst = from BlockReference refEnt in lstEnt
                                       where refEnt.Name.Equals(strName)
                                       select refEnt;
                        BlockReference refEntOld = queryLst.First();
                        ObjectIdCollection objIds = new ObjectIdCollection();
                        objIds.Add(refEntOld.ObjectId);
                        dbNew = databaseFromFile.Wblock(objIds, refEntOld.Position);
                        objId = db.Insert(newBlockName, dbNew, true);
                        trRef.Commit();
                    }
                }
                bref = new BlockReference(ptInsert, objId)
                {
                    Rotation = dAngle
                };
                ObjectId blockobj = btr.AppendEntity(bref);
                tr.AddNewlyCreatedDBObject(bref, true);
            }
            catch (Exception ex)
            {
                if (!File.Exists(sourceFileName))
                {
                    throw new Exception("缺少文件：" + sourceFileName);
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }

            return bref;
        }

        public static List<Entity> ExplodeBlock(this BlockReference refEnt)
        {
            DBObjectCollection dbObjs = new DBObjectCollection();
            refEnt.Explode(dbObjs);

            var query = from DBObject dbObj in dbObjs
                        let ent = dbObj as Entity
                        select ent;
            List<Entity> lstEnt = query.ToList();

            refEnt.Erase();

            return lstEnt;
        }

        public static List<Entity> ExplodeBlock(this BlockReference refEnt, List<Entity> lstEnt)
        {
            if (lstEnt == null) lstEnt = new List<Entity>();
            DBObjectCollection dbObjs = new DBObjectCollection();
            refEnt.Explode(dbObjs);
            if (refEnt.Database != null) refEnt.Erase();

            var query = from DBObject dbObj in dbObjs
                        let ent = dbObj as Entity
                        where ent is BlockReference
                        select ent;

            var queryEnt = from DBObject dbObj in dbObjs
                           let ent = dbObj as Entity
                           where !(ent is BlockReference)
                           select ent;
            List<Entity> lst = queryEnt.ToList();
            if (lst != null && lst.Count > 0) lstEnt.AddRange(lst);
            if (query.Count() > 0)
            {
                foreach (BlockReference block in query)
                {
                    ExplodeBlock(block, lstEnt);
                }
            }

            return lstEnt;
        }
        
        private static Database Getdb(string sourceFileName, System.IO.FileShare rwType, bool isCloseFile)
        {
            try
            {
                Database databaseFromFile = new Database(false, true);
                databaseFromFile.ReadDwgFile(sourceFileName, rwType, false, "");
                databaseFromFile.CloseInput(isCloseFile);
                return databaseFromFile;
            }
            catch
            {
                return null;

            }
        }  

        public static List<Entity> BreakAndDelete(this Entity breakEnt, Entity edgeEnt, string strDir)
        {
            List<Entity> lstTotal = new List<Entity>();

            List<Entity> lstBreak = new List<Entity>();
            List<Entity> lstEdag = new List<Entity>();

            DBObjectCollection dbObjs = new DBObjectCollection();
            breakEnt.Explode(dbObjs);
            foreach (DBObject dBObject in dbObjs)
            {
                Entity ent = dBObject as Entity;
                lstBreak.Add(ent);
            }

            Dictionary<Curve, List<double>> objpts;
            objpts = new Dictionary<Curve, List<double>>(lstBreak.Count);

            foreach (Entity entBreak in lstBreak)
            {
                Curve cvBreak = entBreak as Curve;
                Point3dCollection points = new Point3dCollection();
                cvBreak.IntersectWith(edgeEnt, Intersect.OnBothOperands, points, IntPtr.Zero, IntPtr.Zero);
                if (points.Count == 0)
                {
                    lstTotal.Add(cvBreak);
                    continue;
                }

                List<double> pas = new List<double>();
                if (!objpts.ContainsKey(cvBreak)) objpts.Add(cvBreak, pas);

                foreach (Point3d pt in points)
                {
                    double para = 0;
                    try
                    {
                        para = cvBreak.GetParameterAtPoint(pt);
                        pas.Add(para);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            foreach (KeyValuePair<Curve, List<double>> var in objpts)
            {
                Curve cv = var.Key;
                if (var.Value.Count == 0) continue;
                if (var.Value.Count == 1 && cv.IsPeriodic && cv.IsPersistent)
                    continue;
                var.Value.Sort();
                double[] arrpt = var.Value.ToArray();
                DoubleCollection pts = new DoubleCollection(arrpt);
                DBObjectCollection objs = cv.GetSplitCurves(pts);
                var query = from DBObject dbObj in objs
                            let curve = dbObj as Curve
                            select curve;
                int m = 0;
                foreach (Curve brks in query)
                {
                    #region 判断逻辑
                    if (query.Count() > 1)
                    {
                        if (query.Count() == 2)
                        {
                            switch (strDir)
                            {
                                case "Right":
                                    if (m == 0) { m++; continue; }
                                    break;
                                case "Left":
                                    if (m == 1) continue;
                                    break;
                                case "Up":
                                    if (m == 0) { m++; continue; }
                                    break;
                                case "Down":
                                    if (m == 1) continue;
                                    break;
                            }

                        }
                        if (query.Count() > 2)
                        {
                            if (m != 0 && m != query.Count() - 1)//只留第1和最后
                            {
                                m++;
                                continue;
                            }
                        }
                    }
                    #endregion

                    if (cv.GetDistanceAtParameter(brks.EndParam) > 1e-6)
                    {
                        m++;
                        lstTotal.Add(brks);
                    }
                }
            }
            return lstTotal;
        }
         
        public static List<Entity> DrawingText(this string[] strs, Point3d ptInsert, double disY)
        {
            List<Entity> lstEnt = new List<Entity>();
            double txtHeight = 40;
            foreach (string str in strs)
            {
                ptInsert = new Point3d(ptInsert.X, ptInsert.Y - txtHeight - disY, 0);
                DBText txt = new DBText();
                txt.Position = ptInsert;
                txt.HorizontalMode = TextHorizontalMode.TextLeft;
                txt.Height = 40;
                txt.WidthFactor = 0.7;
                txt.ColorIndex = 3;
                txt.TextString = str;

                lstEnt.Add(txt);

            }

            return lstEnt;
        }
    }
}
