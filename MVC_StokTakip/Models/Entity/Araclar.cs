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
    
    public partial class Araclar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Araclar()
        {
            this.Tamiratlar = new HashSet<Tamiratlar>();
            this.Servis = new HashSet<Servis>();
        }
    
        public int ID { get; set; }
        public Nullable<int> MusteriID { get; set; }
        public int MarkaID { get; set; }
        public string Plaka { get; set; }
        public string Model { get; set; }
        public string SasiNo { get; set; }
        public string Renk { get; set; }
    
        public virtual AracMarka AracMarka { get; set; }
        public virtual Musteriler Musteriler { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tamiratlar> Tamiratlar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Servis> Servis { get; set; }
    }
}