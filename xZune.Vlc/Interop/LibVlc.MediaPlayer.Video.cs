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

    public enum MouseButton
    {
        Left,
        Right,
        Other
    }
}
