using DAL.CreateTable;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Models.Base;

namespace BLL.CreateTable
{
    public class BaseQueryBLL
    {

        static BaseQueryDAL bqd = new BaseQueryDAL();
        /// <summary>
        /// 获取数据库所有table
        /// </summary>using DAL.CreateTable;
        /// <returns></returns>
        public static DataTable GetTables()
        {
            return bqd.GetTables();
        }

        public static DataTable GetFields(string tableName)
        {
            return bqd.GetFields(tableName);

        }

        public static PageInfo GetFields(Dictionary<string, string> dic, PageInfo pi)
        {
            if (string.IsNullOrWhiteSpace(pi.SortExpression))
            {
                pi.SortExpression = dic.First().Key + " asc";
            }
            return bqd.GetFields(dic, pi);
        }
    }
}
