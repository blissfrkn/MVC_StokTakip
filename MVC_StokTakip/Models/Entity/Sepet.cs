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
    
    public partial class Sepet
    {
        public int ID { get; set; }
        public int KullaniciID { get; set; }
        public int UrunID { get; set; }
        public decimal BirimFiyati { get; set; }
        public decimal Miktari { get; set; }
        public decimal ToplamFiyat { get; set; }
        public System.DateTime Tarih { get; set; }
        public System.DateTime Saat { get; set; }
    
        public virtual Kullanicilar Kullanicilar { get; set; }
        public virtual Urunler Urunler { get; set; }
    }
}
