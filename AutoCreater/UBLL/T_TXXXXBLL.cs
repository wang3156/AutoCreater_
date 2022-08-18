using CommModels;
using System;
using System.Collections.Generic;
using TextCreate.DAL;
using Newtonsoft.Json;
using CommLibrary;
using System.Data;

namespace TextCreate.BLL
{
    public class T_TXXXXBLL : IDisposable
    {
        T_TXXXXDAL dal = new T_TXXXXDAL();

        public void Dispose()
        {
            dal.Dispose();
        }
        /// <summary>
        /// 获取分页数据 
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="dic"></param>
        /// <returns>返回的也是PageInfo对象,其中的值只看总页码和Data即可</returns>
        public PageInfo GetPage(PageInfo pi,Dictionary<string, string> dic)
        {   
         if (string.IsNullOrWhiteSpace(pi.SortExpression))
            {
                pi.SortExpression =" [Name]  asc ";
            }     
            return dal.GetPage(pi, dic);
        }

        ///<summary>
        /// 导出数据的方法
        ///</summary>
        ///<param name=""filter"">当前的查询条件</param>
        /// <returns></returns>
       
        public string OnExport(Dictionary<string, string> filter)
        {
         

            //记录列名和显示名映射
            Dictionary<string, string>dic =JsonConvert.DeserializeObject<Dictionary<string, string>>("{\"Name\":\"姓名\",\"Addr\":\"地址\",\"CreateTime\":\"CreateTime\",\"HZ\":\"合资\",\"Explains\":\"说明\",\"Remark\":\"备注\"}");
            DataTable dt = (GetPage(new PageInfo { PageIndex = 1, PageSize = int.MaxValue }, filter).Data as DataTable);
            foreach (DataColumn col in dt.Columns)
            {
                if (dic.ContainsKey(col.ColumnName))
                {
                    col.ColumnName = dic[col.ColumnName];
                }
            }
            byte[] bte = CommFun.DataTableToExcel(dt);
            return Convert.ToBase64String(bte);

        }
        
            /// <summary>
            /// 删除数据 
            /// </summary>
            /// <param name="pk">主键</param>
            /// <returns></returns>
            
            public string OnDelete(string pk)
            {
                return dal.OnDelete(pk);

            }
             
            /// <summary>
            /// 保存数据 
            /// </summary>
            /// <param name="datas">当前提交的数据</param>
            /// <returns></returns>
                  
          public string OnSave(DataTable datas) {
            return dal.OnSave(datas);
        }
        

    }
}
