// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: MediaMetaChangedEventArgs.cs
// Version: 20181231

using System;

namespace Meta.Vlc.Event
{
    public class MediaMetaChangedEventArgs : EventArgs
    {
        public MediaMetaChangedEventArgs(MediaMetaType type)
        {
            Type = type;
        }

        public MediaMetaType Type { get; }
    }
}