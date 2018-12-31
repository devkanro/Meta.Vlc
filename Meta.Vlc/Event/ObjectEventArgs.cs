// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: ObjectEventArgs.cs
// Version: 20181231

using System;

namespace Meta.Vlc.Event
{
    public class ObjectEventArgs<T> : EventArgs
    {
        public ObjectEventArgs()
        {
        }

        public ObjectEventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; set; }
    }
}