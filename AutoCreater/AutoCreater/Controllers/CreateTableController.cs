using AutoCreater.code;
using BLL.Base;
using BLL.CreateTable;
using Models.Base;
using Models.CreateProperty;
using Newtonsoft.Json;
using RazorEngine;
using RazorEngine.Templating;
using RazorEngine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;


using RazorEngine.Configuration;

using System.IO;
using CommLibrary;

namespace AutoCreater.Controllers
{
    public class CreateTableController : Controller
    {

        CreateTableBLL cb = new CreateTableBLL();
        // GET: CreateTable
        public ActionResult Index()
        {

            ViewData["DBType"] = JsonConvert.SerializeObject(BaseBLL<_DBType>.GetData());
            ViewData["Tables"] = JsonConvert.SerializeObject(BaseQueryBLL.GetTables());
            return View("createTable");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsondyn"> 前台提交回来的数据
        /// {"TableName":"表名","IsExport":false/*是否导出数据*/,
        /// /*字段信息*/
        /// "TableDefs":[{"Index":1,"FieldName":"ID","DbType":"int","Length":"","Identity":true,"Nullable":true,"DisplayName":"","DisplayPage":false,"IsQuery":false,"IsLength":false}],
        /// /*外键信息*/
        /// "FkObjs":[{"TableName":"Menus","FkDefs":[{"FieldName":"ID","DisplayPage":false,"DisplayName":"","isFKFiled":true,"FKFiled":"ID"},{"FieldName":"MenuName","DisplayPage":true,"DisplayName":"MenuName","isFKFiled":false,"FKFiled":""}]}]} 
        /// </param>
        /// <returns></returns>
        public ActionResult CreateTable(TableObj Tobj)
        {
            Tobj.IsAdd = Tobj.IsDel = Tobj.IsUpdate = Tobj.IsUpdate = true;
            LogHelper.Info("创建表数据:" + JsonConvert.SerializeObject(Tobj));
            string g = cb.CreateTable(Tobj, Server.MapPath("~/Views/CreateTable"), Server.MapPath("~/Createfiles"));
            return Json(new { msg = g });

        }

        public ActionResult GetFields(string tableName)
        {
            return Content(JsonConvert.SerializeObject(BaseQueryBLL.GetFields(tableName)));
        }


        //public ActionResult page()
        //{
        //    string s = "{ \"TableName\":\"DDD\",\"IsExport\":false,\"TableDefs\":[{\"Index\":1,\"FieldName\":\"ID\",\"DbType\":\"int\",\"Length\":\"\",\"Identity\":true,\"Nullable\":false,\"DisplayName\":\"\",\"DisplayPage\":false,\"IsQuery\":false,\"PrimaryKey\":true,\"Explain\":\"\",\"IsLength\":false},{\"Index\":2,\"FieldName\":\"Name\",\"DbType\":\"nvarchar\",\"Length\":\"32\",\"Identity\":false,\"Nullable\":true,\"DisplayName\":\"姓名\",\"DisplayPage\":true,\"IsQuery\":true,\"PrimaryKey\":false,\"Explain\":\"\",\"IsLength\":true},{\"Index\":3,\"FieldName\":\"Addr\",\"DbType\":\"nvarchar\",\"Length\":\"128\",\"Identity\":false,\"Nullable\":true,\"DisplayName\":\"地址\",\"DisplayPage\":true,\"IsQuery\":true,\"PrimaryKey\":false,\"Explain\":\"\",\"IsLength\":true},{\"Index\":4,\"FieldName\":\"Sex\",\"DbType\":\"int\",\"Length\":\"\",\"Identity\":false,\"Nullable\":true,\"DisplayName\":\"性别 \",\"DisplayPage\":false,\"IsQuery\":false,\"PrimaryKey\":false,\"Explain\":\"\",\"IsLength\":false}],\"FkObjs\":[{\"TableName\":\"Menus\",\"FkDefs\":[{\"FieldName\":\"ID\",\"DisplayPage\":true,\"DisplayName\":\"ID\",\"isFKFiled\":true,\"FKFiled\":\"ID\"},{\"FieldName\":\"MenuName\",\"DisplayPage\":true,\"DisplayName\":\"MenuName\",\"isFKFiled\":false,\"FKFiled\":\"\"},{\"FieldName\":\"MenuUrl\",\"DisplayPage\":true,\"DisplayName\":\"链接\",\"isFKFiled\":false,\"FKFiled\":\"\"}]}]}";

        //    var da = JsonConvert.DeserializeObject<TableObj>(s);
        //    var template = System.IO.File.ReadAllText(@"D:\gztest\AutoCreater\AutoCreater\AutoCreater\Views\CreateTable\page.cshtml");
        //    var result = Engine.Razor.RunCompile(template, Guid.NewGuid().ToString("n"), null, da);
        //    string path = @"C:\Users\ROG\Desktop\Create\"+da.TableName+@"\index.cshtml";
        //    System.IO.File.WriteAllText(path,result);

        //      template = System.IO.File.ReadAllText(@"D:\gztest\AutoCreater\AutoCreater\AutoCreater\Views\CreateTable\DAL.cshtml");
        //      result = Engine.Razor.RunCompile(template, Guid.NewGuid().ToString("n"), null, da);
        //    path = @"C:\Users\ROG\Desktop\Create\" + da.TableName + "DAL.cs";
        //    System.IO.File.WriteAllText(path, result);

        //    template = System.IO.File.ReadAllText(@"D:\gztest\AutoCreater\AutoCreater\AutoCreater\Views\CreateTable\BLL.cshtml");
        //    result = Engine.Razor.RunCompile(template, Guid.NewGuid().ToString("n"), null, da);
        //    path = @"C:\Users\ROG\Desktop\Create\" + da.TableName + "BLL.cs";
        //    System.IO.File.WriteAllText(path, result);

        //    template = System.IO.File.ReadAllText(@"D:\gztest\AutoCreater\AutoCreater\AutoCreater\Views\CreateTable\Controller.cshtml");
        //    result = Engine.Razor.RunCompile(template, Guid.NewGuid().ToString("n"), null, da);
        //    path = @"C:\Users\ROG\Desktop\Create\" + da.TableName + "Controller.cs";
        //    System.IO.File.WriteAllText(path, result);

        //    return Content(result);
        //}
        [HttpPost]
        public ActionResult getdate([Form] PageInfo PageInfo, [Form] Dictionary<string, string> Filter)
        {

            return this.NetJson(BaseQueryBLL.GetFields(Filter, PageInfo));
        }


    }
}





