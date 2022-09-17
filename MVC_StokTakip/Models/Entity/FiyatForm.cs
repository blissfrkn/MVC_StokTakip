using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_StokTakip.Models.Entity
{
    public class FiyatForm
    {
        public string Plaka { get; set; }
        public int MarkaID { get; set; }
        public string Model { get; set; }
        public string SasiNo { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public Nullable<int> Km { get; set; }
    }
}