using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.AutoCAD
{
    /// <summary>
    /// 绘制图形点、实体位置及大小的测试--
    /// 2018.10.25
    /// </summary>
    public static class DrawingTestHelper
    {
        /// <summary>
        /// 测试某个点的位置 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="db"></param>
        /// <param name="p1">点的位置</param>
        public static void TestPoint(this List<Entity> entities, Database db, Point3d p1)
        {
            entities.Add(new Circle(p1, Vector3d.ZAxis, 3));

            entities.Add(new MText() { Location = p1, Contents = "hello", Height = 1.5, Attachment = AttachmentPoint.MiddleCenter });
        }

        /// <summary>
        /// 测试集合中多个点的位置 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="db"></param>
        /// <param name="pts">点的集合</param>
        public static void TestPoint(this List<Entity> entities, Database db, List<Point3d> pts)
        {
            pts.ForEach(m =>
            {
                entities.Add(new Circle(m, Vector3d.ZAxis, 3));

                entities.Add(new MText() { Location = m, Contents = pts.IndexOf(m).ToString(), Height = 1.5, Attachment = AttachmentPoint.MiddleCenter });
            });
        }

        /// <summary>
        /// 添加实体集合到模型空间
        /// </summary>
        /// <param name="result"></param>
        /// <param name="db"></param>
        public static void ToModelSpace(this List<Entity> result, Database db)
        {
            result.ForEach(m =>
            {
                db.Invoke(t =>
                {
                    if (m != null)
                    {
                        t.AddEntity(m);
                    }
                });
            });
        }
    }
}
