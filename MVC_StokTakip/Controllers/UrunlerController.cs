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
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        //public ActionResult Index(string ara)
        //{ 
        //    var model = db.Urunler.ToList();
        //    if (!string.IsNullOrEmpty(ara))
        //    {
        //        model = model.Where(x => x.UrunAdi.ToLower().Contains(ara.ToLower()) || x.BarkodNo.ToLower().Contains(ara.ToLower()) || x.Aciklama.ToLower().Contains(ara.ToLower()) || x.OemKod.ToLower().Contains(ara.ToLower())).ToList();
        //    }
        //    return View("Index",model);
        //}

        public ActionResult Index2(string ara, string ara2)
        {
            var model = db.Urunler.ToList();
            if (!string.IsNullOrEmpty(ara))
            {
                model = model.Where(x => x.BarkodNo.ToLower().Contains(ara.ToLower())).ToList();

            }

            if (!string.IsNullOrEmpty(ara2))
            {
                model = model.Where(x => x.UrunAdi.ToLower().Contains(ara2.ToLower()) || x.Kategoriler.Kategori.ToLower().Contains(ara2.ToLower()) || x.Aciklama.ToLower().Contains(ara2.ToLower()) || x.OemKod.ToLower().Contains(ara2.ToLower())).ToList();
            }
            return View(model);
        }


        [HttpGet]
        public ActionResult Ekle()
        {
            var model = new MyUrunler();
            Yenile(model);
            return View(model);
        }

        private void Yenile(MyUrunler model) //method haline getirdik dropdown doldurma işlemini.
        {
            List<Kategoriler> KategoriList = db.Kategoriler.OrderBy(x => x.Kategori).ToList();
            model.KategoriListesi = (from x in KategoriList select new SelectListItem { Text = x.Kategori, Value = x.ID.ToString() }).ToList();
            List<Birimler> birimList = db.Birimler.OrderBy(x => x.Birim).ToList();
            model.BirimListesi = (from x in birimList select new SelectListItem { Text = x.Birim, Value = x.ID.ToString() }).ToList();
        }

        [HttpPost]
        public ActionResult Ekle(Urunler p)
        {
            var model = db.Urunler.Where(x => x.BarkodNo == p.BarkodNo);
            if (model != null)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
            p.IsDelete = false;
            db.Entry(p).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();
            return Json(new { result = "Redirect", url = Url.Action("Index2", "Urunler") }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult MiktarEkle(int id)
        {
            var model = db.Urunler.Find(id);
            return View();
        }
        [HttpPost]
        public ActionResult MiktarEkle(Urunler p)
        {
            var model = db.Urunler.Find(p.ID);
            model.Miktari = model.Miktari + p.Miktari;
            db.SaveChanges();
            return RedirectToAction("Index2");
        }
        
        [HttpGet]
        public ActionResult GuncelleBilgiGetir(int id)
        {
            MyUrunler c = new MyUrunler();
            var model =  db.Urunler.Find(id);
            c.ID = model.ID;
            c.KategoriID = model.KategoriID;
            c.UrunAdi = model.UrunAdi;
            c.BarkodNo = model.BarkodNo;
            c.OemKod = model.OemKod;
            c.AlisFiyati = model.AlisFiyati;
            c.SatisFiyati = model.SatisFiyati;
            c.Miktari = model.Miktari;
            c.KDV = model.KDV;
            c.BirimID = model.BirimID;
            c.Tarih = model.Tarih;
            c.Aciklama = model.Aciklama;
            Yenile(c);
            //List<Markalar> markaListe = db.Markalar.Where(x => x.KategoriID == model.KategoriID).OrderBy(x => x.Marka).ToList();
            //urun.MarkaListesi = (from x in markaListe select new SelectListItem { Text = x.Marka, Value = x.ID.ToString() }).ToList();
            return View(c);
        }


        [HttpPost]
        public ActionResult Guncelle(Urunler p)
        {
            //if (!ModelState.IsValid)
            //{
            //    var model = db.Urunler.Find(p.ID);
            //    var urun = new MyUrunler();
            //    Yenile(urun);
            //    List<Markalar> markaListe = db.Markalar.Where(x => x.KategoriID == model.KategoriID).OrderBy(x => x.Marka).ToList();
            //    urun.MarkaListesi = (from x in markaListe select new SelectListItem { Text = x.Marka, Value = x.ID.ToString() }).ToList();
            //    return View(urun);
            //}
            //db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            //db.SaveChanges();
            //return RedirectToAction("Index2");
            p.IsDelete = false;
            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(new { result = "Redirect", url = Url.Action("Index2", "Urunler") }, JsonRequestBehavior.AllowGet);
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
            var degerler = db.Urunler.ToList();
            return View(degerler);
        }
 
        public ActionResult StokAra()
        {
            var degerler = db.Urunler.ToList();
            return View(degerler);
        }
        [HttpGet]
        public JsonResult UrunGetir(string Id)
        {
            var list = db.Urunler.ToList();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].BarkodNo == Id)
                {
                    var result = from l in list
                                 where l.BarkodNo == Id && l.IsDelete == false
                                 select new
                                 {
                                     l.ID,
                                     l.UrunAdi,
                                     l.SatisFiyati,
                                     l.Miktari,
                                     l.AlisFiyati,
                                     l.BarkodNo,
                                     l.Kategoriler.Kategori                                   
                                 };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].BarkodNo != Id)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

            }

            //db.Configuration.ProxyCreationEnabled = false;
            //var model = db.Urunler.FirstOrDefault();
            //var result = db.Urunler.Where(x => x.BarkodNo == Id && x.Miktari>0).FirstOrDefault();
            return Json("", JsonRequestBehavior.AllowGet);

        }
    }
}