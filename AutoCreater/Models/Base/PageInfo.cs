using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Base
{
    public class PageInfo
    {


        int _pageSize = 0;
        /// <summary>
        /// 页容量
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {

                _pageSize = value;
            }
        }


        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        int _totalRowNum = 0;
        /// <summary>
        /// 总行数
        /// </summary>
        public int TotalRowNum
        {
            get
            {
                return _totalRowNum;

            }
            set
            {

                _totalRowNum = value;
            }
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortExpression { get; set; }

        public Object Data { get; set; }

    }
}
