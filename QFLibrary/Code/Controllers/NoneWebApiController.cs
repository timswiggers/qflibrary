﻿using System.Web.Mvc;

namespace QFLibrary.Code.Controllers
{
    public class NoneWebApiController : Controller
    {
        public ActionResult ToIndexHtml()
        {
            return new FilePathResult("src/app/index.html", "text/html");
        }
    }
}