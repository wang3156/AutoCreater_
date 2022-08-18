
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Base
{
    using CommLibrary;
    using Models.Base;
    using SqlSugar;
    using System.Linq.Expressions;

    public class BaseDAL<T>
    {
        /// <summary>
        /// 根据分页条件及查询条件获取数据
        /// </summary>
        /// <param name="page">为null时获取所有数据</param>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public List<T> GetData(Expression<Func<T, bool>> where, PageInfo page = null, string orderBy = "1")
        {
            ISugarQueryable<T> sq = DBHelper.db.Queryable<T>().Where(where);

            if (page != null)
            {
                int totle = 0;
                List<T> li = sq.OrderBy(orderBy).ToPageList(page.PageIndex, page.PageSize, ref totle);
                page.TotalRowNum = totle;
                return li;
            }
            else
            {
                return sq.OrderBy(orderBy).ToList();
            }

        }
    }
}
