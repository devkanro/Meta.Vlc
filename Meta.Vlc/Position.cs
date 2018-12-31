// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: Position.cs
// Version: 20181231

using Meta.Vlc.Interop.MediaPlayer;

namespace Meta.Vlc
{
    /// <summary>
    ///     Enumeration of values used to set position (e.g. of video title).
    /// </summary>
    public enum Position
    {
        Disable = libvlc_position_t.libvlc_position_disable,
        Center = libvlc_position_t.libvlc_position_center,
        Left = libvlc_position_t.libvlc_position_left,
        Right = libvlc_position_t.libvlc_position_right,
        Top = libvlc_position_t.libvlc_position_top,
        TopLeft = libvlc_position_t.libvlc_position_top_left,
        TopRight = libvlc_position_t.libvlc_position_top_right,
        Bottom = libvlc_position_t.libvlc_position_bottom,
        BottomLeft = libvlc_position_t.libvlc_position_bottom_left,
        BottomRight = libvlc_position_t.libvlc_position_bottom_right
    }
}