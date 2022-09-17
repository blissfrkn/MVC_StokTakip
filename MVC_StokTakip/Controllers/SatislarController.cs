using System;
using MVC_StokTakip.Models.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;

namespace MVC_StokTakip.Controllers
{
    [Authorize(Roles = "A")]

    public class SatislarController : Controller
    {
        readonly arabamis_MVC_StokTakipEntities db = new arabamis_MVC_StokTakipEntities();
        public ActionResult Index(string ara,string ara2)
        {
            var model = db.Satislar.ToList();
            if (!string.IsNullOrEmpty(ara))
            {
                model = model.Where(x => x.Tarih.ToString().Contains(ara.ToString())).ToList();

            }

            if (!string.IsNullOrEmpty(ara2))
            {
          
                model = model.Where(x => x.BarkodNo.ToLower().Contains(ara2.ToLower())).ToList();

            }
            return View(model);
        }

        public ActionResult SatinAl(int id)
        {
            var model = db.Sepet.FirstOrDefault(x => x.ID == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult SatinAl2(int id)
        {

            try
            {
                if (ModelState.IsValid)
                {

                    var model = db.Sepet.FirstOrDefault(x => x.ID == id);
                    var urun = db.DepoUrun.Where(x => x.UrunID == model.UrunID && x.DepoID == model.DepoID).FirstOrDefault();
                    //var urun2 = db.DepoUrun.Where(x => x.UrunID == model.UrunID).Sum(y => y.Miktar);

                    if (urun == null)
                    {
                        ViewBag.islem = "Bu depoda bu üründen yoktur.Düşme İşlemi Gerçekleştirmek için önce stok ekleyin";
                    }

                    else if (urun.Miktar < model.Miktari)
                    {
                        ViewBag.islem = "Depodaki adet sayısından fazla adet stoktan düşemezsiniz";
                    }
                    else
                    {
                        urun.Miktar -= model.Miktari;
                        //urun2 -= model.Miktari;
                        var satis = new Satislar
                        {
                            KullaniciID = model.KullaniciID,
                            UrunID = model.UrunID,
                            SepetID = model.ID,
                            DepoID = model.DepoID,
                            Durum = "Manuel Çıkış",
                            BarkodNo = model.Urunler.BarkodNo,
                            StokKodu = model.Urunler.StokKodu,
                            OemKod = model.Urunler.OemKod,
                            BirimFiyati = model.BirimFiyati,
                            Miktari = model.Miktari,
                            ToplamFiyati = model.ToplamFiyat,
                            KDV = model.Urunler.KDV,
                            BirimID = model.Urunler.BirimID,
                            Tarih = DateTime.Now,
                            Saat = DateTime.Now
                        };

                        db.Satislar.Add(satis);

                        ViewBag.islem = "Stoktan Düşme İşlemi başarılı bir şekilde gerçekleşmiştir.";
                        if (urun.Miktar < urun.Urunler.KritikSeviye && urun.Urunler.KritikSeviye != null)
                        {
                            SmtpClient client = new SmtpClient("mail.arabamistanbul.xyz", 587)
                            {
                                EnableSsl = true
                            };
                            MailMessage mail = new MailMessage
                            {
                                From = new MailAddress("stok@arabamistanbul.xyz", "Kritik Stok Seviyesi")
                            };
                            mail.To.Add("frknbayraktar34@gmail.com");
                            mail.IsBodyHtml = true;
                            mail.Subject = "Kritik Stok Seviyesi";
                            mail.Body += "Merhaba " + model.Urunler.UrunAdi + " adlı , " + model.Urunler.StokKodu + " stok kodlu üründen " + urun.Depolar.Adi + " deposunda " + urun.Miktar + " " + urun.Urunler.Birimler.Birim + " kalmıştır ";
                            NetworkCredential net = new NetworkCredential("stok@arabamistanbul.xyz", "JacKlondr@-+313");
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

                        db.Sepet.Remove(model);
                        db.SaveChanges();
                    }

                }
            }
            catch (Exception)
            {

                ViewBag.islem = "Stoktan Düşme İşlemi başarısız.";

            }

            return View("islem");




        }

        public ActionResult HepsiniSatinAl(decimal? Tutar)
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
        public ActionResult HepsiniSatinAl2()
        {
            try
            {
                var username = User.Identity.Name;
                var kullanici = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == username);
                var model = db.Sepet.Where(x => x.KullaniciID == kullanici.ID).ToList();
                int row = 0;
                foreach (var item in model)
                {
                    var urun = db.DepoUrun.Where(x => x.UrunID == item.UrunID && x.DepoID == item.DepoID).FirstOrDefault();
                    //var urun2 = db.DepoUrun.Where(x => x.UrunID == item.UrunID).Sum(y => y.Miktar);
                    if (urun == null)
                    {
                        ViewBag.islem = item.Urunler.UrunAdi + " adlı ürün seçtiğiniz depoda mevcut değildir. Önce stok ekleyiniz.";
                        return View("islem");
                    }
                    else if (urun.Miktar < item.Miktari)
                    {
                        ViewBag.islem = item.Urunler.UrunAdi + " adlı ürünün depodaki adet sayısından daha fazla adet stoktan düşemezsiniz";
                        return View("islem");
                    }
                    else
                    {

                        urun.Miktar -= item.Miktari;
                        //urun2 -= item.Miktari;
                        var satis = new Satislar
                        {
                            KullaniciID = model[row].KullaniciID,
                            UrunID = model[row].UrunID,
                            SepetID = model[row].ID,
                            Durum = "Manuel Çıkış",
                            BarkodNo = model[row].Urunler.BarkodNo,
                            StokKodu = model[row].Urunler.StokKodu,
                            OemKod = model[row].Urunler.OemKod,
                            BirimFiyati = model[row].BirimFiyati,
                            Miktari = model[row].Miktari,
                            DepoID = model[row].DepoID,
                            ToplamFiyati = model[row].ToplamFiyat,
                            KDV = model[row].Urunler.KDV,
                            BirimID = model[row].Urunler.BirimID,
                            Tarih = DateTime.Now,
                            Saat = DateTime.Now
                        };
                        db.Satislar.Add(satis);
                        row++;
                        ViewBag.islem = "Stoktan düşme işlemi başarılı bir şekilde gerçekleşmiştir.";
                        if (urun.Miktar <= urun.Urunler.KritikSeviye)
                        {
                            SmtpClient client = new SmtpClient("mail.arabamistanbul.xyz", 587)
                            {
                                EnableSsl = true
                            };
                            MailMessage mail = new MailMessage
                            {
                                From = new MailAddress("reset@arabamistanbul.xyz", "Kritik Stok Seviyesi")
                            };
                            mail.To.Add("info@arabamistanbul.com.tr");
                            mail.IsBodyHtml = true;
                            mail.Subject = "Kritik Stok Seviyesi";
                            mail.Body += "Merhaba " + item.Urunler.UrunAdi + " adlı , " + item.Urunler.StokKodu + " stok kodlu üründen "+ urun.Depolar.Adi + " deposunda " + urun.Miktar + " adet kalmıştır";
                            NetworkCredential net = new NetworkCredential("reset@arabamistanbul.xyz", "^J6lf45l");
                            client.Credentials = net;
                            try
                            {
                                client.Send(mail);
                            }
                            catch (Exception )
                            {
                                ViewBag.islem = "Stoktan düşme işlemi başarılı bir şekilde gerçekleşmiştir. Ancak Kritik seviye maili hakkında bir sorun oluştu.";
                               
                            }                    
                        }
                        
                    }
                }
                db.Sepet.RemoveRange(model);
                db.SaveChanges();
                return View("islem");
            }
            catch (Exception)
            {
                throw;            
            }

        }
    }
}