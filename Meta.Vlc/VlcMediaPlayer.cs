// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: VlcMediaPlayer.cs
// Version: 20181231

using System;
using System.Collections.Generic;
using Meta.Vlc.Event;
using Meta.Vlc.Interop.MediaPlayer;
using Meta.Vlc.Interop.MediaPlayer.Audio;
using Meta.Vlc.Interop.MediaPlayer.Video;

namespace Meta.Vlc
{
    /// <summary>
    ///     The lowest layer API wrapper of LibVlc media player.
    /// </summary>
    public unsafe class VlcMediaPlayer : IVlcObjectWithEvent
    {
        private bool _disposed;

        private VlcMediaPlayer(IVlcObject parentVlcObject, void* pointer)
        {
            VlcInstance = parentVlcObject.VlcInstance;
            InstancePointer = pointer;
            VlcObjectManager.Add(this);

            EventManager = new VlcEventManager(this,
                LibVlcManager.GetFunctionDelegate<libvlc_media_player_event_manager>().Invoke(InstancePointer));

            EventManager.Attach(EventType.MediaPlayerPlaying);
            EventManager.Attach(EventType.MediaPlayerPaused);
            EventManager.Attach(EventType.MediaPlayerOpening);
            EventManager.Attach(EventType.MediaPlayerBuffering);
            EventManager.Attach(EventType.MediaPlayerStopped);
            EventManager.Attach(EventType.MediaPlayerForward);
            EventManager.Attach(EventType.MediaPlayerBackward);
            EventManager.Attach(EventType.MediaPlayerEndReached);
            EventManager.Attach(EventType.MediaPlayerMediaChanged);
            EventManager.Attach(EventType.MediaPlayerNothingSpecial);
            EventManager.Attach(EventType.MediaPlayerPausableChanged);
            EventManager.Attach(EventType.MediaPlayerPositionChanged);
            EventManager.Attach(EventType.MediaPlayerSeekableChanged);
            EventManager.Attach(EventType.MediaPlayerSnapshotTaken);
            EventManager.Attach(EventType.MediaPlayerTimeChanged);
            EventManager.Attach(EventType.MediaPlayerTitleChanged);
            EventManager.Attach(EventType.MediaPlayerLengthChanged);
            EventManager.Attach(EventType.MediaPlayerEncounteredError);
            EventManager.Attach(EventType.MediaPlayerVout);
            EventManager.Attach(EventType.MediaPlayerScrambledChanged);
            EventManager.Attach(EventType.MediaPlayerESAdded);
            EventManager.Attach(EventType.MediaPlayerESDeleted);
            EventManager.Attach(EventType.MediaPlayerESSelected);
            EventManager.Attach(EventType.MediaPlayerCorked);
            EventManager.Attach(EventType.MediaPlayerUncorked);
            EventManager.Attach(EventType.MediaPlayerMuted);
            EventManager.Attach(EventType.MediaPlayerUnmuted);
            EventManager.Attach(EventType.MediaPlayerAudioVolume);
            EventManager.Attach(EventType.MediaPlayerAudioDevice);
            EventManager.Attach(EventType.MediaPlayerChapterChanged);

            EventManager.VlcEventFired += OnVlcEventFired;
        }

        /// <summary>
        ///     Get the media used by the media_player.
        /// </summary>
        public VlcMedia Media
        {
            get => InstancePointer == null
                ? null
                : VlcObjectManager.GetVlcObject(LibVlcManager.GetFunctionDelegate<libvlc_media_player_get_media>()
                    .Invoke(InstancePointer)) as VlcMedia;
            set => LibVlcManager.GetFunctionDelegate<libvlc_media_player_set_media>()
                .Invoke(InstancePointer, value != null ? value.InstancePointer : null);
        }

        /// <summary>
        ///     is playing
        /// </summary>
        public bool IsPlaying =>
            LibVlcManager.GetFunctionDelegate<libvlc_media_player_is_playing>().Invoke(InstancePointer);

        /// <summary>
        ///     Get or set movie position as percentage between 0.0 and 1.0.
        /// </summary>
        public float Position
        {
            get => LibVlcManager.GetFunctionDelegate<libvlc_media_player_get_position>().Invoke(InstancePointer);
            set => LibVlcManager.GetFunctionDelegate<libvlc_media_player_set_position>().Invoke(InstancePointer, value);
        }

        /// <summary>
        ///     Get or set the Windows API window handle (HWND). The handle will be returned even if LibVLC
        ///     is not currently outputting any video to it.
        /// </summary>
        public IntPtr Hwnd
        {
            get => new IntPtr(LibVlcManager.GetFunctionDelegate<libvlc_media_player_get_hwnd>()
                .Invoke(InstancePointer));
            set => LibVlcManager.GetFunctionDelegate<libvlc_media_player_set_hwnd>()
                .Invoke(InstancePointer, value.ToPointer());
        }

        /// <summary>
        ///     Get the current movie length.
        /// </summary>
        public TimeSpan Length
        {
            get
            {
                var ms = LibVlcManager.GetFunctionDelegate<libvlc_media_player_get_length>().Invoke(InstancePointer);
                return ms != -1 ? new TimeSpan(ms * 10000) : TimeSpan.Zero;
            }
        }

        /// <summary>
        ///     Get or set the current movie time.
        /// </summary>
        public TimeSpan Time
        {
            get
            {
                var ms = LibVlcManager.GetFunctionDelegate<libvlc_media_player_get_time>().Invoke(InstancePointer);
                return ms != -1 ? new TimeSpan(ms * 10000) : TimeSpan.Zero;
            }
            set => LibVlcManager.GetFunctionDelegate<libvlc_media_player_set_time>()
                .Invoke(InstancePointer, value.Milliseconds);
        }

        /// <summary>
        ///     Get or set movie chapter.
        /// </summary>
        public int Chapter
        {
            get => LibVlcManager.GetFunctionDelegate<libvlc_media_player_get_chapter>().Invoke(InstancePointer);
            set => LibVlcManager.GetFunctionDelegate<libvlc_media_player_set_chapter>().Invoke(InstancePointer, value);
        }

        /// <summary>
        ///     Get movie chapter count
        /// </summary>
        public int ChapterCount => LibVlcManager.GetFunctionDelegate<libvlc_media_player_get_chapter_count>()
            .Invoke(InstancePointer);

        /// <summary>
        ///     Is the player able to play
        /// </summary>
        public bool CanPlay =>
            LibVlcManager.GetFunctionDelegate<libvlc_media_player_will_play>().Invoke(InstancePointer);

        /// <summary>
        ///     Get or set movie title
        /// </summary>
        public int Title
        {
            get => LibVlcManager.GetFunctionDelegate<libvlc_media_player_get_title>().Invoke(InstancePointer);
            set => LibVlcManager.GetFunctionDelegate<libvlc_media_player_set_title>().Invoke(InstancePointer, value);
        }

        /// <summary>
        ///     Get movie title count
        /// </summary>
        public int TitleCount => LibVlcManager.GetFunctionDelegate<libvlc_media_player_get_title_count>()
            .Invoke(InstancePointer);

        /// <summary>
        ///     Get or set the requested movie play rate.
        ///     <para />
        ///     Depending on the underlying media, the requested rate may be
        ///     different from the real playback rate.
        /// </summary>
        public float Rate
        {
            get => LibVlcManager.GetFunctionDelegate<libvlc_media_player_get_rate>().Invoke(InstancePointer);
            set => LibVlcManager.GetFunctionDelegate<libvlc_media_player_set_rate>().Invoke(InstancePointer, value);
        }

        /// <summary>
        ///     Get current movie state
        /// </summary>
        public MediaState State => (MediaState) LibVlcManager.GetFunctionDelegate<libvlc_media_player_get_state>()
            .Invoke(InstancePointer);

        /// <summary>
        ///     How many video outputs does this media player have?
        /// </summary>
        public uint VideoOutCount =>
            LibVlcManager.GetFunctionDelegate<libvlc_media_player_has_vout>().Invoke(InstancePointer);

        /// <summary>
        ///     Is this media player seekable?
        /// </summary>
        public bool IsSeekable =>
            LibVlcManager.GetFunctionDelegate<libvlc_media_player_is_seekable>().Invoke(InstancePointer);

        /// <summary>
        ///     Can this media player be paused?
        /// </summary>
        public bool CanPause =>
            LibVlcManager.GetFunctionDelegate<libvlc_media_player_can_pause>().Invoke(InstancePointer);

        /// <summary>
        ///     Get or set current software audio volume.
        /// </summary>
        public int Volume
        {
            get => LibVlcManager.GetFunctionDelegate<libvlc_audio_get_volume>().Invoke(InstancePointer);
            set => LibVlcManager.GetFunctionDelegate<libvlc_audio_set_volume>().Invoke(InstancePointer, value);
        }

        /// <summary>
        ///     Get or set current mute status.
        ///     <para />
        ///     warning: This function does not always work. If there are no active audio
        ///     playback stream, the mute status might not be available. If digital
        ///     pass-through (S/PDIF, HDMI...) is in use, muting may be unapplicable. Also
        ///     some audio output plugins do not support muting at all.
        ///     note: To force silent playback, disable all audio tracks. This is more
        ///     efficient and reliable than mute.
        /// </summary>
        public bool IsMute
        {
            get => LibVlcManager.GetFunctionDelegate<libvlc_audio_get_mute>().Invoke(InstancePointer) > 0;
            set => LibVlcManager.GetFunctionDelegate<libvlc_audio_set_volume>().Invoke(InstancePointer, value ? 1 : 0);
        }

        /// <summary>
        ///     Get or set current audio channel.
        /// </summary>
        public AudioOutputChannel AudioOutputChannel
        {
            get => (AudioOutputChannel) LibVlcManager.GetFunctionDelegate<libvlc_audio_get_channel>()
                .Invoke(InstancePointer);
            set => LibVlcManager.GetFunctionDelegate<libvlc_audio_set_channel>()
                .Invoke(InstancePointer, (libvlc_audio_output_channel_t) value);
        }

        /// <summary>
        ///     Get number of available audio tracks.
        /// </summary>
        public int AudioTrackCount =>
            LibVlcManager.GetFunctionDelegate<libvlc_audio_get_track_count>().Invoke(InstancePointer);

        /// <summary>
        ///     Get or set current audio track.
        /// </summary>
        public int AudioTrack
        {
            get => LibVlcManager.GetFunctionDelegate<libvlc_audio_get_track>().Invoke(InstancePointer);
            set => LibVlcManager.GetFunctionDelegate<libvlc_audio_set_track>().Invoke(InstancePointer, value);
        }

        /// <summary>
        ///     Get the description of available audio tracks.
        /// </summary>
        public List<TrackDescription> AudioTrackDescription
        {
            get
            {
                using (var list = new TrackDescriptionList(LibVlcManager
                    .GetFunctionDelegate<libvlc_audio_get_track_description>().Invoke(InstancePointer)))
                {
                    return new List<TrackDescription>(list);
                }
            }
        }

        public int VideoTrackCount =>
            LibVlcManager.GetFunctionDelegate<libvlc_video_get_track_count>().Invoke(InstancePointer);

        public int VideoTrack
        {
            get => LibVlcManager.GetFunctionDelegate<libvlc_video_get_track>().Invoke(InstancePointer);
            set => LibVlcManager.GetFunctionDelegate<libvlc_video_set_track>().Invoke(InstancePointer, value);
        }

        public List<TrackDescription> VideoTrackDescription
        {
            get
            {
                using (var list = new TrackDescriptionList(LibVlcManager
                    .GetFunctionDelegate<libvlc_video_get_track_description>().Invoke(InstancePointer)))
                {
                    return new List<TrackDescription>(list);
                }
            }
        }

        public Size VideoSize
        {
            get
            {
                uint x = 0, y = 0;
                LibVlcManager.GetFunctionDelegate<libvlc_video_get_size>().Invoke(InstancePointer, 0, &x, &y);
                return new Size(x, y);
            }
        }

        public double PixelHeight => VideoSize.Height;

        public double PixelWidth => VideoSize.Width;

        public float Scale
        {
            get => LibVlcManager.GetFunctionDelegate<libvlc_video_get_scale>().Invoke(InstancePointer);
            set => LibVlcManager.GetFunctionDelegate<libvlc_video_set_scale>().Invoke(InstancePointer, value);
        }

        public string AspectRatio
        {
            get => InteropHelper.PtrToString(LibVlcManager.GetFunctionDelegate<libvlc_video_get_aspect_ratio>()
                .Invoke(InstancePointer));
            set
            {
                using (var handle = new StringHandle(value))
                {
                    LibVlcManager.GetFunctionDelegate<libvlc_video_set_aspect_ratio>()
                        .Invoke(InstancePointer, handle.UnsafePointer);
                }
            }
        }

        public int Subtitle
        {
            get => LibVlcManager.GetFunctionDelegate<libvlc_video_get_spu>().Invoke(InstancePointer);
            set => LibVlcManager.GetFunctionDelegate<libvlc_video_set_spu>().Invoke(InstancePointer, value);
        }

        public int SubtitleCount =>
            LibVlcManager.GetFunctionDelegate<libvlc_video_get_spu_count>().Invoke(InstancePointer);

        public long SubtitleDelay
        {
            get => LibVlcManager.GetFunctionDelegate<libvlc_video_get_spu_delay>().Invoke(InstancePointer);
            set => LibVlcManager.GetFunctionDelegate<libvlc_video_set_spu_delay>().Invoke(InstancePointer, value);
        }

        public List<TrackDescription> SubtitleDescription
        {
            get
            {
                using (var list = new TrackDescriptionList(LibVlcManager
                    .GetFunctionDelegate<libvlc_video_get_spu_description>().Invoke(InstancePointer)))
                {
                    return new List<TrackDescription>(list);
                }
            }
        }

        public bool IsAdjustEnable
        {
            get => LibVlcManager.GetFunctionDelegate<libvlc_video_get_adjust_int>()
                       .Invoke(InstancePointer, libvlc_video_adjust_option_t.libvlc_adjust_Enable) > 0;
            set => LibVlcManager.GetFunctionDelegate<libvlc_video_set_adjust_int>().Invoke(InstancePointer,
                libvlc_video_adjust_option_t.libvlc_adjust_Enable, value ? 1 : 0);
        }

        public float Contrast
        {
            get => LibVlcManager.GetFunctionDelegate<libvlc_video_get_adjust_float>().Invoke(InstancePointer,
                libvlc_video_adjust_option_t.libvlc_adjust_Contrast);
            set => LibVlcManager.GetFunctionDelegate<libvlc_video_set_adjust_float>().Invoke(InstancePointer,
                libvlc_video_adjust_option_t.libvlc_adjust_Contrast, value);
        }

        public float Brightness
        {
            get => LibVlcManager.GetFunctionDelegate<libvlc_video_get_adjust_float>().Invoke(InstancePointer,
                libvlc_video_adjust_option_t.libvlc_adjust_Brightness);
            set => LibVlcManager.GetFunctionDelegate<libvlc_video_set_adjust_float>().Invoke(InstancePointer,
                libvlc_video_adjust_option_t.libvlc_adjust_Brightness, value);
        }

        public float Hue
        {
            get => LibVlcManager.GetFunctionDelegate<libvlc_video_get_adjust_float>()
                .Invoke(InstancePointer, libvlc_video_adjust_option_t.libvlc_adjust_Hue);
            set => LibVlcManager.GetFunctionDelegate<libvlc_video_set_adjust_float>().Invoke(InstancePointer,
                libvlc_video_adjust_option_t.libvlc_adjust_Hue, value);
        }

        public float Saturation
        {
            get => LibVlcManager.GetFunctionDelegate<libvlc_video_get_adjust_float>().Invoke(InstancePointer,
                libvlc_video_adjust_option_t.libvlc_adjust_Saturation);
            set => LibVlcManager.GetFunctionDelegate<libvlc_video_set_adjust_float>().Invoke(InstancePointer,
                libvlc_video_adjust_option_t.libvlc_adjust_Saturation, value);
        }

        public float Gamma
        {
            get => LibVlcManager.GetFunctionDelegate<libvlc_video_get_adjust_float>()
                .Invoke(InstancePointer, libvlc_video_adjust_option_t.libvlc_adjust_Gamma);
            set => LibVlcManager.GetFunctionDelegate<libvlc_video_set_adjust_float>().Invoke(InstancePointer,
                libvlc_video_adjust_option_t.libvlc_adjust_Gamma, value);
        }

        public void* InstancePointer { get; private set; }

        public VlcEventManager EventManager { get; }

        public Vlc VlcInstance { get; }

        public void Dispose()
        {
            Dispose(true);
        }

        public static VlcMediaPlayer Create(Vlc vlc)
        {
            return new VlcMediaPlayer(vlc,
                LibVlcManager.GetFunctionDelegate<libvlc_media_player_new>().Invoke(vlc.InstancePointer));
        }

        public static VlcMediaPlayer CreateFormMedia(VlcMedia media)
        {
            return new VlcMediaPlayer(media,
                LibVlcManager.GetFunctionDelegate<libvlc_media_player_new_from_media>().Invoke(media.InstancePointer));
        }

        private void OnVlcEventFired(object sender, VlcEventArgs e)
        {
            switch (e.Type)
            {
                case EventType.MediaPlayerMediaChanged:
                    MediaChanged?.Invoke(this,
                        new MediaPlayerValueChangedEventArgs<IntPtr>(e.EventArgs->media_player_media_changed
                            .new_media));
                    break;
                case EventType.MediaPlayerNothingSpecial:
                    MediaStateChanged?.Invoke(this, new MediaStateChangedEventArgs(MediaState.NothingSpecial));
                    break;
                case EventType.MediaPlayerOpening:
                    MediaStateChanged?.Invoke(this, new MediaStateChangedEventArgs(MediaState.Opening));
                    break;
                case EventType.MediaPlayerBuffering:
                    MediaStateChanged?.Invoke(this, new MediaStateChangedEventArgs(MediaState.Buffering));
                    MediaPlayerBuffing?.Invoke(this,
                        new MediaPlayerBufferingEventArgs(e.EventArgs->media_player_buffering.new_cache));
                    break;
                case EventType.MediaPlayerPlaying:
                    MediaStateChanged?.Invoke(this, new MediaStateChangedEventArgs(MediaState.Playing));
                    break;
                case EventType.MediaPlayerPaused:
                    MediaStateChanged?.Invoke(this, new MediaStateChangedEventArgs(MediaState.Paused));
                    break;
                case EventType.MediaPlayerStopped:
                    MediaStateChanged?.Invoke(this, new MediaStateChangedEventArgs(MediaState.Stopped));
                    break;
                case EventType.MediaPlayerForward:
                    Forward?.Invoke(this, null);
                    break;
                case EventType.MediaPlayerBackward:
                    Backward?.Invoke(this, null);
                    break;
                case EventType.MediaPlayerEndReached:
                    MediaStateChanged?.Invoke(this, new MediaStateChangedEventArgs(MediaState.Ended));
                    break;
                case EventType.MediaPlayerEncounteredError:
                    MediaStateChanged?.Invoke(this, new MediaStateChangedEventArgs(MediaState.Error));
                    break;
                case EventType.MediaPlayerTimeChanged:
                    TimeChanged?.Invoke(this,
                        new MediaPlayerValueChangedEventArgs<long>(e.EventArgs->media_player_time_changed.new_time));
                    break;
                case EventType.MediaPlayerPositionChanged:
                    PositionChanged?.Invoke(this,
                        new MediaPlayerValueChangedEventArgs<float>(e.EventArgs->media_player_position_changed
                            .new_position));
                    break;
                case EventType.MediaPlayerSeekableChanged:
                    SeekableChanged?.Invoke(this,
                        new MediaPlayerValueChangedEventArgs<bool>(
                            e.EventArgs->media_player_seekable_changed.new_seekable > 0));
                    break;
                case EventType.MediaPlayerPausableChanged:
                    PausableChanged?.Invoke(this,
                        new MediaPlayerValueChangedEventArgs<bool>(
                            e.EventArgs->media_player_pausable_changed.new_pausable > 0));
                    break;
                case EventType.MediaPlayerTitleChanged:
                    TitleChanged?.Invoke(this,
                        new MediaPlayerValueChangedEventArgs<int>(e.EventArgs->media_player_title_changed.new_title));
                    break;
                case EventType.MediaPlayerSnapshotTaken:
                    SnapshotTaken?.Invoke(this,
                        new ObjectEventArgs<string>(
                            InteropHelper.PtrToString(e.EventArgs->media_player_snapshot_taken.psz_filename)));
                    break;
                case EventType.MediaPlayerLengthChanged:
                    LengthChanged?.Invoke(this,
                        new MediaPlayerValueChangedEventArgs<long>(e.EventArgs->media_player_length_changed
                            .new_length));
                    break;
                case EventType.MediaPlayerVout:
                    VideoOutChanged?.Invoke(this,
                        new MediaPlayerValueChangedEventArgs<int>(e.EventArgs->media_player_vout.new_count));
                    break;
                case EventType.MediaPlayerScrambledChanged:
                    ScrambledChanged?.Invoke(this,
                        new MediaPlayerValueChangedEventArgs<bool>(
                            e.EventArgs->media_player_scrambled_changed.new_scrambled > 0));
                    break;
                case EventType.MediaPlayerESAdded:
                    break;
                case EventType.MediaPlayerESDeleted:
                    break;
                case EventType.MediaPlayerESSelected:
                    break;
                case EventType.MediaPlayerCorked:
                    Corked?.Invoke(this, null);
                    break;
                case EventType.MediaPlayerUncorked:
                    Uncorked?.Invoke(this, null);
                    break;
                case EventType.MediaPlayerMuted:
                    Muted?.Invoke(this, null);
                    break;
                case EventType.MediaPlayerUnmuted:
                    Unmuted?.Invoke(this, null);
                    break;
                case EventType.MediaPlayerAudioVolume:
                    AudioVolumeChanged?.Invoke(this,
                        new MediaPlayerValueChangedEventArgs<float>(e.EventArgs->media_player_audio_volume.volume));
                    break;
                case EventType.MediaPlayerAudioDevice:
                    AudioDeviceChanged?.Invoke(this,
                        new MediaPlayerValueChangedEventArgs<string>(
                            InteropHelper.PtrToString(e.EventArgs->media_player_audio_device.device)));
                    break;
                case EventType.MediaPlayerChapterChanged:
                    ChapterChanged?.Invoke(this,
                        new MediaPlayerValueChangedEventArgs<int>(e.EventArgs->media_player_chapter_changed
                            .new_chapter));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public event EventHandler Forward;
        public event EventHandler Backward;
        public event EventHandler<MediaStateChangedEventArgs> MediaStateChanged;
        public event EventHandler<MediaPlayerValueChangedEventArgs<IntPtr>> MediaChanged;
        public event EventHandler<MediaPlayerBufferingEventArgs> MediaPlayerBuffing;
        public event EventHandler<MediaPlayerValueChangedEventArgs<long>> TimeChanged;
        public event EventHandler<MediaPlayerValueChangedEventArgs<float>> PositionChanged;
        public event EventHandler<MediaPlayerValueChangedEventArgs<bool>> SeekableChanged;
        public event EventHandler<MediaPlayerValueChangedEventArgs<bool>> PausableChanged;
        public event EventHandler<MediaPlayerValueChangedEventArgs<int>> TitleChanged;
        public event EventHandler<ObjectEventArgs<string>> SnapshotTaken;
        public event EventHandler<MediaPlayerValueChangedEventArgs<int>> VideoOutChanged;
        public event EventHandler<MediaPlayerValueChangedEventArgs<long>> LengthChanged;
        public event EventHandler<MediaPlayerValueChangedEventArgs<bool>> ScrambledChanged;
        public event EventHandler<MediaPlayerValueChangedEventArgs<float>> AudioVolumeChanged;
        public event EventHandler<MediaPlayerValueChangedEventArgs<string>> AudioDeviceChanged;
        public event EventHandler<MediaPlayerValueChangedEventArgs<int>> ChapterChanged;
        public event EventHandler Corked;
        public event EventHandler Uncorked;
        public event EventHandler Muted;
        public event EventHandler Unmuted;

        public void Play()
        {
            LibVlcManager.GetFunctionDelegate<libvlc_media_player_play>().Invoke(InstancePointer);
        }

        public void SetVideoDecodeCallback(libvlc_video_lock_cb lockCallback, libvlc_video_unlock_cb unlockCallback,
            libvlc_video_display_cb displayCallback, IntPtr userData)
        {
            LibVlcManager.GetFunctionDelegate<libvlc_video_set_callbacks>().Invoke(InstancePointer, lockCallback,
                unlockCallback, displayCallback, userData.ToPointer());
        }

        public void SetVideoFormatCallback(libvlc_video_format_cb formatCallback,
            libvlc_video_cleanup_cb cleanupCallback)
        {
            LibVlcManager.GetFunctionDelegate<libvlc_video_set_format_callbacks>()
                .Invoke(InstancePointer, formatCallback, cleanupCallback);
        }

        public void SetPause(bool pause)
        {
            LibVlcManager.GetFunctionDelegate<libvlc_media_player_set_pause>().Invoke(InstancePointer, pause ? 1 : 0);
        }

        public void Pause()
        {
            SetPause(true);
        }

        public void Resume()
        {
            SetPause(false);
        }

        public void PauseOrResume()
        {
            SetPause(IsPlaying);
        }

        public void Stop()
        {
            LibVlcManager.GetFunctionDelegate<libvlc_media_player_stop>().Invoke(InstancePointer);
        }

        public void SetAudioFormat(string format, uint rate, uint channels)
        {
            using (var handle = new StringHandle(format))
            {
                LibVlcManager.GetFunctionDelegate<libvlc_audio_set_format>()
                    .Invoke(InstancePointer, handle.UnsafePointer, rate, channels);
            }
        }

        public int GetTitleChapterCount(int title)
        {
            return LibVlcManager.GetFunctionDelegate<libvlc_media_player_get_chapter_count_for_title>()
                .Invoke(InstancePointer, title);
        }

        public void PreviousChapter()
        {
            LibVlcManager.GetFunctionDelegate<libvlc_media_player_previous_chapter>().Invoke(InstancePointer);
        }

        public void NextChapter()
        {
            LibVlcManager.GetFunctionDelegate<libvlc_media_player_next_chapter>().Invoke(InstancePointer);
        }

        public void NextFrame()
        {
            LibVlcManager.GetFunctionDelegate<libvlc_media_player_next_frame>().Invoke(InstancePointer);
        }

        public void SetVideoTitleDisplay(Position pos, uint timeout)
        {
            LibVlcManager.GetFunctionDelegate<libvlc_media_player_set_video_title_display>()
                .Invoke(InstancePointer, (libvlc_position_t) pos, timeout);
        }

        public void ToggleMute()
        {
            LibVlcManager.GetFunctionDelegate<libvlc_audio_toggle_mute>().Invoke(InstancePointer);
        }

        public void SetVideoFormat(string chroma, uint width, uint height, uint pitch)
        {
            using (var handle = new StringHandle(chroma))
            {
                LibVlcManager.GetFunctionDelegate<libvlc_video_set_format>()
                    .Invoke(InstancePointer, handle.UnsafePointer, width, height, pitch);
            }
        }

        public void GetMouseCursor(uint num, ref int x, ref int y)
        {
            int posX, posY;
            LibVlcManager.GetFunctionDelegate<libvlc_video_get_cursor>().Invoke(InstancePointer, num, &posX, &posY);
            x = posX;
            y = posY;
        }

        [Obsolete("Get some error for compiling VLC 3.x, some extension function disabled.")]
        public bool SetMouseCursor(uint num, int x, int y)
        {
            return false;
        }

        [Obsolete("Get some error for compiling VLC 3.x, some extension function disabled.")]
        public bool SetMouseDown(uint num, MouseButton button)
        {
            return false;
        }

        [Obsolete("Get some error for compiling VLC 3.x, some extension function disabled.")]
        public bool SetMouseUp(uint num, MouseButton button)
        {
            return false;
        }

        public void Navigate(NavigateMode mode)
        {
            LibVlcManager.GetFunctionDelegate<libvlc_media_player_navigate>()
                .Invoke(InstancePointer, (libvlc_navigate_mode_t) mode);
        }

        /// <summary>
        ///     Apply new equalizer settings to a media player.
        ///     <para />
        ///     The media player does not keep a reference to the supplied equalizer so you should set it again when you changed
        ///     some value of equalizer.
        ///     <para />
        ///     After you set equalizer you can dispose it. if you want to disable equalizer set it to <see cref="null" />.
        /// </summary>
        /// <param name="equalizer"></param>
        public bool SetEqualizer(AudioEqualizer equalizer)
        {
            return LibVlcManager.GetFunctionDelegate<libvlc_media_player_set_equalizer>()
                       .Invoke(InstancePointer, equalizer != null ? equalizer.InstancePointer : null) == 0;
        }

        /// <summary>
        ///     Gets a list of potential audio output devices.
        /// </summary>
        /// <returns></returns>
        public List<AudioDevice> EnumAudioDeviceList()
        {
            using (var list = new AudioDeviceList(LibVlcManager.GetFunctionDelegate<libvlc_audio_output_device_enum>()
                .Invoke(InstancePointer)))
            {
                return new List<AudioDevice>(list);
            }
        }

        /// <summary>
        ///     Gets a list of audio output devices for a given audio output module.
        /// </summary>
        /// <param name="audioOutput"></param>
        /// <returns></returns>
        public List<AudioDevice> GetAudioDeviceList(AudioOutput audioOutput)
        {
            using (var handle = new StringHandle(audioOutput.Name))
            {
                using (var list = new AudioDeviceList(LibVlcManager
                    .GetFunctionDelegate<libvlc_audio_output_device_list_get>()
                    .Invoke(VlcInstance.InstancePointer, handle.UnsafePointer)))
                {
                    return new List<AudioDevice>(list);
                }
            }
        }

        /// <summary>
        ///     Gets the list of available audio output modules.
        /// </summary>
        /// <returns></returns>
        public List<AudioOutput> GetAudioOutputList()
        {
            using (var list = new AudioOutputList(LibVlcManager.GetFunctionDelegate<libvlc_audio_output_list_get>()
                .Invoke(InstancePointer)))
            {
                return new List<AudioOutput>(list);
            }
        }

        /// <summary>
        ///     Selects an audio output module.
        ///     Any change will take be effect only after playback is stopped and restarted. Audio output cannot be changed while
        ///     playing.
        /// </summary>
        /// <param name="audioOutput"></param>
        /// <returns></returns>
        public bool SetAudioOutput(AudioOutput audioOutput)
        {
            using (var handle = new StringHandle(audioOutput.Name))
            {
                return LibVlcManager.GetFunctionDelegate<libvlc_audio_output_set>()
                           .Invoke(InstancePointer, handle.UnsafePointer) == 0;
            }
        }

        /// <summary>
        ///     Get the current audio output device identifier.
        /// </summary>
        public string GetAudioDevice()
        {
            return InteropHelper.PtrToString(LibVlcManager.GetFunctionDelegate<libvlc_audio_output_device_get>()
                .Invoke(InstancePointer));
        }

        /// <summary>
        ///     Configures an explicit audio output device. If the module paramater is NULL,
        ///     audio output will be moved to the device specified by the device identifier string immediately.
        ///     This is the recommended usage. A list of adequate potential device strings can be obtained with
        ///     <see cref="EnumAudioDeviceList" />.
        ///     However passing NULL is supported in LibVLC version 2.2.0 and later only; in earlier versions, this function would
        ///     have no effects when the module parameter was NULL.
        ///     If the module parameter is not NULL, the device parameter of the corresponding audio output, if it exists, will be
        ///     set to the specified string.
        ///     Note that some audio output modules do not have such a parameter (notably MMDevice and PulseAudio).
        ///     A list of adequate potential device strings can be obtained with <see cref="GetAudioDeviceList" />.
        /// </summary>
        public void SetAudioDevice(AudioOutput audioOutput, AudioDevice audioDevice)
        {
            using (var outputHandle = new StringHandle(audioOutput.Name))
            {
                using (var deviceHandle = new StringHandle(audioDevice.Device))
                {
                    LibVlcManager.GetFunctionDelegate<libvlc_audio_output_device_set>()
                        .Invoke(InstancePointer, outputHandle.UnsafePointer, deviceHandle.UnsafePointer);
                }
            }
        }

        protected void Dispose(bool disposing)
        {
            if (_disposed) return;

            VlcObjectManager.Remove(this);
            EventManager.VlcEventFired -= OnVlcEventFired;
            EventManager.Dispose();
            LibVlcManager.GetFunctionDelegate<libvlc_media_player_release>().Invoke(InstancePointer);
            InstancePointer = null;

            _disposed = true;
        }
    }
}