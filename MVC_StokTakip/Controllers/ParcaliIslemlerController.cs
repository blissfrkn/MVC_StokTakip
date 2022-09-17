using MVC_StokTakip.Models.ParcaliIslemler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MVC_StokTakip.Controllers
{
    [Authorize(Roles = "A")]

    
    public class ParcaliIslemlerController : Controller
    {

        // GET: ParcaliIslemler
        public PartialViewResult BildirimMenusu()
        {
            return PartialView(new BildirimIslemler().AzalanStokListe());
        }



    }
}