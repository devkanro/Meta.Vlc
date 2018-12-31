// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: MediaTrack.cs
// Version: 20181231

using Meta.Vlc.Interop.Media;

namespace Meta.Vlc
{
    public enum TrackType
    {
        Unknown = libvlc_track_type_t.libvlc_track_unknown,
        Audio = libvlc_track_type_t.libvlc_track_audio,
        Video = libvlc_track_type_t.libvlc_track_video,
        Text = libvlc_track_type_t.libvlc_track_text
    }

    /// <summary>
    ///     A wrapper for <see cref="libvlc_media_track_t" /> struct.
    /// </summary>
    public abstract unsafe class MediaTrack
    {
        protected MediaTrack(libvlc_media_track_t* mediaTrack)
        {
            Initialize(mediaTrack);
        }

        public uint Codec { get; private set; }
        public uint OriginalFourcc { get; private set; }
        public int Id { get; private set; }
        public TrackType Type { get; private set; }
        public int Profile { get; private set; }
        public int Level { get; private set; }
        public uint Bitrate { get; private set; }
        public string Language { get; private set; }
        public string Description { get; private set; }

        /// <summary>
        ///     Create a media track from a pointer, it will distinguish type of media track auto.
        /// </summary>
        /// <param name="pointer">pointer of media track</param>
        /// <returns>a audio track, video track, subtitle track or unknown track</returns>
        public static MediaTrack CreateFromPointer(libvlc_media_track_t* pointer)
        {
            switch ((TrackType) pointer->i_type)
            {
                case TrackType.Audio:
                    return new AudioTrack(pointer);

                case TrackType.Video:
                    return new VideoTrack(pointer);

                case TrackType.Text:
                    return new SubtitleTrack(pointer);

                case TrackType.Unknown:
                default:
                    return new UnknownTrack(pointer);
            }
        }

        protected virtual void Initialize(libvlc_media_track_t* mediaTrack)
        {
            Codec = mediaTrack->i_codec;
            OriginalFourcc = mediaTrack->i_original_fourcc;
            Id = mediaTrack->i_id;
            Type = (TrackType) mediaTrack->i_type;
            Profile = mediaTrack->i_profile;
            Level = mediaTrack->i_level;
            Bitrate = mediaTrack->i_bitrate;
            Language = InteropHelper.PtrToString(mediaTrack->psz_language);
            Description = InteropHelper.PtrToString(mediaTrack->psz_description);
        }
    }

    /// <summary>
    ///     A wrapper for <see cref="libvlc_audio_track_t" /> struct.
    /// </summary>
    public unsafe class AudioTrack : MediaTrack
    {
        internal AudioTrack(libvlc_media_track_t* mediaTrack) : base(mediaTrack)
        {
        }

        public uint Channels { get; private set; }

        public uint Rate { get; private set; }

        protected override void Initialize(libvlc_media_track_t* mediaTrack)
        {
            base.Initialize(mediaTrack);
            var audioData = (libvlc_audio_track_t*) mediaTrack->data;
            Channels = audioData->i_channels;
            Rate = audioData->i_rate;
        }
    }

    /// <summary>
    ///     A wrapper for <see cref="libvlc_video_track_t" /> struct.
    /// </summary>
    public unsafe class VideoTrack : MediaTrack
    {
        internal VideoTrack(libvlc_media_track_t* mediaTrack) : base(mediaTrack)
        {
        }

        public uint Height { get; private set; }
        public uint Width { get; private set; }
        public uint SarNum { get; private set; }
        public uint SarDen { get; private set; }
        public uint FrameRateNum { get; private set; }
        public uint FrameRateDen { get; private set; }

        protected override void Initialize(libvlc_media_track_t* mediaTrack)
        {
            base.Initialize(mediaTrack);

            var videoData = (libvlc_video_track_t*)mediaTrack->data;
            Height = videoData->i_height;
            Width = videoData->i_width;
            SarNum = videoData->i_sar_num;
            SarDen = videoData->i_sar_den;
            FrameRateNum = videoData->i_frame_rate_num;
            FrameRateDen = videoData->i_frame_rate_den;
        }
    }

    /// <summary>
    ///     A wrapper for <see cref="libvlc_subtitle_track_t" /> struct.
    /// </summary>
    public unsafe class SubtitleTrack : MediaTrack
    {
        internal SubtitleTrack(libvlc_media_track_t* mediaTrack) : base(mediaTrack)
        {
        }

        public string Encoding { get; private set; }

        protected override void Initialize(libvlc_media_track_t* mediaTrack)
        {
            base.Initialize(mediaTrack);

            var subtitleData = (libvlc_subtitle_track_t*)mediaTrack->data;
            Encoding = InteropHelper.PtrToString(subtitleData->psz_encoding);
        }
    }

    /// <summary>
    ///     A wrapper for other media track.
    /// </summary>
    public unsafe class UnknownTrack : MediaTrack
    {
        internal UnknownTrack(libvlc_media_track_t* mediaTrack) : base(mediaTrack)
        {
        }
    }

    /// <summary>
    ///     A list wrapper for <see cref="libvlc_media_track_t" /> struct.
    /// </summary>
    public unsafe class MediaTrackList : VlcUnmanagedList<MediaTrack>
    {
        public MediaTrackList(void** pointer, uint count) : base(pointer, count)
        {
        }

        protected override MediaTrack CreateItem(void* data)
        {
            return MediaTrack.CreateFromPointer((libvlc_media_track_t*) data);
        }

        protected override void Release(void** data)
        {
            LibVlcManager.GetFunctionDelegate<libvlc_media_tracks_release>()
                .Invoke((libvlc_media_track_t**) data, (uint) Count);
        }
    }
}