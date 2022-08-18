using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CreateProperty
{
    [SugarTable("DBType")]
    public class _DBType
    {
        public string DBType { get; set; }
        public bool IsLength { get; set; }

        public string Explain { get; set; }
    }
}
