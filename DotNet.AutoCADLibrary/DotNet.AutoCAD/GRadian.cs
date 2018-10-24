using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.AutoCAD
{
    /// <summary>
    /// 全局弧度制
    /// </summary>
    public static class GRadian
    {
        /// <summary>
        /// 角度转弧度
        /// </summary>
        public const double DegreesToRadians = Math.PI / 180.0;

        /// <summary>
        /// 弧度转角度
        /// </summary>
        public const double RadiansToDegrees = 180.0 / Math.PI;

        /// <summary>
        /// 30弧度
        /// </summary>
        public const double Radians30 = Math.PI / 6.0;

        /// <summary>
        ///45弧度
        /// </summary>
        public const double Radians45 = Math.PI / 4.0;

        /// <summary>
        /// 60弧度
        /// </summary>
        public const double Radians60 = Radians30 * 2.0;

        /// <summary>
        ///90弧度
        /// </summary>
        public const double Radians90 = Radians45 * 2.0;

        /// <summary>
        /// 120弧度
        /// </summary>
        public const double Radians120 = Radians30 * 4.0;

        /// <summary>
        /// 150弧度
        /// </summary>
        public const double Radians150 = Radians30 * 5.0;

        /// <summary>
        /// 180弧度
        /// </summary>
        public const double Radians180 = Math.PI;

        /// <summary>
        /// 210弧度
        /// </summary>
        public const double Radians210 = Radians30 * 7.0;

        /// <summary>
        /// 240弧度
        /// </summary>
        public const double Radians240 = Radians30 * 8.0;

        /// <summary>
        /// 270弧度
        /// </summary>
        public const double Radians270 = Radians30 * 9.0;

        /// <summary>
        /// 300弧度
        /// </summary>
        public const double Radians300 = Radians30 * 10.0;

        /// <summary>
        /// 330弧度
        /// </summary>
        public const double Radians330 = Radians30 * 11.0;

        /// <summary>
        /// 360弧度
        /// </summary>
        public const double Radians360 = Math.PI * 2.0;
    }
}
