// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: Function.cs
// Version: 20181231

using System.Runtime.InteropServices;
using Meta.Vlc.Interop.Core.Event;

namespace Meta.Vlc.Interop.Media
{
    /// <summary>
    ///     Create a media with a certain given media resource location,
    ///     for instance a valid URL.
    /// </summary>
    /// <param name="instance">the instance</param>
    /// <param name="psz_mrl">the media location</param>
    /// <remarks>
    ///     To refer to a local file with this function,
    ///     the file://... URI syntax <b>must</b> be used (see IETF RFC3986).
    ///     We recommend using libvlc_media_new_path() instead when dealing with
    ///     local files.
    /// </remarks>
    /// <returns>the newly created media or NULL on error</returns>
    /// <seealso cref="libvlc_media_release" />
    [LibVlcFunction(nameof(libvlc_media_new_location))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void* libvlc_media_new_location(void* instance, byte* psz_mrl);

    /// <summary>
    ///     Create a media for a certain file path.
    /// </summary>
    /// <param name="instance">the instance</param>
    /// <param name="path">local filesystem path</param>
    /// <returns>the newly created media or NULL on error</returns>
    /// <seealso cref="libvlc_media_release" />
    [LibVlcFunction(nameof(libvlc_media_new_path))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void* libvlc_media_new_path(void* instance, byte* path);

    /// <summary>
    ///     Create a media with custom callbacks to read the data from.
    /// </summary>
    /// <param name="instance">LibVLC instance</param>
    /// <param name="open_cb">callback to open the custom bitstream input media</param>
    /// <param name="read_cb">callback to read data (must not be NULL)</param>
    /// <param name="seek_cb">callback to seek, or NULL if seeking is not supported</param>
    /// <param name="close_cb">callback to close the media, or NULL if unnecessary</param>
    /// <param name="opaque">data pointer for the open callback</param>
    /// <remarks>
    ///     If open_cb is NULL, the opaque pointer will be passed to read_cb,
    ///     seek_cb and close_cb, and the stream size will be treated as unknown.
    ///     <para />
    ///     The callbacks may be called asynchronously (from another thread).
    ///     A single stream instance need not be reentrant. However the open_cb needs to
    ///     be reentrant if the media is used by multiple player instances.
    ///     <para />
    ///     The callbacks may be used until all or any player instances
    ///     that were supplied the media item are stopped.
    /// </remarks>
    /// <returns>the newly created media or NULL on error</returns>
    /// <seealso cref="libvlc_media_release" />
    [LibVlcFunction(nameof(libvlc_media_new_callbacks), "3.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void* libvlc_media_new_callbacks(void* instance,
        libvlc_media_open_cb open_cb,
        libvlc_media_read_cb read_cb,
        libvlc_media_seek_cb seek_cb,
        libvlc_media_close_cb close_cb,
        void* opaque);

    /// <summary>
    ///     Create a media as an empty node with a given name.
    /// </summary>
    /// <param name="instance">the instance</param>
    /// <param name="psz_name">the name of the node</param>
    /// <returns>the new empty media or NULL on error</returns>
    /// <seealso cref="libvlc_media_release" />
    [LibVlcFunction(nameof(libvlc_media_new_as_node))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void* libvlc_media_new_as_node(void* instance, byte* psz_name);

    /// <summary>
    ///     Add an option to the media.
    ///     <para />
    ///     This option will be used to determine how the media_player will
    ///     read the media. This allows to use VLC's advanced
    ///     reading/streaming options on a per-media basis.
    /// </summary>
    /// <param name="p_md">the media descriptor</param>
    /// <param name="psz_options">the options (as a string)</param>
    /// <remarks>
    ///     The options are listed in 'vlc --long-help' from the command line,
    ///     e.g. "-sout-all". Keep in mind that available options and their semantics
    ///     vary across LibVLC versions and builds.
    ///     <para />
    ///     Not all options affects libvlc_media_t objects:
    ///     Specifically, due to architectural issues most audio and video options,
    ///     such as text renderer options, have no effects on an individual media.
    ///     These options must be set through libvlc_new() instead.
    /// </remarks>
    [LibVlcFunction(nameof(libvlc_media_add_option))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_add_option(void* p_md, byte* psz_options);

    /// <summary>
    ///     Add an option to the media with configurable flags.
    ///     <para />
    ///     This option will be used to determine how the media_player will
    ///     read the media. This allows to use VLC's advanced
    ///     reading/streaming options on a per-media basis.
    ///     <para />
    ///     The options are detailed in vlc --long-help, for instance
    ///     "--sout-all". Note that all options are not usable on medias:
    ///     specifically, due to architectural issues, video-related options
    ///     such as text renderer options cannot be set on a single media. They
    ///     must be set on the whole libvlc instance instead.
    /// </summary>
    /// <param name="p_md">the media descriptor</param>
    /// <param name="psz_options">the options (as a string)</param>
    /// <param name="i_flags">the flags for this option</param>
    [LibVlcFunction(nameof(libvlc_media_add_option_flag))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_add_option_flag(void* p_md, byte* psz_options, uint i_flags);

    /// <summary>
    ///     Retain a reference to a media descriptor object (libvlc_media_t). Use
    ///     libvlc_media_release() to decrement the reference count of a
    ///     media descriptor object.
    /// </summary>
    /// <param name="p_md">the media descriptor</param>
    [LibVlcFunction(nameof(libvlc_media_retain))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_retain(void* p_md);

    /// <summary>
    ///     Decrement the reference count of a media descriptor object. If the
    ///     reference count is 0, then libvlc_media_release() will release the
    ///     media descriptor object. It will send out an libvlc_MediaFreed event
    ///     to all listeners. If the media descriptor object has been released it
    ///     should not be used again.
    /// </summary>
    /// <param name="p_md">the media descriptor</param>
    [LibVlcFunction(nameof(libvlc_media_release))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_release(void* p_md);

    /// <summary>
    ///     Get the media resource locator (mrl) from a media descriptor object
    /// </summary>
    /// <param name="p_md">a media descriptor object</param>
    /// <returns>string with mrl of media descriptor object</returns>
    [LibVlcFunction(nameof(libvlc_media_get_mrl))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate byte* libvlc_media_get_mrl(void* p_md);

    /// <summary>
    ///     Duplicate a media descriptor object.
    /// </summary>
    /// <param name="p_md">a media descriptor object.</param>
    /// <returns></returns>
    [LibVlcFunction(nameof(libvlc_media_duplicate))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void* libvlc_media_duplicate(void* p_md);

    /// <summary>
    ///     Read the meta of the media.
    ///     <para />
    ///     If the media has not yet been parsed this will return NULL.
    /// </summary>
    /// <param name="p_md">the media descriptor</param>
    /// <param name="e_meta">the meta to read</param>
    /// <returns>the media's meta</returns>
    /// <seealso cref="libvlc_media_parse_with_options" />
    /// <seealso cref="libvlc_event_e.libvlc_MediaMetaChanged" />
    [LibVlcFunction(nameof(libvlc_media_get_meta))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate byte* libvlc_media_get_meta(void* p_md,
        libvlc_meta_t e_meta);

    /// <summary>
    ///     Set the meta of the media (this function will not save the meta, call
    ///     <see cref="libvlc_media_save_meta" /> in order to save the meta)
    /// </summary>
    /// <param name="p_md">the media descriptor</param>
    /// <param name="e_meta">the meta to write</param>
    /// <param name="psz_value">the media's meta</param>
    [LibVlcFunction(nameof(libvlc_media_set_meta))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_set_meta(void* p_md,
        libvlc_meta_t e_meta,
        byte* psz_value);

    /// <summary>
    ///     Save the meta previously set
    /// </summary>
    /// <param name="p_md">the media desriptor</param>
    /// <returns>true if the write operation was successful</returns>
    [LibVlcFunction(nameof(libvlc_media_save_meta))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate bool libvlc_media_save_meta(void* p_md);

    /// <summary>
    ///     Get current state of media descriptor object. Possible media states are
    ///     libvlc_NothingSpecial=0, libvlc_Opening, libvlc_Playing, libvlc_Paused,
    ///     libvlc_Stopped, libvlc_Ended, libvlc_Error.
    /// </summary>
    /// <param name="p_md">a media descriptor object</param>
    /// <returns>state of media descriptor object</returns>
    /// <seealso cref="libvlc_state_t" />
    [LibVlcFunction(nameof(libvlc_media_get_state))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate libvlc_state_t libvlc_media_get_state(void* p_md);

    /// <summary>
    ///     Get the current statistics about the media
    /// </summary>
    /// <param name="p_md">media descriptor object</param>
    /// <param name="p_stats">
    ///     structure that contain the statistics about the media(this structure must be allocated by the
    ///     caller)
    /// </param>
    /// <returns>true if the statistics are available, false otherwise</returns>
    [LibVlcFunction(nameof(libvlc_media_get_stats))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate bool libvlc_media_get_stats(void* p_md, libvlc_media_stats_t* p_stats);

    /// <summary>
    ///     Get subitems of media descriptor object. This will increment
    ///     the reference count of supplied media descriptor object. Use
    ///     <see cref="libvlc_media_list_release" /> to decrement the reference counting.
    /// </summary>
    /// <param name="p_md">media descriptor object</param>
    /// <returns>list of media descriptor subitems or NULL</returns>
    [LibVlcFunction(nameof(libvlc_media_subitems))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void* libvlc_media_subitems(void* p_md); // return libvlc_media_list_t*

    /// <summary>
    ///     Get event manager from media descriptor object.
    /// </summary>
    /// <param name="p_md">a media descriptor object</param>
    /// <remarks>
    ///     this function doesn't increment reference counting.
    /// </remarks>
    /// <returns>event manager object</returns>
    [LibVlcFunction(nameof(libvlc_media_event_manager))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void* libvlc_media_event_manager(void* p_md); // return libvlc_event_manager_t*

    /// <summary>
    ///     Get duration (in ms) of media descriptor object item.
    /// </summary>
    /// <param name="p_md">media descriptor object</param>
    /// <returns>duration of media item or -1 on error</returns>
    [LibVlcFunction(nameof(libvlc_media_get_duration))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate long libvlc_media_get_duration(void* p_md); // return libvlc_time_t*

    /// <summary>
    ///     Parse the media asynchronously with options.
    ///     <para />
    ///     This fetches (local or network) art, meta data and/or tracks information.
    ///     <para />
    ///     To track when this is over you can listen to <see cref="libvlc_event_e.libvlc_MediaParsedChanged" />
    ///     event. However if this functions returns an error, you will not receive any
    ///     events.
    ///     <para />
    ///     It uses a flag to specify parse options (see <see cref="libvlc_media_parse_flag_t" />). All
    ///     these flags can be combined. By default, media is parsed if it's a local
    ///     file.
    /// </summary>
    /// <param name="p_md">media descriptor object</param>
    /// <param name="parse_flag">parse options</param>
    /// <param name="timeout">
    ///     maximum time allowed to preparse the media. If -1, the
    ///     default "preparse-timeout" option will be used as a timeout. If 0, it will
    ///     wait indefinitely. If > 0, the timeout will be used (in milliseconds).
    /// </param>
    /// <remarks>Parsing can be aborted with <see cref="libvlc_media_parse_stop" />.</remarks>
    /// <returns>-1 in case of error, 0 otherwise</returns>
    /// <seealso cref="libvlc_event_e.libvlc_MediaParsedChanged" />
    /// <seealso cref="libvlc_media_get_meta" />
    /// <seealso cref="libvlc_media_tracks_get" />
    /// <seealso cref="libvlc_media_get_parsed_status" />
    /// <seealso cref="libvlc_media_parse_flag_t" />
    [LibVlcFunction(nameof(libvlc_media_parse_with_options), "3.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate int libvlc_media_parse_with_options(void* p_md,
        libvlc_media_parse_flag_t parse_flag,
        int timeout);

    /// <summary>
    ///     Stop the parsing of the media
    ///     <para />
    ///     When the media parsing is stopped, the libvlc_MediaParsedChanged event will
    ///     be sent with the libvlc_media_parsed_status_timeout status.
    /// </summary>
    /// <param name="p_md">media descriptor object</param>
    /// <seealso cref="libvlc_media_parse_with_options" />
    [LibVlcFunction(nameof(libvlc_media_parse_stop), "3.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_parse_stop(void* p_md);

    /// <summary>
    ///     Get Parsed status for media descriptor object.
    /// </summary>
    /// <param name="p_md">media descriptor object</param>
    /// <returns>a value of the <see cref="libvlc_media_parsed_status_t" /> enum</returns>
    /// <seealso cref="libvlc_event_e.libvlc_MediaParsedChanged" />
    /// <seealso cref="libvlc_media_parsed_status_t" />
    [LibVlcFunction(nameof(libvlc_media_get_parsed_status), "3.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate libvlc_media_parsed_status_t libvlc_media_get_parsed_status(void* p_md);

    /// <summary>
    ///     Sets media descriptor's user_data. user_data is specialized data
    ///     accessed by the host application, VLC.framework uses it as a pointer to
    ///     an native object that references a libvlc_media_t pointer
    /// </summary>
    /// <param name="p_md">media descriptor object</param>
    /// <param name="p_new_user_data">pointer to user data</param>
    [LibVlcFunction(nameof(libvlc_media_set_user_data))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_set_user_data(void* p_md, void* p_new_user_data);

    /// <summary>
    ///     Get media descriptor's user_data. user_data is specialized data
    ///     accessed by the host application, VLC.framework uses it as a pointer to
    ///     an native object that references a libvlc_media_t pointer
    /// </summary>
    /// <param name="p_md">media descriptor object</param>
    /// <returns></returns>
    [LibVlcFunction(nameof(libvlc_media_get_user_data))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void* libvlc_media_get_user_data(void* p_md);

    /// <summary>
    ///     Get media descriptor's elementary streams description
    ///     <para />
    ///     Note, you need to call <see cref="libvlc_media_parse_with_options" />() or play the media at least once
    ///     before calling this function.
    ///     Not doing this will result in an empty array.
    /// </summary>
    /// <param name="p_md">media descriptor object</param>
    /// <param name="tracks">
    ///     tracks address to store an allocated array of Elementary Streams
    ///     descriptions (must be freed with <see cref="Media.libvlc_media_tracks_release" />
    ///     by the caller) [OUT]
    /// </param>
    /// <returns>the number of Elementary Streams (zero on error)</returns>
    [LibVlcFunction(nameof(libvlc_media_tracks_get), "2.1.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate uint libvlc_media_tracks_get(void* p_md, libvlc_media_track_t*** tracks);

    /// <summary>
    ///     Get codec description from media elementary stream
    /// </summary>
    /// <param name="i_type">i_type from <see cref="libvlc_media_track_t" /></param>
    /// <param name="i_codec">i_codec or i_original_fourcc from <see cref="libvlc_media_track_t" /></param>
    /// <returns>codec description</returns>
    /// <seealso cref="libvlc_media_track_t" />
    [LibVlcFunction(nameof(libvlc_media_get_codec_description), "3.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate byte* libvlc_media_get_codec_description(libvlc_track_type_t i_type, uint i_codec);

    /// <summary>
    ///     Release media descriptor's elementary streams description array
    /// </summary>
    /// <param name="p_tracks">tracks info array to release</param>
    /// <param name="i_count">number of elements in the array</param>
    [LibVlcFunction(nameof(libvlc_media_tracks_release), "2.1.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_tracks_release(libvlc_media_track_t** p_tracks, uint i_count);

    /// <summary>
    ///     Get the media type of the media descriptor object
    /// </summary>
    /// <param name="p_md">media descriptor object</param>
    /// <returns>media type</returns>
    /// <seealso cref="libvlc_media_type_t" />
    [LibVlcFunction(nameof(libvlc_media_get_type), "3.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate libvlc_media_type_t libvlc_media_get_type(void* p_md);

    /// <summary>
    ///     Add a slave to the current media.
    ///     <para />
    ///     A slave is an external input source that may contains an additional subtitle
    ///     track (like a .srt) or an additional audio track (like a .ac3).
    /// </summary>
    /// <param name="p_md">media descriptor object</param>
    /// <param name="i_type">subtitle or audio</param>
    /// <param name="i_priority">from 0 (low priority) to 4 (high priority)</param>
    /// <param name="psz_uri">Uri of the slave (should contain a valid scheme).</param>
    /// <remarks>
    ///     This function must be called before the media is parsed (via
    ///     <see cref="Media.libvlc_media_parse_with_options" />) or before the media is played (via
    ///     <see cref="libvlc_media_player_play" />)
    /// </remarks>
    /// <returns>0 on success, -1 on error.</returns>
    [LibVlcFunction(nameof(libvlc_media_slaves_add), "3.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate int libvlc_media_slaves_add(void* p_md,
        libvlc_media_slave_type_t i_type,
        uint i_priority,
        byte* psz_uri);

    /// <summary>
    ///     Clear all slaves previously added by <see cref="Media.libvlc_media_slaves_add" /> or
    ///     internally.
    /// </summary>
    /// <param name="p_md">media descriptor object</param>
    [LibVlcFunction(nameof(libvlc_media_slaves_clear), "3.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_slaves_clear(void* p_md);

    /// <summary>
    ///     Get a media descriptor's slave list
    ///     <para />
    ///     The list will contain slaves parsed by VLC or previously added by
    ///     <see cref="libvlc_media_slaves_add" />. The typical use case of this function is to save
    ///     a list of slave in a database for a later use.
    /// </summary>
    /// <param name="p_md">media descriptor object</param>
    /// <param name="ppp_slaves">
    ///     address to store an allocated array of slaves (must be
    ///     freed with <see cref="libvlc_media_slaves_release" />) [OUT]
    /// </param>
    /// <returns>the number of slaves (zero on error)</returns>
    [LibVlcFunction(nameof(libvlc_media_slaves_get), "3.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate uint libvlc_media_slaves_get(void* p_md, libvlc_media_slave_t*** ppp_slaves);

    /// <summary>
    ///     Release a media descriptor's slave list
    /// </summary>
    /// <param name="ppp_slaves">slave array to release</param>
    /// <param name="i_count">number of elements in the array</param>
    [LibVlcFunction(nameof(libvlc_media_slaves_release), "3.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_slaves_release(libvlc_media_slave_t** ppp_slaves, uint i_count);
}