// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: MediaDurationChangedEventArgs.cs
// Version: 20181231

using System;

namespace Meta.Vlc.Event
{
    public class MediaDurationChangedEventArgs : EventArgs
    {
        public MediaDurationChangedEventArgs(long duration)
        {
            Duration = duration;
        }

        public long Duration { get; }
    }
}