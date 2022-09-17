using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;
using System.Web.Security;
using System.Net.Mail;
using System.Net;

namespace MVC_StokTakip.Controllers
{
    [AllowAnonymous]
    public class KullanicilarController : Controller
    {
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();

        [HttpGet]
        public ActionResult trarabamis()
        {
            return View();
        }
        [HttpPost]
        public ActionResult trarabamis(Kullanicilar k)
        {
            var kullanici = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == k.KullaniciAdi && x.Sifre == k.Sifre);
            if (kullanici != null)
            {
                FormsAuthentication.SetAuthCookie(k.KullaniciAdi, false);
                return RedirectToAction("Index", "AdminHome");
            }
            ViewBag.hata = "Kullanıcı adı veya şifre yanlış";
            return View();

        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("trarabamis");
        }

        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(Kullanicilar k)
        {
            var model = db.Kullanicilar.Where(x => x.Email == k.Email).FirstOrDefault();
            if (model != null)
            {
                Guid rastgele = Guid.NewGuid();
                model.Sifre = rastgele.ToString().Substring(0, 16);
                db.SaveChanges();
                SmtpClient client = new SmtpClient("mail.arabamistanbul.xyz", 587);
                client.EnableSsl = true;
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("reset@arabamistanbul.xyz", "Şifre Sıfırlama");
                mail.To.Add(model.Email);
                mail.IsBodyHtml = true;
                mail.Subject = "Şifre Değiştirme İsteği";
                mail.Body += "Merhaba " + model.AdiSoyadi + "<br/> Kullanıcı Adınız="+model.KullaniciAdi+"<br/>  Şifreniz="+model.Sifre;
                NetworkCredential net = new NetworkCredential("reset@arabamistanbul.xyz", "^J6lf45l");
                client.Credentials = net;
                client.Send(mail);
                return RedirectToAction("trarabamis");

            }
            ViewBag.hata = "Böyle bir e-mail adresi bulunamadı.";
            return View();
        }

        //[HttpGet]
        //public ActionResult Kaydol()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Kaydol(Kullanicilar k)
        //{
        //    if (!ModelState.IsValid) return View();
        //    db.Entry(k).State = System.Data.Entity.EntityState.Added;
        //    db.SaveChanges();
        //    return RedirectToAction("Login");
        //}

        //public ActionResult Guncelle()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var kullaniciadi = User.Identity.Name;
        //        var model = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == kullaniciadi);
        //        if (model != null)
        //        {
        //            return View(model);
        //        }
        //        else
        //        {
        //            return View(new Kullanicilar());
        //        }
        //    }
        //    return HttpNotFound();
        //}
        //[HttpPost]
        //public ActionResult Guncelle(Kullanicilar k)
        //{

        //    db.Entry(k).State = System.Data.Entity.EntityState.Modified;
        //    db.SaveChanges();
        //    FormsAuthentication.SignOut();

        //    return RedirectToAction("Login", "Kullanicilar");

        //}
    }
}