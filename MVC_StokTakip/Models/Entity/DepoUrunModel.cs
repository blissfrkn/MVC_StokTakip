using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;
using MVC_StokTakip.MyModel;

namespace MVC_StokTakip.Models.Entity
{
    public class DepoUrunModel
    {
        public int ID { get; set; }
        public string KategoriAdi { get; set; }
        public string UrunAdi { get; set; }
        public string BarkodNo { get; set; }
        public string StokKodu { get; set; }
        public string OemKod { get; set; }
        public decimal AlisFiyati { get; set; }
        public decimal SatisFiyati { get; set; }
        public int KDV { get; set; }
        public string Birim { get; set; }
        public System.DateTime Tarih { get; set; }
        public string Aciklama { get; set; }
        public string FormAciklama { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public decimal? Miktar { get; set; }

        public string SayfaNo { get; set; }

        public  decimal MagazaMiktar { get; set; }
        public  decimal DepoMiktar { get; set; }

        public decimal? KritikMiktar { get; set; }
        public  decimal? Kullanim { get; set; }
        public decimal? Toplam { get; set; }

        public decimal Fiyat { get; set; }


    }
}
