// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: Enum.cs
// Version: 20181231

namespace Meta.Vlc.Interop.Media
{
    /// <summary>
    ///     Type of a media slave: subtitle or audio.
    /// </summary>
    public enum libvlc_media_slave_type_t
    {
        libvlc_media_slave_type_subtitle,
        libvlc_media_slave_type_audio
    }

    public enum libvlc_video_projection_t
    {
        libvlc_video_projection_rectangular,

        /// <summary>
        ///     360 spherical
        /// </summary>
        libvlc_video_projection_equirectangular,
        libvlc_video_projection_cubemap_layout_standard = 0x100
    }

    public enum libvlc_video_orient_t
    {
        /// <summary>
        ///     Normal. Top line represents top, left column left.
        /// </summary>
        libvlc_video_orient_top_left,

        /// <summary>
        ///     Flipped horizontally
        /// </summary>
        libvlc_video_orient_top_right,

        /// <summary>
        ///     Flipped vertically
        /// </summary>
        libvlc_video_orient_bottom_left,

        /// <summary>
        ///     Rotated 180 degrees
        /// </summary>
        libvlc_video_orient_bottom_right,

        /// <summary>
        ///     Transposed
        /// </summary>
        libvlc_video_orient_left_top,

        /// <summary>
        ///     Rotated 90 degrees clockwise (or 270 anti-clockwise)
        /// </summary>
        libvlc_video_orient_left_bottom,

        /// <summary>
        ///     Rotated 90 degrees anti-clockwise
        /// </summary>
        libvlc_video_orient_right_top,

        /// <summary>
        ///     Anti-transposed
        /// </summary>
        libvlc_video_orient_right_bottom
    }

    public enum libvlc_video_multiview_t
    {
        /// <summary>
        ///     No stereoscopy: 2D picture.
        /// </summary>
        libvlc_video_multiview_2d,

        /// <summary>
        ///     Side-by-side
        /// </summary>
        libvlc_video_multiview_stereo_sbs,

        /// <summary>
        ///     Top-bottom
        /// </summary>
        libvlc_video_multiview_stereo_tb,

        /// <summary>
        ///     Row sequential
        /// </summary>
        libvlc_video_multiview_stereo_row,

        /// <summary>
        ///     Column sequential
        /// </summary>
        libvlc_video_multiview_stereo_col,

        /// <summary>
        ///     Frame sequential
        /// </summary>
        libvlc_video_multiview_stereo_frame,

        /// <summary>
        ///     Checkerboard pattern
        /// </summary>
        libvlc_video_multiview_stereo_checkerboard
    }

    public enum libvlc_track_type_t
    {
        libvlc_track_unknown = -1,
        libvlc_track_audio = 0,
        libvlc_track_video = 1,
        libvlc_track_text = 2
    }

    /// <summary>
    ///     Note the order of libvlc_state_t enum must match exactly the order of
    ///     \see mediacontrol_PlayerStatus, \see input_state_e enums,
    ///     and VideoLAN.LibVLC.State (at bindings/cil/src/media.cs).
    ///     Expected states by web plugins are:
    ///     IDLE/CLOSE=0, OPENING=1, PLAYING=3, PAUSED=4,
    ///     STOPPING=5, ENDED=6, ERROR=7
    /// </summary>
    public enum libvlc_state_t
    {
        libvlc_NothingSpecial = 0,
        libvlc_Opening,

        /// <summary>
        ///     Deprecated value. Check the libvlc_MediaPlayerBuffering event to know the buffering state of a libvlc_media_player
        /// </summary>
        libvlc_Buffering,
        libvlc_Playing,
        libvlc_Paused,
        libvlc_Stopped,
        libvlc_Ended,
        libvlc_Error
    }

    /// <summary>
    ///     Meta data types
    /// </summary>
    public enum libvlc_meta_t
    {
        libvlc_meta_Title,
        libvlc_meta_Artist,
        libvlc_meta_Genre,
        libvlc_meta_Copyright,
        libvlc_meta_Album,
        libvlc_meta_TrackNumber,
        libvlc_meta_Description,
        libvlc_meta_Rating,
        libvlc_meta_Date,
        libvlc_meta_Setting,
        libvlc_meta_URL,
        libvlc_meta_Language,
        libvlc_meta_NowPlaying,
        libvlc_meta_Publisher,
        libvlc_meta_EncodedBy,
        libvlc_meta_ArtworkURL,
        libvlc_meta_TrackID,
        libvlc_meta_TrackTotal,
        libvlc_meta_Director,
        libvlc_meta_Season,
        libvlc_meta_Episode,
        libvlc_meta_ShowName,
        libvlc_meta_Actors,
        libvlc_meta_AlbumArtist,
        libvlc_meta_DiscNumber,

        libvlc_meta_DiscTotal
        /* Add new meta types HERE */
    }

    /// <summary>
    ///     Media type
    /// </summary>
    public enum libvlc_media_type_t
    {
        libvlc_media_type_unknown,
        libvlc_media_type_file,
        libvlc_media_type_directory,
        libvlc_media_type_disc,
        libvlc_media_type_stream,
        libvlc_media_type_playlist
    }

    /// <summary>
    ///     Parse status used sent by <see cref="libvlc_media_parse_with_options" /> or returned by
    ///     <see cref="libvlc_media_get_parsed_status" />()
    /// </summary>
    /// <seealso cref="libvlc_media_parse_with_options" />
    /// <seealso cref="libvlc_media_get_parsed_status" />
    public enum libvlc_media_parsed_status_t
    {
        libvlc_media_parsed_status_skipped = 1,
        libvlc_media_parsed_status_failed,
        libvlc_media_parsed_status_timeout,
        libvlc_media_parsed_status_done
    }

    /// <summary>
    ///     Parse flags used by <see cref="libvlc_media_parse_with_options" />
    /// </summary>
    /// <seealso cref="libvlc_media_parse_with_options" />
    public enum libvlc_media_parse_flag_t
    {
        /// <summary>
        ///     Parse media if it's a local file
        /// </summary>
        libvlc_media_parse_local = 0x00,

        /// <summary>
        ///     Parse media even if it's a network file
        /// </summary>
        libvlc_media_parse_network = 0x01,

        /// <summary>
        ///     Fetch meta and covert art using local resources
        /// </summary>
        libvlc_media_fetch_local = 0x02,

        /// <summary>
        ///     Fetch meta and covert art using network resources
        /// </summary>
        libvlc_media_fetch_network = 0x04,

        /// <summary>
        ///     Interact with the user (via libvlc_dialog_cbs) when preparsing this item
        ///     (and not its sub items). Set this flag in order to receive a callback
        ///     when the input is asking for credentials.
        /// </summary>
        libvlc_media_do_interact = 0x08
    }

    public enum libvlc_media_option_t
    {
        libvlc_media_option_trusted = 0x2,
        libvlc_media_option_unique = 0x100
    }
}