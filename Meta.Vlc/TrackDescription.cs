// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: TrackDescription.cs
// Version: 20160214

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Meta.Vlc
{
    /// <summary>
    ///     A warpper for <see cref="Interop.MediaPlayer.TrackDescription" /> struct.
    /// </summary>
    public class TrackDescription
    {
        private IntPtr _pointer;

        internal Interop.MediaPlayer.TrackDescription Struct;

        internal TrackDescription(IntPtr pointer)
        {
            _pointer = pointer;
            if (pointer != IntPtr.Zero)
            {
                Struct =
                    (Interop.MediaPlayer.TrackDescription)
                        Marshal.PtrToStructure(pointer, typeof (Interop.MediaPlayer.TrackDescription));
                Name = InteropHelper.PtrToString(Struct.Name);
                Id = Struct.Id;
            }
        }

        public String Name { get; private set; }

        public int Id { get; private set; }
    }

    /// <summary>
    ///     A list warpper for <see cref="Interop.MediaPlayer.TrackDescription" /> linklist struct.
    /// </summary>
    public class TrackDescriptionList : IDisposable, IEnumerable<TrackDescription>, IEnumerable
    {
        private List<TrackDescription> _list;
        private IntPtr _pointer;

        /// <summary>
        ///     Create a readonly list by a pointer of <see cref="Interop.MediaPlayer.TrackDescription" />.
        /// </summary>
        /// <param name="pointer"></param>
        public TrackDescriptionList(IntPtr pointer)
        {
            _list = new List<TrackDescription>();
            _pointer = pointer;

            while (pointer != IntPtr.Zero)
            {
                var trackDescription = new TrackDescription(pointer);
                _list.Add(trackDescription);

                pointer = trackDescription.Struct.Next;
            }
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public TrackDescription this[int index]
        {
            get { return _list[index]; }
        }

        public void Dispose()
        {
            if (_pointer == IntPtr.Zero) return;

            LibVlcManager.ReleaseTrackDescriptionList(_pointer);
            _pointer = IntPtr.Zero;
            _list.Clear();
        }

        public IEnumerator<TrackDescription> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}