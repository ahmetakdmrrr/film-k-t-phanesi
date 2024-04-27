using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NesneDönemProje
{
    class Film
    {
        public string ad { get; set; }
        public string yönetmen { get; set; }
        public List<string> Oyuncular { get; set; }
        public DateTime yayın_yılı { get; set; }
        public double değerlendirme_puanı { get; set; }
    }
}
