using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;
using Rotativa;
using Rotativa.Options;
using MVC_StokTakip.MyModel;

namespace MVC_StokTakip.Controllers
{

    public class ServisController : Controller
    {
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        // GET: Servis
        public ActionResult Index()
        {
            var degerler = db.Servis.ToList();
            return View(degerler);
        }

        [HttpGet]
        public ActionResult ServisDetay(int id)
        {
            Class1 c = new Class1();
            c.Servis = db.Servis.Where(x => x.ID == id).FirstOrDefault();
            c.ServisKalem = db.ServisKalem.Where(y => y.ServisID == id).FirstOrDefault();
            c.ServisKalemList = db.ServisKalem.Where(z => z.ServisID == id).ToList();
            List<SelectListItem> deger1 = (from x in db.Durumlar.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.Durum,
                                               Value = x.ID.ToString()

                                           }).ToList();
            ViewBag.dgr1 = deger1;
            return View(c);
        }

        [HttpPost]
        public JsonResult AracBilgileri(string id)
        {

            var list = db.Araclar.ToList();

            //if (list != null)
            //{
            //    db.Configuration.ProxyCreationEnabled = false;
            //    var model1 = list.FirstOrDefault();
            //    return Json(model1, JsonRequestBehavior.AllowGet);
            //}
            if (list.Count == 0)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Plaka == id && list[i].MusteriID != null)
                {
                    var result = from l in list
                                 where l.Plaka == id
                                 select new
                                 {
                                     l.ID,
                                     l.Plaka,
                                     l.MarkaID,
                                     l.Model,
                                     l.SasiNo,
                                     l.Renk,
                                     l.MusteriID,
                                     l.Musteriler.Ad,
                                     l.Musteriler.Soyad,
                                     l.Musteriler.Telefon,
                                     l.Musteriler.Email,
                                     l.Musteriler.Adres,
                                     l.Musteriler.VergiDairesi,
                                     l.Musteriler.VergiNo
                                 };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else if (list[i].Plaka == id && list[i].MusteriID == null)
                {
                    var result = from l in list
                                 where l.Plaka == id
                                 select new
                                 {
                                     l.ID,
                                     l.Plaka,
                                     l.MarkaID,
                                     l.Model,
                                     l.SasiNo,
                                     l.Renk
                                 };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Plaka != id)
                {
                    //db.Configuration.ProxyCreationEnabled = false;
                    //var model = list.FirstOrDefault();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

            }

            //var result = db.Araclar.Where(x => x.Plaka == id).Select(s => new { s.ID, s.Plaka, s.MarkaID, s.Model, s.Musteriler.Ad, s.Musteriler.Soyad });

            //db.Configuration.ProxyCreationEnabled = false;
            //var model = db.Araclar.Where(x => x.Plaka == id).FirstOrDefault();

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MusteriBilgileri(string Id)
        {
            var list = db.Musteriler.ToList();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Ad == Id)
                {
                    var result = from l in list
                                 where l.Ad.ToLower().Contains(Id.ToLower())
                                 select new
                                 {
                                     l.ID,
                                     l.Ad,
                                     l.Soyad,
                                     l.Telefon,
                                     l.Adres,
                                     l.VergiDairesi,
                                     l.VergiNo,
                                     l.Email
                                 };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Ad != Id)
                {
                    //db.Configuration.ProxyCreationEnabled = false;
                    //var model = list.FirstOrDefault();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

            }

            //var result = db.Araclar.Where(x => x.Plaka == id).Select(s => new { s.ID, s.Plaka, s.MarkaID, s.Model, s.Musteriler.Ad, s.Musteriler.Soyad });

            //db.Configuration.ProxyCreationEnabled = false;
            //var model = db.Araclar.Where(x => x.Plaka == id).FirstOrDefault();

            return Json("", JsonRequestBehavior.AllowGet);

            //var result = db.Araclar.Where(x => x.Plaka == id).Select(s => new { s.ID, s.Plaka, s.MarkaID, s.Model, s.Musteriler.Ad, s.Musteriler.Soyad });
            //db.Configuration.ProxyCreationEnabled = false;
            //var model = db.Musteriler.Where(x => x.Ad == Id).FirstOrDefault();
        }

        [HttpGet]
        public ActionResult ServisKayit()
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

        public ActionResult AracEkle(Class1 c)
        {

            var arac = db.Araclar.FirstOrDefault(x => x.Plaka == c.Araclar.Plaka);
            if (arac == null)
            {
                db.Araclar.Add(c.Araclar);
                db.SaveChanges();
                var result = c.Araclar.ID;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }


        }

        public ActionResult AracGuncelle(Class1 c)
        {
            List<SelectListItem> deger1 = (from x in db.AracMarka.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.Marka,
                                               Value = x.ID.ToString()
                                           }).ToList();
            ViewBag.dgr1 = deger1;

            var arac = db.Araclar.FirstOrDefault(x => x.ID == c.Araclar.ID);

            arac.MarkaID = c.Araclar.MarkaID;
            arac.Model = c.Araclar.Model;
            arac.SasiNo = c.Araclar.SasiNo;
            arac.Renk = c.Araclar.Renk;
            //var deger2 = (from y in db.Musteriler.ToList() select new { Text = y.Ad, Value = y.ID.ToString() }).ToList();
            //ViewBag.dgr2 = deger2;
            //var servis = db.Servis.FirstOrDefault(x => x.ID == c.Servis.ID);
            //servis.Araclar.Plaka = c.Servis.Araclar.Plaka;
            //servis.Araclar.MarkaID = c.Servis.Araclar.MarkaID;
            //servis.Araclar.Model = c.Servis.Araclar.Model;
            //servis.Araclar.Musteriler.Ad = c.Servis.Araclar.Musteriler.Ad;
            //servis.Araclar.Musteriler.Soyad = c.Servis.Araclar.Musteriler.Soyad;
            db.SaveChanges();

            return Json("Araç Bilgileri Güncellendi.", JsonRequestBehavior.AllowGet);

        }

        public ActionResult MusteriEkle(Class1 c)
        {


            db.Musteriler.Add(c.Musteriler);
            db.SaveChanges();
            var result = c.Musteriler.ID;
            return Json(result, JsonRequestBehavior.AllowGet);


        }

        public ActionResult MusteriGuncelle(Class1 c)
        {

            var musteri = db.Musteriler.FirstOrDefault(x => x.ID == c.Musteriler.ID);
            musteri.Ad = c.Musteriler.Ad;
            musteri.Soyad = c.Musteriler.Soyad;
            musteri.Telefon = c.Musteriler.Telefon;
            musteri.Adres = c.Musteriler.Adres;
            musteri.Email = c.Musteriler.Email;
            musteri.VergiDairesi = c.Musteriler.VergiDairesi;
            musteri.VergiNo = c.Musteriler.VergiNo;
            db.SaveChanges();

            return Json("Müşteri Bilgileri Güncellendi.", JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult ServiseAl(Class1 c, int siteID)
        {

            var arac = db.Araclar.FirstOrDefault(x => x.ID == c.Servis.AracID);
            var servis = db.Servis.FirstOrDefault(x => x.AracID == c.Servis.AracID);

            if (servis == null)
            {
                arac.MusteriID = siteID;
                c.Servis.DurumID = 1;
                c.Servis.Toplam = 0;
                c.Servis.GenelToplam = 0;
                c.Servis.İskonto = 0;
                c.Servis.YToplam = 0;
                c.Servis.İToplam = 0;
                c.Servis.MToplam = 0;
                c.Servis.RotBalans = 0;
                c.Servis.Tarih = DateTime.Now;
                db.Servis.Add(c.Servis);
                db.SaveChanges();
                var result = c.Servis.ID;
                db.SaveChanges();

                return Json(new { result = "Redirect", url = Url.Action("Index", "Servis") }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = "Redirect1", url = Url.Action("Index", "Servis") }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ServisGuncelle(Class1 c)
        {
            List<SelectListItem> deger1 = (from x in db.Durumlar.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.Durum,
                                               Value = x.ID.ToString()

                                           }).ToList();
            ViewBag.dgr1 = deger1;
            var servis = db.Servis.FirstOrDefault(x => x.ID == c.Servis.ID);
            servis.Km = c.Servis.Km;
            servis.Not1 = c.Servis.Not1;
            servis.Not2 = c.Servis.Not2;
            servis.Not3 = c.Servis.Not3;
            servis.Tarih = DateTime.Now;
            servis.DurumID = c.Servis.DurumID;
            db.SaveChanges();

            return Json("Servis Bilgileri Güncellendi.", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult UrunGetir(string Id)
        {
            var list = db.Urunler.Where(x=>x.IsDelete==false).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].BarkodNo == Id && list[i].Miktari > 0)
                {
                    var result = from l in list
                                 where l.BarkodNo == Id && l.Miktari > 0
                                 select new
                                 {
                                     l.UrunAdi,                                  
                                     l.SatisFiyati
                                 };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].BarkodNo == Id || list[i].Miktari <= 0 || list[i].BarkodNo != Id)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

            }

            //db.Configuration.ProxyCreationEnabled = false;
            //var model = db.Urunler.FirstOrDefault();
            //var result = db.Urunler.Where(x => x.BarkodNo == Id && x.Miktari>0).FirstOrDefault();
            return Json("", JsonRequestBehavior.AllowGet);

        }


        public ActionResult SaveOrder(ServisKalem[] kalemler)
        {
            foreach (var item in kalemler)
            {
                var model = db.Urunler.Where(x => x.UrunAdi == item.Aciklama).FirstOrDefault();

                ServisKalem sk = new ServisKalem();
                sk.ServisID = item.ServisID;
                sk.Aciklama = item.Aciklama;
                sk.TurID = item.TurID;
                sk.BirimID = item.BirimID;
                sk.BirimFiyat = item.BirimFiyat;
                sk.Miktari = item.Miktari;
                sk.ToplamTutar = sk.Miktari * sk.BirimFiyat;
                db.ServisKalem.Add(sk);
                db.SaveChanges();
            }
            return Json("İşlem Başarılı", JsonRequestBehavior.AllowGet);
        }

        public ActionResult KalemSil(int id)
        {
            var model = db.ServisKalem.FirstOrDefault(x => x.ID == id);
            db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return Json("Başarılı bir şekilde silinmiştir.", JsonRequestBehavior.AllowGet);
        }


        public ActionResult TotalCount(int? count)
        {


            count = db.Servis.Count();
            ViewBag.Count = count;
            if (count == 0)
            {
                ViewBag.Count = "";
            }
            return PartialView();

        }

        public ActionResult PrintReport(int id)
        {
            Class1 c = new Class1();

            c.Servis = db.Servis.Where(x => x.ID == id).FirstOrDefault();
            c.ServisKalem = db.ServisKalem.Where(y => y.ServisID == id).FirstOrDefault();
            c.ServisKalemList = db.ServisKalem.Where(z => z.ServisID == id).ToList();


            return View(c);
            //return new PartialViewAsPdf("PrintReport", c)
            //{

            //    FileName = "Fiyat_Teklif_Formu.pdf"
            //};
        }


        public ActionResult Arttir1(int id)
        {
            var model = db.ServisKalem.Find(id);
            var servis = model.ServisID;
            model.Miktari++;
            model.ToplamTutar = model.BirimFiyat * model.Miktari;
            db.SaveChanges();
            return RedirectToAction("ServisDetay", new { id = servis });
        }

        public ActionResult Azalt1(int id)
        {
            var model = db.ServisKalem.Find(id);
            var servis = model.ServisID;
            if (model.Miktari == 1)
            {
                db.ServisKalem.Remove(model);
                db.SaveChanges();
            }
            model.Miktari--;
            model.ToplamTutar = model.BirimFiyat * model.Miktari;
            db.SaveChanges();
            return RedirectToAction("ServisDetay", new { id = servis });
        }

        [HttpPost]
        public ActionResult İskontoKaydet(int id, int iskontoP)
        {
            var model = db.Servis.FirstOrDefault(x => x.ID == id);
            model.İskonto = iskontoP;
            if (model.İskonto == 0)
            {
                model.GenelToplam = model.Toplam;
            }
            else
            {
                model.GenelToplam = model.Toplam - model.İskonto;
            }
            db.SaveChanges();
            var result = from l in db.Servis
                         select new
                         {
                             l.İskonto,
                             l.Toplam,
                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }








    }
}
