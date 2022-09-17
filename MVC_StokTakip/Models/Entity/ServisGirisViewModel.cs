using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_StokTakip.Models.Entity
{
    public class ServisGirisViewModel
    {


        
        public string Plaka { get; set; }
        
        public string Marka { get; set; }
       
        public string Model { get; set; }
       
        public string Ad { get; set; }
     
        public string Soyad { get; set; }

        public int DurumID { get; set; }
    }
}