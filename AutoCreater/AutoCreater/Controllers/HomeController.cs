using BLL.Base;
using CommLibrary;
using Models.Base;
using Newtonsoft.Json;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoCreater.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            ViewData["Menus"] = BaseBLL<Menus>.GetData(orderBy: "_index");
         

            ViewResult v = View();
            return v;
        }


    }
}