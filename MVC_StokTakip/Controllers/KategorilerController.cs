using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;
using MVC_StokTakip.MyModel;

namespace MVC_StokTakip.Controllers
{
    
    [Authorize(Roles ="A")]

    public class KategorilerController : Controller
    {
        readonly arabamis_MVC_StokTakipEntities db = new arabamis_MVC_StokTakipEntities();
     
        public ActionResult Index(String ara)
        {
            var model = db.Kategoriler.ToList();
            if (!string.IsNullOrEmpty(ara))
            {
                model = model.Where(x => x.Kategori.ToLower().Contains(ara.ToLower())).ToList();
            }
            return View(model);
            
        }

        public ActionResult Ekle()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        public ActionResult Ekle2(Kategoriler p)
        {
            if (!ModelState.IsValid) return View("Ekle");
            db.Kategoriler.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GuncelleBilgiGetir(int id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var model = db.Kategoriler.Find(id);
            MyKategoriler k = new MyKategoriler
            {
                ID = model.ID,
                Kategori = model.Kategori,
                Aciklama = model.Aciklama,
                SayfaNo = model.SayfaNo
            };
            if (model == null)
                return HttpNotFound();

            return View(k);
        }

        public ActionResult Guncelle(Kategoriler p)
        {
            if (!ModelState.IsValid) return View("GuncelleBilgiGetir");
            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SilBilgiGetir(Kategoriler p) 
        {
            var model = db.Kategoriler.Find(p.ID);
            if (model == null) return HttpNotFound();
            return View(model);
        }
        public ActionResult Sil(Kategoriler p)
        {
            
            db.Entry(p).State = System.Data.Entity.EntityState.Deleted;      
            db.SaveChanges();

            return RedirectToAction("Index");
        }



        public ActionResult Urunler(int id)
        {
            var model = db.Urunler.Where(x => x.Kategoriler.ID == id).ToList();
            var kategori = db.Kategoriler.Where(x => x.ID == id).Select(x => x.Kategori).FirstOrDefault();
            ViewBag.viewkategori = kategori;
            return View(model);
        }
    }
}