using CommLibrary;
using CommModels;
using System;
using System.Collections.Generic;
using System.Data;
using TextCreate.DAL;


namespace TextCreate.BLL
{
    public class XXDFBLL : IDisposable
    {
        XXDFDAL dal = new XXDFDAL();

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
        public PageInfo GetPage(PageInfo pi, Dictionary<string, string> dic)
        {
            if (string.IsNullOrWhiteSpace(pi.SortExpression))
            {
                pi.SortExpression = " [CompayName]  asc ";
            }
            return dal.GetPage(pi, dic);
        }

        public string OnExport(Dictionary<string, string> filter)
        {
            return "";
        }

        public string OnDelete(string pk)
        {
           return dal.OnDelete(pk);
           
        }

        public string OnSave(DataTable datas) {
            return dal.OnSave(datas);
        }
    }
}