﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;
using MVC_StokTakip.MyModel;

namespace MVC_StokTakip.Controllers
{
    [Authorize(Roles = "A")]

    public class SepetController : Controller
    {
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        public ActionResult Index(decimal? Tutar)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var kullanici = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == kullaniciadi);
                var model = db.Sepet.Where(x => x.KullaniciID == kullanici.ID).ToList();
                var kid = db.Sepet.FirstOrDefault(x => x.KullaniciID == kullanici.ID);
                if (model != null)
                {
                    if (kid == null)
                    {
                        ViewBag.Tutar = "Sepetinizde Ürün Bulunmuyor.";
                    }
                    else if (kid != null)
                    {
                        Tutar = db.Sepet.Where(x => x.KullaniciID == kid.KullaniciID).Sum(x => x.ToplamFiyat);
                        ViewBag.Tutar = "Toplam tutar=" + Tutar + "TL";
                    }
                    return View(model);
                }

            }
            return HttpNotFound();
        }

        public ActionResult SepeteEkle(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var model = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == kullaniciadi);
                var u = db.Urunler.Find(id);
                var sepet = db.Sepet.FirstOrDefault(x => x.KullaniciID == model.ID && x.UrunID == id);
                if (model != null)
                {
                    if (sepet != null) // Sepete eklenen ürün sepette varsa bu işlem çalıştırılır
                    {
                        sepet.Miktari++;
                        sepet.ToplamFiyat = u.SatisFiyati * sepet.Miktari;
                        db.SaveChanges();
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                    if (u.Miktari <=0)
                    {
                        return Json("1", JsonRequestBehavior.AllowGet);
                    }
                    var s = new Sepet // Sepete eklenen ürün sepette yoksa yeni sepet girişi yapılır.
                    {
                        KullaniciID = model.ID,
                        UrunID = u.ID,
                        Miktari = 1,
                        BirimFiyati = u.SatisFiyati,
                        ToplamFiyat = u.SatisFiyati,
                        Tarih = DateTime.Now,
                        Saat = DateTime.Now
                    };
                   
                    db.Entry(s).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("" , JsonRequestBehavior.AllowGet);
        }

        public ActionResult SepeteEkle2(int Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var model = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == kullaniciadi);
                var u = db.Urunler.Find(Id);
                var sepet = db.Sepet.FirstOrDefault(x => x.KullaniciID == model.ID && x.UrunID == Id);
                if (model != null)
                {
                    if (sepet != null)
                    {
                        sepet.Miktari++;
                        sepet.ToplamFiyat = u.SatisFiyati * sepet.Miktari;
                        db.SaveChanges();
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                    if (u.Miktari <= 0)
                    {
                        return Json("1", JsonRequestBehavior.AllowGet);
                    }
                    var s = new Sepet
                    {
                        KullaniciID = model.ID,
                        UrunID = u.ID,
                        Miktari = 1,
                        BirimFiyati = u.SatisFiyati,
                        ToplamFiyat = u.SatisFiyati,
                        Tarih = DateTime.Now,
                        Saat = DateTime.Now
                    };
                    db.Entry(s).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
            return HttpNotFound();
        }

        public ActionResult TotalCount(int? count)
        {
            if (User.Identity.IsAuthenticated)
            {
                var model = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);
                count = db.Sepet.Where(x => x.KullaniciID == model.ID).Count();
                ViewBag.Count = count;
                if (count == 0)
                {
                    ViewBag.Count = "";
                }
                return PartialView();
            }
            return HttpNotFound();
        }

        public ActionResult Arttir(int id)
        {
            var model = db.Sepet.Find(id);
            model.Miktari++;
            model.ToplamFiyat = model.BirimFiyati * model.Miktari;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Azalt(int id)
        {
            var model = db.Sepet.Find(id);
            if (model.Miktari == 1)
            {
                db.Sepet.Remove(model);
                db.SaveChanges();
            }
            model.Miktari--;
            model.ToplamFiyat = model.BirimFiyati * model.Miktari;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void DinamikMiktar(int id, decimal miktari)
        {
            var model = db.Sepet.Find(id);
            model.Miktari = miktari;
            model.ToplamFiyat = model.BirimFiyati * model.Miktari;
            db.SaveChanges();
        }

        public ActionResult Sil(int id)
        {
            var model = db.Sepet.Find(id);
            db.Sepet.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        public ActionResult HepsiniSil()
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var model = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi.Equals(kullaniciadi));
                var sil = db.Sepet.Where(x => x.KullaniciID.Equals(model.ID));
                db.Sepet.RemoveRange(sil);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }
    

    }
}