using System;
using System.Runtime.InteropServices;

namespace xZune.Vlc.Interop.Time
{
    /// <summary>
    /// 获取由 LibVlc 定义的当前时间
    /// </summary>
    /// <returns>返回由 LibVlc 定义的当前时间</returns>
    [LibVlcFunction("libvlc_clock")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate Int64 Clock();

    /// <summary>
    /// 获取与提供的时间戳之间的延迟
    /// </summary>
    /// <param name="timestamp">时间戳</param>
    /// <returns>返回与提供的时间戳之间的延迟</returns>
    [LibVlcFunction("libvlc_clock")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate Int64 Delay(Int64 timestamp);
}
