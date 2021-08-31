using BilgeKafe.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BilgeKafe.UI
{
    public partial class SiparisForm : Form
    {
        private readonly KafeVeri db;
        private readonly Siparis siparis;
        private readonly BindingList<SiparisDetay> blSiparisDetaylar;
        public SiparisForm(KafeVeri db, Siparis siparis)
        {
            this.db = db;//contructor ve propertyde aynı isim verildiğii için this le eşitleme yapılıyor.
            this.siparis = siparis;
            blSiparisDetaylar = new BindingList<SiparisDetay>(siparis.SiparisDetaylar);
            InitializeComponent();
            UrunleriListele();
            MasaNoyuGunceller();
            dgwSiparisDetaylari.DataSource = blSiparisDetaylar;
        }

        private void UrunleriListele()
        {
            cboUrun.DataSource = db.Urunler;
        }

        private void MasaNoyuGunceller()
        {
            Text = $"Masa {siparis.MasaNo} Açılış Zamanı {siparis.AcilisZamani}";
            lblMasaNo.Text = $"{siparis.MasaNo:00}";

        }

        private void btnDetayEkle_Click(object sender, EventArgs e)
        {
            Urun urun = (Urun)cboUrun.SelectedItem;
            int adet = (int)nudAdet.Value;


            if (urun == null)
            {
                MessageBox.Show("Bir ürün seçin");
                return;
            }
            SiparisDetay sd = new SiparisDetay()
            {
                UrunAd = urun.UrunAd,
                BirimFiyati=urun.BirimFiyat,
                Adet=adet
            };
            blSiparisDetaylar.Add(sd);
            //todo: ödeme tutarını güncelle view tasklist
        }

       
    }
}
