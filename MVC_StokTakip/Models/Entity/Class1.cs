using MVC_StokTakip.MyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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


        public ServisKalem ServisKalem { get; set; }

        public Tamiratlar Tamiratlar { get; set; }

        public TamiratKalem TamiratKalem { get; set; }

        public List<Araclar> Araclars { get; set; }

        public Araclar Araclar { get; set; }

        public Musteriler Musteriler { get; set; }
    }
}