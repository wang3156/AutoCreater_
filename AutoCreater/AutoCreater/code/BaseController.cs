using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AutoCreater.code
{
    public class BaseController : Controller
    {
        protected new JsonResult Json(object data)
        {
            JsonNetResult jr = new JsonNetResult();
            jr.Data = data;
            return jr;
        }
    }
}