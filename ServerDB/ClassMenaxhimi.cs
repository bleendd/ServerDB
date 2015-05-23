using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ServerDB
{
    public class ClassMenaxhimi
    {
        [XmlAttribute("Java")]
        public int Java { get; set; }
        public string Ekipi1 { get; set; }
        public string Ekipi2 { get; set; }
        public int GolaE1 { get; set; }
        public int GolaE2 { get; set; }
        public string Komenti { get; set; }
    }
}
