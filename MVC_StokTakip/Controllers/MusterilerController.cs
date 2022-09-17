using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;

namespace MVC_StokTakip.Controllers
{
    public class MusterilerController : Controller
    {
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        // GET: Musteriler
        public ActionResult Index()
        {
            var degerler = db.Musteriler.ToList();
            return View(degerler);
        }

        public PartialViewResult Araclar(int id)
        {
            var arac = db.Araclar.Where(x => x.MusteriID == id).ToList();
            return PartialView(arac);
        }

        [HttpGet]
        public ActionResult Ekle()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult Ekle(Musteriler m)
        {
            db.Musteriler.Add(m);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult MusteriGetir(int id)
        {
            var musteri = db.Musteriler.Find(id);
            return View("MusteriGetir", musteri);
        }

        public ActionResult Guncelle(Musteriler m)
        {
            var musteri = db.Musteriler.Find(m.ID);
            musteri.Ad = m.Ad;
            musteri.Soyad = m.Soyad;
            musteri.VergiDairesi = m.VergiDairesi;
            musteri.VergiNo = m.VergiNo;
            musteri.Adres = m.Adres;
            musteri.Email = m.Email;
            musteri.Telefon = m.Telefon;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Detay(int id)
        {
            Class1 c = new Class1();
            c.Musteriler = db.Musteriler.Find(id);
            c.Tamiratlars = db.Tamiratlar.Where(x => x.Araclar.Musteriler.ID == id).ToList();
            c.TamiratKalems = db.TamiratKalem.Where(x => x.Tamiratlar.Araclar.Musteriler.ID == id).ToList();
            c.Araclars = db.Araclar.Where(x => x.Musteriler.ID == id).ToList();

            
            return View(c);
        }


    }
}