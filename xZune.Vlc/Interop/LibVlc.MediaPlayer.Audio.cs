using System;
using System.Runtime.InteropServices;

namespace xZune.Vlc.Interop.MediaPlayer
{
    /// <summary>
    /// 切换音频静音状态
    /// </summary>
    /// <param name="mediaPlayer"></param>
    [LibVlcFunction("libvlc_audio_toggle_mute")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ToggleMute(IntPtr mediaPlayer);

    /// <summary>
    /// 获取音频静音状态
    /// </summary>
    /// <param name="mediaPlayer"></param>
    /// <returns>0为正常,1为静音,-1为未定义</returns>
    [LibVlcFunction("libvlc_audio_get_mute")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetMute(IntPtr mediaPlayer);

    /// <summary>
    /// 设置音频静音状态
    /// </summary>
    /// <param name="mediaPlayer"></param>
    /// <param name="status"></param>
    [LibVlcFunction("libvlc_audio_set_mute")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetMute(IntPtr mediaPlayer,int status);

    /// <summary>
    /// 获取音频音量
    /// </summary>
    /// <param name="mediaPlayer"></param>
    /// <returns>0~100之间</returns>
    [LibVlcFunction("libvlc_audio_get_volume")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetVolume(IntPtr mediaPlayer);

    /// <summary>
    /// 设置音频音量
    /// </summary>
    /// <param name="mediaPlayer"></param>
    /// <param name="volume">0~100之间</param>
    [LibVlcFunction("libvlc_audio_set_volume")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetVolume(IntPtr mediaPlayer, int volume);

    /// <summary>
    /// 获取音频输出通道
    /// </summary>
    /// <param name="mediaPlayer"></param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_audio_get_channel")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate AudioOutputChannel GetOutputChannel(IntPtr mediaPlayer);

    /// <summary>
    /// 设置音频输出通道
    /// </summary>
    /// <param name="mediaPlayer"></param>
    /// <param name="channel"></param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_audio_set_channel")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SetOutputChannel(IntPtr mediaPlayer, AudioOutputChannel channel);

    /// <summary>
    /// 获取音频轨道数
    /// </summary>
    /// <param name="mediaPlayer"></param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_audio_get_track_count")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetAudioTrackCount(IntPtr mediaPlayer);

    /// <summary>
    /// 获取当前音轨
    /// </summary>
    /// <param name="mediaPlayer"></param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_audio_get_track")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetAudioTrack(IntPtr mediaPlayer);

    /// <summary>
    /// 设置当前音轨
    /// </summary>
    /// <param name="mediaPlayer"></param>
    /// <param name="track"></param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_audio_set_track")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SetAudioTrack(IntPtr mediaPlayer, int track);

    /// <summary>
    /// 获取音轨描述
    /// </summary>
    /// <param name="mediaPlayer"></param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_audio_get_track_description")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GetAudioTrackDescription(IntPtr mediaPlayer);

    public enum AudioOutputChannel
    {
        Error = -1,
        Stereo = 1,
        RStereo,
        Left,
        Right,
        Dolbys
    }
}
