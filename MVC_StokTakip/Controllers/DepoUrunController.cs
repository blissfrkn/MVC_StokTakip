using MVC_StokTakip.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_StokTakip.Controllers
{
    public class DepoUrunController : Controller
    {
        readonly arabamis_MVC_StokTakipEntities db = new arabamis_MVC_StokTakipEntities();
        // GET: DepoUrun
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DepoTransfer()
        {

            List<SelectListItem> deger2 = (from x in db.Birimler.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.Birim,
                                               Value = x.ID.ToString()

                                           }).ToList();
            ViewBag.dgr2 = deger2;

            List<SelectListItem> deger1 = (from x in db.Depolar.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.Adi,
                                               Value = x.ID.ToString()

                                           }).ToList();


            deger1.Insert(0, (new SelectListItem { Text = "--Depo Seçiniz", Value = "0" }));
            ViewBag.dgr1 = deger1;

            return View();
        }

        public ActionResult getBirim()
        {
            return Json(db.Birimler.Select(x => new
            {
               x.ID,
               x.Birim
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTur()
        {
            return Json(db.Tur.Select(x => new
            {
                x.ID,
                x.Tur1
            }).ToList(), JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult UrunGetir(int Id)
        {
            var list = db.DepoUrun.Where(x => x.Urunler.IsDelete == false && x.ID == Id).ToList();
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
                                         l.ID,
                                         l.Urunler.UrunAdi,
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


        public JsonResult Search(string term ,int depo)
        {       
                var list = db.DepoUrun.Where(x => x.Urunler.IsDelete == false && x.DepoID == depo).ToList();
                var liste = list.Where(f => f.Urunler.StokKodu.ToLower() == term.ToLower() || f.Urunler.UrunAdi.ToLower().Contains(term.ToLower()));
                var autoSearch = from x in liste
                                 select new
                                 {
                                     id = x.ID,
                                     value = x.Urunler.StokKodu + " - " + x.Urunler.UrunAdi
                                 };
                return Json(autoSearch, JsonRequestBehavior.AllowGet);                         
        }

        public ActionResult dinamikMiktar(int ID , decimal miktar)
        {
            var urun = db.DepoUrun.Where(x => x.ID == ID).FirstOrDefault();
            if (urun.Miktar < miktar || miktar < 0)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult transferET(TransferUrun[] transferUruns)
        {
            foreach (var item in transferUruns)
            {
                var model = db.DepoUrun.Where(x => x.ID == item.ID).FirstOrDefault(); // Depoda gelen ürünün DepoUrunId' sinde bir ürün var mı kontrol edilir ve model değişkenine aktarılır.

                if (model != null) // eğer boş değil ise işlemler başlanır.
                {
                    var model2 = db.DepoUrun.Where(x => x.UrunID == model.UrunID && x.DepoID == item.GirisDepoID).FirstOrDefault(); // burada ürünün giriş yapılacağı depoda daha önceden var mı yok mu kontrol edilir
                    if (model2 == null) // eğer ürün giriş yapılacak depoda yok ise o depoda kaydı açılır ve miktarı gelen miktar ile girilir.
                    {
                        DepoUrun d = new DepoUrun
                        {
                            UrunID = model.UrunID,
                            DepoID = item.GirisDepoID,
                            Miktar = item.Miktar,
                            BirimID = model.BirimID
                        };
                        model.Miktar -= item.Miktar; // Düşülecek olan depodan miktarı düşeriz.
                        db.DepoUrun.Add(d); 

                        var satis = new Satislar
                        {
                            KullaniciID = 1,
                            UrunID = model.UrunID,
                            SepetID = 0,
                            Durum = "Depo Transfer",
                            DepoID = model.DepoID,
                            BarkodNo = model.Urunler.BarkodNo,
                            StokKodu = model.Urunler.StokKodu,
                            OemKod = model.Urunler.OemKod,
                            BirimFiyati = model.Urunler.SatisFiyati,
                            Miktari = item.Miktar,
                            ToplamFiyati = model.Urunler.SatisFiyati * item.Miktar,
                            KDV = model.Urunler.KDV,
                            BirimID = model.BirimID,
                            Tarih = DateTime.Now,
                            Saat = DateTime.Now
                        };
                        db.Satislar.Add(satis); // düşüş logunu tutarız.

                        var giris = new Girisler
                        {
                            KullaniciID = 1,
                            UrunID = model.UrunID,
                            SepetID = 0,
                            Durum = "Depo Transfer",
                            DepoID = item.GirisDepoID,
                            BarkodNo = model.Urunler.BarkodNo,
                            StokKodu = model.Urunler.StokKodu,
                            OemKod = model.Urunler.OemKod,
                            BirimFiyati = model.Urunler.SatisFiyati,
                            Miktari = item.Miktar,
                            ToplamFiyati = model.Urunler.SatisFiyati * item.Miktar,
                            KDV = model.Urunler.KDV,
                            BirimID = model.BirimID,
                            Tarih = DateTime.Now,
                            Saat = DateTime.Now
                        };
                        db.Girisler.Add(giris); // giriş logunu tutarız

                    }
                    else //  eğer giriş yapılacak depoda kaydı var ise bu kodlar işletilir.
                    {
                        model.Miktar -= item.Miktar; // düşüş yapılack depodan düşüş yapılır
                        model2.Miktar += item.Miktar; //giriş yapılacak depoya giriş yapılır.

                        var satis = new Satislar
                        {
                            KullaniciID = 1,
                            UrunID = model.UrunID,
                            SepetID = 0,
                            Durum = "Depo Transfer",
                            DepoID = model.DepoID,
                            BarkodNo = model.Urunler.BarkodNo,
                            StokKodu = model.Urunler.StokKodu,
                            OemKod = model.Urunler.OemKod,
                            BirimFiyati = model.Urunler.SatisFiyati,
                            Miktari = item.Miktar,
                            ToplamFiyati = model.Urunler.SatisFiyati * item.Miktar,
                            KDV = model.Urunler.KDV,
                            BirimID = model.BirimID,
                            Tarih = DateTime.Now,
                            Saat = DateTime.Now
                        };
                        db.Satislar.Add(satis);

                        var giris = new Girisler
                        {
                            KullaniciID = 1,
                            UrunID = model.UrunID,
                            SepetID = 0,
                            Durum = "Depo Transfer",
                            DepoID = item.GirisDepoID,
                            BarkodNo = model.Urunler.BarkodNo,
                            StokKodu = model.Urunler.StokKodu,
                            OemKod = model.Urunler.OemKod,
                            BirimFiyati = model.Urunler.SatisFiyati,
                            Miktari = item.Miktar,
                            ToplamFiyati = model.Urunler.SatisFiyati * item.Miktar,
                            KDV = model.Urunler.KDV,
                            BirimID = model.BirimID,
                            Tarih = DateTime.Now,
                            Saat = DateTime.Now
                        };
                        db.Girisler.Add(giris);

                    }
                }
                else
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
                }
            }
            db.SaveChanges();
            return Json("0", JsonRequestBehavior.AllowGet);
        }


    }


}