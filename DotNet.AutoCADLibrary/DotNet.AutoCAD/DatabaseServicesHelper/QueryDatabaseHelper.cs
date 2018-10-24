using System.Collections.Generic;
using Autodesk.AutoCAD.ApplicationServices.Core;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

namespace DotNet.AutoCAD.DatabaseServicesHelper
{
    /// <summary>
    /// 查询AutoCAD数据库--
    /// 实现对数据库的批量操作（内部均使用OpenClose事务处理）--
    /// 2018.10.20
    /// </summary>
    public static class QueryDatabaseHelper
    {

        /// <summary>
        /// 获取用户选择的类型为T的所有实体
        /// </summary>
        /// <typeparam name="T">实体的类型</typeparam>
        /// <param name="db">数据库对象</param>
        /// <param name="mode">实体的打开方式</param>
        /// <param name="openErased">是否打开已删除的实体</param>
        /// <returns>返回类型为T的实体</returns>
        public static List<T> GetSelection<T>(this Database db, OpenMode mode = OpenMode.ForRead, bool openErased = false) where T : Entity
        {
            var result = new List<T>();

            using (var trans = db.TransactionManager.StartOpenCloseTransaction())
            {
                string dxfname = RXClass.GetClass(typeof(T)).DxfName;

                var ed = Application.DocumentManager.MdiActiveDocument.Editor;

                TypedValue[] values = { new TypedValue((int)DxfCode.Start, dxfname) };

                var filter = new SelectionFilter(values);

                var entSelected = ed.GetSelection(filter);

                if (entSelected.Status == PromptStatus.OK)
                {
                    foreach (var id in entSelected.Value.GetObjectIds())
                    {

                        T t = (T)(object)trans.GetObject(id, mode, openErased);

                        result.Add(t);
                    }
                }

                trans.Commit();
            }

            return result;
        }

        /// <summary>
        /// 获取模型空间中类型为T的所有实体
        /// </summary>
        /// <typeparam name="T">实体的类型</typeparam>
        /// <param name="db">数据库对象</param>
        /// <param name="mode">实体打开方式</param>
        /// <param name="openErased">是否打开已删除的实体</param>
        /// <returns>返回模型空间中类型为T的实体</returns>
        public static List<T> GetEntsInModelSpace<T>(this Database db, OpenMode mode = OpenMode.ForRead, bool openErased = false) where T : Entity
        {
            var ed = Application.DocumentManager.MdiActiveDocument.Editor;

            //声明一个List类的变量，用于返回类型为T为的实体列表
            var result = new List<T>();

            //获取类型T代表的DXF代码名用于构建选择集过滤器
            string dxfname = RXClass.GetClass(typeof(T)).DxfName;

            //构建选择集过滤器        
            TypedValue[] values =
            {
                new TypedValue((int)DxfCode.Start, dxfname),

                new TypedValue((int)DxfCode.LayoutName,"Model")
            };

            var filter = new SelectionFilter(values);

            //选择符合条件的所有实体
            var entSelected = ed.SelectAll(filter);

            using (var trans = db.TransactionManager.StartOpenCloseTransaction())
            {
                if (entSelected.Status == PromptStatus.OK)
                {
                    //循环遍历符合条件的实体
                    foreach (var id in entSelected.Value.GetObjectIds())
                    {
                        //将实体强制转化为T类型的对象
                        //不能将实体直接转化成泛型T，必须首先转换成object类
                        T t = (T)(object)trans.GetObject(id, mode, openErased);

                        result.Add(t);//将实体添加到返回列表中
                    }
                }

                trans.Commit();
            }
            return result;//返回类型为T为的实体列表
        }

        /// <summary>
        /// 获取图纸空间中类型为T的所有实体
        /// </summary>
        /// <typeparam name="T">实体的类型</typeparam>
        /// <param name="db">数据库对象</param>
        /// <param name="mode">实体打开方式</param>
        /// <param name="openErased">是否打开已删除的实体</param>
        /// <returns>返回图纸空间中类型为T的实体</returns>
        public static List<T> GetEntsInPaperSpace<T>(this Database db, OpenMode mode= OpenMode.ForRead, bool openErased=false) where T : Entity
        {
            var result = new List<T>();

            using (var trans = db.TransactionManager.StartOpenCloseTransaction())
            {
                string dxfname = RXClass.GetClass(typeof(T)).DxfName;

                var ed = Application.DocumentManager.MdiActiveDocument.Editor;

                TypedValue[] values =
                {
                   new TypedValue((int)DxfCode.Start, dxfname),

                   new TypedValue((int)DxfCode.ViewportVisibility,1)
                };

                var filter = new SelectionFilter(values);

                var entSelected = ed.SelectAll(filter);

                if (entSelected.Status == PromptStatus.OK)
                {
                    foreach (var id in entSelected.Value.GetObjectIds())
                    {
                        T t = (T)(object)trans.GetObject(id, mode, openErased);

                        result.Add(t);
                    }
                }

                trans.Commit();
            }
            return result;
        }

        /// <summary>
        /// 获取数据库中所有的实体
        /// </summary>
        /// <param name="db">数据库对象</param>
        /// <param name="mode">实体打开方式</param>
        /// <param name="openErased">是否打开已删除的实体</param>
        /// <returns>返回数据库中所有的实体</returns>
        public static List<Entity> GetEntsInDatabase(this Database db, OpenMode mode = OpenMode.ForRead, bool openErased = false)
        {
            //声明一个List类的变量，用于返回所有实体
            var result = new List<Entity>();

            using (var trans = db.TransactionManager.StartOpenCloseTransaction())
            {
                var ed = Application.DocumentManager.MdiActiveDocument.Editor;

                var entSelected = ed.SelectAll();

                if (entSelected.Status == PromptStatus.OK)
                {
                    //循环遍历符合条件的实体
                    foreach (var id in entSelected.Value.GetObjectIds())
                    {
                        var ent = (Entity)(object)trans.GetObject(id, mode, openErased);

                        result.Add(ent);
                    }
                }

                trans.Commit();
            }

            return result;
        }

        /// <summary>
        /// 获取用户选择的实体
        /// </summary>
        /// <param name="db">数据库对象</param>
        /// <param name="mode">实体打开方式</param>
        /// <param name="openErased">是否打开已删除的实体</param>
        /// <returns>返回用户选择的实体</returns>
        public static List<Entity> GetSelection(this Database db, OpenMode mode = OpenMode.ForRead, bool openErased = false)
        {
            var result = new List<Entity>();

            using (var trans = db.TransactionManager.StartOpenCloseTransaction())
            {
                var ed = Application.DocumentManager.MdiActiveDocument.Editor;

                var entSelected = ed.GetSelection();

                if (entSelected.Status == PromptStatus.OK)
                {
                    foreach (var id in entSelected.Value.GetObjectIds())
                    {
                        var ent = (Entity)(object)trans.GetObject(id, mode, openErased);

                        result.Add(ent);
                    }
                }

                trans.Commit();
            }

            return result;
        }
    }
}
