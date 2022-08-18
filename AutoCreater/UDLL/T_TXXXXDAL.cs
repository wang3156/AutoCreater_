
using CommLibrary.DB;
using CommModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using CommLibrary;
using Newtonsoft.Json;

namespace TextCreate.DAL
{
    public class T_TXXXXDAL : IDisposable
    {
        SqlServerDBHelper sdb = new SqlServerDBHelper();

        public void Dispose()
        {
            sdb.Dispose();
        }
        /// <summary>
        /// 获取分页数据
        ///</summary>
        /// <param name="pi"></param>
        /// <param name="dic"></param>
        /// <returns>返回的也是PageInfo对象,其中的值只看总页码和Data即可</returns>
        public PageInfo GetPage(PageInfo pi, Dictionary<string, string>
            dic)
        {

            List<SqlParameter> pars = new List<SqlParameter> { new SqlParameter("@pi", pi.PageIndex), new SqlParameter("@pz", pi.PageSize) };
            List<string> where = new List<string>();
            Dictionary<string, string> Querys = JsonConvert.DeserializeObject<Dictionary<string, string>>("{\"Name\":\"like\",\"Addr\":\"like%\",\"CreateTime\":\"=\",\"HZ\":\"=\"}");
            int i = 0;
            foreach (var item in dic)
            {
                if (!string.IsNullOrWhiteSpace(item.Value))
                {
                    pars.Add(new SqlParameter("@par" + i, CommFun.GetParameterValue(item.Value.Trim(), Querys[item.Key])));
                    where.Add($" a.[{item.Key}] {(Querys[item.Key] == "=" ? "=" : "like")} @par{i}");
                    i++;
                }
            }

            pars.Add(new SqlParameter("@count", SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output });
            pi.Data = sdb.GetDataTable($" select a.[ID],a.[Name],a.[Addr],[CreateTime]=Convert(varchar(19),a.[CreateTime],120),a.[HZ],a0.[Explains],a0.[Remark] into  #k From  T_TXXXX a,ERG_Marks a0  where  a.[ID]=a0.[EM_ID] and  1=1 {(where.Count > 0 ? " and " : "") + string.Join(" and ", where)} ;select @count=count(1) From #k ;select * From #k order by {pi.SortExpression} offset  (@pi-1)*@pz rows fetch next @pz rows only  ;drop table #k", pars);
            pi.TotalRowNum = Convert.ToInt32(pars.Last().Value);
            return pi;

        }
        /// <summary>
        /// 删除数据 
        /// </summary>
        /// <param name="pk">主键</param>
        /// <returns></returns>

        public string OnDelete(string pk)
        {
            try
            {
               
                sdb.ExecuteNonQuery("Delete T_TXXXX where [ID]=@pk ", new SqlParameter[] { new SqlParameter("@pk", SqlDbType.Int) { SqlValue = pk } });
                return "";
            }
            catch (Exception ex)
            {

                LogHelper.Error(ex);
                return ex.Message;
            }
        }

        /// <summary>
        /// 保存数据 
        /// </summary>
        /// <param name="datas">当前提交的数据</param>
        /// <returns></returns>

        public string OnSave(DataTable datas)
        {


            try
            {
                sdb.ExecuteNonQuery(" create table #k(ID int,Name nvarchar(32),Addr nvarchar(128),CreateTime datetime,HZ bit)");

                sdb.BulkCopy("#k", datas);

                sdb.ExecuteNonQuery(@"Update a set a.Name=b.Name,a.Addr=b.Addr,a.CreateTime=b.CreateTime,a.HZ=b.HZ   From T_TXXXX a,#k b where a.ID=b.ID
      Insert into T_TXXXX (Name,Addr,CreateTime,HZ) select b.Name,b.Addr,b.CreateTime,b.HZ From #k b where b.ID=0");

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message + "\r\n堆栈:" + ex.StackTrace);
                return ex.Message;
            }

            return "";
        }


    }
}




