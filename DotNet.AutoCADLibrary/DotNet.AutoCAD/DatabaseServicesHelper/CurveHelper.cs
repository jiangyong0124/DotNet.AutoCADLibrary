using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
namespace DotNet.AutoCAD.DatabaseServicesHelper
{
    /// <summary>
    /// 曲线帮助类--
    /// 2018.10.25
    /// </summary>
    public static class CurveHelper
    {
        /// <summary>
        /// 基于公差求交
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="tolerance">公差</param>
        /// <returns></returns>
        public static List<Point3d> IntersectByCurve(this Curve a, Curve b, double tolerance = 1e-4)
        {
            var geCurve = a.GetGeCurve();

            var result = new List<Point3d>();

            using (var pc3d = new Point3dCollection())
            {
                a.IntersectWith(b, Intersect.ExtendBoth, pc3d, IntPtr.Zero, IntPtr.Zero);

                //using()

            }

            return result;
        }
    }
}
