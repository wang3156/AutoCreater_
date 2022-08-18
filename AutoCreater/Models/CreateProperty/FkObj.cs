using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CreateProperty
{
    /// <summary>
    /// [{"TableName":"Menus","FkDefs":[{"FieldName":"ID","DisplayPage":false,"DisplayName":"","isFKFiled":true,"FKFiled":"ID"},{"FieldName":"MenuName","DisplayPage":true,"DisplayName":"MenuName","isFKFiled":false,"FKFiled":""}]}]} 
    /// </summary>
    public class FkObj
    {
        /// <summary>
        /// 外键主表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 关联字段信息
        /// </summary>
        public List<FkDef> FkDefs { get; set; }
    }
}
