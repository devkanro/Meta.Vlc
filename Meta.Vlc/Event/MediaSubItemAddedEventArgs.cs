// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: MediaSubItemAddedEventArgs.cs
// Version: 20181231

using System;

namespace Meta.Vlc.Event
{
    public class MediaSubItemAddedEventArgs : EventArgs
    {
        public MediaSubItemAddedEventArgs(IntPtr newChild)
        {
            NewChild = newChild;
        }

        public IntPtr NewChild { get; }
    }
}