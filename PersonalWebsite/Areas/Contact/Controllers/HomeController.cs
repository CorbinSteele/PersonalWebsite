﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalWebsite.Areas.Contact.Controllers
{
    public class HomeController : Controller
    {
        // GET: About/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}