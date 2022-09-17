using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;
using Aspose.BarCode.BarCodeRecognition;
using System.IO;

namespace MVC_StokTakip.Controllers
{
    public class HomeController : Controller
    {
        //readonly MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Hakkimizda()
        {
            return View();
        }
        public ActionResult Iletisim()
        {
            return View();
        }

        [Route("robots.txt", Name = "GetRobotsText"), OutputCache(Duration = 86400)]
        public ContentResult RobotsText()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("user-agent: *");
            stringBuilder.AppendLine("disallow: /");


            return this.Content(stringBuilder.ToString(), "text/plain", Encoding.UTF8);
        }

    }

}
