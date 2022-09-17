using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;
using MVC_StokTakip.MyModel;



namespace MVC_StokTakip.Controllers
{
    [Authorize(Roles ="A")]

    public class UrunlerController : Controller
    {
        readonly arabamis_MVC_StokTakipEntities db = new arabamis_MVC_StokTakipEntities();

        [HttpGet]
        public ActionResult Product()
        {
            //List<SelectListItem> deger1 = (from x in db.Depolar.ToList()
            //                               select new SelectListItem
            //                               {
            //                                   Text = x.Adi,
            //                                   Value = x.ID.ToString()
            //                               }).ToList();

            //deger1.Add(new SelectListItem { Text = "Tümü", Value = "0" });
            //deger1.Insert(0, new SelectListItem { Text = "Tümü", Value = "0" });
            //ViewBag.dgr1 = deger1;



            //var urunler = db.Urunler.Select(i => new DepoUrunModel()
            //{
            //    ID = i.ID,
            //    KategoriAdi = i.Kategoriler.Kategori,
            //    UrunAdi = i.UrunAdi,
            //    BarkodNo = i.BarkodNo,
            //    StokKodu = i.StokKodu,
            //    OemKod = i.OemKod,
            //    AlisFiyati = i.AlisFiyati,
            //    SatisFiyati = i.SatisFiyati,
            //    KDV = i.KDV,
            //    Tarih = i.Tarih,
            //    Birim = i.Birimler.Birim,
            //    Aciklama = i.Aciklama,
            //    FormAciklama = i.FormAciklama,
            //    IsDelete = i.IsDelete,
            //    Miktar = db.DepoUrun.Where(z => z.UrunID == i.ID).Select(y => y.Miktar).DefaultIfEmpty(0).Sum(),


            //}).ToList();

            //var product = urunler.Where(x => x.Miktar != 0).ToList();

            //Class1 a = new Class1();
            //a.DepoUrunModels = product;


            var urunler = db.Urunler.Select(i => new DepoUrunModel()
            {
                ID = i.ID,
                KategoriAdi = i.Kategoriler.Kategori,
                UrunAdi = i.UrunAdi,
                BarkodNo = i.BarkodNo,
                StokKodu = i.StokKodu,
                OemKod = i.OemKod,
                AlisFiyati = i.AlisFiyati,
                SatisFiyati = i.SatisFiyati,
                KDV = i.KDV,
                Tarih = i.Tarih,
                Birim = i.Birimler.Birim,
                Aciklama = i.Aciklama,
                FormAciklama = i.FormAciklama,
                IsDelete = i.IsDelete,
                MagazaMiktar = db.DepoUrun.Where(x => x.UrunID == i.ID && x.DepoID == 1).Select(y => y.Miktar).FirstOrDefault(),
                DepoMiktar = db.DepoUrun.Where(x => x.UrunID == i.ID && x.DepoID == 2).Select(y => y.Miktar).FirstOrDefault(),
          
            }).ToList();

            Class1 a = new Class1();
            a.DepoUrunModels = urunler;

            return View(a);
        }

        private void Yenile(MyUrunler model) //method haline getirdik dropdown doldurma işlemini.
        {
            List<Kategoriler> KategoriList = db.Kategoriler.OrderBy(x => x.Kategori).ToList();
            model.KategoriListesi = (from x in KategoriList select new SelectListItem { Text = x.Kategori, Value = x.ID.ToString() }).ToList();
            List<Birimler> birimList = db.Birimler.OrderBy(x => x.Birim).ToList();
            model.BirimListesi = (from x in birimList select new SelectListItem { Text = x.Birim, Value = x.ID.ToString() }).ToList();
        }

        [HttpGet]
        public ActionResult Ekle()
        {
            var model = new MyUrunler();
            Yenile(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Ekle(Urunler p)
        {
            var model = db.Urunler.Where(x=> x.StokKodu == p.StokKodu || x.BarkodNo == p.BarkodNo).ToList();
           
            if (model.Count != 0)
            {
                foreach (var item in model)
                {
                    if (item.IsDelete == true && (item.StokKodu == p.StokKodu || item.BarkodNo == p.BarkodNo))
                    {
                        return Json("2", JsonRequestBehavior.AllowGet);
                    }
                    else if (item.BarkodNo == p.BarkodNo && item.IsDelete == false)
                    {
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                    else if (item.IsDelete == false)
                    {
                        p.IsDelete = false;
                        db.Entry(p).State = System.Data.Entity.EntityState.Added;
                        db.SaveChanges();
                        return Json(new { result = "Redirect", url = Url.Action("Product", "Urunler") }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            else
            {
                p.IsDelete = false;
                db.Entry(p).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
                return Json(new { result = "Redirect", url = Url.Action("Product", "Urunler") }, JsonRequestBehavior.AllowGet);
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Ekle2(Urunler p)
        {
            
                p.IsDelete = false;
                db.Entry(p).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
                return Json(new { result = "Redirect", url = Url.Action("Product", "Urunler") }, JsonRequestBehavior.AllowGet);
          
        }


        [HttpGet]
        public ActionResult MiktarEkle(int id)
        {
            _ = db.Urunler.Find(id);
            return View();
        }
        //[HttpPost]
        //public ActionResult MiktarEkle(Urunler p)
        //{
        //    var model = db.Urunler.Find(p.ID);
        //    model.Miktari += p.Miktari;
        //    db.SaveChanges();
        //    return RedirectToAction("Index2");
        //}

        [HttpGet]
        public ActionResult UrunDetay(int id)
        {
            var model = db.Urunler.Find(id);
            MyUrunler c = new MyUrunler();
            c.ID = model.ID;
            c.KategoriID = model.KategoriID;
            c.UrunAdi = model.UrunAdi;
            c.BarkodNo = model.BarkodNo;
            c.StokKodu = model.StokKodu;
            c.OemKod = model.OemKod;
            c.AlisFiyati = model.AlisFiyati;
            c.SatisFiyati = model.SatisFiyati;
            //c.Miktari = model.Miktari;
            c.KDV = model.KDV;
            c.BirimID = model.BirimID;
            c.Tarih = model.Tarih;
            c.Aciklama = model.Aciklama;
            c.FormAciklama = model.FormAciklama;
            c.KritikSeviye = model.KritikSeviye;
            c.KritikStok = model.KritikStok;
            c.Konum = model.Konum;
            Yenile(c);
            Class1 a = new Class1();
            a.Urunler = c;
            a.Satislars = db.Satislar.Where(x => x.UrunID == id).ToList();
            a.Girislers = db.Girisler.Where(x => x.UrunID == id).ToList();
            a.DepoUruns = db.DepoUrun.Where(x => x.UrunID == id).ToList();

       
            var giris = db.Girisler.Where(x => x.UrunID == id && x.Durum != "Depo Transfer").Select(y => y.Miktari).DefaultIfEmpty(0).Sum(); 
            ViewBag.grs = giris;
            var cikis = db.Satislar.Where(x => x.UrunID == id && x.Durum != "Depo Transfer").Select(y => y.Miktari).DefaultIfEmpty(0).Sum();
            ViewBag.cks = cikis;
            var toplam = db.DepoUrun.Where(x => x.UrunID == id).Select(y => y.Miktar).DefaultIfEmpty(0).Sum();
            ViewBag.tplm = toplam;
            var birim = db.Urunler.Where(x => x.ID == id).Select(y => y.Birimler.Birim).FirstOrDefault();
            ViewBag.brm = birim;
            
            //List<Markalar> markaListe = db.Markalar.Where(x => x.KategoriID == model.KategoriID).OrderBy(x => x.Marka).ToList();
            //urun.MarkaListesi = (from x in markaListe select new SelectListItem { Text = x.Marka, Value = x.ID.ToString() }).ToList();
            return View(a);
        }

        public ActionResult Guncelle(Class1 p)
        {
            try
            {

                var model = db.Urunler.Where(x => x.ID == p.Urunler.ID).FirstOrDefault();
                model.UrunAdi = p.Urunler.UrunAdi;
                model.BarkodNo = p.Urunler.BarkodNo;
                model.StokKodu = p.Urunler.StokKodu;
                model.KategoriID = p.Urunler.KategoriID;
                model.KDV = p.Urunler.KDV;
                model.OemKod = p.Urunler.OemKod;
                model.Tarih = DateTime.Now;
                model.AlisFiyati = (decimal)p.Urunler.AlisFiyati;
                model.BirimID = p.Urunler.BirimID;
                model.SatisFiyati = (decimal)p.Urunler.SatisFiyati;
                model.Aciklama = p.Urunler.Aciklama;
                model.FormAciklama = p.Urunler.FormAciklama;
                model.KritikSeviye = (decimal)p.Urunler.KritikSeviye;
                model.KritikStok = (decimal)p.Urunler.KritikStok;
                model.Konum = p.Urunler.Konum;
                db.SaveChanges();
                return Json("Ürün başarı ile güncellendi.", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return HttpNotFound();
                throw;
            }
           
        }
        public ActionResult Sil(int id)
        {
            var model = db.Urunler.FirstOrDefault(x => x.ID == id);
            model.IsDelete = true;
            db.SaveChanges();
            return RedirectToAction("Index2");
        }

        public ActionResult UrunListe()
        {

            var urunler = db.Urunler.Select(i => new DepoUrunModel()
            {

                ID = i.ID,
                KategoriAdi = i.Kategoriler.Kategori,
                UrunAdi = i.UrunAdi,
                BarkodNo = i.BarkodNo,
                StokKodu = i.StokKodu,
                OemKod = i.OemKod,
                AlisFiyati = i.AlisFiyati,
                SatisFiyati = i.SatisFiyati,
                KDV = i.KDV,
                Tarih = i.Tarih,
                Birim = i.Birimler.Birim,
                Aciklama = i.Aciklama,
                FormAciklama = i.FormAciklama,
                IsDelete = i.IsDelete,
                MagazaMiktar = db.DepoUrun.Where(x => x.UrunID == i.ID && x.DepoID == 1).Select(y => y.Miktar).FirstOrDefault(),
                DepoMiktar = db.DepoUrun.Where(x => x.UrunID == i.ID && x.DepoID == 2).Select(y => y.Miktar).FirstOrDefault(),
                Toplam = db.DepoUrun.Where(x => x.UrunID == i.ID).Select(y => y.Miktar).Sum(),
                Kullanim = db.Satislar.Where(x => (x.UrunID == i.ID)).Where(x=> (x.Durum != "Depo Transfer")).Where(x=> (x.Durum != "Manuel Çıkış")).Select(y => y.Miktari).Sum(),
                SayfaNo = db.Kategoriler.Where(x=>x.ID == i.KategoriID).Select(y=>y.SayfaNo).FirstOrDefault(),
            }).ToList();

            Class1 a = new Class1();
            a.DepoUrunModels = urunler;

            return View(a);
        }

        public ActionResult SiparisListe()
        {

            var urunler = db.Urunler.Select(i => new DepoUrunModel()
            {

                ID = i.ID,
                KategoriAdi = i.Kategoriler.Kategori,
                UrunAdi = i.UrunAdi,
                BarkodNo = i.BarkodNo,
                StokKodu = i.StokKodu,
                OemKod = i.OemKod,
                AlisFiyati = i.AlisFiyati,
                SatisFiyati = i.SatisFiyati,
                KDV = i.KDV,
                Tarih = i.Tarih,
                Birim = i.Birimler.Birim,
                Aciklama = i.Aciklama,
                FormAciklama = i.FormAciklama,
                IsDelete = i.IsDelete,
                MagazaMiktar = db.DepoUrun.Where(x => x.UrunID == i.ID && x.DepoID == 1).Select(y => y.Miktar).FirstOrDefault(),
                DepoMiktar = db.DepoUrun.Where(x => x.UrunID == i.ID && x.DepoID == 2).Select(y => y.Miktar).FirstOrDefault(),
                KritikMiktar = i.KritikSeviye - db.DepoUrun.Where(x => x.Urunler.StokKodu == i.StokKodu).Select(y => y.Miktar).Sum(),
                Toplam = db.DepoUrun.Where(x => x.UrunID == i.ID).Select(y => y.Miktar).Sum(),
                Kullanim = db.Satislar.Where(x => x.UrunID == i.ID).Select(y => y.Miktari).Sum(),
            }).ToList();

            Class1 a = new Class1();
            a.DepoUrunModels = urunler;

            return View(a);
        }

        public ActionResult EksikListe()
        {

            var urunler = db.Urunler.Select(i => new DepoUrunModel()
            {

                ID = i.ID,
                KategoriAdi = i.Kategoriler.Kategori,
                UrunAdi = i.UrunAdi,
                BarkodNo = i.BarkodNo,
                StokKodu = i.StokKodu,
                OemKod = i.OemKod,
                AlisFiyati = i.AlisFiyati,
                SatisFiyati = i.SatisFiyati,
                KDV = i.KDV,
                Tarih = i.Tarih,
                Birim = i.Birimler.Birim,
                Aciklama = i.Aciklama,
                FormAciklama = i.FormAciklama,
                IsDelete = i.IsDelete,
                MagazaMiktar = db.DepoUrun.Where(x => x.UrunID == i.ID && x.DepoID == 1).Select(y => y.Miktar).FirstOrDefault(),
                DepoMiktar = db.DepoUrun.Where(x => x.UrunID == i.ID && x.DepoID == 2).Select(y => y.Miktar).FirstOrDefault(),
                KritikMiktar = i.KritikStok - db.DepoUrun.Where(x => x.Urunler.StokKodu == i.StokKodu && x.DepoID == 1 ).Select(y => y.Miktar).Sum(),
                Toplam = db.DepoUrun.Where(x => x.UrunID == i.ID).Select(y => y.Miktar).Sum(),
                Kullanim = db.Satislar.Where(x => x.UrunID == i.ID).Select(y => y.Miktari).Sum(),
            }).ToList();

            Class1 a = new Class1();
            a.DepoUrunModels = urunler;

            return View(a);
        }


        public ActionResult StokAra()
        {
            var degerler = db.Urunler.Where(x=>x.IsDelete == false).ToList();
            return View(degerler);
        }

        public ActionResult HizliSepet()
        {
            var degerler = db.Urunler.Where(x=>x.IsDelete == false).ToList();
            return View(degerler);
        }
        [HttpGet]
        public JsonResult UrunGetir(string Id)
        {
            var list = db.Urunler.ToList();

            for (int i = 0; i < list.Count; i++)
            {
                if ((list[i].BarkodNo == Id || list[i].StokKodu == Id) && list[i].IsDelete == false)
                {
                    var result = from l in list
                                 where (l.BarkodNo == Id || l.StokKodu == Id) && l.IsDelete == false
                                 select new
                                 {
                                     l.ID,
                                     l.UrunAdi,
                                     l.SatisFiyati,
                                     l.AlisFiyati,
                                     l.BarkodNo,
                                     l.StokKodu,
                                     l.Kategoriler.Kategori
                                 };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].BarkodNo == Id || list[i].StokKodu == Id && list[i].IsDelete == true)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

            }

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].BarkodNo != Id || list[i].StokKodu != Id || list[i].IsDelete == true)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

            }

            //db.Configuration.ProxyCreationEnabled = false;
            //var model = db.Urunler.FirstOrDefault();
            //var result = db.Urunler.Where(x => x.BarkodNo == Id && x.Miktari>0).FirstOrDefault();
            return Json("", JsonRequestBehavior.AllowGet);


        }

        public ActionResult DeleteProducts()
        {
            var model = db.Urunler.Where(x => x.IsDelete == true).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult GeriYukle(int id)
        {
            var model = db.Urunler.Find(id);
            model.IsDelete = false;
            db.SaveChanges();
            return Json("Ürün başarıyla geri yüklendi", JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        //public ActionResult updateGrid(int id)
        //{
        //    //decimal decimalID = Convert.ToDecimal(id);

        //    //Use LINQ to query DB & Get all of the parts
        //    //Join PPLPart & PPLDescriptor Tables
        //    if (id == 0)
        //    {
        //        var urunlerr = db.Urunler.Select(i => new DepoUrunModel()
        //        {
        //            ID = i.ID,
        //            KategoriAdi = i.Kategoriler.Kategori,
        //            UrunAdi = i.UrunAdi,
        //            BarkodNo = i.BarkodNo,
        //            StokKodu = i.StokKodu,
        //            OemKod = i.OemKod,
        //            AlisFiyati = i.AlisFiyati,
        //            SatisFiyati = i.SatisFiyati,
        //            KDV = i.KDV,
        //            Tarih = i.Tarih,
        //            Birim = i.Birimler.Birim,
        //            Aciklama = i.Aciklama,
        //            FormAciklama = i.FormAciklama,
        //            IsDelete = i.IsDelete,
        //            Miktar = db.DepoUrun.Where(z => z.UrunID == i.ID).Select(y => y.Miktar).DefaultIfEmpty(0).Sum(),


        //        }).ToList();

        //        var product = urunlerr.Where(x => x.Miktar != 0).ToList();
        //        Class1 k = new Class1();
        //        k.DepoUrunModels = product;


        //        return PartialView("GridView", k);
        //    }
        //    else
        //    {
        //        var urunler = db.Urunler.Select(i => new DepoUrunModel()
        //        {
        //            ID = i.ID,
        //            KategoriAdi = i.Kategoriler.Kategori,
        //            UrunAdi = i.UrunAdi,
        //            BarkodNo = i.BarkodNo,
        //            StokKodu = i.StokKodu,
        //            OemKod = i.OemKod,
        //            AlisFiyati = i.AlisFiyati,
        //            SatisFiyati = i.SatisFiyati,
        //            KDV = i.KDV,
        //            Tarih = i.Tarih,
        //            Birim = i.Birimler.Birim,
        //            Aciklama = i.Aciklama,
        //            FormAciklama = i.FormAciklama,
        //            IsDelete = i.IsDelete,
        //            Miktar = db.DepoUrun.Where(z => z.UrunID == i.ID && z.DepoID == id).Select(y => y.Miktar).FirstOrDefault(),


        //        }).ToList();

        //        var product = urunler.Where(x => x.Miktar != 0).ToList();

        //        Class1 a = new Class1();
        //        a.DepoUrunModels = product;


        //        return PartialView("GridView", a);
        //    }

        //}

    }
}