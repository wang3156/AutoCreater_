using CommLibrary.DB;
using DAL.CreateTable;
using Models.CreateProperty;
using Newtonsoft.Json;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BLL.CreateTable
{
    public class CreateTableBLL
    {
        TableObj Tobj;
        string TmpPath;
        string SavePath;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dyn">{TableName:"",def:[],IsExport:false/true,fkObj:[]}</param>
        /// <param name="tmpPath">模板的目录</param>
        /// <param name="savePath">文件保存的路径 </param>
        /// <returns></returns>
        public string CreateTable(TableObj dyn, string tmpPath, string savePath)
        {

            Tobj = dyn;
            TmpPath = tmpPath;
            SavePath = Path.Combine(savePath, Tobj.TableName);
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }
            if (!dyn.Existing)
            {
                string sql = GetSqlWithCreateTable();
                SqlServerDBHelper sdb = new SqlServerDBHelper();
                sdb.BeginTransaction();
                try
                {
                    sdb.ExecuteNonQuery(sql);
                    CreateFile();
                    sdb.Commit();
                }
                catch (Exception ex)
                {

                    sdb.RollBack();
                    return ex.Message;
                }
            }
            else
            {
                CreateFile();
            }


            return "";
        }
        /// <summary>
        ///  生成创建表的Sql
        /// </summary> 
        /// <returns></returns>
        string GetSqlWithCreateTable()
        {
            string TableName = Tobj.TableName;
            List<TableDef> Li_Defs = Tobj.TableDefs;
            Li_Defs.Sort((a, b) => a.Index - b.Index);
            StringBuilder sb = new StringBuilder(200);
            sb.Append(" Create Table " + TableName);
            sb.AppendLine("(");
            string[] str_length = { "decimal", "nvarchar", "varchar" };
            List<string> li_sl = new List<string>();
            List<string> pks = new List<string>();
            string Sl = " create nonclustered index IX_{tableName}_" + DateTime.Now.ToString("ffff") + @"      --表示创建唯一非聚集索引
on {tableName}({fl})        --数据表名称（建立索引的列名）
with 
(   --drop_existing=on ,
    pad_index=on,    --表示使用填充
    fillfactor=30,    --表示填充因子为50%
    statistics_norecompute=off    --表示启用统计信息自动更新功能
)";

            foreach (TableDef cdef in Li_Defs)
            {
                sb.AppendLine(cdef.FieldName);
                sb.Append(" " + cdef.DbType);
                if (str_length.Contains(cdef.DbType))
                {
                    sb.Append(" (" + cdef.Length + ") ");
                }
                if (!cdef.Nullable)
                {
                    sb.Append(" " + " not null ");
                }
                if (cdef.PrimaryKey)
                {
                    pks.Add(cdef.FieldName);
                }
                if (cdef.Identity)
                {
                    sb.Append(" " + " identity(1,1) ");
                }
                if (cdef.IsQuery)
                {
                    li_sl.Add(cdef.FieldName);
                }
                sb.Append(",");
            }

            if (pks.Count > 0)
            {
                sb.AppendLine(" primary key(" + string.Join(",", pks) + ")");
            }
            else
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.AppendLine(")");


            if (li_sl.Count > 0)
            {
                Sl = Sl.Replace("{tableName}", TableName).Replace("{fl}", string.Join(",", li_sl));
                sb.AppendLine(Sl);
            }

            return sb.ToString();
        }

        void CreateFile()
        {

            var template = System.IO.File.ReadAllText(Path.Combine(TmpPath, "page.cshtml"));
            var result = Engine.Razor.RunCompile(template, Guid.NewGuid().ToString("n"), null, Tobj);
            string path = Path.Combine(SavePath, Tobj.TableName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);

            }
            path = Path.Combine(path, @"index.cshtml");
            File.WriteAllText(path, result, System.Text.Encoding.UTF8);

            template = System.IO.File.ReadAllText(Path.Combine(TmpPath, "DAL.cshtml"));
            result = Engine.Razor.RunCompile(template, Guid.NewGuid().ToString("n"), null, Tobj);
            path = Path.Combine(SavePath, Tobj.TableName + "DAL.cs");
            File.WriteAllText(path, result, System.Text.Encoding.UTF8);

            template = System.IO.File.ReadAllText(Path.Combine(TmpPath, "BLL.cshtml"));
            result = Engine.Razor.RunCompile(template, Guid.NewGuid().ToString("n"), null, Tobj);
            path = Path.Combine(SavePath, Tobj.TableName + "BLL.cs");
            File.WriteAllText(path, result, System.Text.Encoding.UTF8);

            template = System.IO.File.ReadAllText(Path.Combine(TmpPath, "Controller.cshtml"));
            result = Engine.Razor.RunCompile(template, Guid.NewGuid().ToString("n"), null, Tobj);
            path = Path.Combine(SavePath, Tobj.TableName + "Controller.cs");
            File.WriteAllText(path, result, System.Text.Encoding.UTF8);

        }
    }
}

namespace RazorEngineTest
{
    public class MyRazorHtmlHelper
    {
        //输出原始html
        public IEncodedString RawString(string value)
        {
            return new RawString(value);
        }
        //输出一个checkbox
        public IEncodedString CheckBox(string id, string value, bool IsChecked)
        {
            StringBuilder sb = new StringBuilder(string.Empty);
            sb.Append("<input type='checkbox' id=").Append(id).Append("' value='").Append("' ");
            if (IsChecked)
            {
                sb.Append("checked");
            }
            sb.Append("/>");
            return new RawString(sb.ToString());
        }
    }
    public class MyTemplate<T> : TemplateBase<T>
    {
        private MyRazorHtmlHelper htmlhelper;
        public MyRazorHtmlHelper Html
        {
            get
            {
                if (htmlhelper == null)
                {
                    htmlhelper = new MyRazorHtmlHelper();
                }
                return htmlhelper;
            }
        }
        //用于输出原始html
        public IEncodedString OutputRawString(string value)
        {
            return new RawString(value);
        }
        //输出转义后内容
        public HtmlEncodedString OutputHtmlString(string value)
        {
            return new HtmlEncodedString(value);
        }
    }
    public class RazorHelper
    {
        public static void SetTemplate(Type templateType)
        {
            Razor.SetTemplateService(new TemplateService(new TemplateServiceConfiguration()
            {
                BaseTemplateType = templateType
            }));

        }
        public static string Parse<T>(HttpContext context, string virtualPath, T Model)
        {
            string path = context.Server.MapPath(virtualPath);
            string rawhtml = File.ReadAllText(path);
            //设置cacheName缓存。
            string cacheName = path + File.GetLastWriteTime(path);
            string parseHtml = Razor.Parse<T>(rawhtml, Model, cacheName);
            return parseHtml;
        }
        /// <summary>
        /// 用于输出原始html
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns></returns>
        public static IEncodedString RawHtml(string value)
        {
            return new RawString(value);
        }
        /// <summary>
        /// 用于输出转义后字符
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns></returns>
        public static HtmlEncodedString Text(string value)
        {
            return new HtmlEncodedString(value);
        }



    }
}
