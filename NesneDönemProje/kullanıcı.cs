using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NesneDönemProje
{
    abstract public class kullanıcı
    {   
        public string ad_soyad { get; set; }
        
        public string uyelik_türü { get; set; }
        public DateTime dogum_tarihi { get; set; }
        public string kullanici_adi { get;set; }
        public string Sifre { get; set; }
        public string tc_kimlik { get; set; }
        public string cinsiyet { get; set; }
        

        public virtual int üyelik_ücreti()
        {
            return 100;
        }



    }
}
