// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: AudioOutputChannel.cs
// Version: 20181231

using Meta.Vlc.Interop.MediaPlayer.Audio;

namespace Meta.Vlc
{
    /// <summary>
    ///     Audio channels
    /// </summary>
    public enum AudioOutputChannel
    {
        Error = libvlc_audio_output_channel_t.libvlc_AudioChannel_Error,
        Stereo = libvlc_audio_output_channel_t.libvlc_AudioChannel_Stereo,
        RStereo = libvlc_audio_output_channel_t.libvlc_AudioChannel_RStereo,
        Left = libvlc_audio_output_channel_t.libvlc_AudioChannel_Left,
        Right = libvlc_audio_output_channel_t.libvlc_AudioChannel_Right,
        Dolbys = libvlc_audio_output_channel_t.libvlc_AudioChannel_Dolbys
    }
}