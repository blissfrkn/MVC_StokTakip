using MVC_StokTakip.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_StokTakip.Controllers
{
    public class GrafikController : Controller
    {

        readonly arabamis_MVC_StokTakipEntities db = new arabamis_MVC_StokTakipEntities();

        // GET: Grafik
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult VisualizeAracResult()
        //{

        //    var arac = db.Tamiratlar.AsEnumerable().Where(x=>x.Tarih.ToString("MMMM") == DateTime.Now.ToString("MMMM")).Select(i => new İstatistik()
        //    {
        //        Tarih = db.Tamiratlar.AsEnumerable().Where(x => x.Tarih.ToString("MMMM") == DateTime.Now.ToString("MMMM")).Select(z=>z.Tarih.ToString("MMMM")).FirstOrDefault(),
        //        adet = db.Tamiratlar.AsEnumerable().Where(x => x.Tarih.ToString("MMMM") == DateTime.Now.ToString("MMMM")).Select(y=>y.AracID).Count()

        //    }).ToList();



        //    return Json(arac, JsonRequestBehavior.AllowGet);
        //}

    }
}