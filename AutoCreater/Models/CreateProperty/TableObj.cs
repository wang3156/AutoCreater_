using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CreateProperty
{
    /// <summary>
    ///  {"TableName":"表名","IsExport":false/*是否导出数据*/,
    /// /*字段信息*/
    /// "def":[{"Index":1,"FieldName":"ID","DbType":"int","Length":"","Identity":true,"Nullable":true,"DisplayName":"","DisplayPage":false,"IsQuery":false,"IsLength":false}],
    /// /*外键信息*/
    /// "FkObjs":[{"TableName":"Menus","Data":[{"FieldName":"ID","DisplayPage":false,"DisplayName":"","isFKFiled":true,"FKFiled":"ID"},{"FieldName":"MenuName","DisplayPage":true,"DisplayName":"MenuName","isFKFiled":false,"FKFiled":""}]}]}
    /// </summary>
    public class TableObj
    {
        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { get; set; }


        /// <summary>
        /// 表相关的列属性
        /// </summary>
        public List<TableDef> TableDefs { get; set; }
        /// <summary>
        /// 关联表信息
        /// </summary>
        public List<FkObj> FkObjs { get; set; }


        /// <summary>
        /// 是否生成导出功能(页面属性)
        /// </summary>
        public bool IsExport { get; set; }

        /// <summary>
        /// 命名空间前缀
        /// </summary>
        public string Namespace { get; set; }
        /// <summary>
        /// 是否需要新增功能
        /// </summary>
        public bool IsAdd { get; set; }
        /// <summary>
        /// 是否需要删除功能
        /// </summary>
        public bool IsDel { get; set; }
        /// <summary>
        /// 是否需要修改功能
        /// </summary>
        public bool IsUpdate { get; set; }

        /// <summary>
        /// 标识当前表是存在的还是新加的
        /// </summary>
        public bool Existing { get; set; }

    }
}
