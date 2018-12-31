// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: MediaStats.cs
// Version: 20181231

using Meta.Vlc.Interop.Media;

namespace Meta.Vlc
{
    public class MediaStats
    {
        internal libvlc_media_stats_t Struct;

        internal MediaStats()
        {
        }

        public int ReadBytes => Struct.i_read_bytes;
        public float InputBitRate => Struct.i_read_bytes;

        public int DemuxReadBytes => Struct.i_demux_read_bytes;
        public float DemuxBitRate => Struct.f_demux_bitrate;
        public int DemuxCorrupted => Struct.i_demux_corrupted;
        public int DemuxDiscontinuity => Struct.i_demux_discontinuity;

        public int DecodedVideo => Struct.i_decoded_video;
        public int DecodedAudio => Struct.i_decoded_audio;

        public int DisplayedPictures => Struct.i_displayed_pictures;
        public int LostPictures => Struct.i_lost_pictures;

        public int PlayedAudioBuffers => Struct.i_played_abuffers;
        public int LostAudioBuffers => Struct.i_lost_abuffers;

        public int SentPackets => Struct.i_sent_packets;
        public int SentBytes => Struct.i_sent_bytes;
        public float SendBitRate => Struct.f_send_bitrate;
    }
}