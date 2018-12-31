// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: AudioDevice.cs
// Version: 20181231

using System;
using System.Collections;
using System.Collections.Generic;
using Meta.Vlc.Interop.MediaPlayer;
using Meta.Vlc.Interop.MediaPlayer.Audio;

namespace Meta.Vlc
{
    /// <summary>
    ///     A wrapper for <see cref="libvlc_audio_output_device_t" /> struct.
    /// </summary>
    public unsafe class AudioDevice
    {
        internal AudioDevice(libvlc_audio_output_device_t* pointer)
        {
            if (pointer == null) return;

            Device = InteropHelper.PtrToString(pointer->psz_device);
            Description = InteropHelper.PtrToString(pointer->psz_description);
        }

        public string Device { get; }

        public string Description { get; }
    }

    /// <summary>
    ///     A list wrapper for <see cref="libvlc_audio_output_device_t" /> linked list struct.
    /// </summary>
    public unsafe class AudioDeviceList : VlcUnmanagedLinkedList<AudioDevice>
    {
        public AudioDeviceList(void* pointer) : base(pointer)
        {
        }

        protected override AudioDevice CreateItem(void* data)
        {
            return new AudioDevice((libvlc_audio_output_device_t*) data);
        }

        protected override void* NextItem(void* data)
        {
            return ((libvlc_audio_output_device_t*) data)->p_next;
        }

        protected override void Release(void* data)
        {
            LibVlcManager.GetFunctionDelegate<libvlc_audio_output_device_list_release>()
                .Invoke((libvlc_audio_output_device_t*) data);
        }
    }
}