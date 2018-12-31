// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: MediaParsedStatus.cs
// Version: 20181231

using Meta.Vlc.Interop.Media;

namespace Meta.Vlc
{
    /// <summary>
    ///     Parse status used sent by <see cref="libvlc_media_parse_with_options" /> or returned by
    ///     <see cref="libvlc_media_get_parsed_status" />()
    /// </summary>
    /// <seealso cref="libvlc_media_parse_with_options" />
    /// <seealso cref="libvlc_media_get_parsed_status" />
    public enum MediaParsedStatus
    {
        Skipped = libvlc_media_parsed_status_t.libvlc_media_parsed_status_skipped,
        Failed = libvlc_media_parsed_status_t.libvlc_media_parsed_status_failed,
        Timeout = libvlc_media_parsed_status_t.libvlc_media_parsed_status_timeout,
        Done = libvlc_media_parsed_status_t.libvlc_media_parsed_status_done
    }
}