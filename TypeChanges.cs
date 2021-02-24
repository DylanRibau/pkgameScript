using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pkgameScript
{
    class TypeChanges
    {
        public string Dex_no { get; set; }
        public string Name { get; set; }
        public PkmnType Old_Type { get; set; }
        public PkmnType New_Type { get; set; }
        public string Justification { get; set; }

        public TypeChanges()
        {

        }
    }
}
