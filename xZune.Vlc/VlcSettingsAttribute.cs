using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xZune.Vlc
{
    public class VlcSettingsAttribute : Attribute
    {
        public VlcSettingsAttribute(String vlcPath) : this(vlcPath, null)
        {

        }

        public VlcSettingsAttribute(String vlcPath, String[] option)
        {
            LibVlcPath = vlcPath;
            VlcOption = option;
        }

        public String LibVlcPath { get; set; }

        public String[] VlcOption { get; set; }
    }
}
