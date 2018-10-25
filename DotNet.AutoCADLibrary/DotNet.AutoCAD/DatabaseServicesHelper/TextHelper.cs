using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.AutoCAD.DatabaseServicesHelper
{
    /// <summary>
    /// 文字帮助类--
    /// 2018.10.25
    /// </summary>
    public static class TextHelper
    {
        /// <summary>
        /// 获取多行文字
        /// </summary>
        /// <param name="p1">定位点</param>
        /// <param name="height">文字高度</param>
        /// <param name="contents">文字内容</param>
        /// <returns><多行文字对象/returns>
        public static MText GetMText(Point3d p1, double height, string contents)
        {
            var mtx = new MText()
            {
                Contents = contents,

                Location = p1,

                Height = height,

                Attachment = AttachmentPoint.MiddleCenter,

                ColorIndex = 7,
            };

            return mtx;
        }
    }
}
