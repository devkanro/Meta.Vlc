// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: MediaStateChangedEventArgs.cs
// Version: 20181231

using System;

namespace Meta.Vlc.Event
{
    public class MediaStateChangedEventArgs : EventArgs
    {
        public MediaStateChangedEventArgs(MediaState state)
        {
            State = state;
        }

        public MediaState State { get; }
    }
}