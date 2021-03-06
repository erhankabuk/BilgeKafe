using BilgeKafe.Data;
using BilgeKafe.UI.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BilgeKafe.UI
{
    public partial class AnaForm : Form
    {
        KafeVeri db = new KafeVeri();
        public AnaForm()
        {
            VerileriOku();
            InitializeComponent();
            MasalariOlustur();
        }

        private void VerileriOku()
        {
            try
            {
                string json = File.ReadAllText("veri.json");//Diskten okuma
                db = JsonConvert.DeserializeObject<KafeVeri>(json);//Deserialization
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void MasalariOlustur()
        {
            ImageList imageList = new ImageList();
            imageList.Images.Add("bos", Resources.bos);
            imageList.Images.Add("dolu", Resources.dolu);
            imageList.ImageSize = new Size(64, 64);
            lvmMasalar.LargeImageList = imageList;

            for (int i = 1; i <= db.MasaAdet; i++)
            {
                ListViewItem lvi = new ListViewItem($"Masa {i}");
                lvi.Tag = i;
                lvi.ImageKey = db.AktifSiparisler.Any(s => s.MasaNo == i) ? "dolu" : "bos";
                lvmMasalar.Items.Add(lvi);
            }
        }

        private void lvmMasalar_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem lvi = lvmMasalar.SelectedItems[0];
            int masaNo = (int)lvi.Tag;
            lvi.ImageKey = "dolu";
            Siparis siparis = db.AktifSiparisler.FirstOrDefault(x => x.MasaNo == masaNo);
            if (siparis == null)
            {
                siparis = new Siparis() { MasaNo = masaNo };
                db.AktifSiparisler.Add(siparis);
            }
            SiparisForm frmSiparis = new SiparisForm(db, siparis);
            frmSiparis.MasaTasindi += FrmSiparis_MasaTasindi;
            frmSiparis.ShowDialog();
            if (siparis.Durum != SiparisDurum.Aktif)
                lvi.ImageKey = "bos";
        }

        private void FrmSiparis_MasaTasindi(object sender, MasaTasindiEventArgs e)
        {
            foreach (ListViewItem lvi in lvmMasalar.Items)
            {
                if ((int)lvi.Tag == e.EskiMasaNo)
                    lvi.ImageKey = "bos";
                if ((int)lvi.Tag == e.YeniMasaNo)
                    lvi.ImageKey = "dolu";
            }
        }

        private void tsmiUrunler_Click(object sender, EventArgs e)
        {
            new UrunlerForm(db).ShowDialog();
        }

        private void tsmiGecmisSiparisler_Click(object sender, EventArgs e)
        {
            new GecmisSiparislerForm(db).ShowDialog();
        }

        private void AnaForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string json = JsonConvert.SerializeObject(db);//Serialization
            File.WriteAllText("veri.json", json);//diske yazılması.
        }
    }
}
