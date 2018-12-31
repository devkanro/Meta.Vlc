// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: MediaTrackInfo.cs
// Version: 20181231

using Meta.Vlc.Interop.Core;
using Meta.Vlc.Interop.Media;

namespace Meta.Vlc
{
    /// <summary>
    ///     A wrapper for <see cref="libvlc_media_track_info_t" /> struct.
    /// </summary>
    public abstract unsafe class MediaTrackInfo
    {
        protected MediaTrackInfo(libvlc_media_track_info_t* mediaTrack)
        {
            Initialize(mediaTrack);
        }

        public uint Codec { get; private set; }
        public int Id { get; private set; }
        public TrackType Type { get; private set; }
        public int Profile { get; private set; }
        public int Level { get; private set; }

        /// <summary>
        ///     Create a media track from a pointer, it will distinguish type of media track auto.
        /// </summary>
        /// <param name="pointer">pointer of media track</param>
        /// <returns>a audio track, video track, subtitle track or unknown track</returns>
        public static MediaTrackInfo CreateFromPointer(libvlc_media_track_info_t* pointer)
        {
            switch ((TrackType) pointer->i_type)
            {
                case TrackType.Audio:
                    return new AudioTrackInfo(pointer);

                case TrackType.Video:
                    return new VideoTrackInfo(pointer);

                default:
                    return new UnknownTrackInfo(pointer);
            }
        }

        protected virtual void Initialize(libvlc_media_track_info_t* mediaTrack)
        {
            Codec = mediaTrack->i_codec;
            Id = mediaTrack->i_id;
            Type = (TrackType) mediaTrack->i_type;
            Profile = mediaTrack->i_profile;
            Level = mediaTrack->i_level;
        }
    }

    /// <summary>
    ///     A wrapper for <see cref="libvlc_media_track_info_t_audio" /> struct.
    /// </summary>
    public unsafe class AudioTrackInfo : MediaTrackInfo
    {
        internal AudioTrackInfo(libvlc_media_track_info_t* mediaTrack) : base(mediaTrack)
        {
        }

        public uint Channels { get; private set; }

        public uint Rate { get; private set; }

        protected override void Initialize(libvlc_media_track_info_t* mediaTrack)
        {
            base.Initialize(mediaTrack);
            Channels = mediaTrack->audio.i_channels;
            Rate = mediaTrack->audio.i_rate;
        }
    }

    /// <summary>
    ///     A wrapper for <see cref="libvlc_media_track_info_t_video" /> struct.
    /// </summary>
    public unsafe class VideoTrackInfo : MediaTrackInfo
    {
        internal VideoTrackInfo(libvlc_media_track_info_t* mediaTrack) : base(mediaTrack)
        {
        }

        public uint Height { get; private set; }
        public uint Width { get; private set; }

        protected override void Initialize(libvlc_media_track_info_t* mediaTrack)
        {
            base.Initialize(mediaTrack);

            Height = mediaTrack->video.i_height;
            Width = mediaTrack->video.i_width;
        }
    }

    /// <summary>
    ///     A wrapper for other media track info.
    /// </summary>
    public unsafe class UnknownTrackInfo : MediaTrackInfo
    {
        internal UnknownTrackInfo(libvlc_media_track_info_t* mediaTrack) : base(mediaTrack)
        {
        }
    }

    /// <summary>
    ///     A list wrapper for <see cref="libvlc_media_track_info_t" /> struct.
    /// </summary>
    public unsafe class MediaTrackInfoList : VlcUnmanagedList<MediaTrackInfo>
    {
        public MediaTrackInfoList(void** pointer, uint count) : base(pointer, count)
        {
        }

        protected override MediaTrackInfo CreateItem(void* data)
        {
            return MediaTrackInfo.CreateFromPointer((libvlc_media_track_info_t*) data);
        }

        protected override void Release(void** data)
        {
            LibVlcManager.GetFunctionDelegate<libvlc_free>().Invoke(data);
        }
    }
}