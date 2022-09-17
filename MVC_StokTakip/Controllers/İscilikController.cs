using MVC_StokTakip.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_StokTakip.Controllers
{
    public class İscilikController : Controller
    {
        readonly arabamis_MVC_StokTakipEntities db = new arabamis_MVC_StokTakipEntities();
        // GET: İscilik
        public ActionResult Index()
        {
            var model = db.İscilik.ToList();
            return View(model);

        }

        [HttpGet]
        public ActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Ekle(İscilik p)
        {
            p.Tarih = DateTime.Now;
            p.IsDelete = false;
            db.Entry(p).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();
            return Json(new { result = "Redirect", url = Url.Action("Index", "İscilik") }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GuncelleBilgiGetir(int id)
        {
            var model = db.İscilik.Find(id);
            return View("GuncelleBilgiGetir", model);
        }


        [HttpPost]
        public ActionResult Guncelle(İscilik p)
        {
            p.Tarih = DateTime.Now;
            p.IsDelete = false;
            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(new { result = "Redirect", url = Url.Action("Index", "İscilik") }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Sil(int id)
        {
            var model = db.İscilik.FirstOrDefault(x => x.ID == id);
            model.IsDelete = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Deleteİscilik()
        {
            var model = db.İscilik.Where(x => x.IsDelete == true).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult GeriYukle(int id)
        {
            var model = db.İscilik.Find(id);
            model.IsDelete = false;
            db.SaveChanges();
            return Json("İşçilik başarıyla geri yüklendi", JsonRequestBehavior.AllowGet);
        }

        public ActionResult İscilikListe()
        {

            var iscilik = db.İscilik.Where(x => x.IsDelete == false).ToList();        

            return View(iscilik);
        }
    }
}