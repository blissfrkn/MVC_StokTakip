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
        readonly arabamis_MVC_StokTakipEntities db = new arabamis_MVC_StokTakipEntities();
        // GET: Musteriler
        public ActionResult Index()
        {
            var degerler = db.Musteriler.Where(x=> x.IsDelete == false).ToList();
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
            m.IsDelete = false;
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
            Class1 c = new Class1
            {
                Musteriler = db.Musteriler.Find(id),
                Tamiratlars = db.Tamiratlar.Where(x => x.Araclar.Musteriler.ID == id).ToList(),
                TamiratKalems = db.TamiratKalem.Where(x => x.Tamiratlar.Araclar.Musteriler.ID == id).ToList(),
                Araclars = db.Araclar.Where(x => x.Musteriler.ID == id).ToList()
            };


            return View(c);
        }
        public ActionResult Sil(int id)
        {
            var model = db.Musteriler.FirstOrDefault(x => x.ID == id);
            model.IsDelete = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteCustomers()
        {
            var model = db.Musteriler.Where(x => x.IsDelete == true).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult GeriYukle(int id)
        {
            var model = db.Musteriler.Find(id);
            model.IsDelete = false;
            db.SaveChanges();
            return Json("Müşteri başarıyla geri yüklendi", JsonRequestBehavior.AllowGet);
        }

    }
}