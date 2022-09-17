//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVC_StokTakip.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public partial class Tamiratlar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tamiratlar()
        {
            this.TamiratKalem = new HashSet<TamiratKalem>();
        }


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

        public virtual Araclar Araclar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TamiratKalem> TamiratKalem { get; set; }
    }
}
