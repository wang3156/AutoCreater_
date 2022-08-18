
using CommModels;
using System.Collections.Generic;
using System.Web.Mvc;
using UBLL;

namespace AutoCreater.Controllers
{
    public class DDFController : BaseController
    {
        DDFBLL bll = new DDFBLL();
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPage(Dictionary<string,string> Filter, PageInfo PageInfo) {
            return Json(bll.GetPage(PageInfo, Filter));
        }
    }
}