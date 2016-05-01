// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: AudioOutput.cs
// Version: 20160214

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Meta.Vlc
{
    /// <summary>
    ///     A warpper for <see cref="Interop.MediaPlayer.AudioOutput" /> struct.
    /// </summary>
    public class AudioOutput
    {
        internal IntPtr _pointer;

        internal Interop.MediaPlayer.AudioOutput _struct;

        internal AudioOutput(IntPtr pointer)
        {
            _pointer = pointer;
            if (pointer != IntPtr.Zero)
            {
                _struct =
                    (Interop.MediaPlayer.AudioOutput)
                        Marshal.PtrToStructure(pointer, typeof (Interop.MediaPlayer.AudioOutput));
                Name = InteropHelper.PtrToString(_struct.Name);
                Description = InteropHelper.PtrToString(_struct.Description);
            }
        }

        public String Name { get; private set; }

        public String Description { get; private set; }
    }

    /// <summary>
    ///     A list warpper for <see cref="Interop.MediaPlayer.AudioOutput" /> linklist struct.
    /// </summary>
    public class AudioOutputList : IDisposable, IEnumerable<AudioOutput>, IEnumerable
    {
        private List<AudioOutput> _list;
        private IntPtr _pointer;

        /// <summary>
        ///     Create a readonly list by a pointer of <see cref="Interop.MediaPlayer.AudioOutput" />.
        /// </summary>
        /// <param name="pointer"></param>
        public AudioOutputList(IntPtr pointer)
        {
            _list = new List<AudioOutput>();
            _pointer = pointer;

            while (pointer != IntPtr.Zero)
            {
                var audioOutput = new AudioOutput(pointer);
                _list.Add(audioOutput);

                pointer = audioOutput._struct.Next;
            }
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public AudioOutput this[int index]
        {
            get { return _list[index]; }
        }

        public void Dispose()
        {
            if (_pointer == IntPtr.Zero) return;

            LibVlcManager.ReleaseAudioOutputList(_pointer);
            _pointer = IntPtr.Zero;
            _list.Clear();
        }

        public IEnumerator<AudioOutput> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}