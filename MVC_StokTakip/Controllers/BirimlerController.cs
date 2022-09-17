using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;
using MVC_StokTakip.MyModel;

namespace MVC_StokTakip.Controllers
{
    [Authorize(Roles ="A")]

    public class BirimlerController : Controller
    {
        readonly arabamis_MVC_StokTakipEntities db = new arabamis_MVC_StokTakipEntities();
        public ActionResult Index()
        {
            return View(db.Birimler.ToList());
        }

        [HttpGet]    
        public ActionResult Ekle()
        {
            return View("Kaydet");
        }

        [HttpPost]
        public ActionResult Kaydet(Birimler p)
        {

                if (p.ID == 0)
            {
                if(p.Birim==null || p.Aciklama == null)
                {
                    return View();
                }
                db.Birimler.Add(p);
            }
            else if(p.ID > 0)
            {
                if (p.Birim == null || p.Aciklama == null)
                {
                    return View();
                }
                db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            }
            
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult GuncelleBilgiGetir(int? id)
        {
            var model = db.Birimler.Find(id);
            MyBirimler k = new MyBirimler
            {
                ID = model.ID,
                Birim = model.Birim,
                Aciklama = model.Aciklama
            };
            if (model == null) return HttpNotFound();
            return View("Kaydet",k);
        }

        public ActionResult SilBilgiGetir(Birimler p)
        {
            var model = db.Birimler.Find(p.ID);
            if (model == null) return HttpNotFound();
            return View(model);
        }
        public ActionResult Sil(Birimler p)
        {
            db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}