// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: MediaState.cs
// Version: 20181231

using System;
using Meta.Vlc.Interop.Media;

namespace Meta.Vlc
{
    public enum MediaState
    {
        NothingSpecial = libvlc_state_t.libvlc_NothingSpecial,
        Opening = libvlc_state_t.libvlc_Opening,

        [Obsolete(
            "Deprecated value. Check the VlcMediaPlayer.Buffering event to know the buffering state of a VlcMediaPlayer")]
        Buffering = libvlc_state_t.libvlc_Buffering,
        Playing = libvlc_state_t.libvlc_Playing,
        Paused = libvlc_state_t.libvlc_Paused,
        Stopped = libvlc_state_t.libvlc_Stopped,
        Ended = libvlc_state_t.libvlc_Ended,
        Error = libvlc_state_t.libvlc_Error
    }
}