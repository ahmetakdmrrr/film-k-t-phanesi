using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NesneDönemProje
{
    public class Premium:kullanıcı
    {
        public Premium(string ad_soyad, string uyelik_turu, DateTime dogum_tarihi, string kulanici_adi, string Sifre, string tc_kimlik, string cinsiyet)
        {
            this.ad_soyad = ad_soyad;
            this.uyelik_türü = uyelik_turu;
            this.dogum_tarihi = dogum_tarihi;
            this.kullanici_adi = kulanici_adi;
            this.Sifre = Sifre;
            this.tc_kimlik = tc_kimlik;
            this.cinsiyet = cinsiyet;


        }
            
    


        public override int üyelik_ücreti()
        {
            return base.üyelik_ücreti() + (base.üyelik_ücreti()/4);
        }
    }
}
