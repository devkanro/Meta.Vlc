// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: Function.cs
// Version: 20181231

using System.Runtime.InteropServices;
using Meta.Vlc.Interop.Core;
using Meta.Vlc.Interop.Media;
using VlcTime = System.Int64;

namespace Meta.Vlc.Interop.MediaPlayer
{
    /// <summary>
    ///     Create an empty Media Player object
    /// </summary>
    /// <param name="p_libvlc_instance">
    ///     the libvlc instance in which the Media Player should be created.
    /// </param>
    /// <returns>a new media player object, or NULL on error.</returns>
    [LibVlcFunction(nameof(libvlc_media_player_new))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void* libvlc_media_player_new(void* p_libvlc_instance);

    /// <summary>
    ///     Create a Media Player object from a Media
    /// </summary>
    /// <param name="p_md">the media. Afterwards the p_md can be safely destroyed.</param>
    /// <returns>a new media player object, or NULL on error.</returns>
    [LibVlcFunction(nameof(libvlc_media_player_new_from_media))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void* libvlc_media_player_new_from_media(void* p_md);

    /// <summary>
    ///     Release a media_player after use
    ///     Decrement the reference count of a media player object. If the
    ///     reference count is 0, then <see cref="libvlc_media_player_release" /> will
    ///     release the media player object. If the media player object
    ///     has been released, then it should not be used again.
    /// </summary>
    /// <param name="p_mi">the Media Player to free</param>
    [LibVlcFunction(nameof(libvlc_media_player_release))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_player_release(void* p_mi);

    /// <summary>
    ///     Retain a reference to a media player object. Use <see cref="libvlc_media_player_release" /> to decrement reference
    ///     count.
    /// </summary>
    /// <param name="p_mi">media player object</param>
    [LibVlcFunction(nameof(libvlc_media_player_retain))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_player_retain(void* p_mi);

    /// <summary>
    ///     Set the media that will be used by the media_player. If any, previous md will be released.
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <param name="p_md">the Media. Afterwards the p_md can be safely destroyed.</param>
    [LibVlcFunction(nameof(libvlc_media_player_set_media))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_player_set_media(void* p_mi, void* p_md);

    /// <summary>
    ///     Get the media used by the media_player.
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <returns>the media associated with p_mi, or NULL if no media is associated</returns>
    [LibVlcFunction(nameof(libvlc_media_player_get_media))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void* libvlc_media_player_get_media(void* p_mi);

    /// <summary>
    ///     Get the Event Manager from which the media player send event.
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <returns>the event manager associated with p_mi</returns>
    [LibVlcFunction(nameof(libvlc_media_player_event_manager))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void* libvlc_media_player_event_manager(void* p_mi);

    /// <summary>
    ///     is playing
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <returns>1 if the media player is playing, 0 otherwise</returns>
    [LibVlcFunction(nameof(libvlc_media_player_is_playing))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate bool libvlc_media_player_is_playing(void* p_mi);

    /// <summary>
    ///     Play
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <returns>0 if playback started (and was already started), or -1 on error.</returns>
    [LibVlcFunction(nameof(libvlc_media_player_play))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate int libvlc_media_player_play(void* p_mi);

    /// <summary>
    ///     Pause or resume (no effect if there is no media)
    /// </summary>
    /// <param name="mp">the Media Player</param>
    /// <param name="do_pause">play/resume if zero, pause if non-zero</param>
    [LibVlcFunction(nameof(libvlc_media_player_set_pause), "1.1.1")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_player_set_pause(void* mp, int do_pause);

    /// <summary>
    ///     Toggle pause (no effect if there is no media)
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    [LibVlcFunction(nameof(libvlc_media_player_pause))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_player_pause(void* p_mi);

    /// <summary>
    ///     Stop (no effect if there is no media)
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    [LibVlcFunction(nameof(libvlc_media_player_stop))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_player_stop(void* p_mi);

    /// <summary>
    ///     Set a renderer to the media player
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <param name="p_item">an item discovered by libvlc_renderer_discoverer_start</param>
    /// <returns>0 on success, -1 on error.</returns>
    /// seealso libvlc_renderer_discoverer_new
    /// <remarks>
    ///     must be called before the first call of <see cref="libvlc_media_player_play" /> to take effect.
    /// </remarks>
    [LibVlcFunction(nameof(libvlc_media_player_set_renderer), "3.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate int libvlc_media_player_set_renderer(void* p_mi, void* p_item);

    /// <summary>
    ///     Set callbacks and private data to render decoded video to a custom area
    ///     in memory.
    ///     Use <see cref="libvlc_video_set_format" /> or <see cref="libvlc_video_set_format_callbacks" />
    ///     to configure the decoded format.
    ///     <para />
    ///     warning: Rendering video into custom memory buffers is considerably less
    ///     efficient than rendering in a custom window as normal.
    ///     <para />
    ///     For optimal perfomances, VLC media player renders into a custom window, and
    ///     does not use this function and associated callbacks. It is
    ///     <b>
    ///         highly
    ///         recommended
    ///     </b>
    ///     that other LibVLC-based application do likewise.
    ///     To embed video in a window, use libvlc_media_player_set_xid() or equivalent
    ///     depending on the operating system.
    ///     <para />
    ///     If window embedding does not fit the application use case, then a custom
    ///     LibVLC video output display plugin is required to maintain optimal video
    ///     rendering performances.
    ///     <para />
    ///     The following limitations affect performance:
    ///     <para />
    ///     - Hardware video decoding acceleration will either be disabled completely,
    ///     or require (relatively slow) copy from video/DSP memory to main memory.
    ///     <para />
    ///     - Sub-pictures (subtitles, on-screen display, etc.) must be blent into the
    ///     main picture by the CPU instead of the GPU.
    ///     <para />
    ///     - Depending on the video format, pixel format conversion, picture scaling,
    ///     cropping and/or picture re-orientation, must be performed by the CPU
    ///     instead of the GPU.
    ///     <para />
    ///     - Memory copying is required between LibVLC reference picture buffers and
    ///     application buffers (between lock and unlock callbacks).
    ///     <para />
    /// </summary>
    /// <param name="mp">the media player</param>
    /// <param name="lock_cb">callback to lock video memory (must not be NULL)</param>
    /// <param name="unlock_cb">callback to unlock video memory (or NULL if not needed)</param>
    /// <param name="display_cb">callback to display video (or NULL if not needed)</param>
    /// <param name="opaque">private pointer for the three callbacks (as first parameter)</param>
    [LibVlcFunction(nameof(libvlc_video_set_callbacks), "1.1.1")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_video_set_callbacks(void* mp,
        libvlc_video_lock_cb lock_cb,
        libvlc_video_unlock_cb unlock_cb,
        libvlc_video_display_cb display_cb,
        void* opaque);

    /// <summary>
    ///     Set decoded video chroma and dimensions.
    ///     This only works in combination with <see cref="libvlc_video_set_callbacks" />,
    ///     and is mutually exclusive with <see cref="libvlc_video_set_format_callbacks" />.
    /// </summary>
    /// <param name="mp">the media player</param>
    /// <param name="chroma">a four-characters string identifying the chroma (e.g. "RV32" or "YUYV")</param>
    /// <param name="width">pixel width</param>
    /// <param name="height">pixel height</param>
    /// <param name="pitch">line pitch (in bytes)</param>
    /// <remarks>
    ///     All pixel planes are expected to have the same pitch.
    ///     <para />
    ///     To use the YCbCr color space with chrominance subsampling, consider using
    ///     <see cref="libvlc_video_set_format_callbacks" /> instead.
    ///     consider using libvlc_video_set_format_callbacks() instead.
    /// </remarks>
    [LibVlcFunction(nameof(libvlc_video_set_format), "1.1.1")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_video_set_format(void* mp, byte* chroma, uint width, uint height, uint pitch);

    /// <summary>
    ///     Set decoded video chroma and dimensions. This only works in combination with
    ///     <see cref="libvlc_video_set_callbacks" />.
    /// </summary>
    /// <param name="mp">the media player</param>
    /// <param name="setup">callback to select the video format (cannot be NULL)</param>
    /// <param name="cleanup">callback to release any allocated resources (or NULL)</param>
    [LibVlcFunction(nameof(libvlc_video_set_format_callbacks), "2.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_video_set_format_callbacks(void* mp,
        libvlc_video_format_cb setup,
        libvlc_video_cleanup_cb cleanup);

    /// <summary>
    ///     Set a Win32/Win64 API window handle (HWND) where the media player should
    ///     render its video output. If LibVLC was built without Win32/Win64 API output
    ///     support, then this has no effects.
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <param name="drawable">windows handle of the drawable</param>
    [LibVlcFunction(nameof(libvlc_media_player_set_hwnd))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_player_set_hwnd(void* p_mi, void* drawable);

    /// <summary>
    ///     Get the Windows API window handle (HWND) previously set with
    ///     libvlc_media_player_set_hwnd(). The handle will be returned even if LibVLC
    ///     is not currently outputting any video to it.
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <returns>a window handle or NULL if there are none.</returns>
    [LibVlcFunction(nameof(libvlc_media_player_get_hwnd))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void* libvlc_media_player_get_hwnd(void* p_mi);

    /**
     * Sets callbacks and private data for decoded audio.
     *
     * Use libvlc_audio_set_format() or libvlc_audio_set_format_callbacks()
     * to configure the decoded audio format.
     *
     * \note The audio callbacks override any other audio output mechanism.
     * If the callbacks are set, LibVLC will <b>not</b> output audio in any way.
     *
     * \param mp the media player
     * \param play callback to play audio samples (must not be NULL)
     * \param pause callback to pause playback (or NULL to ignore)
     * \param resume callback to resume playback (or NULL to ignore)
     * \param flush callback to flush audio buffers (or NULL to ignore)
     * \param drain callback to drain audio buffers (or NULL to ignore)
     * \param opaque private pointer for the audio callbacks (as first parameter)
     * \version LibVLC 2.0.0 or later
     */
    /// <summary>
    ///     Sets callbacks and private data for decoded audio.
    ///     <para />
    ///     Use <see cref="libvlc_audio_set_format" /> or <see cref="libvlc_audio_set_format_callbacks" />
    ///     to configure the decoded audio format.
    /// </summary>
    /// <param name="mp">the media player</param>
    /// <param name="play">callback to play audio samples (must not be NULL)</param>
    /// <param name="pause">callback to pause playback (or NULL to ignore)</param>
    /// <param name="resume">callback to resume playback (or NULL to ignore)</param>
    /// <param name="flush">callback to flush audio buffers (or NULL to ignore)</param>
    /// <param name="drain">callback to drain audio buffers (or NULL to ignore)</param>
    /// <param name="opaque">private pointer for the audio callbacks (as first parameter)</param>
    [LibVlcFunction(nameof(libvlc_audio_set_callbacks), "2.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_audio_set_callbacks(void* mp,
        libvlc_audio_play_cb play,
        libvlc_audio_pause_cb pause,
        libvlc_audio_resume_cb resume,
        libvlc_audio_flush_cb flush,
        libvlc_audio_drain_cb drain,
        void* opaque);

    /// <summary>
    ///     Set callbacks and private data for decoded audio. This only works in
    ///     combination with <see cref="libvlc_audio_set_callbacks" />.
    ///     <para />
    ///     Use <see cref="libvlc_audio_set_format" /> or <see cref="libvlc_audio_set_format_callbacks" />
    ///     to configure the decoded audio format.
    /// </summary>
    /// <param name="mp">the media player</param>
    /// <param name="set_volume">callback to apply audio volume, or NULL to apply volume in software</param>
    [LibVlcFunction(nameof(libvlc_audio_set_callbacks), "2.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_audio_set_volume_callback(void* mp,
        libvlc_audio_set_volume_cb set_volume);

    /// <summary>
    ///     Sets decoded audio format via callbacks.
    ///     <para />
    ///     This only works in combination with <see cref="libvlc_audio_set_callbacks" />.
    /// </summary>
    /// <param name="mp">the media player</param>
    /// <param name="setup">callback to select the audio format (cannot be NULL)</param>
    /// <param name="cleanup">callback to release any allocated resources (or NULL)</param>
    [LibVlcFunction(nameof(libvlc_audio_set_callbacks), "2.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_audio_set_format_callbacks(void* mp,
        libvlc_audio_setup_cb setup,
        libvlc_audio_cleanup_cb cleanup);

    /// <summary>
    ///     Sets a fixed decoded audio format.
    ///     <para />
    ///     This only works in combination with <see cref="libvlc_audio_set_callbacks" />,
    ///     and is mutually exclusive with <see cref="libvlc_audio_set_format_callbacks" />.
    /// </summary>
    /// <param name="mp">the media player</param>
    /// <param name="format">a four-characters string identifying the sample format (e.g. "S16N" or "FL32")</param>
    /// <param name="rate">sample rate (expressed in Hz)</param>
    /// <param name="channels">channels count</param>
    [LibVlcFunction(nameof(libvlc_audio_set_callbacks), "2.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_audio_set_format(void* mp, byte* format, uint rate, uint channels);

    /// <summary>
    ///     Get the current movie length (in ms).
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <returns>the movie length (in ms), or -1 if there is no media.</returns>
    [LibVlcFunction(nameof(libvlc_media_player_get_length))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate long libvlc_media_player_get_length(void* p_mi);

    /// <summary>
    ///     Get the current movie time (in ms).
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <returns>the movie time (in ms), or -1 if there is no media.</returns>
    [LibVlcFunction(nameof(libvlc_media_player_get_time))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate long libvlc_media_player_get_time(void* p_mi);

    /// <summary>
    ///     Set the movie time (in ms). This has no effect if no media is being played.
    ///     Not all formats and protocols support this.
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <param name="i_time">the movie time (in ms).</param>
    [LibVlcFunction(nameof(libvlc_media_player_set_time))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_player_set_time(void* p_mi, long i_time);

    /// <summary>
    ///     Get movie position as percentage between 0.0 and 1.0.
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <returns>movie position, or -1. in case of error</returns>
    [LibVlcFunction(nameof(libvlc_media_player_get_position))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate float libvlc_media_player_get_position(void* p_mi);

    /// <summary>
    ///     Set movie position as percentage between 0.0 and 1.0.
    ///     This has no effect if playback is not enabled.
    ///     This might not work depending on the underlying input format and protocol.
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <param name="f_pos">the position</param>
    [LibVlcFunction(nameof(libvlc_media_player_set_position))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_player_set_position(void* p_mi, float f_pos);

    /// <summary>
    ///     Set movie chapter (if applicable).
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <param name="i_chapter">chapter number to play</param>
    [LibVlcFunction(nameof(libvlc_media_player_set_chapter))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_player_set_chapter(void* p_mi, int i_chapter);

    /// <summary>
    ///     Get movie chapter.
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <returns>chapter number currently playing, or -1 if there is no media.</returns>
    [LibVlcFunction(nameof(libvlc_media_player_get_chapter))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate int libvlc_media_player_get_chapter(void* p_mi);

    /// <summary>
    ///     Get movie chapter count
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <returns>number of chapters in movie, or -1.</returns>
    [LibVlcFunction(nameof(libvlc_media_player_get_chapter_count))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate int libvlc_media_player_get_chapter_count(void* p_mi);

    /// <summary>
    ///     Is the player able to play
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <returns></returns>
    [LibVlcFunction(nameof(libvlc_media_player_will_play))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate bool libvlc_media_player_will_play(void* p_mi);

    /// <summary>
    ///     Get title chapter count
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <param name="i_title">title</param>
    /// <returns>number of chapters in title, or -1</returns>
    [LibVlcFunction(nameof(libvlc_media_player_get_chapter_count_for_title))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate int libvlc_media_player_get_chapter_count_for_title(
        void* p_mi, int i_title);

    /// <summary>
    ///     Set movie title
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <param name="i_title">title number to play</param>
    [LibVlcFunction(nameof(libvlc_media_player_set_title))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_player_set_title(void* p_mi, int i_title);

    /// <summary>
    ///     Get movie title
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <returns>title number currently playing, or -1</returns>
    [LibVlcFunction(nameof(libvlc_media_player_get_title))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate int libvlc_media_player_get_title(void* p_mi);

    /// <summary>
    ///     Get movie title count
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <returns>title number count, or -1</returns>
    [LibVlcFunction(nameof(libvlc_media_player_get_title_count))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate int libvlc_media_player_get_title_count(void* p_mi);

    /// <summary>
    ///     Set previous chapter (if applicable)
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    [LibVlcFunction(nameof(libvlc_media_player_previous_chapter))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_player_previous_chapter(void* p_mi);

    /// <summary>
    ///     Set next chapter (if applicable)
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    [LibVlcFunction(nameof(libvlc_media_player_next_chapter))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_player_next_chapter(void* p_mi);

    /// <summary>
    ///     Get the requested movie play rate.
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <returns>movie play rate</returns>
    /// <remarks>
    ///     Depending on the underlying media, the requested rate may be
    ///     different from the real playback rate.
    /// </remarks>
    [LibVlcFunction(nameof(libvlc_media_player_get_rate))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate float libvlc_media_player_get_rate(void* p_mi);

    /// <summary>
    ///     Set movie play rate
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <param name="rate">movie play rate to set</param>
    /// <returns>
    ///     -1 if an error was detected, 0 otherwise (but even then, it might
    ///     not actually work depending on the underlying media protocol)
    /// </returns>
    [LibVlcFunction(nameof(libvlc_media_player_set_rate))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate int libvlc_media_player_set_rate(void* p_mi, float rate);

    /// <summary>
    ///     Get current movie state
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <returns>the current state of the media player (playing, paused, ...)</returns>
    /// <seealso cref="libvlc_state_t" />
    [LibVlcFunction(nameof(libvlc_media_player_get_state))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate libvlc_state_t libvlc_media_player_get_state(void* p_mi);

    /// <summary>
    ///     How many video outputs does this media player have?
    /// </summary>
    /// <param name="p_mi">the media player</param>
    /// <returns>the number of video outputs</returns>
    [LibVlcFunction(nameof(libvlc_media_player_has_vout))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate uint libvlc_media_player_has_vout(void* p_mi);

    /// <summary>
    ///     Is this media player seekable?
    /// </summary>
    /// <param name="p_mi">the media player</param>
    /// <returns>true if the media player can seek</returns>
    [LibVlcFunction(nameof(libvlc_media_player_is_seekable))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate bool libvlc_media_player_is_seekable(void* p_mi);

    /// <summary>
    ///     Can this media player be paused?
    /// </summary>
    /// <param name="p_mi">the media player</param>
    /// <returns>true if the media player can pause</returns>
    [LibVlcFunction(nameof(libvlc_media_player_can_pause))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate bool libvlc_media_player_can_pause(void* p_mi);

    /// <summary>
    ///     Check if the current program is scrambled
    /// </summary>
    /// <param name="p_mi">the media player</param>
    /// <returns>true if the current program is scrambled</returns>
    [LibVlcFunction(nameof(libvlc_media_player_program_scrambled), "2.2.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate int libvlc_media_player_program_scrambled(void* p_mi);

    /// <summary>
    ///     Display the next frame (if supported)
    /// </summary>
    /// <param name="p_mi">the media player</param>
    [LibVlcFunction(nameof(libvlc_media_player_next_frame))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_player_next_frame(void* p_mi);

    /// <summary>
    ///     Navigate through DVD Menu
    /// </summary>
    /// <param name="p_mi">the Media Player</param>
    /// <param name="navigate">the Navigation mode</param>
    [LibVlcFunction(nameof(libvlc_media_player_navigate), "2.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_player_navigate(void* p_mi,
        libvlc_navigate_mode_t navigate);

    /// <summary>
    ///     Set if, and how, the video title will be shown when media is played.
    /// </summary>
    /// <param name="p_mi">the media player</param>
    /// <param name="position">
    ///     position at which to display the title, or
    ///     <see cref="libvlc_position_t.libvlc_position_disable" /> to prevent the title from being displayed
    /// </param>
    /// <param name="timeout">
    ///     title display timeout in milliseconds (ignored if
    ///     <see cref="libvlc_position_t.libvlc_position_disable" />)
    /// </param>
    [LibVlcFunction(nameof(libvlc_media_player_set_video_title_display), "2.1.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_media_player_set_video_title_display(void* p_mi, libvlc_position_t position,
        uint timeout);

    /// <summary>
    ///     Add a slave to the current media player.
    /// </summary>
    /// <param name="p_mi">the media player</param>
    /// <param name="i_type">subtitle or audio</param>
    /// <param name="psz_uri">Uri of the slave (should contain a valid scheme).</param>
    /// <param name="b_select">True if this slave should be selected when it's loaded</param>
    /// <returns>0 on success, -1 on error.</returns>
    /// <remarks>
    ///     If the player is playing, the slave will be added directly. This call
    ///     will also update the slave list of the attached libvlc_media_t.
    /// </remarks>
    /// <seealso cref="libvlc_media_slaves_add" />
    [LibVlcFunction(nameof(libvlc_media_player_add_slave), "3.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate int libvlc_media_player_add_slave(void* p_mi,
        libvlc_media_slave_type_t i_type,
        byte* psz_uri, bool b_select);

    /// <summary>
    ///     Release (free) libvlc_track_description_t
    /// </summary>
    /// <param name="p_track_description">the structure to release</param>
    [LibVlcFunction(nameof(libvlc_track_description_list_release))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_track_description_list_release(libvlc_track_description_t* p_track_description);

    namespace Video
    {
        /// <summary>
        ///     Toggle fullscreen status on non-embedded video outputs.
        /// </summary>
        /// <param name="p_mi">the media player</param>
        /// <remarks>
        ///     The same limitations applies to this function
        ///     as to libvlc_set_fullscreen().
        /// </remarks>
        [LibVlcFunction(nameof(libvlc_toggle_fullscreen))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_toggle_fullscreen(void* p_mi);

        /// <summary>
        ///     Enable or disable fullscreen.
        /// </summary>
        /// <param name="p_mi">the media player</param>
        /// <param name="b_fullscreen">boolean for fullscreen status</param>
        /// <remarks>
        ///     With most window managers, only a top-level windows can be in
        ///     full-screen mode. Hence, this function will not operate properly if
        ///     libvlc_media_player_set_xwindow was used to embed the video in a
        ///     non-top-level window. In that case, the embedding window must be reparented
        ///     to the root window <b>before</b> fullscreen mode is enabled. You will want
        ///     to reparent it back to its normal parent when disabling fullscreen.
        /// </remarks>
        [LibVlcFunction(nameof(libvlc_set_fullscreen))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_set_fullscreen(void* p_mi, bool b_fullscreen);

        /// <summary>
        ///     Get current fullscreen status.
        /// </summary>
        /// <param name="p_mi">the media player</param>
        /// <returns>the fullscreen status (boolean)</returns>
        [LibVlcFunction(nameof(libvlc_get_fullscreen))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate bool libvlc_get_fullscreen(void* p_mi);

        /// <summary>
        ///     Enable or disable key press events handling, according to the LibVLC hotkeys
        ///     configuration. By default and for historical reasons, keyboard events are
        ///     handled by the LibVLC video widget.
        ///     On X11, there can be only one subscriber for key press and mouse
        ///     click events per window. If your application has subscribed to those events
        ///     for the X window ID of the video widget, then LibVLC will not be able to
        ///     handle key presses and mouse clicks in any case.
        /// </summary>
        /// <param name="p_mi">the media player</param>
        /// <param name="on">true to handle key press events, false to ignore them.</param>
        /// <remarks>
        ///     This function is only implemented for X11 and Win32 at the moment.
        /// </remarks>
        [LibVlcFunction(nameof(libvlc_video_set_key_input))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_video_set_key_input(void* p_mi, uint on);

        /// <summary>
        ///     Enable or disable mouse click events handling. By default, those events are
        ///     handled. This is needed for DVD menus to work, as well as a few video
        ///     filters such as "puzzle".
        /// </summary>
        /// <param name="p_mi">the media player</param>
        /// <param name="on">true to handle mouse click events, false to ignore them.</param>
        /// <remarks>
        ///     This function is only implemented for X11 and Win32 at the moment.
        /// </remarks>
        /// <seealso cref="libvlc_video_set_key_input" />
        [LibVlcFunction(nameof(libvlc_video_set_mouse_input))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_video_set_mouse_input(void* p_mi, uint on);

        /// <summary>
        ///     Get the pixel dimensions of a video.
        /// </summary>
        /// <param name="p_mi">media player</param>
        /// <param name="num">number of the video (starting from, and most commonly 0)</param>
        /// <param name="px">pointer to get the pixel width [OUT]</param>
        /// <param name="py">pointer to get the pixel height [OUT]</param>
        /// <returns>0 on success, -1 if the specified video does not exist</returns>
        [LibVlcFunction(nameof(libvlc_video_get_size))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_video_get_size(void* p_mi, uint num, uint* px, uint* py);


        /**
         * Get the mouse pointer coordinates over a video.
         * Coordinates are expressed in terms of the decoded video resolution,
         * <b>not</b> in terms of pixels on the screen/viewport (to get the latter,
         * you can query your windowing system directly).
         *
         * Either of the coordinates may be negative or larger than the corresponding
         * dimension of the video, if the cursor is outside the rendering area.
         *
         * @warning The coordinates may be out-of-date if the pointer is not located
         * on the video rendering area. LibVLC does not track the pointer if it is
         * outside of the video widget.
         *
         * @note LibVLC does not support multiple pointers (it does of course support
         * multiple input devices sharing the same pointer) at the moment.
         *
         * \param p_mi media player
         * \param num number of the video (starting from, and most commonly 0)
         * \param px pointer to get the abscissa [OUT]
         * \param py pointer to get the ordinate [OUT]
         * \return 0 on success, -1 if the specified video does not exist
         */
        [LibVlcFunction(nameof(libvlc_video_get_cursor))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_video_get_cursor(void* p_mi, uint num,
            int* px, int* py);

        /**
         * Get the current video scaling factor.
         * See also libvlc_video_set_scale().
         *
         * \param p_mi the media player
         * \return the currently configured zoom factor, or 0. if the video is set
         * to fit to the output window/drawable automatically.
         */
        [LibVlcFunction(nameof(libvlc_video_get_scale))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate float libvlc_video_get_scale(void* p_mi);

        /**
         * Set the video scaling factor. That is the ratio of the number of pixels on
         * screen to the number of pixels in the original decoded video in each
         * dimension. Zero is a special value; it will adjust the video to the output
         * window/drawable (in windowed mode) or the entire screen.
         *
         * Note that not all video outputs support scaling.
         *
         * \param p_mi the media player
         * \param f_factor the scaling factor, or zero
         */
        [LibVlcFunction(nameof(libvlc_video_set_scale))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_video_set_scale(void* p_mi, float f_factor);

        /**
         * Get current video aspect ratio.
         *
         * \param p_mi the media player
         * \return the video aspect ratio or NULL if unspecified
         * (the result must be released with free() or libvlc_free()).
         */
        [LibVlcFunction(nameof(libvlc_video_get_aspect_ratio))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate byte* libvlc_video_get_aspect_ratio(void* p_mi);

        /**
         * Set new video aspect ratio.
         *
         * \param p_mi the media player
         * \param psz_aspect new video aspect-ratio or NULL to reset to default
         * \note Invalid aspect ratios are ignored.
         */
        [LibVlcFunction(nameof(libvlc_video_set_aspect_ratio))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_video_set_aspect_ratio(void* p_mi, byte* psz_aspect);

        /// <summary>
        ///     Create a video viewpoint structure.
        /// </summary>
        /// <returns>video viewpoint or NULL (the result must be released with free() or libvlc_free()).</returns>
        [LibVlcFunction(nameof(libvlc_video_new_viewpoint), "3.0.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate libvlc_video_viewpoint_t* libvlc_video_new_viewpoint();

        /// <summary>
        ///     Update the video viewpoint information.
        /// </summary>
        /// <param name="p_mi">media player</param>
        /// <param name="p_viewpoint">video viewpoint allocated via <see cref="libvlc_video_new_viewpoint" /></param>
        /// <param name="b_absolute">if true replace the old viewpoint with the new one. If false, increase/decrease it.</param>
        /// <returns>-1 in case of error, 0 otherwise</returns>
        /// <remarks>
        ///     It is safe to call this function before the media player is started.
        ///     <para />
        ///     the values are set asynchronously, it will be used by the next frame displayed.
        /// </remarks>
        [LibVlcFunction(nameof(libvlc_video_update_viewpoint), "3.0.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_video_update_viewpoint(void* p_mi,
            libvlc_video_viewpoint_t* p_viewpoint,
            bool b_absolute);

        /// <summary>
        ///     Get current video subtitle.
        /// </summary>
        /// <param name="p_mi">the media player</param>
        /// <returns>the video subtitle selected, or -1 if none</returns>
        [LibVlcFunction(nameof(libvlc_video_get_spu))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_video_get_spu(void* p_mi);

        /// <summary>
        ///     Get the number of available video subtitles.
        /// </summary>
        /// <param name="p_mi">the media player</param>
        /// <returns>the number of available video subtitles</returns>
        [LibVlcFunction(nameof(libvlc_video_get_spu_count))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_video_get_spu_count(void* p_mi);

        /// <summary>
        ///     Get the description of available video subtitles.
        /// </summary>
        /// <param name="p_mi">the media player</param>
        /// <returns>
        ///     list containing description of available video subtitles. It must be freed with
        ///     <see cref="libvlc_track_description_list_release" />
        /// </returns>
        [LibVlcFunction(nameof(libvlc_video_get_spu_description))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate libvlc_track_description_t* libvlc_video_get_spu_description(void* p_mi);

        /// <summary>
        ///     Set new video subtitle.
        /// </summary>
        /// <param name="p_mi">the media player</param>
        /// <param name="i_spu">video subtitle track to select (i_id from track description)</param>
        /// <returns>0 on success, -1 if out of range</returns>
        [LibVlcFunction(nameof(libvlc_video_set_spu))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_video_set_spu(void* p_mi, int i_spu);

        /// <summary>
        ///     Get the current subtitle delay. Positive values means subtitles are being
        ///     displayed later, negative values earlier.
        /// </summary>
        /// <param name="p_mi">media player</param>
        /// <returns>time (in microseconds) the display of subtitles is being delayed</returns>
        [LibVlcFunction(nameof(libvlc_video_get_spu_delay), "2.0.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate long libvlc_video_get_spu_delay(void* p_mi);

        /// <summary>
        ///     Set the subtitle delay. This affects the timing of when the subtitle will
        ///     be displayed. Positive values result in subtitles being displayed later,
        ///     while negative values will result in subtitles being displayed earlier.
        ///     <para />
        ///     The subtitle delay will be reset to zero each time the media changes.
        /// </summary>
        /// <param name="p_mi">media player</param>
        /// <param name="i_delay">time (in microseconds) the display of subtitles should be delayed</param>
        /// <returns>0 on success, -1 on error</returns>
        [LibVlcFunction(nameof(libvlc_video_set_spu_delay), "2.0.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_video_set_spu_delay(void* p_mi, long i_delay);

        /// <summary>
        ///     Get the full description of available titles
        /// </summary>
        /// <param name="p_mi">the media player</param>
        /// <param name="titles">
        ///     address to store an allocated array of title descriptions
        ///     descriptions (must be freed with <see cref="libvlc_title_descriptions_release" />
        ///     by the caller) [OUT]
        /// </param>
        /// <returns>the number of titles (-1 on error)</returns>
        [LibVlcFunction(nameof(libvlc_media_player_get_full_title_descriptions), "3.0.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_media_player_get_full_title_descriptions(void* p_mi,
            libvlc_title_description_t*** titles);

        /// <summary>
        ///     Release a title description
        /// </summary>
        /// <param name="p_titles">title description array to release</param>
        /// <param name="i_count">number of title descriptions to release</param>
        [LibVlcFunction(nameof(libvlc_title_descriptions_release), "3.0.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_title_descriptions_release(libvlc_title_description_t** p_titles,
            uint i_count);

        /// <summary>
        ///     Get the full description of available chapters
        /// </summary>
        /// <param name="p_mi">the media player</param>
        /// <param name="i_chapters_of_title">index of the title to query for chapters (uses current title if set to -1)</param>
        /// <param name="pp_chapters">
        ///     address to store an allocated array of chapter descriptions
        ///     descriptions (must be freed with <see cref="libvlc_chapter_descriptions_release" />
        ///     by the caller) [OUT]
        /// </param>
        /// <returns>the number of chapters (-1 on error)</returns>
        [LibVlcFunction(nameof(libvlc_media_player_get_full_chapter_descriptions), "3.0.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_media_player_get_full_chapter_descriptions(void* p_mi,
            int i_chapters_of_title,
            libvlc_chapter_description_t*** pp_chapters);

        /// <summary>
        ///     Release a chapter description
        /// </summary>
        /// <param name="p_chapters">chapter description array to release</param>
        /// <param name="i_count">number of chapter descriptions to release</param>
        [LibVlcFunction(nameof(libvlc_chapter_descriptions_release), "3.0.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_chapter_descriptions_release(libvlc_chapter_description_t** p_chapters,
            uint i_count);

        /// <summary>
        ///     Get current crop filter geometry.
        /// </summary>
        /// <param name="p_mi">the media player</param>
        /// <returns>the crop filter geometry or NULL if unset</returns>
        [LibVlcFunction(nameof(libvlc_video_get_crop_geometry))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate byte* libvlc_video_get_crop_geometry(void* p_mi);

        /// <summary>
        ///     Set new crop filter geometry.
        /// </summary>
        /// <param name="p_mi">the media player</param>
        /// <param name="psz_geometry">new crop filter geometry (NULL to unset)</param>
        [LibVlcFunction(nameof(libvlc_video_set_crop_geometry))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_video_set_crop_geometry(void* p_mi, byte* psz_geometry);

        /// <summary>
        ///     Get current teletext page requested or 0 if it's disabled.
        ///     <para />
        ///     Teletext is disabled by default, call <see cref="libvlc_video_set_teletext" /> to enable it.
        /// </summary>
        /// <param name="p_mi">the media player</param>
        /// <returns>the current teletext page requested.</returns>
        [LibVlcFunction(nameof(libvlc_video_get_teletext))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_video_get_teletext(void* p_mi);

        /// <summary>
        ///     Set new teletext page to retrieve.
        ///     <para />
        ///     This function can also be used to send a teletext key.
        /// </summary>
        /// <param name="p_mi">the media player</param>
        /// <param name="i_page">
        ///     teletex page number requested. This value can be 0 to disable
        ///     teletext, a number in the range ]0;1000[ to show the requested page, or a
        ///     <see cref="libvlc_teletext_key_t" />. 100 is the default teletext page.
        /// </param>
        [LibVlcFunction(nameof(libvlc_video_set_teletext))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_video_set_teletext(void* p_mi, int i_page);

        /// <summary>
        ///     Get number of available video tracks.
        /// </summary>
        /// <param name="p_mi">media player</param>
        /// <returns>the number of available video tracks (int)</returns>
        [LibVlcFunction(nameof(libvlc_video_get_track_count))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_video_get_track_count(void* p_mi);

        /// <summary>
        ///     Get the description of available video tracks.
        /// </summary>
        /// <param name="p_mi">media player</param>
        /// <returns>
        ///     list with description of available video tracks, or NULL on error. It must be freed with
        ///     <see cref="libvlc_track_description_list_release" />
        /// </returns>
        [LibVlcFunction(nameof(libvlc_video_get_track_description))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate libvlc_track_description_t* libvlc_video_get_track_description(void* p_mi);

        /// <summary>
        ///     Get current video track.
        /// </summary>
        /// <param name="p_mi">media player</param>
        /// <returns>the video track ID (int) or -1 if no active input</returns>
        [LibVlcFunction(nameof(libvlc_video_get_track))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_video_get_track(void* p_mi);

        /// <summary>
        ///     Set video track.
        /// </summary>
        /// <param name="p_mi">media player</param>
        /// <param name="i_track">the track ID (i_id field from track description)</param>
        /// <returns>0 on success, -1 if out of range</returns>
        [LibVlcFunction(nameof(libvlc_video_set_track))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_video_set_track(void* p_mi, int i_track);

        /// <summary>
        ///     Take a snapshot of the current video window.
        ///     If i_width AND i_height is 0, original size is used.
        ///     If i_width XOR i_height is 0, original aspect-ratio is preserved.
        /// </summary>
        /// <param name="p_mi">media player instance</param>
        /// <param name="num">number of video output (typically 0 for the first/only one)</param>
        /// <param name="psz_filepath">the path of a file or a folder to save the screenshot into</param>
        /// <param name="i_width">the snapshot's width</param>
        /// <param name="i_height">the snapshot's height</param>
        /// <returns>0 on success, -1 if the video was not found</returns>
        [LibVlcFunction(nameof(libvlc_video_take_snapshot))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_video_take_snapshot(void* p_mi, uint num,
            byte* psz_filepath, uint i_width,
            uint i_height);

        /// <summary>
        ///     Enable or disable deinterlace filter
        /// </summary>
        /// <param name="p_mi">libvlc media player</param>
        /// <param name="deinterlace">state -1: auto (default), 0: disabled, 1: enabled</param>
        /// <param name="psz_mode">type of deinterlace filter, NULL for current/default filter</param>
        [LibVlcFunction(nameof(libvlc_video_set_deinterlace), "4.0.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_video_set_deinterlace(void* p_mi,
            int deinterlace,
            byte* psz_mode);

        /// <summary>
        ///     Get an integer marquee option value
        /// </summary>
        /// <param name="p_mi">libvlc media player</param>
        /// <param name="option">option marq option to get <see cref="libvlc_video_marquee_option_t" /></param>
        /// <returns></returns>
        [LibVlcFunction(nameof(libvlc_video_get_marquee_int))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_video_get_marquee_int(void* p_mi,
            uint option);

        /// <summary>
        ///     Get a string marquee option value
        /// </summary>
        /// <param name="p_mi">libvlc media player</param>
        /// <param name="option">option marq option to get <see cref="libvlc_video_marquee_option_t" /></param>
        /// <returns></returns>
        [LibVlcFunction(nameof(libvlc_video_get_marquee_string))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate byte* libvlc_video_get_marquee_string(void* p_mi,
            uint option);

        /// <summary>
        ///     Enable, disable or set an integer marquee option
        ///     <para />
        ///     Setting libvlc_marquee_Enable has the side effect of enabling (arg !0)
        ///     or disabling (arg 0) the marq filter.
        /// </summary>
        /// <param name="p_mi">libvlc media player</param>
        /// <param name="option">option marq option to get <see cref="libvlc_video_marquee_option_t" /></param>
        /// <param name="i_val">marq option value</param>
        [LibVlcFunction(nameof(libvlc_video_set_marquee_int))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_video_set_marquee_int(void* p_mi,
            uint option, int i_val);

        /// <summary>
        ///     Set a marquee string option
        /// </summary>
        /// <param name="p_mi">libvlc media player</param>
        /// <param name="option">option marq option to get <see cref="libvlc_video_marquee_option_t" /></param>
        /// <param name="psz_text">marq option value</param>
        [LibVlcFunction(nameof(libvlc_video_set_marquee_string))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_video_set_marquee_string(void* p_mi,
            uint option, byte* psz_text);

        /// <summary>
        ///     Get integer logo option.
        /// </summary>
        /// <param name="p_mi">libvlc media player instance</param>
        /// <param name="option">logo option to get, values of <see cref="libvlc_video_logo_option_t" /></param>
        /// <returns></returns>
        [LibVlcFunction(nameof(libvlc_video_get_logo_int))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_video_get_logo_int(void* p_mi,
            libvlc_video_logo_option_t option);

        /**
         * Set logo option as integer. Options that take a different type value
         * are ignored.
         * Passing libvlc_logo_enable as option value has the side effect of
         * starting (arg !0) or stopping (arg 0) the logo filter.
         *
         * \param p_mi libvlc media player instance
         * \param option logo option to set, values of libvlc_video_logo_option_t
         * \param value logo option value
         */
        /// <summary>
        ///     Set logo option as integer. Options that take a different type value
        ///     are ignored.
        ///     Passing libvlc_logo_enable as option value has the side effect of
        ///     starting (arg !0) or stopping (arg 0) the logo filter.
        /// </summary>
        /// <param name="p_mi">libvlc media player instance</param>
        /// <param name="option">logo option to set, values of <see cref="libvlc_video_logo_option_t" /></param>
        /// <param name="value">logo option value</param>
        [LibVlcFunction(nameof(libvlc_video_set_logo_int))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_video_set_logo_int(void* p_mi,
            libvlc_video_logo_option_t option, int value);

        /// <summary>
        ///     Set logo option as string. Options that take a different type value
        ///     are ignored.
        /// </summary>
        /// <param name="p_mi">libvlc media player instance</param>
        /// <param name="option">logo option to set, values of <see cref="libvlc_video_logo_option_t" /></param>
        /// <param name="psz_value">logo option value</param>
        [LibVlcFunction(nameof(libvlc_video_set_logo_string))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_video_set_logo_string(void* p_mi,
            libvlc_video_logo_option_t option, byte* psz_valueSubtitle);

        /// <summary>
        ///     Get integer adjust option.
        /// </summary>
        /// <param name="p_mi">libvlc media player instance</param>
        /// <param name="option">adjust option to get, values of <see cref="libvlc_video_adjust_option_t" /></param>
        /// <returns></returns>
        [LibVlcFunction(nameof(libvlc_video_get_adjust_int), "1.1.1")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_video_get_adjust_int(void* p_mi,
            libvlc_video_adjust_option_t option);

        /// <summary>
        ///     Set adjust option as integer. Options that take a different type value
        ///     are ignored.
        ///     Passing libvlc_adjust_enable as option value has the side effect of
        ///     starting (arg !0) or stopping (arg 0) the adjust filter.
        /// </summary>
        /// <param name="p_mi">libvlc media player instance</param>
        /// <param name="option">logo option to set, values of <see cref="libvlc_video_logo_option_t" /></param>
        /// <param name="value">adjust option value</param>
        [LibVlcFunction(nameof(libvlc_video_set_adjust_int), "1.1.1")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_video_set_adjust_int(void* p_mi,
            libvlc_video_adjust_option_t option, int value);

        /// <summary>
        ///     Get float adjust option.
        /// </summary>
        /// <param name="p_mi">libvlc media player instance</param>
        /// <param name="option">adjust option to get, values of <see cref="libvlc_video_adjust_option_t" /></param>
        /// <returns></returns>
        [LibVlcFunction(nameof(libvlc_video_get_adjust_float), "1.1.1")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate float libvlc_video_get_adjust_float(void* p_mi,
            libvlc_video_adjust_option_t option);

        /// <summary>
        ///     Set adjust option as float. Options that take a different type value are ignored.
        /// </summary>
        /// <param name="p_mi">libvlc media player instance</param>
        /// <param name="option">adust option to set, values of <see cref="libvlc_video_adjust_option_t" /></param>
        /// <param name="value">adjust option value</param>
        [LibVlcFunction(nameof(libvlc_video_set_adjust_float), "1.1.1")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_video_set_adjust_float(void* p_mi,
            libvlc_video_adjust_option_t option, float value);
    }

    namespace Audio
    {
        /// <summary>
        ///     Gets the list of available audio output modules.
        /// </summary>
        /// <param name="p_instance">libvlc instance</param>
        /// <returns>
        ///     list of available audio outputs. It must be freed with
        ///     <see cref="libvlc_audio_output_list_release" /> <see cref="libvlc_audio_output_t" />.
        ///     In case of error, NULL is returned.
        /// </returns>
        [LibVlcFunction(nameof(libvlc_audio_output_list_get))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate libvlc_audio_output_t* libvlc_audio_output_list_get(void* p_instance);

        /// <summary>
        ///     Frees the list of available audio output modules.
        /// </summary>
        /// <param name="p_list">list with audio outputs for release</param>
        [LibVlcFunction(nameof(libvlc_audio_output_list_release))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_audio_output_list_release(libvlc_audio_output_t* p_list);

        /// <summary>
        ///     Selects an audio output module.
        /// </summary>
        /// <param name="p_mi">media player</param>
        /// <param name="psz_name">name of audio output, use psz_name of <see cref="libvlc_audio_output_t" /></param>
        /// <returns>0 if function succeeded, -1 on error</returns>
        /// <remarks>
        ///     Any change will take be effect only after playback is stopped and
        ///     restarted. Audio output cannot be changed while playing.
        /// </remarks>
        [LibVlcFunction(nameof(libvlc_audio_output_set))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_audio_output_set(void* p_mi,
            byte* psz_name);

        /// <summary>
        ///     Gets a list of potential audio output devices, <see cref="libvlc_audio_output_device_set" />
        /// </summary>
        /// <param name="mp">media player</param>
        /// <returns>
        ///     A NULL-terminated linked list of potential audio output devices.
        ///     It must be freed with <see cref="libvlc_audio_output_device_list_release" />
        /// </returns>
        /// <remarks>
        ///     Not all audio outputs support enumerating devices.
        ///     The audio output may be functional even if the list is empty (NULL).
        ///     <para />
        ///     The list may not be exhaustive.
        ///     <para />
        ///     warning: Some audio output devices in the list might not actually work in
        ///     some circumstances. By default, it is recommended to not specify any
        ///     explicit audio device.
        /// </remarks>
        [LibVlcFunction(nameof(libvlc_audio_output_set), "2.2.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate libvlc_audio_output_device_t* libvlc_audio_output_device_enum(void* mp);

        /// <summary>
        ///     Gets a list of audio output devices for a given audio output module, <see cref="libvlc_audio_output_device_set" />.
        /// </summary>
        /// <param name="p_instance">libvlc instance</param>
        /// <param name="aout">audio output name (as returned by <see cref="libvlc_audio_output_list_get" />)</param>
        /// <returns>
        ///     A NULL-terminated linked list of potential audio output devices. It must be freed with
        ///     <see cref="libvlc_audio_output_device_list_release" />
        /// </returns>
        /// <remarks>
        ///     Not all audio outputs support this. In particular, an empty (NULL)
        ///     list of devices does <b>not</b> imply that the specified audio output does
        ///     not work.
        ///     <para />
        ///     The list may not be exhaustive.
        ///     <para />
        ///     warning: Some audio output devices in the list might not actually work in
        ///     some circumstances. By default, it is recommended to not specify any
        ///     explicit audio device.
        /// </remarks>
        [LibVlcFunction(nameof(libvlc_audio_output_set), "2.1.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate libvlc_audio_output_device_t* libvlc_audio_output_device_list_get(void* p_instance,
            byte* aout);

        /// <summary>
        ///     Frees a list of available audio output devices.
        /// </summary>
        /// <param name="p_list">list with audio outputs for release</param>
        [LibVlcFunction(nameof(libvlc_audio_output_device_list_release), "2.1.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_audio_output_device_list_release(
            libvlc_audio_output_device_t* p_list);

        /// <summary>
        ///     Configures an explicit audio output device.
        ///     <para />
        ///     If the module paramater is NULL, audio output will be moved to the device
        ///     specified by the device identifier string immediately. This is the
        ///     recommended usage.
        ///     <para />
        ///     A list of adequate potential device strings can be obtained with
        ///     <see cref="libvlc_audio_output_device_enum" />.
        ///     <para />
        ///     However passing NULL is supported in LibVLC version 2.2.0 and later only;
        ///     in earlier versions, this function would have no effects when the module
        ///     parameter was NULL.
        ///     <para />
        ///     If the module parameter is not NULL, the device parameter of the
        ///     corresponding audio output, if it exists, will be set to the specified
        ///     string. Note that some audio output modules do not have such a parameter
        ///     (notably MMDevice and PulseAudio).
        ///     <para />
        ///     A list of adequate potential device strings can be obtained with
        ///     <see cref="libvlc_audio_output_device_list_get" />.
        /// </summary>
        /// <param name="mp">media player</param>
        /// <param name="module">
        ///     If NULL, current audio output module.
        ///     if non-NULL, name of audio output module
        ///     (<see cref="libvlc_audio_output_t" />)
        /// </param>
        /// <param name="device_id">device identifier string</param>
        /// <remarks>
        ///     note: This function does not select the specified audio output plugin.
        ///     libvlc_audio_output_set() is used for that purpose.
        ///     <para />
        ///     warning: The syntax for the device parameter depends on the audio output.
        ///     <para />
        ///     Some audio output modules require further parameters (e.g. a channels map
        ///     in the case of ALSA).
        /// </remarks>
        [LibVlcFunction(nameof(libvlc_audio_output_device_set))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_audio_output_device_set(void* mp,
            byte* module,
            byte* device_id);

        /// <summary>
        ///     Get the current audio output device identifier.
        ///     <para />
        ///     This complements <see cref="libvlc_audio_output_device_set" />.
        /// </summary>
        /// <param name="mp"></param>
        /// <returns>
        ///     the current audio output device identifier
        ///     NULL if no device is selected or in case of error
        ///     (the result must be released with free() or <see cref="libvlc_free" />).
        /// </returns>
        /// <remarks>
        ///     warning: The initial value for the current audio output device identifier
        ///     may not be set or may be some unknown value. A LibVLC application should
        ///     compare this value against the known device identifiers (e.g. those that
        ///     were previously retrieved by a call to <see cref="libvlc_audio_output_device_enum" /> or
        ///     <see cref="libvlc_audio_output_device_list_get" />) to find the current audio output device.
        ///     <para />
        ///     It is possible that the selected audio output device changes (an external
        ///     change) without a call to libvlc_audio_output_device_set. That may make this
        ///     method unsuitable to use if a LibVLC application is attempting to track
        ///     dynamic audio device changes as they happen.
        /// </remarks>
        [LibVlcFunction(nameof(libvlc_audio_output_device_get), "3.0.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate byte* libvlc_audio_output_device_get(void* mp);

        /// <summary>
        ///     Toggle mute status.
        /// </summary>
        /// <param name="p_mi">media player</param>
        /// <remarks>
        ///     warning: Toggling mute atomically is not always possible: On some platforms,
        ///     other processes can mute the VLC audio playback stream asynchronously. Thus,
        ///     there is a small race condition where toggling will not work.
        ///     See also the limitations of <see cref="libvlc_audio_set_mute" />.
        /// </remarks>
        [LibVlcFunction(nameof(libvlc_audio_toggle_mute))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_audio_toggle_mute(void* p_mi);

        /// <summary>
        ///     Get current mute status.
        /// </summary>
        /// <param name="p_mi">media player</param>
        /// <returns>the mute status (boolean) if defined, -1 if undefined/unapplicable</returns>
        [LibVlcFunction(nameof(libvlc_audio_get_mute))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_audio_get_mute(void* p_mi);

        /// <summary>
        ///     Set mute status.
        /// </summary>
        /// <param name="p_mi">media player</param>
        /// <param name="status">If status is true then mute, otherwise unmute</param>
        /// <remarks>
        ///     warning: This function does not always work. If there are no active audio
        ///     playback stream, the mute status might not be available. If digital
        ///     pass-through (S/PDIF, HDMI...) is in use, muting may be unapplicable. Also
        ///     some audio output plugins do not support muting at all.
        ///     note: To force silent playback, disable all audio tracks. This is more
        ///     efficient and reliable than mute.
        /// </remarks>
        [LibVlcFunction(nameof(libvlc_audio_set_mute))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_audio_set_mute(void* p_mi, int status);

        /// <summary>
        ///     Get current software audio volume.
        /// </summary>
        /// <param name="p_mi">media player</param>
        /// <returns>the software volume in percents (0 = mute, 100 = nominal / 0dB)</returns>
        [LibVlcFunction(nameof(libvlc_audio_get_volume))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_audio_get_volume(void* p_mi);

        /// <summary>
        ///     Set current software audio volume.
        /// </summary>
        /// <param name="p_mi">media player</param>
        /// <param name="i_volume">the volume in percents (0 = mute, 100 = 0dB)</param>
        /// <returns>0 if the volume was set, -1 if it was out of range</returns>
        [LibVlcFunction(nameof(libvlc_audio_set_volume))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_audio_set_volume(void* p_mi, int i_volume);

        /// <summary>
        ///     Get number of available audio tracks.
        /// </summary>
        /// <param name="p_mi">media player</param>
        /// <returns>the number of available audio tracks (int), or -1 if unavailable</returns>
        [LibVlcFunction(nameof(libvlc_audio_get_track_count))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_audio_get_track_count(void* p_mi);

        /// <summary>
        ///     Get the description of available audio tracks.
        /// </summary>
        /// <param name="p_mi">media player</param>
        /// <returns>
        ///     list with description of available audio tracks, or NULL. It must be freed with
        ///     <see cref="libvlc_track_description_list_release" />
        /// </returns>
        [LibVlcFunction(nameof(libvlc_audio_get_track_description))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate libvlc_track_description_t* libvlc_audio_get_track_description(void* p_mi);

        /// <summary>
        ///     Get current audio track.
        /// </summary>
        /// <param name="p_mi">media player</param>
        /// <returns>the audio track ID or -1 if no active input.</returns>
        [LibVlcFunction(nameof(libvlc_audio_get_track))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_audio_get_track(void* p_mi);

        /// <summary>
        ///     Set current audio track.
        /// </summary>
        /// <param name="p_mi">media player</param>
        /// <param name="i_track">the track ID (i_id field from track description)</param>
        /// <returns>0 on success, -1 on error</returns>
        [LibVlcFunction(nameof(libvlc_audio_set_track))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_audio_set_track(void* p_mi, int i_track);

        /// <summary>
        ///     Get current audio channel.
        /// </summary>
        /// <param name="p_mi">media player</param>
        /// <returns>the audio channel <see cref="libvlc_audio_output_channel_t" /></returns>
        [LibVlcFunction(nameof(libvlc_audio_get_channel))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate libvlc_audio_output_channel_t libvlc_audio_get_channel(void* p_mi);

        /// <summary>
        ///     Set current audio channel.
        /// </summary>
        /// <param name="p_mi">media player</param>
        /// <param name="channel">the audio channel, <see cref="libvlc_audio_output_channel_t" /></param>
        /// <returns>0 on success, -1 on error</returns>
        [LibVlcFunction(nameof(libvlc_audio_set_channel))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_audio_set_channel(void* p_mi, libvlc_audio_output_channel_t channel);

        /// <summary>
        ///     Get current audio delay.
        /// </summary>
        /// <param name="p_mi">media player</param>
        /// <returns>the audio delay (microseconds)</returns>
        [LibVlcFunction(nameof(libvlc_audio_get_delay), "1.1.1")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate long libvlc_audio_get_delay(void* p_mi);

        /// <summary>
        ///     Set current audio delay. The audio delay will be reset to zero each time the media changes.
        /// </summary>
        /// <param name="p_mi">media player</param>
        /// <param name="i_delay">the audio delay (microseconds)</param>
        /// <returns>0 on success, -1 on error</returns>
        [LibVlcFunction(nameof(libvlc_audio_set_delay), "1.1.1")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_audio_set_delay(void* p_mi, long i_delay);

        /// <summary>
        ///     Get the number of equalizer presets.
        /// </summary>
        /// <returns>number of presets</returns>
        [LibVlcFunction(nameof(libvlc_audio_equalizer_get_preset_count), "2.2.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate uint libvlc_audio_equalizer_get_preset_count();

        /// <summary>
        ///     Get the name of a particular equalizer preset.
        ///     <para />
        ///     This name can be used, for example, to prepare a preset label or menu in a user
        ///     interface.
        /// </summary>
        /// <param name="u_index">index of the preset, counting from zero</param>
        /// <returns>preset name, or NULL if there is no such preset</returns>
        [LibVlcFunction(nameof(libvlc_audio_equalizer_get_preset_name), "2.2.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate byte* libvlc_audio_equalizer_get_preset_name(uint u_index);

        /// <summary>
        ///     Get the number of distinct frequency bands for an equalizer.
        /// </summary>
        /// <returns>number of frequency bands</returns>
        [LibVlcFunction(nameof(libvlc_audio_equalizer_get_band_count), "2.2.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate uint libvlc_audio_equalizer_get_band_count();

        /// <summary>
        ///     Get a particular equalizer band frequency.
        ///     <para />
        ///     This value can be used, for example, to create a label for an equalizer band control
        ///     in a user interface.
        /// </summary>
        /// <param name="u_index">index of the band, counting from zero</param>
        /// <returns>equalizer band frequency (Hz), or -1 if there is no such band</returns>
        [LibVlcFunction(nameof(libvlc_audio_equalizer_get_band_frequency), "2.2.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate float libvlc_audio_equalizer_get_band_frequency(uint u_index);

        /// <summary>
        ///     Create a new default equalizer, with all frequency values zeroed.
        ///     <para />
        ///     The new equalizer can subsequently be applied to a media player by invoking
        ///     <see cref="libvlc_media_player_set_equalizer" />.
        ///     <para />
        ///     The returned handle should be freed via <see cref="libvlc_audio_equalizer_release" /> when
        ///     it is no longer needed.
        /// </summary>
        /// <returns>opaque equalizer handle, or NULL on error</returns>
        [LibVlcFunction(nameof(libvlc_audio_equalizer_new), "2.2.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void* libvlc_audio_equalizer_new();

        /// <summary>
        ///     Create a new equalizer, with initial frequency values copied from an existing
        ///     preset.
        ///     <para />
        ///     The new equalizer can subsequently be applied to a media player by invoking
        ///     <see cref="libvlc_media_player_set_equalizer" />.
        ///     <para />
        ///     The returned handle should be freed via libvlc_audio_equalizer_release() when
        ///     it is no longer needed.
        /// </summary>
        /// <param name="u_index">index of the preset, counting from zero</param>
        /// <returns>opaque equalizer handle, or NULL on error</returns>
        [LibVlcFunction(nameof(libvlc_audio_equalizer_new_from_preset), "2.2.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void* libvlc_audio_equalizer_new_from_preset(uint u_index);

        /// <summary>
        ///     Release a previously created equalizer instance.
        ///     <para />
        ///     The equalizer was previously created by using <see cref="libvlc_audio_equalizer_new" /> or
        ///     <see cref="libvlc_audio_equalizer_new_from_preset" />.
        ///     <para />
        ///     It is safe to invoke this method with a NULL p_equalizer parameter for no effect.
        /// </summary>
        /// <param name="p_equalizer"></param>
        [LibVlcFunction(nameof(libvlc_audio_equalizer_release), "2.2.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_audio_equalizer_release(void* p_equalizer);

        /// <summary>
        ///     Set a new pre-amplification value for an equalizer.
        ///     <para />
        ///     The new equalizer settings are subsequently applied to a media player by invoking
        ///     libvlc_media_player_set_equalizer().
        ///     <para />
        ///     The supplied amplification value will be clamped to the -20.0 to +20.0 range.
        /// </summary>
        /// <param name="p_equalizer">valid equalizer handle, must not be NULL</param>
        /// <param name="f_preamp">preamp value (-20.0 to 20.0 Hz)</param>
        /// <returns>zero on success, -1 on error</returns>
        [LibVlcFunction(nameof(libvlc_audio_equalizer_set_preamp), "2.2.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_audio_equalizer_set_preamp(void* p_equalizer, float f_preamp);

        /// <summary>
        ///     Get the current pre-amplification value from an equalizer.
        /// </summary>
        /// <param name="p_equalizer">p_equalizer valid equalizer handle, must not be NULL</param>
        /// <returns> preamp value (Hz)</returns>
        [LibVlcFunction(nameof(libvlc_audio_equalizer_get_preamp), "2.2.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate float libvlc_audio_equalizer_get_preamp(void* p_equalizer);

        /// <summary>
        ///     Set a new amplification value for a particular equalizer frequency band.
        ///     The new equalizer settings are subsequently applied to a media player by invoking
        ///     libvlc_media_player_set_equalizer().
        ///     The supplied amplification value will be clamped to the -20.0 to +20.0 range.
        /// </summary>
        /// <param name="p_equalizer">valid equalizer handle, must not be NULL</param>
        /// <param name="f_amp">amplification value (-20.0 to 20.0 Hz)</param>
        /// <param name="u_band">index, counting from zero, of the frequency band to set</param>
        /// <returns>zero on success, -1 on error</returns>
        [LibVlcFunction(nameof(libvlc_audio_equalizer_set_amp_at_index), "2.2.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_audio_equalizer_set_amp_at_index(void* p_equalizer, float f_amp, uint u_band);

        /// <summary>
        ///     Get the amplification value for a particular equalizer frequency band.
        /// </summary>
        /// <param name="p_equalizer">valid equalizer handle, must not be NULL</param>
        /// <param name="u_band">index, counting from zero, of the frequency band to get</param>
        /// <returns>amplification value (Hz); NaN if there is no such frequency band</returns>
        [LibVlcFunction(nameof(libvlc_audio_equalizer_get_amp_at_index), "2.2.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate float libvlc_audio_equalizer_get_amp_at_index(void* p_equalizer, uint u_band);

        /// <summary>
        ///     Apply new equalizer settings to a media player.
        ///     The equalizer is first created by invoking libvlc_audio_equalizer_new() or
        ///     libvlc_audio_equalizer_new_from_preset().
        ///     It is possible to apply new equalizer settings to a media player whether the media
        ///     player is currently playing media or not.
        ///     Invoking this method will immediately apply the new equalizer settings to the audio
        ///     output of the currently playing media if there is any.
        ///     If there is no currently playing media, the new equalizer settings will be applied
        ///     later if and when new media is played.
        ///     Equalizer settings will automatically be applied to subsequently played media.
        ///     To disable the equalizer for a media player invoke this method passing NULL for the
        ///     p_equalizer parameter.
        ///     The media player does not keep a reference to the supplied equalizer so it is safe
        ///     for an application to release the equalizer reference any time after this method
        ///     returns.
        /// </summary>
        /// <param name="p_mi">opaque media player handle</param>
        /// <param name="p_equalizer">opaque equalizer handle, or NULL to disable the equalizer for this media player</param>
        /// <returns>zero on success, -1 on error</returns>
        [LibVlcFunction(nameof(libvlc_media_player_set_equalizer), "2.2.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_media_player_set_equalizer(void* p_mi, void* p_equalizer);

        /// <summary>
        ///     Gets the media role.
        /// </summary>
        /// <param name="p_mi">media player</param>
        /// <returns>the media player role (<see cref="libvlc_media_player_role_t" />)</returns>
        [LibVlcFunction(nameof(libvlc_media_player_get_role), "3.0.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate libvlc_media_player_role_t libvlc_media_player_get_role(void* p_mi);

        /// <summary>
        ///     Sets the media role.
        /// </summary>
        /// <param name="p_mi">media player</param>
        /// <param name="role">the media player role (<see cref="libvlc_media_player_role_t" />)</param>
        /// <returns>0 on success, -1 on error</returns>
        [LibVlcFunction(nameof(libvlc_media_player_set_role), "3.0.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_media_player_set_role(void* p_mi, uint role);
    }
}