using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommLibrary
{
    public class CommFun
    {
        /// <summary>
        /// 根据查询类型获取val
        /// </summary>
        /// <param name="val"></param>
        /// <param name="QueryType"></param>
        /// <returns></returns>
        public static string GetParameterValue(string val, string QueryType)
        {

            string[] sup = new[] { "=", "like", "like%", "%like" };
            if (!sup.Contains(QueryType, true))
            {
                new Exception("GetParameterValue 时传入的QueryType不正确! " + QueryType);
            }
            val = val.Trim();
            switch (QueryType)
            {
                case "=":
                    break;
                case "like":
                    val = "%" + val + "%";
                    break;
                case "like%":
                    val = val + "%";
                    break;
                case "%like":
                    val = "%" + val;
                    break;
            }
            return val;
        }


        public static byte[] DataTableToExcel(DataTable dt)
        {
            byte[] re = null;
            using (ExcelPackage ep = new ExcelPackage())
            {
                ep.Workbook.Worksheets.Add("Sheet1").Cells["A1"].LoadFromDataTable(dt, true);
                re = ep.GetAsByteArray();
            }
            return re;
        }
        /// <summary>
        /// 获取调用该方法的方法的完全限定名
        /// </summary>
        /// <returns></returns>
        public static string GetMethodName()
        {
            var method = new StackFrame(1).GetMethod(); // 这里忽略1层堆栈，也就忽略了当前方法GetMethodName，这样拿到的就正好是外部调用GetMethodName的方法信息
            var property = (
            from p in method.DeclaringType.GetProperties(
            BindingFlags.Instance |
            BindingFlags.Static |
            BindingFlags.Public |
            BindingFlags.NonPublic)
            where p.GetGetMethod(true) == method || p.GetSetMethod(true) == method
            select p).FirstOrDefault();
            return property == null ? method.ReflectedType.Name + "." + method.Name : property.ReflectedType.Name + "." + property.Name;
        }
    }
}
