using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices.Core;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
namespace DotNet.AutoCAD.DatabaseServicesHelper
{
    /// <summary>
    /// 基于多线段对象的拓展api-
    /// 2018.10.25
    /// </summary>
    public static class PolylineHelper
    {
        /// <summary>
        /// 获取多线段的顶点
        /// </summary>
        /// <param name="poly">多线段对象</param>
        /// <returns>顶点的集合</returns>
        public static IList<Point3d> GetVertexs(this Polyline poly)
        {
            var result = new List<Point3d>();
            
            for (int i = 0; i < poly.NumberOfVertices; i++)
            {
                result.Add(poly.GetPoint3dAt(i));
            }

            return result;
        }
    }
}
