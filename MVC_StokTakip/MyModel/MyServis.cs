using MVC_StokTakip.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_StokTakip.MyModel
{
    public class MyServis
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MyServis()
        {

        }

        public int ID { get; set; }
        public int DurumID { get; set; }
        public int AracID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}")]
        public System.DateTime Tarih { get; set; }
        public int Km { get; set; }
        [AllowHtml]
        public string Not1 { get; set; }
        [AllowHtml]
        public string Not2 { get; set; }
        [AllowHtml]
        public string Not3 { get; set; }
        public decimal YToplam { get; set; }
        public decimal İToplam { get; set; }
        public decimal RotBalans { get; set; }
        public decimal MToplam { get; set; }
        public decimal Toplam { get; set; }
        public decimal İskonto { get; set; }
        public decimal GenelToplam { get; set; }
        public virtual Araclar Araclar { get; set; }
        public virtual Durumlar Durumlar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServisKalem> ServisKalem { get; set; }
    }
}