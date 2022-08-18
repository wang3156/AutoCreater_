
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Base
{
    using DAL.Base;
    using Models.Base;
    using System.Linq.Expressions;
    public class BaseBLL<T>
    {
        static BaseDAL<T> DB = new BaseDAL<T>();

        public static List<T> GetData(PageInfo page = null, Expression<Func<T, bool>> where = null, string orderBy = "1")
        {
            if (where == null)
            {
                where = c => true;
            }
            return DB.GetData(where, page, orderBy);
        }
    }
}
