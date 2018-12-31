// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: MediaPlayerBufferingEventArgs.cs
// Version: 20181231

using System;

namespace Meta.Vlc.Event
{
    public class MediaPlayerBufferingEventArgs : EventArgs
    {
        public MediaPlayerBufferingEventArgs(float newCache)
        {
            NewCache = newCache;
        }

        public float NewCache { get; }
    }
}