// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: Struct.cs
// Version: 20181231

using System.Runtime.InteropServices;

namespace Meta.Vlc.Interop.MediaPlayer
{
    /// <summary>
    ///     Description for video, audio tracks and subtitles. It contains
    ///     id, name (description string) and pointer to next record.
    /// </summary>
    /// [StructLayout(LayoutKind.Sequential)]
    public unsafe struct libvlc_track_description_t
    {
        public int i_id;
        public byte* psz_name;
        public libvlc_track_description_t* p_next;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct libvlc_title_description_t
    {
        /// <summary>
        ///     duration in milliseconds
        /// </summary>
        public long i_duration;

        /// <summary>
        ///     title name
        /// </summary>
        public byte* psz_name;

        /// <summary>
        ///     info if item was recognized as a menu, interactive or plain content by the demuxer
        /// </summary>
        public uint i_flags;
    }

    /**
     * Description for chapters
     */
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct libvlc_chapter_description_t
    {
        /// <summary>
        ///     time-offset of the chapter in milliseconds
        /// </summary>
        public long i_time_offset;

        /// <summary>
        ///     duration of the chapter in milliseconds
        /// </summary>
        public long i_duration;

        /// <summary>
        ///     chapter name
        /// </summary>
        public byte* psz_name;
    }

    /// <summary>
    ///     Description for audio output. It contains
    ///     name, description and pointer to next record.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct libvlc_audio_output_t
    {
        public byte* psz_name;
        public byte* psz_description;
        public libvlc_audio_output_t* p_next;
    }

    /// <summary>
    ///     Description for audio output device.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct libvlc_audio_output_device_t
    {
        /// <summary>
        ///     Next entry in list
        /// </summary>
        public libvlc_audio_output_device_t* p_next;

        /// <summary>
        ///     Device identifier string
        /// </summary>
        public byte* psz_device;

        /// <summary>
        ///     User-friendly device description
        /// </summary>
        public byte* psz_description;

        /* More fields may be added here in later versions */
    }
}