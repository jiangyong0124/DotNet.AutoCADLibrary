using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices.Core;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Colors;

namespace DotNet.AutoCAD.DatabaseServicesHelper
{
    /// <summary>
    /// 创建、修改、批量处理图层--
    /// 2018.10.20
    /// </summary>
    public static class LayerHelper
    {
        /// <summary>
        /// 添加图层
        /// </summary>
        /// <param name="host">封住事务对象</param>
        /// <param name="layers">图层</param>
        /// <returns>返回添加图层的ID集合</returns>
        public static List<ObjectId> AddLayer(this TransactionHost host, IDictionary<string, short> layers)
        {
            var objectIds = new List<ObjectId>();

            using (var layerTable = host.GetObject<LayerTable>(host.Database.LayerTableId))
            {
                foreach (KeyValuePair<string, short> layer in layers)
                {
                    if (!layerTable.Has(layer.Key))
                    {
                        var newLayer = new LayerTableRecord()
                        {
                            Name = layer.Key,

                            Color = Color.FromColorIndex(ColorMethod.ByAci, layer.Value)
                        };

                        var a1 = layerTable.IsReadEnabled;

                        if (!layerTable.IsWriteEnabled)
                        {
                            layerTable.UpgradeOpen();

                            var c1 = layerTable.IsWriteEnabled;
                        }

                        objectIds.Add(layerTable.Add(newLayer));

                        host.UpdateDBObject(newLayer);
                    }
                }

                host.Commit();
            }

            return objectIds;
        }

        /// <summary>
        /// 将图层设置为当前图像
        /// </summary>
        /// <param name="host">封装事务对象</param>
        /// <param name="layerName">图层名称</param>
        public static void SetCurrentLayer(this TransactionHost host, string layerName)
        {
            using (var layerTable = host.GetObject<LayerTable>(host.Database.LayerTableId))
            {
                if (layerTable.Has(layerName))
                {
                    var layer = host.GetObject<LayerTableRecord>(layerTable[layerName]);

                    if (layer.IsWriteEnabled)
                    {
                        layer.DowngradeOpen();

                        host.Database.Clayer = layer.ObjectId;
                    }
                    else
                    {
                        host.Database.Clayer = layerTable[layerName];
                    }
                }
                host.Commit();
            }
        }

        /// <summary>
        /// 删除图层
        /// </summary>
        /// <param name="host">数据库对象</param>
        /// <param name="layerNmaes">图层名称</param>
        public static void DelectLayer(this TransactionHost host, params string[] layerNmaes)
        {
            using (var layerTable = host.GetObject<LayerTable>(host.Database.LayerTableId))
            {
                foreach (var name in layerNmaes)
                {
                    if (layerTable.Has(name))
                    {
                        var layerID = layerTable[name];

                        if (layerID != host.Database.Clayer)
                        {
                            layerTable.GenerateUsageData();

                            var layer = host.GetObject<LayerTableRecord>(layerID);

                            if (!layer.IsUsed)
                            {
                                if (!layer.IsWriteEnabled)
                                {
                                    layer.UpgradeOpen();
                                }

                                layer.Erase(true);
                            }
                        }
                    }
                }
                host.Commit();
            }
        }
    }
}
