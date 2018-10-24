using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.AutoCAD.DatabaseServicesHelper
{
    /// <summary>
    /// 标注--
    /// 操作转角标注、直径标注、半径标注......--
    /// 2018.10.20
    /// </summary>
    public static class DimensionHelper
    {
        /// <summary>
        /// 转角标注
        /// </summary>
        /// <param name="worker">工作数据库</param>
        /// <param name="direction">方向</param>
        /// <param name="start">起始点</param>
        /// <param name="end">终止点</param>
        /// <param name="dimDistance">偏移距离</param>
        /// <returns></returns>
        public static RotatedDimension RotatedDimension(this DatabaseWorker worker, RotateDirection direction, Point3d start, Point3d end, double dimDistance)
        {
            var dim = RotatedDimension(worker, direction, start, end, dimDistance, string.Empty);

            //dim.DimensionStyle=worker.

            return dim;
        }

        /// <summary>
        /// 转角标注
        /// </summary>
        /// <param name="worker">工作数据库</param>
        /// <param name="direction">方向</param>
        /// <param name="start">起始点</param>
        /// <param name="end">终止点</param>
        /// <param name="dimDistance">偏移距离</param>
        /// <param name="textReplace">文字替代</param>
        /// <returns></returns>
        public static RotatedDimension RotatedDimension(this DatabaseWorker worker, RotateDirection direction, Point3d start, Point3d end, double dimDistance, string textReplace = "")
        {
            //根据位置计算参照其中一点移动指定距离

            var x = Math.Abs(start.X - end.X);//水平差值

            var y = Math.Abs(start.Y - end.Y);//竖直差值

            var radian = 0.0;

            var dimLinePoint = new Point3d();

            switch (direction)
            {
                case RotateDirection.PX:
                    radian = -GRadian.Radians90;
                    dimLinePoint = start.X > end.X ? start + new Vector3d(dimDistance, y, 0) : end + new Vector3d(dimDistance, y, 0);
                    break;
                case RotateDirection.NY:
                    radian = GRadian.Radians270;
                    dimLinePoint = start.X > end.X ? start + new Vector3d(x, -dimDistance, 0) : end + new Vector3d(x, -dimDistance, 0);
                    break;
                case RotateDirection.NX:
                    radian = GRadian.Radians180;
                    dimLinePoint = start.X > end.X ? start + new Vector3d(-dimDistance, y, 0) : end + new Vector3d(-dimDistance, y, 0);
                    break;
                case RotateDirection.PY:
                    radian = GRadian.Radians90;
                    dimLinePoint = start.X > end.X ? start + new Vector3d(x, dimDistance, 0) : end + new Vector3d(x, dimDistance, 0);
                    break;
            }

            var result = new RotatedDimension();

            if (textReplace == string.Empty)
            {
                textReplace = result.Measurement.ToString();
            }

            result.Rotation = radian;

            result.XLine1Point = start;

            result.XLine2Point = end;

            result.DimLinePoint = dimLinePoint;

            result.DimensionText = textReplace;

            //result.DimensionStyle=worker.

            return result;
        }

        /// <summary>
        /// 将集合中（不存在其他类型实体）的标注统一进行缩放并返回新的集合
        /// </summary>
        /// <param name="dimensions">原标注集合</param>
        /// <param name="scale">缩放比例</param>
        /// <returns></returns>
        public static List<Dimension> DimensionScale(this IList<Dimension> dimensions, double scale)
        {
            var result = new List<Dimension>();

            foreach (Dimension dim in dimensions)
            {
                dim.Dimtxt = dim.Dimtxt * scale;

                dim.Dimasz = dim.Dimasz * scale;

                dim.Dimexe = dim.Dimexe * scale;

                dim.Dimgap = dim.Dimgap * scale;

                result.Add(dim);//添加操作后的标注
            }

            return result;
        }

        /// <summary>
        /// 将集合（存在其他类型实体）中的标注统一进行缩放并返回新的集合
        /// </summary>
        /// <param name="dimensions">原标注集合</param>
        /// <param name="scale">缩放比例</param>
        /// <returns></returns>
        public static List<Dimension> DimensionScale(this IList<Entity> entities, double scale)
        {
            var result = new List<Dimension>();

            var cloneDims = new List<Dimension>();

            foreach (var item in entities)
            {
                if (item is Dimension dim)
                {
                    cloneDims.Add(dim);
                }
            }

            foreach (Dimension dim in cloneDims)
            {
                entities.Remove(dim);//实体集合移除操作前的标注

                dim.Dimtxt = dim.Dimtxt * scale;

                dim.Dimasz = dim.Dimasz * scale;

                dim.Dimexe = dim.Dimexe * scale;

                dim.Dimgap = dim.Dimgap * scale;

                result.Add(dim);//返回标注集合添加操作后的标注
            }

            return result;
        }
    }
}
