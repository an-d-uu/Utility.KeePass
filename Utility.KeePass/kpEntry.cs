using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.KeePass
{
    public class kpEntry
    {
        public string Title { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string URL { get; set; }
        public string Notes { get; set; }
        public kpGroup Group { get; set; }

        public kpEntry() : base()
        {
            Title = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;
            URL = string.Empty;
            Notes = string.Empty;
            Group = new kpGroup();
        }
    }
}
