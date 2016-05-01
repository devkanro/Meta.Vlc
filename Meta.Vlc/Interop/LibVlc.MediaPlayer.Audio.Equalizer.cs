// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: LibVlc.MediaPlayer.Audio.Equalizer.cs
// Version: 20160214

using System;
using System.Runtime.InteropServices;

namespace Meta.Vlc.Interop.MediaPlayer
{
    /// <summary>
    ///     获取预设均衡器数量
    /// </summary>
    /// <param name="mediaPlayer"></param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_audio_equalizer_get_preset_count", "2.2.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate uint GetEqualizerPresetCount();

    /// <summary>
    ///     获取预设均衡器名称
    /// </summary>
    /// <param name="index">均衡器编号</param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_audio_equalizer_get_preset_name", "2.2.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GetEqualizerPresetName(uint index);

    /// <summary>
    ///     获取均衡器频带数目
    /// </summary>
    /// <returns></returns>
    [LibVlcFunction("libvlc_audio_equalizer_get_band_count", "2.2.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate uint GetEqualizerBandCount();

    /// <summary>
    ///     获取均衡器频带的频率
    /// </summary>
    /// <param name="index">频带编号</param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_audio_equalizer_get_band_frequency", "2.2.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate float GetEqualizerBandFrequency(uint index);

    /// <summary>
    ///     创建一个新的均衡器
    /// </summary>
    /// <returns></returns>
    [LibVlcFunction("libvlc_audio_equalizer_new", "2.2.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr CreateEqualizer();

    /// <summary>
    ///     从预设创建一个新的均衡器
    /// </summary>
    /// <param name="index">预设均衡器编号</param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_audio_equalizer_new_from_preset", "2.2.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr CreateEqualizerFromPreset(uint index);

    /// <summary>
    ///     释放均衡器
    /// </summary>
    /// <returns></returns>
    [LibVlcFunction("libvlc_audio_equalizer_release", "2.2.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr ReleaseEqualizer(IntPtr equalizer);

    /// <summary>
    ///     设置均衡器的新预设放大值
    /// </summary>
    /// <param name="equalizer"></param>
    /// <param name="preamp">取值范围为 -20.0~+20.0</param>
    /// <returns>0 成功，-1 失败</returns>
    [LibVlcFunction("libvlc_audio_equalizer_set_preamp", "2.2.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SetEqualizerPreamp(IntPtr equalizer, float preamp);

    /// <summary>
    ///     获取均衡器的新预设放大值
    /// </summary>
    /// <param name="equalizer"></param>
    [LibVlcFunction("libvlc_audio_equalizer_get_preamp", "2.2.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate float GetEqualizerPreamp(IntPtr equalizer);

    /// <summary>
    ///     设置均衡器的放大值
    /// </summary>
    /// <param name="equalizer">均衡器</param>
    /// <param name="preamp">取值范围为 -20.0~+20.0</param>
    /// <param name="band">屏带编号</param>
    /// <returns>0 成功，-1 失败</returns>
    [LibVlcFunction("libvlc_audio_equalizer_set_amp_at_index", "2.2.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SetEqualizerAmplification(IntPtr equalizer, float preamp, uint band);

    /// <summary>
    ///     获取均衡器的放大值
    /// </summary>
    /// <param name="equalizer">均衡器</param>
    /// <param name="band">屏带编号</param>
    [LibVlcFunction("libvlc_audio_equalizer_get_amp_at_index", "2.2.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate float GetEqualizerAmplification(IntPtr equalizer, uint band);

    /// <summary>
    ///     为播放器设置均衡器，提供 NULL 来关闭均衡器，在该方法返回后即可立即释放均衡器，播放器不会引用均衡器实例
    /// </summary>
    /// <param name="meidaPlayer">播放器</param>
    /// <param name="equalizer">均衡器</param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_media_player_set_equalizer", "2.2.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SetEqualizer(IntPtr meidaPlayer, IntPtr equalizer);
}