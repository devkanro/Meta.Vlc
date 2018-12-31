// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: VlcSettingsAttribute.cs
// Version: 20181231

using System;

namespace Meta.Vlc
{
    public class VlcSettingsAttribute : Attribute
    {
        public VlcSettingsAttribute(string vlcPath) : this(vlcPath, null)
        {
        }

        public VlcSettingsAttribute(string vlcPath, params string[] option)
        {
            LibVlcPath = vlcPath;
            VlcOption = option;
        }

        public string LibVlcPath { get; set; }

        public string[] VlcOption { get; set; }
    }
}