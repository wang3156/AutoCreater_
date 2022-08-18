
using CommModels;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data;
using TextCreate.Models;
using TextCreate.BLL;
using CommLibrary;

namespace TextCreate.Controllers
{
    public class T_TXXXXController : BaseController
    {
        T_TXXXXBLL bll = new T_TXXXXBLL();
        Cut_Result cr = new Cut_Result();
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPage(Dictionary<string, string> Filter, PageInfo PageInfo)
        {
            try
            {
                cr.Data = bll.GetPage(PageInfo, Filter);
                cr.Ok = true;
            }
            catch (System.Exception ex)
            {
                cr.Data = ex.Message;
                cr.Ok = false;
                LogHelper.Error(ex);

            }
           
            return Json(cr);
        }


        public ActionResult OnExport(Dictionary<string, string> Filter)
        {
            cr.Data = bll.OnExport(Filter);
            cr.Ok = true;
            return Json(cr);
        }


        public ActionResult OnDelete(string pk)
        {
            cr.Data = bll.OnDelete(pk);
            cr.Ok = string.IsNullOrWhiteSpace(cr.Data.ToString());
            return Json(cr);
        }


        public ActionResult OnSave(DataTable datas)
        {
            cr.Data = bll.OnSave(datas);
            cr.Ok = true;
            return Json(cr);

        }


    }
}