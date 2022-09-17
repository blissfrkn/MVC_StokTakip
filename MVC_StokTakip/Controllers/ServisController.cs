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
        readonly arabamis_MVC_StokTakipEntities db = new arabamis_MVC_StokTakipEntities();
        // GET: Servis
        public ActionResult Index()
        {
            var degerler = db.Servis.ToList();
            return View(degerler);
        }
        public ActionResult ServisKayit()
        {

            List<SelectListItem> deger1 = (from x in db.AracMarka.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.Marka,
                                               Value = x.ID.ToString()
                                           }).ToList();

            ViewBag.dgr1 = deger1;
            var model = new MyUrunler();
            List<Birimler> birimList = db.Birimler.OrderBy(x => x.Birim).ToList();
            model.BirimListesi = (from x in birimList select new SelectListItem { Text = x.Birim, Value = x.ID.ToString() }).ToList();
            return View();
        }

        [HttpGet]
        public ActionResult ServisDetay(int id)
        {
            Class1 c = new Class1
            {

                Servis = db.Servis.Where(x => x.ID == id).FirstOrDefault(),
                ServisKalem = db.ServisKalem.Where(y => y.ServisID == id).FirstOrDefault(),
                ServisKalemList = db.ServisKalem.Where(z => z.ServisID == id).ToList()
            };

            List<SelectListItem> deger1 = (from x in db.Durumlar.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.Durum,
                                               Value = x.ID.ToString()

                                           }).ToList();
            ViewBag.dgr1 = deger1;
            List<SelectListItem> deger2 = (from x in db.Birimler.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.Birim,
                                               Value = x.ID.ToString()

                                           }).ToList();
            ViewBag.dgr2 = deger2;
            List<SelectListItem> deger3 = (from x in db.Tur.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.Tur1,
                                               Value = x.ID.ToString()

                                           }).ToList();
            ViewBag.dgr3 = deger3;

            return View(c);
        }

        [HttpPost]
        public JsonResult AracBilgileri(string id)
        {

            var list = db.Araclar.ToList();
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
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchCustomer(string term)
        {
            var list = db.Musteriler.ToList();
            var liste = list.Where(f => f.Ad.StartsWith(term));
            var autoSearch = from x in liste
                             select new
                             {
                                 id = x.ID,
                                 value = x.Ad + " " + x.Soyad
                             };
            return Json(autoSearch, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult MusteriBilgileri(string Id)
        {
            var list = db.Musteriler.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Ad.ToLower().Contains(Id.ToLower()))
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
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

            }
            return Json("", JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult MusteriBilgileri2(int id)
        {
            var list = db.Musteriler.ToList();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ID == id)
                {
                    var result = from l in list
                                 where l.ID == id
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
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AracEkle(Class1 c)
        {
            var arac = db.Araclar.FirstOrDefault(x => x.Plaka == c.Araclar.Plaka);
            if (arac == null)
            {
                c.Araclar.Tarih = DateTime.Now;
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
            db.SaveChanges();
            return Json("Araç Bilgileri Güncellendi.", JsonRequestBehavior.AllowGet);

        }

        public ActionResult MusteriEkle(Class1 c)
        {


            c.Musteriler.IsDelete = false;
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
            //servis.Tarih = DateTime.Now;
            servis.DurumID = c.Servis.DurumID;
            db.SaveChanges();

            return Json("Servis Bilgileri Güncellendi.", JsonRequestBehavior.AllowGet);
        }

        public JsonResult Search(string term)
        {
            var list = db.Urunler.Where(x => x.IsDelete == false).ToList();
            var liste = list.Where(f => f.StokKodu.ToLower() == term.ToLower() || f.UrunAdi.ToLower().Contains(term.ToLower()));
            var autoSearch = from x in liste
                             select new
                             {
                                 id = x.ID,
                                 value = x.StokKodu + " - " + x.UrunAdi

                             };
            return Json(autoSearch, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Search2(string term)
        {
            var list = db.İscilik.Where(x=> x.IsDelete == false).ToList();
            var liste = list.Where(f => f.İscilik1.ToLower().Contains(term.ToLower()));
            var autoSearch = from x in liste
                             select new
                             {
                                 value = x.İscilik1,
                                 sfiyat = x.SatisFiyat

                             };
            return Json(autoSearch, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult UrunGetir(string Id)
        {
            var list = db.DepoUrun.Where(x => x.Urunler.IsDelete == false && x.Depolar.IsDefault == true && (x.Urunler.BarkodNo == Id || x.Urunler.StokKodu == Id)).ToList();
            if (list.Count == 0)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Miktar > 0)
                    {
                        var result = from l in list
                                     select new
                                     {
                                         l.Urunler.ID,
                                         l.Urunler.KategoriID,
                                         l.Urunler.Kategoriler.Kategori,
                                         l.Urunler.UrunAdi,
                                         l.Urunler.SatisFiyati,
                                         l.Urunler.FormAciklama,
                                         l.Urunler.StokKodu,
                                         l.BirimID

                                     };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else if (list[i].Miktar <= 0)
                    {
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }

                }
            }

            return Json("", JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult UrunGetir2(string Id)
        {
            var list = db.DepoUrun.Where(x => x.Urunler.IsDelete == false).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Urunler.BarkodNo == Id && list[i].Miktar > 0)
                {
                    var result = from l in list
                                 where l.Urunler.BarkodNo == Id && l.Miktar > 0
                                 select new
                                 {
                                     l.Urunler.UrunAdi,
                                     l.Urunler.SatisFiyati,
                                     l.Miktar,

                                 };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }

            return Json("", JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult UrunGetir3(int Id)
        {
            var list = db.DepoUrun.Where(x => x.Urunler.IsDelete == false && x.UrunID == Id && x.Depolar.IsDefault == true).ToList();
            if (list.Count == 0)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
            else
            {
                for (int i = 0; i < list.Count;)
                {
                    if (list[i].Miktar > 0)
                    {
                        var result = from l in list
                                     where l.Miktar > 0
                                     select new
                                     {
                                         l.UrunID,
                                         l.Urunler.KategoriID,
                                         l.Urunler.Kategoriler.Kategori,
                                         l.Urunler.UrunAdi,
                                         l.Urunler.SatisFiyati,
                                         l.Urunler.FormAciklama,
                                         l.Urunler.StokKodu,
                                         l.Depolar.Adi,
                                         l.Miktar,
                                         l.BirimID
                                     };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else if (list[i].Miktar <= 0)
                    {
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                }

            }
            return Json("", JsonRequestBehavior.AllowGet);

        }


        public ActionResult SaveOrder(Kalemler[] kalemler)
        {

            foreach (var item in kalemler)
            {
                var model = db.Urunler.Where(x => x.ID == item.ID).FirstOrDefault();
                var model2 = db.ServisKalem.Where(x => x.UrunID == item.ID && x.ServisID == item.ServisID).FirstOrDefault();
                var model3 = db.Servis.Where(x => x.ID == item.ServisID).FirstOrDefault();


                if (model != null)
                {

                    if (model2 != null)
                    {
                        model3.YToplam = model3.YToplam - (model2.Miktari * model2.BirimFiyat);
                        model3.Toplam = model3.Toplam - (model2.Miktari * model2.BirimFiyat);
                        model3.GenelToplam = model3.GenelToplam - (model2.Miktari * model2.BirimFiyat);
                        model2.Miktari = model2.Miktari + item.Miktari;
                        model2.ToplamTutar = model2.Miktari * model2.BirimFiyat;
                        model3.YToplam = model3.YToplam + (model2.Miktari * model2.BirimFiyat);
                        model3.Toplam = model3.Toplam + (model2.Miktari * model2.BirimFiyat);
                        model3.GenelToplam = model3.GenelToplam + (model2.Miktari * model2.BirimFiyat);
                        db.SaveChanges();
                    }
                    else
                    {
                        ServisKalem sk = new ServisKalem
                        {
                            StokKodu = item.StokKodu,
                            UrunID = item.ID,
                            KategoriAd = item.KategoriAd,
                            ServisID = item.ServisID,
                            Aciklama = item.Aciklama,
                            TurID = item.TurID,
                            BirimID = item.BirimID,
                            BirimFiyat = item.BirimFiyat,
                            Miktari = item.Miktari
                        };
                        sk.ToplamTutar = sk.Miktari * sk.BirimFiyat;
                        db.ServisKalem.Add(sk);
                        db.SaveChanges();
                    }
                }
                else
                {
                    ServisKalem sk = new ServisKalem
                    {
                        StokKodu = item.StokKodu,
                        UrunID = 0,
                        KategoriAd = "",
                        ServisID = item.ServisID,
                        Aciklama = item.Aciklama,
                        TurID = item.TurID,
                        BirimID = item.BirimID,
                        BirimFiyat = item.BirimFiyat,
                        Miktari = item.Miktari
                    };
                    sk.ToplamTutar = sk.Miktari * sk.BirimFiyat;
                    db.ServisKalem.Add(sk);
                    db.SaveChanges();
                }
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
            Class1 c = new Class1
            {
                Servis = db.Servis.Where(x => x.ID == id).FirstOrDefault(),
                ServisKalem = db.ServisKalem.Where(y => y.ServisID == id).FirstOrDefault(),
                ServisKalemList = db.ServisKalem.Where(z => z.ServisID == id).ToList()
            };


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
        public ActionResult İskontoKaydet(int id, decimal iskontoP)
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

        [HttpPost]
        public ActionResult Servisİptal(int id)
        {
            var model = db.ServisKalem.Where(x => x.ServisID == id).ToList();
            var model1 = db.Servis.FirstOrDefault(x => x.ID == id);
            db.ServisKalem.RemoveRange(model);
            db.Servis.Remove(model1);
            db.SaveChanges();
            return Json(new { result = "Redirect", url = Url.Action("Index", "Servis") }, JsonRequestBehavior.AllowGet);
        }



        //BU KISIM GÜNCELLENECEK.

        public void DinamikMiktar1(int id, decimal miktari)
        {
            var model = db.ServisKalem.Find(id);
            var model1 = db.Servis.Find(model.ServisID);
            // Öncelikle veritabanında ne kadar miktar girilmişse servisin toplam fiyatlarından tek tek o fiyatları miktari kadar düşüyoruz.
            if (model.TurID == 1)
            {
                model1.YToplam = model1.YToplam - (model.BirimFiyat * model.Miktari);
            }
            else if (model.TurID == 2)
                model1.İToplam = model1.İToplam - (model.BirimFiyat * model.Miktari);
            else if (model.TurID == 3)
            {
                model1.MToplam = model1.MToplam - (model.BirimFiyat * model.Miktari);
            }
            else if (model.TurID == 4)
            {
                model1.RotBalans = model1.RotBalans - (model.BirimFiyat * model.Miktari);
            }

            model1.Toplam = model1.Toplam - (model.BirimFiyat * model.Miktari);
            model1.GenelToplam = model1.GenelToplam - (model.BirimFiyat * model.Miktari);
            // sonra miktar bilgisini güncelledikten sonra toplam tutarı hesaplayıp tek tek servis tablosundaki fiyatlara geri ekliyoruz ve kaydediyoruz.
            model.Miktari = miktari;
            model.ToplamTutar = model.BirimFiyat * model.Miktari;

            if (model.TurID == 1)
            {
                model1.YToplam = model1.YToplam + (model.BirimFiyat * model.Miktari);

            }
            else if (model.TurID == 2)
            {
                model1.İToplam = model1.İToplam + (model.BirimFiyat * model.Miktari);
            }
            else if (model.TurID == 3)
                model1.MToplam = model1.MToplam + (model.BirimFiyat * model.Miktari);
            else if (model.TurID == 4)
                model1.RotBalans = model1.RotBalans + (model.BirimFiyat * model.Miktari);

            model1.Toplam = model1.Toplam + (model.BirimFiyat * model.Miktari);
            model1.GenelToplam = model1.GenelToplam + (model.BirimFiyat * model.Miktari);
            db.SaveChanges();
        }

        public void DinamikMiktar2(int id, decimal satisfiyat)
        {
            var model = db.ServisKalem.Find(id);
            var model1 = db.Servis.Find(model.ServisID);
            if (model.TurID == 1)
            {
                model1.YToplam = model1.YToplam - (model.BirimFiyat * model.Miktari);
            }
            else if (model.TurID == 2)
                model1.İToplam = model1.İToplam - (model.BirimFiyat * model.Miktari);
            else if (model.TurID == 3)
            {
                model1.MToplam = model1.MToplam - (model.BirimFiyat * model.Miktari);
            }
            else if (model.TurID == 4)
            {
                model1.RotBalans = model1.RotBalans - (model.BirimFiyat * model.Miktari);
            }

            model1.Toplam = model1.Toplam - (model.BirimFiyat * model.Miktari);
            model1.GenelToplam = model1.GenelToplam - (model.BirimFiyat * model.Miktari);
            model.BirimFiyat = satisfiyat;
            model.ToplamTutar = model.BirimFiyat * model.Miktari;

            if (model.TurID == 1)
            {
                model1.YToplam = model1.YToplam + model.ToplamTutar;

            }
            else if (model.TurID == 2)
            {
                model1.İToplam = model1.İToplam + model.ToplamTutar;
            }
            else if (model.TurID == 3)
                model1.MToplam = model1.MToplam + model.ToplamTutar;
            else if (model.TurID == 4)
                model1.RotBalans = model1.RotBalans + model.ToplamTutar;

            model1.Toplam = model1.Toplam + model.ToplamTutar;
            model1.GenelToplam = model1.GenelToplam + model.ToplamTutar;
            db.SaveChanges();
        }

        public void DinamikAciklama(int id, string aciklama)
        {
            var model = db.ServisKalem.Find(id);
            if (model.TurID == 1)
            {
                model.Aciklama = aciklama;
            }
            else if (model.TurID == 2)
            {
                model.Aciklama = aciklama;
                model.StokKodu = aciklama;
            }
            else if (model.TurID == 3)
            {
                model.Aciklama = aciklama;
                model.StokKodu = aciklama;
            }
            else if (model.TurID == 4)
            {
                model.Aciklama = aciklama;
                model.StokKodu = aciklama;
            }

            db.SaveChanges();
        }

        public ActionResult HizliFiyat()
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
        public ActionResult FiyatEkle(Class1 c, Kalemler[] kalemler)
        {

            return View();
        }

        public ActionResult KalemEkle(Kalemler[] kalemler)
        {
            return View();
        }

        public ActionResult getMarka()
        {
            return Json(db.AracMarka.Select(x => new
            {
                x.ID,
                x.Marka
            }).ToList(), JsonRequestBehavior.AllowGet);
        }
    }

}
