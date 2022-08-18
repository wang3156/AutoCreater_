using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CreateProperty
{
    /// <summary>
    /// 一个表结构的字段信息
    /// {"Index":1,"FieldName":"ID","DbType":"int","Length":"","Identity":true,"Nullable":true,"DisplayName":"","DisplayPage":false,"IsQuery":false,/*以下为前台使用字段*/"IsLength":false}
    /// </summary>
    public class TableDef
    {
        
        /// <summary>
        /// 列的顺序 
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string DbType { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
        public string Length { get; set; }
        /// <summary>
        /// 是否自增
        /// </summary>

        public bool Identity { get; set; }

        /// <summary>
        /// 是否可空
        /// </summary>
        public bool Nullable { get; set; }
        /// <summary>
        /// 是否主键
        /// </summary>
        public bool PrimaryKey { get; set; }
        /// <summary>
        /// 字段说明
        /// </summary>
        public string Explain { get; set; }


        /*以下为页面显示设置字段*/
        /// <summary>
        /// 是否显示到页面上
        /// </summary>
        public bool DisplayPage { get; set; }
        /// <summary>
        /// 页面显示名称
        /// </summary>
        public string DisplayName { get; set; }

        public bool IsQuery { get; set; }

        /// <summary>
        /// 查询类型  = 完全匹配 like 模糊查询  %like 以什么结尾 like%以什么开头
        /// </summary>
        public string QueryType { get; set; }


    }
}
