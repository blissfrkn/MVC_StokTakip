using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC_StokTakip.Models.Entity;
using MVC_StokTakip.MyModel;

namespace MVC_StokTakip.Controllers
{
    public class EnrollmentDateGroup
    {
        public int ID { get; set; }
        public int KategoriID { get; set; }
        public string UrunAdi { get; set; }
        public string BarkodNo { get; set; }
        public string StokKodu { get; set; }
        public string OemKod { get; set; }
        public decimal AlisFiyati { get; set; }
        public decimal SatisFiyati { get; set; }
        public int KDV { get; set; }
        public int BirimID { get; set; }
        public System.DateTime Tarih { get; set; }
        public string Aciklama { get; set; }
        public string FormAciklama { get; set; }
        public Nullable<bool> IsDelete { get; set; }

        public decimal Miktar { get; set; }
    }
}