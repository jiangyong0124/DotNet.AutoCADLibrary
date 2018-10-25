
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DotNet.AutoCAD.IEnumerable
{
    /// <summary>
    /// CAD实体集合枚举器--
    /// 2018.10.25
    /// </summary>
    public static class IEnumerableEntity
    {
        /// <summary>
        /// 获取指定实体的有效包围盒
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="match">实体是否有效的条件</param>
        /// <returns>范围对象</returns>
        public static Extents3d GetExtents(this IEnumerable<Entity> entities, Predicate<Entity> match = null)
        {
            using(var eum = entities.GetEnumerator())
            {
                var points = new List<Point3d>();

                while (eum.MoveNext())
                {
                    var current = eum.Current;

                    if (!current.Bounds.HasValue)
                    {
                        continue;
                    }

                    if (match == null || match(current))
                    {
                        points.Add(current.Bounds.Value.MinPoint);

                        points.Add(current.Bounds.Value.MaxPoint);
                    }
                }

                return new Extents3d(new Point3d(points.Min(m => m.X), points.Min(m => m.Y), 0),
                    new Point3d(points.Max(m => m.X), points.Max(m => m.Y), 0));
            }
        }
    }
}
