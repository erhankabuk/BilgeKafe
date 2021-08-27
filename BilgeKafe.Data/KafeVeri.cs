﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeKafe.Data
{
   public class KafeVeri
    {/** MasaAdet: int
* Urunler: List<Urun>
* AktifSiparisler: List<Siparis>
* GecmisSiparisler: List<Siparis>
*/

        public int MasaAdet { get; set; }
        public List<Urun> Urunler { get; set; }
        public List<Siparis> AktifSiparisler { get; set; }
        public List<Siparis> GecmisSiparisler { get; set; }
    }
}