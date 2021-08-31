using BilgeKafe.Data;
using BilgeKafe.UI.Properties;
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
    public partial class AnaForm : Form
    {   KafeVeri db= new KafeVeri();
        public AnaForm()
        {
            OrnekUrunleriOlustur();
            InitializeComponent();
            MasalariOlustur();
        }

        private void OrnekUrunleriOlustur()
        {
            db.Urunler.Add(new Urun() { UrunAd = "Kola", BirimFiyat = 5.99m });
            db.Urunler.Add(new Urun() { UrunAd = "Ayran", BirimFiyat = 4.50m });
            db.Urunler.Add(new Urun() { UrunAd = "Çay", BirimFiyat = 2.99m });



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
                lvi.ImageKey = "bos";
                lvmMasalar.Items.Add(lvi);
            }
        }

        private void lvmMasalar_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem lvi = lvmMasalar.SelectedItems[0];
            lvi.ImageKey = "dolu";
            int masaNo = (int)lvi.Tag;
            //Tıklanan masaya ait vaesrsa siparişi bul
            Siparis siparis = db.AktifSiparisler.FirstOrDefault(x => x.MasaNo == masaNo);
            //siparis oluşturulmadıysa o masanın
            if (siparis == null)
            {
                siparis = new Siparis() { MasaNo = masaNo};
                db.AktifSiparisler.Add(siparis);
            }
            

            SiparisForm frmSiparis = new SiparisForm(db, siparis);
            frmSiparis.ShowDialog();
           
        }
    }
}
