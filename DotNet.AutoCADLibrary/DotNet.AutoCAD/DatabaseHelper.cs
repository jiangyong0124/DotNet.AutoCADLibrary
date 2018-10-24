using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.AutoCAD
{
    /// <summary>
    /// 数据库拓展api-
    /// 委托封装Autocad事务-
    /// 2018.10.12
    /// </summary>
    public static class DatabaseHelper
    {
        /// <summary>
        /// 封装数据库对象api，内部调用事务
        /// </summary>
        /// <param name="database">数据库对象</param>
        /// <param name="action">运行函数</param>
        public static void Invoke(this Database database, Action<TransactionHost> action)
        {
            using (var host = new TransactionHost(database))
            {
                action?.Invoke(host);

                host.Commit();
            }
        }

        /// <summary>
        /// 封装文档对象api并锁住文档，内部调用事务
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <param name="action">运行函数</param>
        public static void Invoke(this Document document, Action<TransactionHost> action)
        {
            using (document.LockDocument())
            {
                var database = document.Database;

                using (var host = new TransactionHost(database))
                {
                    action?.Invoke(host);

                    host.Commit();
                }
            }
        }

        ///// <summary>
        ///// 开启带一个参数的事务
        ///// </summary>
        ///// <param name="db">The database.</param>
        ///// <param name="action">The action.</param>
        ///// <param name="parameter">The parameter.</param>
        //public static void Invoke(this Database db, Action<TransactionHost, object> action, object parameter)
        //{
        //    using (var host = new TransactionHost(db))
        //    {
        //        action?.Invoke(host, parameter);

        //        host.Commit();
        //    }
        //}

        ///// <summary>
        ///// 开启一个带返回值的事务
        ///// </summary>
        //public static TResult Invoke<TResult>(this Database db, Func<TransactionHost, TResult> func)
        //{
        //    using (var host = new TransactionHost(db))
        //    {
        //        var result = func.Invoke(host);

        //        host.Commit();

        //        return result;
        //    }
        //}

        ///// <summary>
        ///// 开启一个带一个参数、一个返回值的事务
        ///// </summary>
        ///// <typeparam name="TResult">The type of the result.</typeparam>
        ///// <param name="db">The database.</param>
        ///// <param name="func">The function.</param>
        ///// <param name="parameter">The parameter.</param>
        ///// <returns></returns>
        //public static TResult Invoke<TResult>(this Database db, Func<TransactionHost, object, TResult> func, object parameter)
        //{
        //    using (var host = new TransactionHost(db))
        //    {
        //        var result = func.Invoke(host, parameter);

        //        host.Commit();

        //        return result;
        //    }
        //}

        /// <summary>
        /// 读取dwg文件
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="file">The file.</param>
        public static void ReadDwgFile(this Database database, string file)
        {
            database.ReadDwgFile(file, FileShare.ReadWrite, true, string.Empty);
        }
    }
}
