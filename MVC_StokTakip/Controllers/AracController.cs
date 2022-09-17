using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;

namespace MVC_StokTakip.Controllers
{
    [Authorize(Roles = "A")]
    public class AracController : Controller
    {
        readonly arabamis_MVC_StokTakipEntities db = new arabamis_MVC_StokTakipEntities();
        // GET: Arac
        public ActionResult Index()
        {


            var degerler = db.Araclar.ToList();          
            return View(degerler);
        }


        [HttpGet]
        public ActionResult Ekle()
        {
            List<SelectListItem> deger1 = (from x in db.AracMarka.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.Marka,
                                               Value = x.ID.ToString()
                                           }).ToList();
            ViewBag.dgr1 = deger1;
            return View();
        }

        [HttpPost]
        public ActionResult Ekle(Araclar a)
        {
            a.Tarih = DateTime.Now;
            db.Araclar.Add(a);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AracGetir(int id)
        {
            var arac = db.Araclar.Find(id);
            List<SelectListItem> deger1 = (from x in db.AracMarka.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.Marka,
                                               Value = x.ID.ToString()
                                           }).ToList();
            ViewBag.dgr1 = deger1;
            return View("AracGetir", arac);
        }

        public ActionResult Guncelle(Araclar a)
        {
            var arac = db.Araclar.Find(a.ID);
            arac.Plaka = a.Plaka;
            arac.MarkaID = a.MarkaID;
            arac.Model = a.Model;
            arac.Renk = a.Renk;
            arac.SasiNo = a.SasiNo;
            List<SelectListItem> deger1 = (from x in db.AracMarka.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.Marka,
                                               Value = x.ID.ToString()
                                           }).ToList();
            ViewBag.dgr1 = deger1;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AracDetay(int id)
        {
            var degerler = db.Tamiratlar.Where(x => x.AracID == id).ToList();
            return View(degerler);
        }

        public ActionResult Detay(int id)
        {
            Class1 c = new Class1
            {
                Araclar = db.Araclar.Find(id),
                Tamiratlars = db.Tamiratlar.Where(x => x.Araclar.ID == id).ToList(),
                TamiratKalems = db.TamiratKalem.Where(x => x.Tamiratlar.Araclar.ID == id).ToList()
            };
            return View(c);
        }

    }
}