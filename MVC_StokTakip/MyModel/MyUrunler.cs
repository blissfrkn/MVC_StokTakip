using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_StokTakip.MyModel
{
    public class MyUrunler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MyUrunler()
        {
            //this.Satislar = new HashSet<Satislar>();
            //this.Sepet = new HashSet<Sepet>();
            //this.MarkaListesi = new List<SelectListItem>();
            //MarkaListesi.Insert(0, new SelectListItem { Text = "Önce Kategori seçilmelidir.", Value = "" });

        }

        public int ID { get; set; }

        public int KategoriID { get; set; }
   

        public string UrunAdi { get; set; }

        [Display(Name = "Barkod No")]

        public string BarkodNo { get; set; }
        //[Required(ErrorMessage = "Stok Kodu alanı boş bırakılamaz.")]
        //[Display(Name = "Stok Kodu")]
        
        //public string StokKodu { get; set; }


        [Display(Name = "Oem Kodu")]

        public string OemKod { get; set; }
 
        [Display(Name = "Alış Fiyatı")]
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true)]
        public decimal? AlisFiyati { get; set; }
      
        [Display(Name = "Satış Fiyatı")]
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true)]
        public decimal? SatisFiyati { get; set; }
   
        [Range(0, 100, ErrorMessage = "0-100 arası sayı giriniz.")]
        [Display(Name = "K.D.V")]

        public int? KDV { get; set; }
        [Required(ErrorMessage = "Birim alanı boş bırakılamaz.")]

        public int BirimID { get; set; }
        [Required(ErrorMessage = "Tarih alanı boş bırakılamaz.")]

        [DataType(DataType.Date)]
        public System.DateTime Tarih { get; set; }

        [Display(Name = "Açıklama")]
        public string Aciklama { get; set; }

        [Display(Name = "Miktarı")]
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true)]
        public decimal? Miktari { get; set; }

        public virtual MyBirimler Birimler { get; set; }
        public virtual MyKategoriler Kategoriler { get; set; }

        //public virtual MyMarkalar Markalar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Satislar> Satislar { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Sepet> Sepet { get; set; }

        public List<SelectListItem> KategoriListesi { get; set; }
        //public List<SelectListItem> MarkaListesi { get; set; }
        public List<SelectListItem> BirimListesi { get; set; }
    }
}