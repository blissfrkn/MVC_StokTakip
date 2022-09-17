using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC_StokTakip.Models.Entity;

namespace MVC_StokTakip.Models.ParcaliIslemler
{
    public class BildirimIslemler
    {
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();

           public List<Urunler> AzalanStokListe()
        {
            return db.Urunler.Where(x => x.Miktari <= 5 && x.Miktari>=1).ToList();
        }
    }
}