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
        public event EventHandler<MasaTasindiEventArgs> MasaTasindi;
        private readonly KafeVeri db;
        private readonly Siparis siparis;
        private readonly BindingList<SiparisDetay> blSiparisDetaylar;
        public SiparisForm(KafeVeri db, Siparis siparis)
        {
            this.db = db;
            this.siparis = siparis;
            blSiparisDetaylar = new BindingList<SiparisDetay>(siparis.SiparisDetaylar);
            blSiparisDetaylar.ListChanged += BlSiparisDetaylar_ListChanged;
            InitializeComponent();
            dgwSiparisDetaylari.AutoGenerateColumns = false;
            dgwSiparisDetaylari.DataSource = blSiparisDetaylar;
            UrunleriListele();
            MasaNoyuGunceller();
            MasaNolariListele();
            blSiparisDetaylar.ResetBindings();
        }

        private void MasaNolariListele()
        {
            cboMasaNo.DataSource = Enumerable.Range(1, db.MasaAdet).Where(i => !db.AktifSiparisler.Any(s => s.MasaNo == i)).ToList();
        }
        private void BlSiparisDetaylar_ListChanged(object sender, ListChangedEventArgs e)
        {
            OdemeTutariniGuncelle();
        }

        private void OdemeTutariniGuncelle()
        {
            lblOdemeTutari.Text = siparis.ToplamTutarTL;
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
                BirimFiyati = urun.BirimFiyat,
                Adet = adet
            };
            blSiparisDetaylar.Add(sd);

        }

        private void btnAnasayfayaDon_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOdemeAl_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show($"{siparis.ToplamTutarTL} tutarı tahsil edildiyse siparişi kapatılacaktır. Onaylıyor musunuz?", "Ödeme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (dr == DialogResult.Yes)
            {
                SiparisiKapat(SiparisDurum.Odendi);
            }
        }

        private void btnSiparisIptal_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show($"Sipariş iptal edilecektir. Onaylıyor musunuz?", "İptal Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            if (dr == DialogResult.Yes)
            {
                SiparisiKapat(SiparisDurum.Iptal);
            }
        }
        private void SiparisiKapat(SiparisDurum durum)
        {
            siparis.OdenenTutar = durum == SiparisDurum.Odendi ? siparis.ToplamTutar() : 0;
            siparis.Durum = durum;
            siparis.KapanisZamani = DateTime.Now;
            db.AktifSiparisler.Remove(siparis);
            db.GecmisSiparisler.Add(siparis);
            Close();
        }

        private void btnMasaTasi_Click(object sender, EventArgs e)
        {
            int eskiMasaNo = siparis.MasaNo;
            int yeniMasaNo = (int)cboMasaNo.SelectedItem;
            siparis.MasaNo = yeniMasaNo;
            MasaNoyuGunceller();
            MasaNolariListele();
            MasaTasindiEventArgs args = new MasaTasindiEventArgs()
            {
                EskiMasaNo = eskiMasaNo,
                YeniMasaNo = yeniMasaNo
            };
            if (MasaTasindi != null)
                MasaTasindi(this, args);
        }
    }
}
