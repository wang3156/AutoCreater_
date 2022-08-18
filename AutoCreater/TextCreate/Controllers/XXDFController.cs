
using CommModels;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using TextCreate.BLL;
using TextCreate.Models;

namespace TextCreate.Controllers
{
    public class XXDFController : BaseController
    {
        XXDFBLL bll = new XXDFBLL();
        Cut_Result cr = new Cut_Result();
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPage(Dictionary<string, string> Filter, PageInfo PageInfo)
        {
            cr.Data = bll.GetPage(PageInfo, Filter);
            cr.Ok = string.IsNullOrWhiteSpace(cr.Data.ToString());
            return Json(cr);
        }
        public ActionResult OnDelete(string pk)
        {
            cr.Data = bll.OnDelete(pk);
            cr.Ok = string.IsNullOrWhiteSpace(cr.Data.ToString());
            return Json(cr);
        }

        public ActionResult OnExport(Dictionary<string, string> Filter)
        {
            cr.Data = bll.OnExport(Filter);
            cr.Ok = true;
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