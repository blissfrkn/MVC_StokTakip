using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;

namespace MVC_StokTakip.Controllers
{
    public class TamiratlarController : Controller
    {
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
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

                };

            db.Tamiratlar.Add(tamirat);


            foreach (var item in servisk)
            {
                var tamiratkalem = new TamiratKalem
                {
                    TamiratID = tamirat.ID,
                    BirimID = item.BirimID,
                    TurID = item.TurID,
                    Aciklama = item.Aciklama,
                    BirimFiyat = item.BirimFiyat,
                    Miktari = item.Miktari,
                    ToplamTutar = item.ToplamTutar
                };
                db.TamiratKalem.Add(tamiratkalem);

                var urun = db.Urunler.FirstOrDefault(x => x.UrunAdi == item.Aciklama);
                if (urun != null &&  item.Miktari < urun.Miktari && urun.Miktari > 0)
                {
                    urun.Miktari = (decimal)(urun.Miktari - item.Miktari);
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }

            db.Servis.Remove(servis);
            db.ServisKalem.RemoveRange(servisk);
            db.SaveChanges();


            return Json(new { result = "Redirect", url = Url.Action("Index", "AdminHome") }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Detay(int id)
        {
            Class1 c = new Class1();
            c.Tamiratlar = db.Tamiratlar.Where(x => x.ID == id).FirstOrDefault();
            c.TamiratKalem = db.TamiratKalem.Where(y => y.TamiratID == id).FirstOrDefault();
            c.TamiratKalems = db.TamiratKalem.Where(z => z.TamiratID == id).ToList();
            List<SelectListItem> deger1 = (from x in db.Durumlar.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.Durum,
                                               Value = x.ID.ToString()

                                           }).ToList();
            ViewBag.dgr1 = deger1;
            return View(c);
        }

        public ActionResult Yazdir(int id)
        {
            Class1 c = new Class1();

            c.Tamiratlar = db.Tamiratlar.Where(x => x.ID == id).FirstOrDefault();
            c.TamiratKalem = db.TamiratKalem.Where(y => y.TamiratID == id).FirstOrDefault();
            c.TamiratKalems = db.TamiratKalem.Where(z => z.TamiratID == id).ToList();


            return View(c);
            //return new PartialViewAsPdf("PrintReport", c)
            //{

            //    FileName = "Fiyat_Teklif_Formu.pdf"
            //};
        }
    }
}