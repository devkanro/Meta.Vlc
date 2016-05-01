// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: MediaTrack.cs
// Version: 20160214

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Meta.Vlc.Interop.Media;

namespace Meta.Vlc
{
    /// <summary>
    ///     A warpper for <see cref="Interop.Media.MediaTrack" /> struct.
    /// </summary>
    public abstract class MediaTrack
    {
        protected MediaTrack(Interop.Media.MediaTrack mediaTrack)
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
        public String Language { get; private set; }
        public String Description { get; private set; }
        protected IntPtr Track { get; private set; }

        /// <summary>
        ///     Create a media track from a pointer, it will distinguish type of media track auto.
        /// </summary>
        /// <param name="pointer">pointer of media track</param>
        /// <returns>a audio track, video track, subtitle track or unknow track</returns>
        public static MediaTrack CreateFromPointer(IntPtr pointer)
        {
            var track = (Interop.Media.MediaTrack) Marshal.PtrToStructure(pointer, typeof (Interop.Media.MediaTrack));

            switch (track.Type)
            {
                case TrackType.Audio:
                    return new AudioTrack(track);

                case TrackType.Video:
                    return new VideoTrack(track);

                case TrackType.Text:
                    return new SubtitleTrack(track);

                case TrackType.Unkown:
                default:
                    return new UnkownTrack(track);
            }
        }

        protected virtual void Initialize(Interop.Media.MediaTrack mediaTrack)
        {
            Codec = mediaTrack.Codec;
            OriginalFourcc = mediaTrack.OriginalFourcc;
            Id = mediaTrack.Id;
            Type = mediaTrack.Type;
            Profile = mediaTrack.Profile;
            Level = mediaTrack.Level;
            Bitrate = mediaTrack.Bitrate;
            Language = InteropHelper.PtrToString(mediaTrack.Language);
            Description = InteropHelper.PtrToString(mediaTrack.Description);
            Track = mediaTrack.Track;
        }
    }

    /// <summary>
    ///     A warpper for <see cref="Interop.Media.AudioTrack" /> struct.
    /// </summary>
    public class AudioTrack : MediaTrack
    {
        internal AudioTrack(Interop.Media.MediaTrack mediaTrack) : base(mediaTrack)
        {
        }

        public uint Channels { get; private set; }

        public uint Rate { get; private set; }

        protected override void Initialize(Interop.Media.MediaTrack mediaTrack)
        {
            base.Initialize(mediaTrack);

            var audioTrack = (Interop.Media.AudioTrack) Marshal.PtrToStructure(Track, typeof (Interop.Media.AudioTrack));

            Channels = audioTrack.Channels;
            Rate = audioTrack.Rate;
        }
    }

    /// <summary>
    ///     A warpper for <see cref="Interop.Media.VideoTrack" /> struct.
    /// </summary>
    public class VideoTrack : MediaTrack
    {
        internal VideoTrack(Interop.Media.MediaTrack mediaTrack) : base(mediaTrack)
        {
        }

        public uint Height { get; private set; }
        public uint Width { get; private set; }
        public uint SarNum { get; private set; }
        public uint SarDen { get; private set; }
        public uint FrameRateNum { get; private set; }
        public uint FrameRateDen { get; private set; }

        protected override void Initialize(Interop.Media.MediaTrack mediaTrack)
        {
            base.Initialize(mediaTrack);

            var videoTrack = (Interop.Media.VideoTrack) Marshal.PtrToStructure(Track, typeof (Interop.Media.VideoTrack));

            Height = videoTrack.Height;
            Width = videoTrack.Width;
            SarNum = videoTrack.SarNum;
            SarDen = videoTrack.SarDen;
            FrameRateNum = videoTrack.FrameRateNum;
            FrameRateDen = videoTrack.FrameRateDen;
        }
    }

    /// <summary>
    ///     A warpper for <see cref="Interop.Media.SubtitleTrack" /> struct.
    /// </summary>
    public class SubtitleTrack : MediaTrack
    {
        internal SubtitleTrack(Interop.Media.MediaTrack mediaTrack) : base(mediaTrack)
        {
        }

        public String Encoding { get; private set; }

        protected override void Initialize(Interop.Media.MediaTrack mediaTrack)
        {
            base.Initialize(mediaTrack);

            var subtitleTrack =
                (Interop.Media.SubtitleTrack) Marshal.PtrToStructure(Track, typeof (Interop.Media.SubtitleTrack));

            Encoding = InteropHelper.PtrToString(subtitleTrack.Encoding);
        }
    }

    /// <summary>
    ///     A warpper for orther media track.
    /// </summary>
    public class UnkownTrack : MediaTrack
    {
        internal UnkownTrack(Interop.Media.MediaTrack mediaTrack) : base(mediaTrack)
        {
        }

        public IntPtr TrackPointer
        {
            get { return Track; }
        }
    }

    /// <summary>
    ///     A list warpper for <see cref="Interop.Media.MediaTrack" /> struct.
    /// </summary>
    public class MediaTrackList : IEnumerable<MediaTrack>, IEnumerable
    {
        private List<MediaTrack> _list;

        /// <summary>
        ///     Create a list of media track from a pointer of array.
        /// </summary>
        /// <param name="pointer">pointer of media track array</param>
        /// <param name="count">count of media track array</param>
        public MediaTrackList(IntPtr pointer, uint count)
        {
            _list = new List<MediaTrack>();

            if (pointer == IntPtr.Zero) return;

            var tmp = pointer;

            for (var i = 0; i < count; i++)
            {
                var p = Marshal.ReadIntPtr(tmp);
                _list.Add(MediaTrack.CreateFromPointer(p));
                tmp = (IntPtr) ((Int64) tmp + IntPtr.Size);
            }

            LibVlcManager.ReleaseTracks(pointer, count);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public MediaTrack this[int index]
        {
            get { return _list[index]; }
        }

        public IEnumerator<MediaTrack> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}