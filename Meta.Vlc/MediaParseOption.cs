// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: MediaParseOption.cs
// Version: 20181231

using System;
using Meta.Vlc.Interop.Media;

namespace Meta.Vlc
{
    /// <summary>
    ///     Parse flags used by <see cref="VlcMedia.ParseWithOptionAsync" />
    /// </summary>
    [Flags]
    public enum MediaParseOption
    {
        /// <summary>
        ///     Parse media if it's a local file
        /// </summary>
        ParseLocal = libvlc_media_parse_flag_t.libvlc_media_parse_local,

        /// <summary>
        ///     Parse media even if it's a network file
        /// </summary>
        ParseNetwork = libvlc_media_parse_flag_t.libvlc_media_parse_network,

        /// <summary>
        ///     Fetch meta and covert art using local resources
        /// </summary>
        FetchLocal = libvlc_media_parse_flag_t.libvlc_media_fetch_local,

        /// <summary>
        ///     Fetch meta and covert art using network resources
        /// </summary>
        FetchNetwork = libvlc_media_parse_flag_t.libvlc_media_fetch_network,

        /// <summary>
        ///     Interact with the user (via libvlc_dialog_cbs) when preparsing this item
        ///     (and not its sub items). Set this flag in order to receive a callback
        ///     when the input is asking for credentials.
        /// </summary>
        DoInteract = libvlc_media_parse_flag_t.libvlc_media_do_interact
    }
}