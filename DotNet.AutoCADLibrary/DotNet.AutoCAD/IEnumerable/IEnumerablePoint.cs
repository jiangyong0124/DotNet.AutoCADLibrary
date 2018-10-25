using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using DotNet.AutoCAD.GeometryHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.AutoCAD.IEnumerable
{
    /// <summary>
    /// 点集合对象枚举器--
    /// 2018.10.25
    /// </summary>
    public static class IEnumerablePoint
    {
        /// <summary>
        /// 点集合转换为多线段
        /// </summary>
        /// <param name="points">3维点</param>
        /// <param name="closed">是否闭合多线段</param>
        /// <returns></returns>
        public static Polyline ToPolyline(this IEnumerable<Point3d> points, bool closed = true)
        {
            var i = 0;

            var result = new Polyline();

            var eum = points.GetEnumerator();

            while (eum.MoveNext())
            {
                result.AddVertexAt(i, eum.Current.ToPoint2d(), 0, 0, 0);
                i++;
            }

            result.Closed = closed;

            return result;
        }
    }
}
