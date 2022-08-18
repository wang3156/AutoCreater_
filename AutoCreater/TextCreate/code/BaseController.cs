using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

public class BaseController : Controller
{
    protected new JsonResult Json(object data)
    {
        JsonNetResult jr = new JsonNetResult();
        jr.Data = data;
        return jr;
    }

    protected dynamic GetResposeModel(bool Success, Object Data)
    {
        return new { Ok = Success, Data = Data };
    }
}