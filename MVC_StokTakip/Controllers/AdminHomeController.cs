using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;

namespace MVC_StokTakip.Controllers
{
    [Authorize(Roles = "A")]
    public class AdminHomeController : Controller
    {
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        // GET: AdminHome
        public ActionResult Index()
        {
            return View();
        }
    }
}