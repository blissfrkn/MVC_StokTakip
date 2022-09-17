using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_StokTakip.Models.Entity
{
    public class TransferUrun
    {
        public int CikisDepoID { get; set; }

        public int GirisDepoID { get; set; }
        public int ID { get; set; }

        public decimal Miktar { get; set; }

        public int BirimID { get; set; }

    }
}