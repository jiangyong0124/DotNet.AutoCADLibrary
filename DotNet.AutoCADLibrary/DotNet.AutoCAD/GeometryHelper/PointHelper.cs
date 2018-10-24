using Autodesk.AutoCAD.Geometry;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DotNet.AutoCAD.GeometryHelper
{
    /// <summary>
    /// 点对象api的拓展-
    /// 2018.10.14
    /// </summary>
    public static class PointHelper
    {
        /// <summary>
        /// 3维点转为2维点
        /// </summary>
        /// <param name="OP">目标点</param>
        /// <returns>11111</returns>
        public static Point2d ToPoint2d(this Point3d OP)
        {
            return new Point2d(OP.X, OP.Y);
        }

        /// <summary>
        /// 返回2个点的中点
        /// </summary>
        /// <param name="start">起始点</param>
        /// <param name="end">终止点</param>
        /// <returns></returns>
        public static Point3d GetCenter(this Point3d start, Point3d end)
        {
            return new Point3d((start.X + end.X) * 0.5, (start.Y + end.Y) * 0.5, 0);
        }

        /// <summary>
        /// 返回多个点的中点
        /// </summary>
        /// <param name="points">3点集合</param>
        /// <returns></returns>
        public static Point3d GetCenter(this IList<Point3d> points)
        {

            var x1 = 0.0;
            var y1 = 0.0;
            var z1 = 0.0;

            var n = points.Count;

            for (int i = 0; i < n; i++)
            {
                x1 += points[i].X;
                y1 += points[i].Y;
                z1 += points[i].Z;
            }

            return new Point3d(x1 / n, y1 / n, z1 / n);
        }

        /// <summary>
        /// 世界原点指向目标点的向量
        /// </summary>
        /// <param name="targetPoint">目标点</param>
        /// <returns>向量</returns>
        public static Vector3d ToVector3d(this Point3d targetPoint)
        {
            return targetPoint - Point3d.Origin;
        }

        /// <summary>
        /// 世界原点指向目标点的向量
        /// </summary>
        /// <param name="targetPoint">目标点</param>
        /// <returns>向量</returns>
        public static Vector2d ToVector2d(this Point3d targetPoint)
        {
            var p = new Point2d(targetPoint.X, targetPoint.Y);

            return p - Point2d.Origin;
        }
    }
}
