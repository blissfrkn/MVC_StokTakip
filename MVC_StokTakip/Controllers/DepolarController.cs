using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;
using MVC_StokTakip.MyModel;



namespace MVC_StokTakip.Controllers
{
    public class DepolarController : Controller
    {
        readonly arabamis_MVC_StokTakipEntities db = new arabamis_MVC_StokTakipEntities();

        // GET: Depolar
        public ActionResult Index()
        {
            var model = db.Depolar.ToList();
            return View(model);
        }



        [HttpPost]
        public ActionResult DepoEkle(string Ad , string Adres , string Aciklama)
        {

                Depolar d = new Depolar();
                d.Adi = Ad;
                d.Aciklama = Aciklama;
                d.Adres = Adres;
                db.Depolar.Add(d);
                db.SaveChanges();
                 return Json(new { result = "Redirect", url = Url.Action("Index", "Depolar") }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult DepoDetay(int id)
        {
            var model = db.Depolar.Find(id);
            var model2 = db.DepoUrun.Where(x => x.DepoID == id).ToList();
            var model3 = db.Satislar.Where(x => x.DepoID == id).ToList();
            var model4 = db.Girisler.Where(x => x.DepoID == id).ToList();
            Class1 a = new Class1();
            a.Depolar = model;
            a.DepoUruns = model2;
            a.Satislars = model3;
            a.Girislers = model4;
            return View(a);
        }

        [HttpPost]
        public ActionResult DepoGuncelle(Class1 d,string guncelle, string cancel)
        {
            if (guncelle != null)
            {
                db.Entry(d.Depolar).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            else if(cancel != null)
            {
                return Json("2", JsonRequestBehavior.AllowGet);
            }

            return Json("Finish");
        }

        [HttpPost]
        public ActionResult DefaultDepo(int id)
        {
            var depo = db.Depolar.ToList();
            foreach (var item in depo)
            {
                if (item.ID == id)
                {
                    item.IsDefault = true;
                }
                else
                {
                    item.IsDefault = false;
                }
            }
            db.SaveChanges();

            return Json("0",JsonRequestBehavior.AllowGet);
        }

    }
}