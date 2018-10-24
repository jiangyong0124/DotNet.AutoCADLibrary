using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices.Core;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
namespace DotNet_AutoCAD_API_Helper
{
    /// <summary>
    /// 引线-
    /// 操作引线、多重引线api的二次封装-
    /// 2018.10.13
    /// </summary>
    public static class LeaderHelper
    {
        
        public static Leader GetLeader()
        {
            var acLed = new Leader();

            var p1 = Point3d.Origin;

            acLed.AppendVertex(p1);

            acLed.AppendVertex(p1 + new Vector3d(4, 4, 0));

            acLed.AppendVertex(p1 + new Vector3d(4, 5, 0));

            acLed.HasArrowHead = true;

            return acLed;
        }
    }
}
