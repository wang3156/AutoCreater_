using AutoCreater.code;
using Models.Base;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AutoCreater.Controllers
{
    public class DefaultController : BaseController
    {
       // XXXBLL bll = new XXXBLL();
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPage(Dictionary<string,string> dic,PageInfo pi) {
            //bll.GetPage(pi, dic)
            return Json("");
        }
    }
}