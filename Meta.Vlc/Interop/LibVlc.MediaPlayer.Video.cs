// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: LibVlc.MediaPlayer.Video.cs
// Version: 20160214

using System;
using System.Runtime.InteropServices;

namespace Meta.Vlc.Interop.MediaPlayer
{
    /// <summary>
    ///     Get the mouse pointer coordinates over a video.
    ///     Coordinates are expressed in terms of the decoded video resolution, not in terms of pixels on the screen/viewport
    ///     (to get the latter, you can query your windowing system directly).
    ///     Either of the coordinates may be negative or larger than the corresponding dimension of the video, if the cursor is
    ///     outside the rendering area.
    /// </summary>
    /// <param name="mediaPlayer">media player </param>
    /// <param name="num">number of the video (starting from, and most commonly 0) </param>
    /// <param name="px">pointer to get the abscissa [OUT] </param>
    /// <param name="py">pointer to get the ordinate [OUT] </param>
    /// <returns>0 on success, -1 if the specified video does not exist </returns>
    [LibVlcFunction("libvlc_video_get_cursor")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetCursor(IntPtr mediaPlayer, uint num, ref int px, ref int py);

    /// <summary>
    ///     Set the mouse pointer coordinates over a video.
    ///     This is a special function of xZune dev version. If you display using HWND, you will needn't this function.
    /// </summary>
    /// <param name="mediaPlayer">media player </param>
    /// <param name="num">number of the video (starting from, and most commonly 0) </param>
    /// <param name="px">pointer to get the abscissa [OUT] </param>
    /// <param name="py">pointer to get the ordinate [OUT] </param>
    /// <returns>0 on success, -1 if the specified video does not exist </returns>
    [LibVlcFunction("libvlc_video_set_cursor", "2.2.0", null, "xZune")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SetCursor(IntPtr mediaPlayer, uint num, int px, int py);

    /// <summary>
    ///     Set the a mouse button is down.
    ///     This is a special function of xZune dev version. If you display using HWND, you will needn't this function.
    /// </summary>
    /// <param name="mediaPlayer">media player </param>
    /// <param name="num">number of the video (starting from, and most commonly 0) </param>
    /// <param name="mouseButton">a enum of mouse button </param>
    /// <returns>0 on success, -1 if the specified video does not exist </returns>
    [LibVlcFunction("libvlc_video_set_mouse_down", "2.2.0", null, "xZune")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SetMouseDown(IntPtr mediaPlayer, uint num, MouseButton mouseButton);

    /// <summary>
    ///     Set the a mouse button is up.
    ///     This is a special function of xZune dev version. If you display using HWND, you will needn't this function.
    /// </summary>
    /// <param name="mediaPlayer">media player </param>
    /// <param name="num">number of the video (starting from, and most commonly 0) </param>
    /// <param name="mouseButton">a enum of mouse button </param>
    /// <returns>0 on success, -1 if the specified video does not exist </returns>
    [LibVlcFunction("libvlc_video_set_mouse_up", "2.2.0", null, "xZune")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SetMouseUp(IntPtr mediaPlayer, uint num, MouseButton mouseButton);

    /// <summary>
    ///     Get the pixel dimensions of a video.
    /// </summary>
    /// <param name="mediaPlayer">media player </param>
    /// <param name="num">number of the video (starting from, and most commonly 0) </param>
    /// <param name="px">pointer to get the pixel width [OUT] </param>
    /// <param name="py">pointer to get the pixel height [OUT] </param>
    /// <returns>0 on success, -1 if the specified video does not exist </returns>
    [LibVlcFunction("libvlc_video_get_size")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetSize(IntPtr mediaPlayer, uint num, ref uint px, ref uint py);

    /// <summary>
    ///     Get the current video scaling factor.
    /// </summary>
    /// <param name="mediaPlayer">media player </param>
    /// <returns>
    ///     the currently configured zoom factor, or 0. if the video is set to fit to the output window/drawable
    ///     automatically.
    /// </returns>
    [LibVlcFunction("libvlc_video_get_scale")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate float GetScale(IntPtr mediaPlayer);

    /// <summary>
    ///     Set the video scaling factor.
    ///     That is the ratio of the number of pixels on screen to the number of pixels in the original decoded video in each
    ///     dimension.
    ///     Zero is a special value; it will adjust the video to the output window/drawable (in windowed mode) or the entire
    ///     screen.
    /// </summary>
    /// <param name="mediaPlayer">media player </param>
    /// <param name="scale">the scaling factor, or zero </param>
    [LibVlcFunction("libvlc_video_set_scale")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetScale(IntPtr mediaPlayer, float scale);

    /// <summary>
    ///     Get current video aspect ratio.
    /// </summary>
    /// <param name="mediaPlayer">media player </param>
    /// <returns>the video aspect ratio or NULL if unspecified (the result must be released with <see cref="Free" />). </returns>
    [LibVlcFunction("libvlc_video_get_aspect_ratio")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GetAspectRatio(IntPtr mediaPlayer);

    /// <summary>
    ///     Set new video aspect ratio.
    /// </summary>
    /// <param name="mediaPlayer">media player </param>
    /// <param name="scale">new video aspect-ratio or NULL to reset to default </param>
    [LibVlcFunction("libvlc_video_set_aspect_ratio")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetAspectRatio(IntPtr mediaPlayer, IntPtr scale);

    /// <summary>
    ///     Get current video width. Use <seealso cref="GetSize" /> instead.
    /// </summary>
    /// <param name="mediaPlayer">media player </param>
    /// <returns>the video pixel width or 0 if not applicable </returns>
    [LibVlcFunction("libvlc_video_get_width")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [Obsolete]
    public delegate int GetVideoWidth(IntPtr mediaPlayer);

    /// <summary>
    ///     Get current video height. Use <seealso cref="GetSize" /> instead.
    /// </summary>
    /// <param name="mediaPlayer">media player </param>
    /// <returns>the video pixel height or 0 if not applicable </returns>
    [LibVlcFunction("libvlc_video_get_height")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [Obsolete]
    public delegate int GetVideoHeight(IntPtr mediaPlayer);

    /// <summary>
    ///     Get number of available video tracks.
    /// </summary>
    /// <param name="mediaPlayer">media player </param>
    /// <returns>the number of available video tracks (int) </returns>
    [LibVlcFunction("libvlc_video_get_track_count")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetVideoTrackCount(IntPtr mediaPlayer);

    /// <summary>
    ///     Get current video track.
    /// </summary>
    /// <param name="mediaPlayer">media player </param>
    /// <returns>the video track ID(int) or -1 if no active input</returns>
    [LibVlcFunction("libvlc_video_get_track")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetVideoTrack(IntPtr mediaPlayer);

    /// <summary>
    ///     Set video track.
    /// </summary>
    /// <param name="mediaPlayer">media player </param>
    /// <param name="track">the track ID (i_id field from track description)</param>
    /// <returns>0 on success, -1 if out of range </returns>
    [LibVlcFunction("libvlc_video_set_track")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SetVideoTrack(IntPtr mediaPlayer, int track);

    /// <summary>
    ///     Get the description of available video tracks.
    /// </summary>
    /// <param name="mediaPlayer">media player </param>
    /// <returns>
    ///     list with description of available video tracks, or NULL on error. It must be freed with
    ///     <see cref="ReleaseTrackDescription" />
    /// </returns>
    [LibVlcFunction("libvlc_video_get_track_description")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GetVideoTrackDescription(IntPtr mediaPlayer);

    /// <summary>
    /// Get integer adjust option. 
    /// </summary>
    /// <param name="mediaPlayer">media player </param>
    /// <param name="adjust">adjust option to get, values of <see cref="VideoAdjust"/></param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_video_get_adjust_int")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetVideoAdjustInt(IntPtr mediaPlayer, VideoAdjust adjust);

    /// <summary>
    /// Get float adjust option. 
    /// </summary>
    /// <param name="mediaPlayer">media player </param>
    /// <param name="adjust">adjust option to get, values of <see cref="VideoAdjust"/></param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_video_get_adjust_float")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate float GetVideoAdjustFloat(IntPtr mediaPlayer, VideoAdjust adjust);

    /// <summary>
    /// Set adjust option as integer. Options that take a different type value are ignored. Passing libvlc_adjust_enable as option value has the side effect of starting (arg !0) or stopping (arg 0) the adjust filter.
    /// </summary>
    /// <param name="mediaPlayer">media player </param>
    /// <param name="adjust">adjust option to set, values of <see cref="VideoAdjust"/></param>
    /// <param name="value">adjust option value </param>
    [LibVlcFunction("libvlc_video_set_adjust_int")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetVideoAdjustInt(IntPtr mediaPlayer, VideoAdjust adjust, int value);

    /// <summary>
    /// Set adjust option as float. Options that take a different type value are ignored.
    /// </summary>
    /// <param name="mediaPlayer">media player </param>
    /// <param name="adjust">adjust option to set, values of <see cref="VideoAdjust"/></param>
    /// <param name="value">adjust option value </param>
    [LibVlcFunction("libvlc_video_set_adjust_float", "1.1.1")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetVideoAdjustFloat(IntPtr mediaPlayer, VideoAdjust adjust, float value);
    

    /// <summary>
    ///     A enum of mouse button.
    /// </summary>
    public enum MouseButton
    {
        /// <summary>
        ///     The left button of mouse.
        /// </summary>
        Left,

        /// <summary>
        ///     The right button of mouse.
        /// </summary>
        Right,

        /// <summary>
        ///     Other buttons of mouse, it is not commonly used.
        /// </summary>
        Other
    }
    
    public enum VideoAdjust
    {
        Enable,
        Contrast,
        Brightness,
        Hue,
        Saturation,
        Gamma
    }
}