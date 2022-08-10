using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMappingBuilder
{
    internal class Branche
    {
        public int key { get; set; }
        public string value { get; set; }
        public ObservableCollection<Branche> sous_groupe { get; set; }
        public ObservableCollection<Feuille> feuille { get; set; }
    }
}
