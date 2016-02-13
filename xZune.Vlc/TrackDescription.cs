//Project: xZune.Vlc (https://github.com/higankanshi/xZune.Vlc)
//Filename: TrackDescription.cs
//Version: 20160213

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace xZune.Vlc
{
    /// <summary>
    /// A warpper for <see cref="Interop.MediaPlayer.TrackDescription"/> struct.
    /// </summary>
    public class TrackDescription
    {
        internal TrackDescription(IntPtr pointer)
        {
            _pointer = pointer;
            if (pointer != IntPtr.Zero)
            {
                _struct = (Interop.MediaPlayer.TrackDescription)Marshal.PtrToStructure(pointer, typeof(Interop.MediaPlayer.TrackDescription));
                Name = InteropHelper.PtrToString(_struct.Name);
                Id = _struct.Id;
            }
        }

        internal Interop.MediaPlayer.TrackDescription _struct;
        internal IntPtr _pointer;

        public String Name { get; private set; }

        public int Id { get; private set; }
    }

    /// <summary>
    /// A list warpper for <see cref="Interop.MediaPlayer.TrackDescription"/> linklist struct.
    /// </summary>
    public class TrackDescriptionList : IDisposable, IEnumerable<TrackDescription>, IEnumerable
    {
        /// <summary>
        /// Create a readonly list by a pointer of <see cref="Interop.MediaPlayer.TrackDescription"/>.
        /// </summary>
        /// <param name="pointer"></param>
        public TrackDescriptionList(IntPtr pointer)
        {
            _list = new List<TrackDescription>();
            _pointer = pointer;

            while (pointer != IntPtr.Zero)
            {
                var TrackDescription = new TrackDescription(pointer);
                _list.Add(TrackDescription);

                pointer = TrackDescription._struct.Next;
            }
        }

        private List<TrackDescription> _list;
        private IntPtr _pointer;

        public IEnumerator<TrackDescription> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
    }
}