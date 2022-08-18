
using CommLibrary.DB;
using CommModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Data;


namespace UDAL
{
    public class DDFDAL : IDisposable
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
    public PageInfo GetPage(PageInfo pi,Dictionary<string, string> dic)
            {
             
            List<SqlParameter> pars = new List<SqlParameter>  { new SqlParameter("@pi", pi.PageIndex), new SqlParameter("@pz", pi.PageSize) };
                    List<string> where = new List<string> ();
                            int i = 0;
                            foreach (var item in dic)
                            {
                             if(!string.IsNullOrWhiteSpace(item.Value)){
                                pars.Add(new SqlParameter("@par" + i, item.Value));
                                where.Add($" [{item.Key}]=@par{i}");
                                i++;
                             }
                            }

                            pars.Add(new SqlParameter("@count", SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output });
                            pi.Data = sdb.GetDataTable($" select a.[MID],a.[XName],a.[DDDL],a0.[Name],a0.[Addr],a0.[Age] into  #k From  DDF a,DDS a0  where  a.[MID]=a0.[ID] and  1=1 {(where.Count > 0 ? " and " : "")+string.Join(" and ", where)} ;select @count=count(1) From #k ;select * From #k order by {pi.SortExpression} offset  (@pi-1)*@pz rows fetch next @pz rows only  ;drop table #k", pars);
                            pi.TotalRowNum = Convert.ToInt32(pars.Last().Value);
                            return pi;

        }
    }
}


