using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;

namespace MVC_StokTakip.Controllers
{
    public class TamiratlarController : Controller
    {
        readonly arabamis_MVC_StokTakipEntities db = new arabamis_MVC_StokTakipEntities();
        // GET: Tamiratlar
        public ActionResult Index()
        {
            var degerler = db.Tamiratlar.ToList();
            return View(degerler);
        }

        [HttpPost]
        public ActionResult TamiratAl(int Id)
        {

            var servis = db.Servis.FirstOrDefault(x => x.ID == Id);
            var servisk = db.ServisKalem.Where(y => y.ServisID == Id).ToList();
            if (servisk.Count == 0)// eğer serviskalem boş ise alert döner.
            {
                return Json("2", JsonRequestBehavior.AllowGet);
            }

            var tamirat = new Tamiratlar
            {
                AracID = servis.AracID,
                ServisID = servis.ID,
                DurumID = 4,
                Tarih = DateTime.Now,
                Km = servis.Km,
                Not1 = servis.Not1,
                Not2 = servis.Not2,
                Not3 = servis.Not3,
                YToplam = servis.YToplam,
                İToplam = servis.İToplam,
                MToplam = servis.MToplam,
                RotBalans = servis.RotBalans,
                İskonto = servis.İskonto,
                Toplam = servis.Toplam,
                GenelToplam = servis.GenelToplam

            }; // servisteki bilgileri tamirata aktarıyoruz.
            db.Tamiratlar.Add(tamirat);
            foreach (var item in servisk)
            {
                if (item.UrunID != 0 && item.KategoriAd != "") //Serviskalem stoktaki bir ürünse bu kodlar işletilir.
                {
                    var tamiratkalem = new TamiratKalem
                    {

                        TamiratID = tamirat.ID,
                        UrunID = item.UrunID,
                        StokKodu = item.StokKodu,
                        KategoriAd = item.KategoriAd,
                        BirimID = item.BirimID,
                        TurID = item.TurID,
                        Aciklama = item.Aciklama,
                        BirimFiyat = item.BirimFiyat,
                        Miktari = item.Miktari,
                        ToplamTutar = item.ToplamTutar
                    };
                    db.TamiratKalem.Add(tamiratkalem);

                    var urun = db.DepoUrun.Where(x => x.UrunID == item.UrunID && x.Depolar.IsDefault == true).FirstOrDefault(); // serviskalemdeki ürün varsayılan depoda ise urun değişkenine aktarılır.
                    if (urun == null) // null gelirse bu ürünün depoda olmadığı var isede varsayılan depoda olmadığı kullanıcıya söylenir.
                    {
                        return Json("4", JsonRequestBehavior.AllowGet);

                    }
                    if (urun != null && item.Miktari <= urun.Miktar && urun.Miktar > 0 && item.Miktari > 0) // eğer depodaki miktardan az bir miktar girilmişse ve depodaki miktarı 0 dan büyükse ürün stoktan düşer ve logu satışlar tablosuna kaydedilir.
                    {
                        urun.Miktar -= item.Miktari;
                        var satis = new Satislar
                        {
                            KullaniciID = 1,
                            UrunID = item.UrunID,
                            SepetID = 0,
                            Durum = "Servisten Satış",
                            DepoID = urun.DepoID,
                            BarkodNo = urun.Urunler.BarkodNo,
                            StokKodu = urun.Urunler.StokKodu,
                            OemKod = urun.Urunler.OemKod,
                            BirimFiyati = urun.Urunler.SatisFiyati,
                            Miktari = item.Miktari,
                            ToplamFiyati = urun.Urunler.SatisFiyati * item.Miktari,
                            KDV = urun.Urunler.KDV,
                            BirimID = urun.BirimID,
                            Tarih = DateTime.Now,
                            Saat = DateTime.Now
                        };
                        db.Satislar.Add(satis);
                        if (urun.Miktar < urun.Urunler.KritikSeviye && urun.Urunler.KritikSeviye != null)
                        {
                            SmtpClient client = new SmtpClient("mail.arabamistanbul.xyz", 587)
                            {
                                EnableSsl = true
                            };
                            MailMessage mail = new MailMessage
                            {
                                From = new MailAddress("reset@arabamistanbul.xyz", "Kritik Stok Seviyesi")
                            };
                            mail.To.Add("arabam.ist@gmail.com");
                            mail.IsBodyHtml = true;
                            mail.Subject = "Kritik Stok Seviyesi";
                            mail.Body += "Merhaba " + urun.Urunler.UrunAdi + " adlı , " + urun.Urunler.StokKodu + " stok kodlu üründen " + urun.Depolar.Adi + " deposunda " + urun.Miktar + " " + urun.Urunler.Birimler.Birim + " kalmıştır ";
                            NetworkCredential net = new NetworkCredential("reset@arabamistanbul.xyz", "^J6lf45l");
                            client.Credentials = net;
                            try
                            {
                                client.Send(mail);
                            }
                            catch (Exception)
                            {
                                ViewBag.islem = "Stoktan düşme işlemi başarılı bir şekilde gerçekleşmiştir. Ancak Kritik seviye maili hakkında bir sorun oluştu.";

                            }
                        }
                    }
                    else if (item.Miktari <= 0) // 0 ve ya eksi adetli giriş yapılırsa uyarı verilir.
                    {
                        return Json("3", JsonRequestBehavior.AllowGet);
                    }
                    else if (urun != null && item.Miktari > urun.Miktar) // depodaki miktardan fazla miktar girilmişse uyarı verilir.
                    {
                        var urun1 = db.DepoUrun.Where(x => x.UrunID == item.UrunID && x.Depolar.IsDefault == true).ToList();
                        var result1 = from l in urun1
                                     select new
                                     {
                                         l.Depolar.Adi,
                                         l.ID,
                                         l.Urunler.KategoriID,
                                         l.Urunler.Kategoriler.Kategori,
                                         l.Urunler.UrunAdi,
                                         l.Urunler.SatisFiyati,
                                         l.Urunler.FormAciklama,
                                         l.Urunler.StokKodu,
                                         l.Miktar,
                                         l.Urunler.BirimID

                                     };
                        return Json(result1, JsonRequestBehavior.AllowGet);
                    }
                }
                else // eğer stoktan bir ürün değil el ile girilirse bu kodlar işletilir.
                {
                    var tamiratkalem = new TamiratKalem
                    {

                        TamiratID = tamirat.ID,
                        UrunID = 0,
                        StokKodu = item.StokKodu,
                        KategoriAd = "",
                        BirimID = item.BirimID,
                        TurID = item.TurID,
                        Aciklama = item.Aciklama,
                        BirimFiyat = item.BirimFiyat,
                        Miktari = item.Miktari,
                        ToplamTutar = item.ToplamTutar
                    };
                    db.TamiratKalem.Add(tamiratkalem);

                }

            }

            db.Servis.Remove(servis); // servisi siler
            db.ServisKalem.RemoveRange(servisk); // serviskalemi siler
            db.SaveChanges();


            return Json(new { result = "Redirect", url = Url.Action("Index", "AdminHome") }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Detay(int id)
        {
            Class1 c = new Class1
            {
                Tamiratlar = db.Tamiratlar.Where(x => x.ID == id).FirstOrDefault(),
                TamiratKalem = db.TamiratKalem.Where(y => y.TamiratID == id).FirstOrDefault(),
                TamiratKalems = db.TamiratKalem.Where(z => z.TamiratID == id).ToList()
            };
            List<SelectListItem> deger1 = (from x in db.Durumlar.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.Durum,
                                               Value = x.ID.ToString()

                                           }).ToList();
            ViewBag.dgr1 = deger1;
            return View(c);
        }

        public ActionResult TamiratGuncelle(Class1 c)
        {
            List<SelectListItem> deger1 = (from x in db.Durumlar.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.Durum,
                                               Value = x.ID.ToString()

                                           }).ToList();
            ViewBag.dgr1 = deger1;
            var tamirat = db.Tamiratlar.FirstOrDefault(x => x.ID == c.Tamiratlar.ID);
            tamirat.Km = c.Tamiratlar.Km;
            tamirat.Not1 = c.Tamiratlar.Not1;
            tamirat.Not2 = c.Tamiratlar.Not2;
            tamirat.Not3 = c.Tamiratlar.Not3;
            //tamirat.Tarih = DateTime.Now;
            //tamirat.DurumID = c.Tamiratlar.DurumID;
            db.SaveChanges();

            return Json("Tamirat Bilgileri Güncellendi.", JsonRequestBehavior.AllowGet);
        }


        public ActionResult Yazdir(int id)
        {
            Class1 c = new Class1
            {
                Tamiratlar = db.Tamiratlar.Where(x => x.ID == id).FirstOrDefault(),
                TamiratKalem = db.TamiratKalem.Where(y => y.TamiratID == id).FirstOrDefault(),
                TamiratKalems = db.TamiratKalem.Where(z => z.TamiratID == id).ToList()
            };


            return View(c);
            //return new PartialViewAsPdf("PrintReport", c)
            //{

            //    FileName = "Fiyat_Teklif_Formu.pdf"
            //};
        }
    }
}