using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC_StokTakip.Models.Entity;

namespace MVC_StokTakip.Models.ParcaliIslemler
{
    public class BildirimIslemler
    {
        arabamis_MVC_StokTakipEntities db = new arabamis_MVC_StokTakipEntities();

           public List<DepoUrun> AzalanStokListe()
        {
            return db.DepoUrun.Where(x => x.Miktar <= 5 && x.Miktar>=1 && x.Urunler.IsDelete== false).OrderBy(x=>x.Miktar).ToList();
        }
        
    }
}