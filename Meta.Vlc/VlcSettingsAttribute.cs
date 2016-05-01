// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: VlcSettingsAttribute.cs
// Version: 20160214

using System;

namespace Meta.Vlc
{
    public class VlcSettingsAttribute : Attribute
    {
        public VlcSettingsAttribute(String vlcPath) : this(vlcPath, null)
        {
        }

        public VlcSettingsAttribute(String vlcPath, params String[] option)
        {
            LibVlcPath = vlcPath;
            VlcOption = option;
        }

        public String LibVlcPath { get; set; }

        public String[] VlcOption { get; set; }
    }
}