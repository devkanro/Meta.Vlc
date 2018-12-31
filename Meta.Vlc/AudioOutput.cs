// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: AudioOutput.cs
// Version: 20181231

using System;
using System.Collections;
using System.Collections.Generic;
using Meta.Vlc.Interop.MediaPlayer;
using Meta.Vlc.Interop.MediaPlayer.Audio;

namespace Meta.Vlc
{
    /// <summary>
    ///     A wrapper for <see cref="libvlc_audio_output_t" /> struct.
    /// </summary>
    public unsafe class AudioOutput
    {
        internal AudioOutput(libvlc_audio_output_t* pointer)
        {
            if (pointer == null) return;

            Name = InteropHelper.PtrToString(pointer->psz_name);
            Description = InteropHelper.PtrToString(pointer->psz_description);
        }

        public string Name { get; }

        public string Description { get; }
    }

    /// <summary>
    ///     A list wrapper for <see cref="AudioOutput" /> linked list struct.
    /// </summary>
    public unsafe class AudioOutputList : VlcUnmanagedLinkedList<AudioOutput>
    {
        public AudioOutputList(void* pointer) : base(pointer)
        {
        }

        protected override AudioOutput CreateItem(void* data)
        {
            return new AudioOutput((libvlc_audio_output_t*) data);
        }

        protected override void* NextItem(void* data)
        {
            return ((libvlc_audio_output_t*) data)->p_next;
        }

        protected override void Release(void* data)
        {
            LibVlcManager.GetFunctionDelegate<libvlc_audio_output_list_release>().Invoke((libvlc_audio_output_t*) data);
        }
    }
}