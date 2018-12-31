// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: MediaParsedStatusChangedEventArgs.cs
// Version: 20181231

using System;

namespace Meta.Vlc.Event
{
    public class MediaParsedStatusChangedEventArgs : EventArgs
    {
        public MediaParsedStatusChangedEventArgs(MediaParsedStatus status)
        {
            Status = status;
        }

        public MediaParsedStatus Status { get; }
    }
}