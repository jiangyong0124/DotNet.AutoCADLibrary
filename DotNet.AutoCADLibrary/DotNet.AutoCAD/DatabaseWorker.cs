using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.AutoCAD
{
    /// <summary>
    /// 数据库封装-
    /// 针对web提供对数据库的访问-
    /// 2018.10.12
    /// </summary>
    public class DatabaseWorker
    {
        /// <summary>
        /// 当前数据库对象
        /// </summary>
        public readonly Database Database;

        /// <summary>
        /// 连续线型
        /// </summary>
        public readonly ObjectId LineType_Continuous;

        public DatabaseWorker(Database database)
        {
            this.Database = database;

            using (var host = database.TransactionManager.StartOpenCloseTransaction())
            {
                {
                    var lineTypeTb = host.GetObject(database.LinetypeTableId, OpenMode.ForRead) as LinetypeTable;

                    if (lineTypeTb == null)
                    {
                        return;
                    }

                    if (lineTypeTb.Has("Continuous"))
                    {
                        this.LineType_Continuous = lineTypeTb["Continuous"];
                    }
                }//获取线型
            }
        }
    }
}
