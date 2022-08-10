using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMappingBuilder
{
    internal class Groupe_Feuilles
    {
        public int key { get; set; }
        public string value { get; set; }
        public IList<Racine> sous_groupe { get; set; }
        public IList<Feuille> feuilles { get; set; }
    }
}
