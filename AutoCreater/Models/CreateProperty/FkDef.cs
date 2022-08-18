using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CreateProperty
{
    /// <summary>
    /// 关联表主表关联字段信息
    /// {"FieldName":"ID","DisplayPage":false,"DisplayName":"","isFKFiled":true,"FKFiled":"ID"}
    /// </summary>
    public class FkDef
    {
        /// <summary>
        /// 主表字段名
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 是否在页面上显示
        /// </summary>
        public bool DisplayPage { get; set; }
        /// <summary>
        /// 页面上显示名
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 是否关联字段
        /// </summary>
        public bool IsFKFiled { get; set; }
        /// <summary>
        /// 外键表字段名
        /// </summary>
        public string FKFiled { get; set; }

        public string DbType { get; set; }
    }
}
