using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices.Core;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
namespace DotNet.AutoCAD.GeometryHelper
{
    /// <summary>
    /// 针对实体的矩阵操作--
    /// 2018.10.25
    /// </summary>
    public static class MatrixHelper
    {
        /// <summary>
        /// 实现集合接口的对象遍历矩阵操作
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <param name="matrix">矩阵</param>
        public static void Transform(this IList<Entity> entities, Matrix3d matrix)
        {
            using (var enumtor = entities.GetEnumerator())
            {
                while (enumtor.MoveNext())
                {
                    enumtor.Current.TransformBy(matrix);
                }
            }
        }

        /// <summary>
        /// 由向量得到矩阵
        /// </summary>
        /// <param name="vector3d">基向量</param>
        /// <returns>矩阵</returns>
        public static Matrix3d ToMatrix(this Vector3d vector3d)
        {
            return Matrix3d.Displacement(vector3d);
        }
    }
}
