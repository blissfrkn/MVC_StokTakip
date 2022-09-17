using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_StokTakip.Models.Entity
{
    public class Kalemler
    {
        public int ID { get; set; }

        public int UrunID { get; set; }
        public int ServisID { get; set; }

        public int TurID { get; set; }
        public int BirimID { get; set; }
        public string StokKodu { get; set; }

        public string KategoriAd { get; set; }

        public string Aciklama { get; set; }


        public string OemKod { get; set; }

        public decimal Miktari { get; set; }
        public decimal BirimFiyat { get; set; }
    }
}