using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;
using MVC_StokTakip.MyModel;

namespace MVC_StokTakip.Models.Entity
{
    public class Class1
    {


        //public IEnumerable<Servis> deger1 { get; set; }
        //public IEnumerable<ServisKalem> deger2 { get; set; }
        public List<Servis> ServisList { get; set; }

        public List<ServisKalem> ServisKalemList { get; set; }

        public List<TamiratKalem> TamiratKalems { get; set; }

        public List<Tamiratlar> Tamiratlars { get; set; }

        public Servis Servis { get; set; }

        public Birimler Birimler { get; set; }

        public Tur Tur { get; set; }

        public ServisKalem ServisKalem { get; set; }

        public Tamiratlar Tamiratlar { get; set; }

        public TamiratKalem TamiratKalem { get; set; }

        public List<Araclar> Araclars { get; set; }

        public Araclar Araclar { get; set; }

        public Musteriler Musteriler { get; set; }

        public MyUrunler Urunler { get; set; }

        public List<Urunler> Urunlers { get; set; }

        public DepoUrun DepoUrun { get; set; }
        public List<DepoUrun> DepoUruns { get; set; }
        public Satislar Satislar { get; set; }
        public Girisler Girisler { get; set; }
        public List<Satislar> Satislars { get; set; }
        public List<Girisler> Girislers { get; set; }
        
        public Depolar Depolar { get; set; }

        public List<DepoUrunModel> DepoUrunModels { get; set; }

        public List<Depolar> Depolars { get; set; }

        public SelectList DepolarListe { get; set; }
        public FiyatForm FiyatForm { get; set; }


    }
}