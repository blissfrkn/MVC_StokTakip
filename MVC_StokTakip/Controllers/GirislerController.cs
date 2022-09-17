using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;

namespace MVC_StokTakip.Controllers
{
    [Authorize(Roles = "A")]
    public class GirislerController : Controller
    {
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        public ActionResult Index(string ara,string ara2)
        {
            var model = db.Girisler.ToList();
            if (!string.IsNullOrEmpty(ara))
            {
                model = model.Where(x => x.Tarih.ToString().Contains(ara)).ToList();

            }

            if (!string.IsNullOrEmpty(ara2))
            {
                model = model.Where(x => x.BarkodNo.ToLower().Contains(ara2.ToLower())).ToList();

            }

            return View(model);
        }

        public ActionResult StokEkle(int id)
        {
            var model = db.Sepet.FirstOrDefault(x => x.ID == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult StokEkle2(int id)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var model = db.Sepet.FirstOrDefault(x => x.ID == id);
                    var urun = db.Urunler.FirstOrDefault(x => x.ID == model.UrunID);
                    urun.Miktari = urun.Miktari + model.Miktari;

                    var giris = new Girisler
                    {
                        KullaniciID = model.KullaniciID,
                        UrunID = model.UrunID,
                        SepetID = model.ID,
                        BarkodNo = model.Urunler.BarkodNo,
                        //StokKodu = model.Urunler.StokKodu,
                        OemKod = model.Urunler.OemKod,
                        BirimFiyati = model.BirimFiyati,
                        Miktari = model.Miktari,
                        ToplamFiyati = model.ToplamFiyat,
                        KDV = model.Urunler.KDV,
                        BirimID = model.Urunler.BirimID,
                        Tarih = DateTime.Now,
                        Saat = DateTime.Now
                    };
                    db.Sepet.Remove(model);
                    db.Girisler.Add(giris);
                    db.SaveChanges();
                    ViewBag.islem = "Stoğa Ekleme İşlemi başarılı bir şekilde gerçekleşmiştir.";


                }
            }
            catch (Exception)
            {

                ViewBag.islem = "Stoğa Ekleme İşlemi başarısız.";

            }

            return View("islem");




        }

        public ActionResult HepsiniStokEkle(decimal? Tutar)
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

        [HttpPost]
        public ActionResult HepsiniStokEkle2()
        {
            var username = User.Identity.Name;
            var kullanici = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == username);
            var model = db.Sepet.Where(x => x.KullaniciID == kullanici.ID).ToList();
            int row = 0;
            foreach (var item in model)
            {
                var giris = new Girisler
                {
                    KullaniciID = model[row].KullaniciID,
                    UrunID = model[row].UrunID,
                    SepetID = model[row].ID,
                    BarkodNo = model[row].Urunler.BarkodNo,
                    //StokKodu = model[row].Urunler.StokKodu,
                    OemKod = model[row].Urunler.OemKod,
                    BirimFiyati = model[row].BirimFiyati,
                    Miktari = model[row].Miktari,
                    ToplamFiyati = model[row].ToplamFiyat,
                    KDV = model[row].Urunler.KDV,
                    BirimID = model[row].Urunler.BirimID,
                    Tarih = DateTime.Now,
                    Saat = DateTime.Now


                };
                db.Girisler.Add(giris);
                row++;
            }
            foreach (var item in model)
            {
                var urun = db.Urunler.FirstOrDefault(x => x.ID == item.UrunID);
                if (urun != null)
                {
                    urun.Miktari = urun.Miktari + item.Miktari;

                }
            }

            db.Sepet.RemoveRange(model);
            db.SaveChanges();
            return RedirectToAction("Index", "Sepet");
        }
    }
}