
using CommModels;
using System.Collections.Generic;
using System.Web.Mvc;
using UBLL;

namespace TextCreate.Controllers
{
    public class UserTController : BaseController
    {
        UserTBLL bll = new UserTBLL();
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPage(Dictionary<string,string> dic,PageInfo pi) {            
            return Json(bll.GetPage(pi, dic));
        }
    }
}