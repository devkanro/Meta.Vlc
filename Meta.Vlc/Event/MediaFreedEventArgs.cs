// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: MediaFreedEventArgs.cs
// Version: 20181231

using System;

namespace Meta.Vlc.Event
{
    public class MediaFreedEventArgs : EventArgs
    {
        public MediaFreedEventArgs(IntPtr media)
        {
            Media = media;
        }

        public IntPtr Media { get; }
    }
}