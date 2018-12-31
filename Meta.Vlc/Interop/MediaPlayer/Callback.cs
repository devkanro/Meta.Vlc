// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: Callback.cs
// Version: 20181231

using System.Runtime.InteropServices;

namespace Meta.Vlc.Interop.MediaPlayer
{
    /// <summary>
    ///     Callback prototype to allocate and lock a picture buffer.
    ///     <para />
    ///     Whenever a new video frame needs to be decoded, the lock callback is
    ///     invoked. Depending on the video chroma, one or three pixel planes of
    ///     adequate dimensions must be returned via the second parameter. Those
    ///     planes must be aligned on 32-bytes boundaries.
    /// </summary>
    /// <param name="opaque">private pointer as passed to <see cref="libvlc_video_set_callbacks" /> [IN]</param>
    /// <param name="planes">
    ///     start address of the pixel planes (LibVLC allocates the array of void pointers, this callback must
    ///     initialize the array) [OUT]
    /// </param>
    /// <returns>a private pointer for the display and unlock callbacks to identify the picture buffers</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void* libvlc_video_lock_cb(void* opaque, void** planes);

    /// <summary>
    ///     Callback prototype to unlock a picture buffer.
    ///     <para />
    ///     When the video frame decoding is complete, the unlock callback is invoked.
    ///     This callback might not be needed at all. It is only an indication that the
    ///     application can now read the pixel values if it needs to.
    /// </summary>
    /// <param name="opaque">A picture buffer is unlocked after the picture is decoded, but before the picture is displayed.</param>
    /// <param name="picture">private pointer as passed to <see cref="libvlc_video_set_callbacks" /> [IN]</param>
    /// <param name="planes">
    ///     private pointer returned from the <see cref="libvlc_video_lock_cb" /> callback (this parameter is
    ///     only for convenience) [IN]
    /// </param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void libvlc_video_unlock_cb(void* opaque, void* picture, void** planes);

    /// <summary>
    ///     Callback prototype to display a picture.
    ///     <para />
    ///     When the video frame needs to be shown, as determined by the media playback
    ///     clock, the display callback is invoked.
    /// </summary>
    /// <param name="opaque">private pointer as passed to <see cref="libvlc_video_set_callbacks" /> [IN]</param>
    /// <param name="picture">private pointer returned from the <see cref="libvlc_video_lock_cb" /> callback [IN]</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void libvlc_video_display_cb(void* opaque, void* picture);

    /// <summary>
    ///     Callback prototype to configure picture buffers format.
    ///     This callback gets the format of the video as output by the video decoder
    ///     and the chain of video filters (if any). It can opt to change any parameter
    ///     as it needs. In that case, LibVLC will attempt to convert the video format
    ///     (rescaling and chroma conversion) but these operations can be CPU intensive.
    /// </summary>
    /// <param name="opaque">pointer to the private pointer passed to <see cref="libvlc_video_set_callbacks" /> [IN/OUT]</param>
    /// <param name="chroma">pointer to the 4 bytes video format identifier [IN/OUT]</param>
    /// <param name="width">pointer to the pixel width [IN/OUT]</param>
    /// <param name="height">pointer to the pixel height [IN/OUT]</param>
    /// <param name="pitches">table of scanline pitches in bytes for each pixel plane (the table is allocated by LibVLC) [OUT]</param>
    /// <param name="lines">table of scanlines count for each plane [OUT]</param>
    /// <returns>the number of picture buffers allocated, 0 indicates failure</returns>
    /// <remarks>
    ///     For each pixels plane, the scanline pitch must be bigger than or equal to
    ///     the number of bytes per pixel multiplied by the pixel width.
    ///     Similarly, the number of scanlines must be bigger than of equal to
    ///     the pixel height.
    ///     <para />
    ///     Furthermore, we recommend that pitches and lines be multiple of 32
    ///     to not break assumptions that might be held by optimized code
    ///     in the video decoders, video filters and/or video converters.
    /// </remarks>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate uint libvlc_video_format_cb(void** opaque, byte* chroma, uint* width, uint* height,
        uint* pitches, uint* lines);

    /// <summary>
    ///     Callback prototype to configure picture buffers format.
    /// </summary>
    /// <param name="opaque">
    ///     opaque private pointer as passed to <see cref="libvlc_video_set_callbacks" /> (and possibly
    ///     modified by <see cref="libvlc_video_format_cb" />) [IN]
    /// </param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void libvlc_video_cleanup_cb(void* opaque);

    /// <summary>
    ///     Callback prototype for audio playback.
    ///     <para />
    ///     The LibVLC media player decodes and post-processes the audio signal
    ///     asynchronously (in an internal thread). Whenever audio samples are ready
    ///     to be queued to the output, this callback is invoked.
    ///     <para />
    ///     The number of samples provided per invocation may depend on the file format,
    ///     the audio coding algorithm, the decoder plug-in, the post-processing
    ///     filters and timing. Application must not assume a certain number of samples.
    ///     <para />
    ///     The exact format of audio samples is determined by <see cref="libvlc_audio_set_format" />
    ///     or <see cref="libvlc_audio_set_format_callbacks" /> as is the channels layout.
    ///     <para />
    ///     Note that the number of samples is per channel. For instance, if the audio
    ///     track sampling rate is 48000 Hz, then 1200 samples represent 25 milliseconds
    ///     of audio signal - regardless of the number of audio channels.
    /// </summary>
    /// <param name="data">data pointer as passed to <see cref="libvlc_audio_set_callbacks" /> [IN]</param>
    /// <param name="samples">pointer to a table of audio samples to play back [IN]</param>
    /// <param name="count">number of audio samples to play back</param>
    /// <param name="pts">expected play time stamp (see libvlc_delay())</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void libvlc_audio_play_cb(void* data, void* samples, uint count, long pts);

    /// <summary>
    ///     Callback prototype for audio pause.
    ///     <para />
    ///     LibVLC invokes this callback to pause audio playback.
    /// </summary>
    /// <param name="data">data pointer as passed to <see cref="libvlc_audio_set_callbacks" /> [IN]</param>
    /// <param name="pts">time stamp of the pause request (should be elapsed already)</param>
    /// <remarks>The pause callback is never called if the audio is already paused.</remarks>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void libvlc_audio_pause_cb(void* data, long pts);

    /// <summary>
    ///     Callback prototype for audio resumption.
    ///     <para />
    ///     LibVLC invokes this callback to resume audio playback after it was
    ///     previously paused.
    /// </summary>
    /// <param name="data">data pointer as passed to <see cref="libvlc_audio_set_callbacks" /> [IN]</param>
    /// <param name="pts">time stamp of the resumption request (should be elapsed already)</param>
    /// <remarks>The resume callback is never called if the audio is not paused.</remarks>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void libvlc_audio_resume_cb(void* data, long pts);

    /// <summary>
    ///     Callback prototype for audio buffer flush.
    ///     <para />
    ///     LibVLC invokes this callback if it needs to discard all pending buffers and
    ///     stop playback as soon as possible. This typically occurs when the media is
    ///     stopped.
    /// </summary>
    /// <param name="data">data pointer as passed to <see cref="libvlc_audio_set_callbacks" /> [IN]</param>
    /// <param name="pts"></param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void libvlc_audio_flush_cb(void* data, long pts);

    /// <summary>
    ///     Callback prototype for audio buffer drain.
    ///     <para />
    ///     LibVLC may invoke this callback when the decoded audio track is ending.
    ///     There will be no further decoded samples for the track, but playback should
    ///     nevertheless continue until all already pending buffers are rendered.
    /// </summary>
    /// <param name="data">data pointer as passed to <see cref="libvlc_audio_set_callbacks" /> [IN]</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void libvlc_audio_drain_cb(void* data);

    /// <summary>
    ///     Callback prototype for audio volume change.
    /// </summary>
    /// <param name="data">data pointer as passed to <see cref="libvlc_audio_set_callbacks" /> [IN]</param>
    /// <param name="volume">software volume (1. = nominal, 0. = mute)</param>
    /// <param name="mute">muted flag</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void libvlc_audio_set_volume_cb(void* data, float volume, bool mute);

    /// <summary>
    ///     Callback prototype to setup the audio playback.
    ///     <para />
    ///     This is called when the media player needs to create a new audio output.
    /// </summary>
    /// <param name="data">pointer to the data pointer passed to libvlc_audio_set_callbacks() [IN/OUT]</param>
    /// <param name="format">4 bytes sample format [IN/OUT]</param>
    /// <param name="rate">sample rate [IN/OUT]</param>
    /// <param name="channels">channels count [IN/OUT]</param>
    /// <returns>0 on success, anything else to skip audio playback</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int libvlc_audio_setup_cb(void** data, char* format, uint* rate, uint* channels);

    /// <summary>
    ///     Callback prototype for audio playback cleanup.
    ///     <para />
    ///     This is called when the media player no longer needs an audio output.
    /// </summary>
    /// <param name="data">data pointer as passed to <see cref="libvlc_audio_set_callbacks" /> [IN]</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void libvlc_audio_cleanup_cb(void* data);
}