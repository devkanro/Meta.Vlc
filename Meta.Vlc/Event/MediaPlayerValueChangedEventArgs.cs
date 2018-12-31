// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: MediaPlayerValueChangedEventArgs.cs
// Version: 20181231

using System;

namespace Meta.Vlc.Event
{
    public class MediaPlayerValueChangedEventArgs<T> : EventArgs
    {
        public MediaPlayerValueChangedEventArgs(T newValue)
        {
            NewValue = newValue;
        }

        public T NewValue { get; }
    }
}