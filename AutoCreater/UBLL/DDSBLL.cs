using CommModels;
using System;
using System.Collections.Generic;
using UDAL;

namespace UBLL
{
    public class DDSBLL : IDisposable
    {
        DDSDAL dal = new DDSDAL();

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
    }
} 