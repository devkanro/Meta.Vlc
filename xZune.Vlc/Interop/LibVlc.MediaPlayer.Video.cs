using System;
using System.Runtime.InteropServices;

namespace xZune.Vlc.Interop.MediaPlayer
{
    [LibVlcFunction("libvlc_video_get_cursor")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetCursor(IntPtr mediaPlayer, uint num, ref int px, ref int py);

    [LibVlcFunction("libvlc_video_set_cursor", "2.2.0", null, "xZune")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SetCursor(IntPtr mediaPlayer, uint num, int px, int py);

    [LibVlcFunction("libvlc_video_set_mouse_down", "2.2.0", null, "xZune")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SetMouseDown(IntPtr mediaPlayer, uint num, MouseButton mouseButton);

    [LibVlcFunction("libvlc_video_set_mouse_up", "2.2.0", null, "xZune")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SetMouseUp(IntPtr mediaPlayer, uint num, MouseButton mouseButton);
    
    [LibVlcFunction("libvlc_video_get_size")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetSize(IntPtr mediaPlayer, uint num, ref uint px, ref uint py);

    [LibVlcFunction("libvlc_video_get_scale")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate float GetScale(IntPtr mediaPlayer);

    [LibVlcFunction("libvlc_video_set_scale")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetScale(IntPtr mediaPlayer, float scale);

    [LibVlcFunction("libvlc_video_get_aspect_ratio")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GetAspectRatio(IntPtr mediaPlayer);

    [LibVlcFunction("libvlc_video_set_aspect_ratio")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetAspectRatio(IntPtr mediaPlayer, IntPtr scale);

    public enum MouseButton
    {
        Left,
        Right,
        Other
    }
}
