using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.KeePass
{
    public class kpGroup
    {
        public string Name { get; set; }
        public string Notes { get; set; }

        public kpGroup() : base()
        {
            Name = string.Empty;
            Notes = string.Empty;
        }
        public kpGroup(string name, string notes) : base()
        {
            Name = name;
            Notes = notes;
        }
    }
}
