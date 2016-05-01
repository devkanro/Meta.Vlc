// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: AudioDevice.cs
// Version: 20160214

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Meta.Vlc
{
    /// <summary>
    ///     A warpper for <see cref="Interop.MediaPlayer.AudioDevice" /> struct.
    /// </summary>
    public class AudioDevice
    {
        internal IntPtr _pointer;

        internal Interop.MediaPlayer.AudioDevice _struct;

        internal AudioDevice(IntPtr pointer)
        {
            _pointer = pointer;
            if (pointer != IntPtr.Zero)
            {
                _struct =
                    (Interop.MediaPlayer.AudioDevice)
                        Marshal.PtrToStructure(pointer, typeof (Interop.MediaPlayer.AudioDevice));
                Device = InteropHelper.PtrToString(_struct.Device);
                Description = InteropHelper.PtrToString(_struct.Description);
            }
        }

        public String Device { get; private set; }

        public String Description { get; private set; }
    }

    /// <summary>
    ///     A list warpper for <see cref="Interop.MediaPlayer.AudioDevice" /> linklist struct.
    /// </summary>
    public class AudioDeviceList : IDisposable, IEnumerable<AudioDevice>, IEnumerable
    {
        private List<AudioDevice> _list;
        private IntPtr _pointer;

        /// <summary>
        ///     Create a readonly list by a pointer of <see cref="Interop.MediaPlayer.AudioDevice" />.
        /// </summary>
        /// <param name="pointer"></param>
        public AudioDeviceList(IntPtr pointer)
        {
            _list = new List<AudioDevice>();
            _pointer = pointer;

            while (pointer != IntPtr.Zero)
            {
                var AudioDevice = new AudioDevice(pointer);
                _list.Add(AudioDevice);

                pointer = AudioDevice._struct.Next;
            }
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public AudioDevice this[int index]
        {
            get { return _list[index]; }
        }

        public void Dispose()
        {
            if (_pointer == IntPtr.Zero) return;

            LibVlcManager.ReleaseAudioDeviceList(_pointer);
            _pointer = IntPtr.Zero;
            _list.Clear();
        }

        public IEnumerator<AudioDevice> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}