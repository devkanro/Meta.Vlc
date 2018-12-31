// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: MouseButton.cs
// Version: 20181231

using Meta.Vlc.Interop.MediaPlayer;

namespace Meta.Vlc
{
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


    /// <summary>
    ///     Navigation mode
    /// </summary>
    public enum NavigateMode : uint
    {
        Activate = libvlc_navigate_mode_t.libvlc_navigate_activate,
        Up = libvlc_navigate_mode_t.libvlc_navigate_up,
        Down = libvlc_navigate_mode_t.libvlc_navigate_down,
        Left = libvlc_navigate_mode_t.libvlc_navigate_left,
        Right = libvlc_navigate_mode_t.libvlc_navigate_right,
        Popup = libvlc_navigate_mode_t.libvlc_navigate_popup
    }
}