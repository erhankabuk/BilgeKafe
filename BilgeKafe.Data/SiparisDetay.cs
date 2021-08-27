﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeKafe.Data
{
    public class SiparisDetay
    {
        public string UrunAd { get; set; }
        public decimal BirimFiyati { get; set; }
        public int Adet { get; set; }
        public string TutarTL
        {
            get
            {
                return $"{Tutar():n2}₺";
            }
        }
        public decimal Tutar()
        {
            return Adet * BirimFiyati;
        }

    }
}
