using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_StokTakip.MyModel
{
    public class MyTamirat
    {
        public int ID { get; set; }
        public Nullable<int> AracID { get; set; }
        public Nullable<int> ServisID { get; set; }
        public Nullable<int> DurumID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}")]
        public Nullable<System.DateTime> Tarih { get; set; }
        public Nullable<int> Km { get; set; }
        [AllowHtml]
        public string Not1 { get; set; }
        [AllowHtml]
        public string Not2 { get; set; }
        [AllowHtml]
        public string Not3 { get; set; }
        public Nullable<decimal> YToplam { get; set; }
        public Nullable<decimal> İToplam { get; set; }
        public Nullable<decimal> RotBalans { get; set; }
        public Nullable<decimal> MToplam { get; set; }
        public Nullable<decimal> Toplam { get; set; }
        public Nullable<decimal> İskonto { get; set; }
        public Nullable<decimal> GenelToplam { get; set; }
    }
}