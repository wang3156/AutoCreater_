using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommLibrary
{
    public class DBHelper
    {
        public static SqlSugarClient db = new SqlSugarClient(
           new ConnectionConfig()
           {
               ConnectionString = ConfigHelper.GetConfing("AutoCreaterCon"),
               DbType = (DbType)Enum.Parse(typeof(DbType), ConfigHelper.GetConfing("DbType", ConfigType.AppConfig), false),//设置数据库类型
               IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
               InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
           });

        /// <summary>
        /// 当实例化DBHelper对象
        /// </summary>
        /// <param name="showSql">指示是否记录生成的SQL</param>
        public DBHelper(bool showSql = false)
        {
            if (showSql)
            {
                //用来打印Sql方便你调式    
                db.Aop.OnLogExecuting = (sql, pars) =>
                {

                    LogHelper.Info(sql + "\r\n" +
                    db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));

                };
            }

        }




    }
}
