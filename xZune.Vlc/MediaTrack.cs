using System;
using System.Runtime.InteropServices;
using xZune.Vlc.Interop.Media;

namespace xZune.Vlc
{
    public class MediaTrack
    {
        public MediaTrack(IntPtr pointer)
        {
            var track = (Interop.Media.MediaTrack)Marshal.PtrToStructure(pointer, typeof(Interop.Media.MediaTrack));
            Codec = track.Codec;
            OriginalFourcc = track.OriginalFourcc;
            Id = track.Id;
            Type = track.Type;
            Profile = track.Profile;
            Level = track.Level;
            Bitrate = track.Bitrate;
            Language = track.Language;
            Description = track.Description;
            switch (Type)
            {
                case TrackType.Audio:
                    AudioTrack = (AudioTrack)Marshal.PtrToStructure(track.Track, typeof(AudioTrack));
                    break;
                case TrackType.Video:
                    VideoTrack = (VideoTrack)Marshal.PtrToStructure(track.Track, typeof(VideoTrack));
                    break;
                case TrackType.Text:
                    SubtitleTrack = (SubtitleTrack)Marshal.PtrToStructure(track.Track, typeof(SubtitleTrack));
                    break;

            }
        }
        public uint Codec { get; private set; }
        public uint OriginalFourcc { get; private set; }
        public int Id { get; private set; }
        public TrackType Type { get; private set; }
        public int Profile { get; private set; }
        public int Level { get; private set; }
        public VideoTrack? VideoTrack { get; private set; }
        public AudioTrack? AudioTrack { get; private set; }
        public SubtitleTrack? SubtitleTrack { get; private set; }
        public uint Bitrate { get; private set; }
        public String Language { get; private set; }
        public String Description { get; private set; }
    }
}
