// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: Struct.cs
// Version: 20181231

using System.Runtime.InteropServices;

namespace Meta.Vlc.Interop.Media
{
    /// <summary>
    ///     A slave of a libvlc_media_t
    /// </summary>
    /// <seealso cref="Media.libvlc_media_slaves_get" />
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct libvlc_media_slave_t
    {
        public byte* psz_uri;
        public libvlc_media_slave_type_t i_type;
        public uint i_priority;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct libvlc_media_track_t
    {
        /* Codec fourcc */
        public uint i_codec;

        public uint i_original_fourcc;
        
        public int i_id;
        
        public libvlc_track_type_t i_type;

        /* Codec specific */
        public int i_profile;
        
        public int i_level;
        
        public void* data;
        
        public uint i_bitrate;
        
        public byte* psz_language;
        
        public byte* psz_description;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public unsafe struct libvlc_subtitle_track_t
    {
        public byte* psz_encoding;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct libvlc_video_track_t
    {
        public uint i_height;
        public uint i_width;
        public uint i_sar_num;
        public uint i_sar_den;
        public uint i_frame_rate_num;
        public uint i_frame_rate_den;

        public libvlc_video_orient_t i_orientation;
        public libvlc_video_projection_t i_projection;
        public libvlc_video_viewpoint_t pose; /**< Initial view point */
        public libvlc_video_multiview_t i_multiview;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct libvlc_video_viewpoint_t
    {
        /// <summary>
        ///     view point yaw in degrees  ]-180;180]
        /// </summary>
        public float f_yaw;

        /// <summary>
        ///     view point pitch in degrees  ]-90;90]
        /// </summary>
        public float f_pitch;

        /// <summary>
        ///     view point roll in degrees ]-180;180]
        /// </summary>
        public float f_roll;

        /// <summary>
        ///     field of view in degrees ]0;180[ (default 80.)
        /// </summary>
        public float f_field_of_view;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct libvlc_audio_track_t
    {
        public uint i_channels;
        public uint i_rate;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct libvlc_media_track_info_t_audio
    {
        public uint i_channels;
        public uint i_rate;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct libvlc_media_track_info_t_video
    {
        public uint i_height;
        public uint i_width;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct libvlc_media_track_info_t
    {
        /* Codec fourcc */
        [FieldOffset(0)] public uint i_codec;
        [FieldOffset(Platform.UIntSize)] public int i_id;

        [FieldOffset(Platform.UIntSize + Platform.IntSize)]
        public libvlc_track_type_t i_type;

        /* Codec specific */
        [FieldOffset(Platform.UIntSize + Platform.IntSize + Platform.IntSize)]
        public int i_profile;

        [FieldOffset(Platform.UIntSize + Platform.IntSize + Platform.IntSize + Platform.IntSize)]
        public int i_level;

        /* Audio specific */
        [FieldOffset(Platform.UIntSize + Platform.IntSize + Platform.IntSize + Platform.IntSize + Platform.IntSize)]
        public libvlc_media_track_info_t_audio audio;

        /* Video specific */
        [FieldOffset(Platform.UIntSize + Platform.IntSize + Platform.IntSize + Platform.IntSize + Platform.IntSize)]
        public libvlc_media_track_info_t_video video;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct libvlc_media_stats_t
    {
        /* Input */
        public int i_read_bytes;
        public float f_input_bitrate;

        /* Demux */
        public int i_demux_read_bytes;
        public float f_demux_bitrate;
        public int i_demux_corrupted;
        public int i_demux_discontinuity;

        /* Decoders */
        public int i_decoded_video;
        public int i_decoded_audio;

        /* Video Output */
        public int i_displayed_pictures;
        public int i_lost_pictures;

        /* Audio output */
        public int i_played_abuffers;
        public int i_lost_abuffers;

        /* Stream output */
        public int i_sent_packets;
        public int i_sent_bytes;
        public float f_send_bitrate;
    }
}