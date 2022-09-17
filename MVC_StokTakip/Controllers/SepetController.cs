using System;
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
        readonly arabamis_MVC_StokTakipEntities db = new arabamis_MVC_StokTakipEntities();
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
                    foreach (var item in model)
                    {
                        List<Depolar> Depolar = db.Depolar.OrderBy(x => x.Adi).ToList();
                        item.DepoListesi = (from x in Depolar select new SelectListItem { Text = x.Adi, Value = x.ID.ToString(), Selected = (x.ID == item.DepoID ? true : false) }).ToList();
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
                var depo = db.Depolar.Where(x => x.IsDefault == true).FirstOrDefault();
                if (model != null)
                {
                    if (sepet != null) // Sepete eklenen ürün sepette varsa bu işlem çalıştırılır
                    {
                        sepet.Miktari++;
                        sepet.ToplamFiyat = u.SatisFiyati * sepet.Miktari;
                        db.SaveChanges();
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (depo != null)
                        {
                            var s = new Sepet // Sepete eklenen ürün sepette yoksa yeni sepet girişi yapılır.
                            {
                                KullaniciID = model.ID,
                                UrunID = u.ID,
                                Miktari = 1,
                                DepoID = depo.ID,
                                BirimFiyati = u.SatisFiyati,
                                ToplamFiyat = u.SatisFiyati,
                                Tarih = DateTime.Now,
                                Saat = DateTime.Now
                            };

                            db.Entry(s).State = System.Data.Entity.EntityState.Added;
                            db.SaveChanges();
                            return Json("0", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("1", JsonRequestBehavior.AllowGet);
                        }

                    }
            
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
                var depo = db.Depolar.Where(x => x.IsDefault == true).FirstOrDefault();

                if (model != null)
                {
                    if (sepet != null)
                    {
                        sepet.Miktari++;
                        sepet.ToplamFiyat = u.SatisFiyati * sepet.Miktari;
                        db.SaveChanges();
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (depo != null)
                        {
                            var s = new Sepet // Sepete eklenen ürün sepette yoksa yeni sepet girişi yapılır.
                            {
                                KullaniciID = model.ID,
                                UrunID = u.ID,
                                Miktari = 1,
                                DepoID = depo.ID,
                                BirimFiyati = u.SatisFiyati,
                                ToplamFiyat = u.SatisFiyati,
                                Tarih = DateTime.Now,
                                Saat = DateTime.Now
                            };

                            db.Entry(s).State = System.Data.Entity.EntityState.Added;
                            db.SaveChanges();
                            return Json("0", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("1", JsonRequestBehavior.AllowGet);
                        }

                    }
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SepeteEkle3(string Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var model = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == kullaniciadi);
                var u = db.Urunler.Where(x=>x.BarkodNo == Id && x.IsDelete == false).FirstOrDefault();
                var sepet = db.Sepet.FirstOrDefault(x => x.KullaniciID == model.ID && x.UrunID == u.ID);
                var depo = db.Depolar.Where(x => x.IsDefault == true).FirstOrDefault();

                if (model != null)
                {
                    if (sepet != null)
                    {
                        sepet.Miktari++;
                        sepet.ToplamFiyat = u.SatisFiyati * sepet.Miktari;
                        db.SaveChanges();
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (depo != null)
                        {
                            var s = new Sepet // Sepete eklenen ürün sepette yoksa yeni sepet girişi yapılır.
                            {
                                KullaniciID = model.ID,
                                UrunID = u.ID,
                                Miktari = 1,
                                DepoID = depo.ID,
                                BirimFiyati = u.SatisFiyati,
                                ToplamFiyat = u.SatisFiyati,
                                Tarih = DateTime.Now,
                                Saat = DateTime.Now
                            };

                            db.Entry(s).State = System.Data.Entity.EntityState.Added;
                            db.SaveChanges();
                            return Json("0", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("1", JsonRequestBehavior.AllowGet);
                        }

                    }
                }
            }
            return Json("",JsonRequestBehavior.AllowGet);
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

        public ActionResult DinamikDepo(int id,int depo)
        {
            var model = db.Sepet.Find(id);
            model.DepoID = depo;
            db.SaveChanges();
            return Json("", JsonRequestBehavior.AllowGet);
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