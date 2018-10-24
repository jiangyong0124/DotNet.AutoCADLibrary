using Autodesk.AutoCAD.DatabaseServices;

namespace DotNet.AutoCAD
{
    /// <summary>
    /// 事务对象-
    /// 继承Autocad内置事务并封装，提供工作效率-
    /// 2018.10.12
    /// </summary>
    public class TransactionHost : OpenCloseTransaction
    {
        /// <summary>
        /// 数据库对象
        /// </summary>
        private Database _database;

        /// <summary>
        /// 模型空间块表记录
        /// </summary>
        private BlockTableRecord _modelSpace;

        /// <summary>
        /// 创建事务对象
        /// </summary>
        /// <param name="database"></param>
        public TransactionHost(Database database)
        {
            this._database = database;

            this._modelSpace = new BlockTableRecord();
        }

        /// <summary>
        /// 返回数据库对象
        /// </summary>
        public Database Database
        {
            get
            {
                return _database;
            }
        }

        /// <summary>
        /// 返回当前模型空间
        /// </summary>
        public BlockTableRecord ModelSpace
        {
            get
            {
                if (_modelSpace.ObjectId.IsNull)
                {
                    var block = (BlockTable)GetObject(_database.BlockTableId, OpenMode.ForRead);

                    return _modelSpace = GetObject<BlockTableRecord>(block[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                }
                else
                {
                    return _modelSpace;
                }
            }
        }

        /// <summary>
        /// 依据对象ID来获取实体对象
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="object">对象ID</param>
        /// <param name="openMode">对象可操作的状态</param>
        /// <returns></returns>
        public TResult GetObject<TResult>(ObjectId @object, OpenMode openMode = OpenMode.ForRead)
            where TResult : DBObject
        {
            var result = default(TResult);

            if (@object.IsNull)
            {
                return result;
            }

            result = GetObject(@object, openMode) as TResult;

            return result;
        }

        /// <summary>
        /// 添加新创建实体到当前模型空间
        /// </summary>
        /// <typeparam name="TResult">Enitity父类</typeparam>
        /// <param name="result">具体的实体对象</param>
        public void AddEntity<TResult>(TResult result)
            where TResult : Entity
        {
            var block = (BlockTable)GetObject(_database.BlockTableId, OpenMode.ForRead);

            var blockRecord = (BlockTableRecord)GetObject(block[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

            var objID = blockRecord.AppendEntity(result);

            AddNewlyCreatedDBObject(result, true);
        }

        /// <summary>
        /// 依据对象ID进行擦除
        /// </summary>
        /// <param name="array">The array.</param>
        public void Erase(params ObjectId[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].IsNull || array[i].IsErased)
                {
                    continue;
                }

                using (var db = this.GetObject<DBObject>(array[i], OpenMode.ForWrite))
                {
                    db.Erase();
                }
            }
        }

        /// <summary>
        /// 从内存写入虚拟数据库，等待提交更新
        /// </summary>
        /// <param name="object"></param>
        public void UpdateDBObject(DBObject @object)
        {
            AddNewlyCreatedDBObject(@object, true);
        }
    }
}
