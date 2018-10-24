using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.AutoCAD.GeometryHelper
{
    /// <summary>
    /// 向量操作-
    /// 2018.10.25
    /// </summary>
    public static class VectorHelper
    {
        /// <summary>
        /// 三维向量转为二维向量
        /// </summary>
        /// <param name="vector3">三维向量</param>
        /// <returns>二维向量</returns>
        public static Vector2d ToVector2d(this Vector3d vector3)
        {
            return new Vector2d(vector3.X, vector3.Y);
        }
        /// <summary>
        /// 二维向量转为三维向量
        /// </summary>
        /// <param name="vector2">二维向量</param>
        /// <returns>三维向量</returns>
        public static Vector3d ToVector2d(this Vector2d vector2)
        {
            return new Vector3d(vector2.X, vector2.Y, 0);
        }
    }
}
