using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Geometry;

namespace DotNet.AutoCAD.DatabaseServicesHelper
{
    /// <summary>
    /// 填充帮助类--
    /// 2018.10.25
    /// </summary>
    public static class HatchHelper
    {
        /// <summary>
        /// 创建填充实体
        /// </summary>
        /// <param name="poly">多线段参数</param>
        /// <param name="patternScale">填充比例</param>
        /// <returns>填充对象</returns>
        public static Hatch GetHatch(this Polyline poly, double patternScale = 1)
        {
            var pts = poly.GetVertexs();

            var distinctPts = pts.Distinct(new PointEqual()).ToList();

            var point2dCollection = new Point2dCollection();

            distinctPts.ForEach(m =>
            {
                point2dCollection.Add(new Point2d(m.X, m.Y));
            });

            point2dCollection.Add(new Point2d(distinctPts[0].X, distinctPts[0].Y));

            var hatch = new Hatch();

            var dbCollection = new DoubleCollection() { 0 };

            hatch.AppendLoop(HatchLoopTypes.Polyline, point2dCollection, dbCollection);

            hatch.SetHatchPattern(HatchPatternType.PreDefined, "ANSI31");

            hatch.PatternScale = patternScale;

            hatch.ColorIndex = GColor.Green;

            hatch.EvaluateHatch(true);

            return hatch;
        }
    }

    /// <summary>
    /// 重载点对象是否相等的哈希接口
    /// </summary>
    public class PointEqual : IEqualityComparer<Point3d>
    {
        /// <summary>
        /// 当两个对象的Hash值相等时，通过equal来重新做判断
        /// </summary>
        /// <param name="x">类型1</param>
        /// <param name="y">类型2</param>
        /// <returns>是否相等</returns>
        public bool Equals(Point3d x, Point3d y)
        {
            if (x.X == y.X && x.Y == y.Y)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 返回对象的唯一Hash值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>hash值</returns>
        public int GetHashCode(Point3d obj)
        {
            return (int)obj.X ^ (int)obj.Y;
        }
    }
}
