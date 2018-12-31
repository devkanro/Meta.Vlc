// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: Enum.cs
// Version: 20181231

using Meta.Vlc.Interop.MediaPlayer.Video;

namespace Meta.Vlc.Interop.MediaPlayer
{
    /// <summary>
    ///     Description for titles
    /// </summary>
    public enum libvlc_title_t
    {
        libvlc_title_menu = 0x01,
        libvlc_title_interactive = 0x02
    }

    /// <summary>
    ///     Marq options definition
    /// </summary>
    public enum libvlc_video_marquee_option_t
    {
        libvlc_marquee_Enable = 0,

        /// <summary>
        ///     string argument
        /// </summary>
        libvlc_marquee_Text,
        libvlc_marquee_Color,
        libvlc_marquee_Opacity,
        libvlc_marquee_Position,
        libvlc_marquee_Refresh,
        libvlc_marquee_Size,
        libvlc_marquee_Timeout,
        libvlc_marquee_X,
        libvlc_marquee_Y
    }

    /// <summary>
    ///     Navigation mode
    /// </summary>
    public enum libvlc_navigate_mode_t : uint
    {
        libvlc_navigate_activate = 0,
        libvlc_navigate_up,
        libvlc_navigate_down,
        libvlc_navigate_left,
        libvlc_navigate_right,
        libvlc_navigate_popup
    }

    /// <summary>
    ///     Enumeration of values used to set position (e.g. of video title).
    /// </summary>
    public enum libvlc_position_t
    {
        libvlc_position_disable = -1,
        libvlc_position_center,
        libvlc_position_left,
        libvlc_position_right,
        libvlc_position_top,
        libvlc_position_top_left,
        libvlc_position_top_right,
        libvlc_position_bottom,
        libvlc_position_bottom_left,
        libvlc_position_bottom_right
    }

    /// <summary>
    ///     Enumeration of teletext keys than can be passed via <see cref="libvlc_video_set_teletext" />
    /// </summary>
    public enum libvlc_teletext_key_t
    {
        libvlc_teletext_key_red = 'r' << 16,
        libvlc_teletext_key_green = 'g' << 16,
        libvlc_teletext_key_yellow = 'y' << 16,
        libvlc_teletext_key_blue = 'b' << 16,
        libvlc_teletext_key_index = 'i' << 16
    }

    namespace Video
    {
        /// <summary>
        ///     option values for libvlc_video_{get,set}_logo_{int,string}
        /// </summary>
        public enum libvlc_video_logo_option_t : uint
        {
            libvlc_logo_enable,

            /// <summary>
            ///     string argument, "file,d,t;file,d,t;..."
            /// </summary>
            libvlc_logo_file,
            libvlc_logo_x,
            libvlc_logo_y,
            libvlc_logo_delay,
            libvlc_logo_repeat,
            libvlc_logo_opacity,
            libvlc_logo_position
        }

        /// <summary>
        ///     option values for libvlc_video_{get,set}_adjust_{int,float,bool}
        /// </summary>
        public enum libvlc_video_adjust_option_t : uint
        {
            libvlc_adjust_Enable = 0,
            libvlc_adjust_Contrast,
            libvlc_adjust_Brightness,
            libvlc_adjust_Hue,
            libvlc_adjust_Saturation,
            libvlc_adjust_Gamma
        }
    }

    namespace Audio
    {
        /// <summary>
        ///     Audio device types
        /// </summary>
        public enum libvlc_audio_output_device_types_t
        {
            libvlc_AudioOutputDevice_Error = -1,
            libvlc_AudioOutputDevice_Mono = 1,
            libvlc_AudioOutputDevice_Stereo = 2,
            libvlc_AudioOutputDevice_2F2R = 4,
            libvlc_AudioOutputDevice_3F2R = 5,
            libvlc_AudioOutputDevice_5_1 = 6,
            libvlc_AudioOutputDevice_6_1 = 7,
            libvlc_AudioOutputDevice_7_1 = 8,
            libvlc_AudioOutputDevice_SPDIF = 10
        }

        /// <summary>
        ///     Audio channels
        /// </summary>
        public enum libvlc_audio_output_channel_t
        {
            libvlc_AudioChannel_Error = -1,
            libvlc_AudioChannel_Stereo = 1,
            libvlc_AudioChannel_RStereo = 2,
            libvlc_AudioChannel_Left = 3,
            libvlc_AudioChannel_Right = 4,
            libvlc_AudioChannel_Dolbys = 5
        }

        /// <summary>
        ///     Media player roles.
        /// </summary>
        /// <seealso cref="libvlc_media_player_set_role" />
        public enum libvlc_media_player_role_t
        {
            /// <summary>
            ///     Don't use a media player role
            /// </summary>
            libvlc_role_None = 0,

            /// <summary>
            ///     Music (or radio) playback
            /// </summary>
            libvlc_role_Music,

            /// <summary>
            ///     Video playback
            /// </summary>
            libvlc_role_Video,

            /// <summary>
            ///     Speech, real-time communication
            /// </summary>
            libvlc_role_Communication,

            /// <summary>
            ///     Video game
            /// </summary>
            libvlc_role_Game,

            /// <summary>
            ///     User interaction feedback
            /// </summary>
            libvlc_role_Notification,

            /// <summary>
            ///     Embedded animation (e.g. in web page)
            /// </summary>
            libvlc_role_Animation,

            /// <summary>
            ///     Audio editting/production
            /// </summary>
            libvlc_role_Production,

            /// <summary>
            ///     Accessibility
            /// </summary>
            libvlc_role_Accessibility,

            /// <summary>
            ///     Testing
            /// </summary>
            libvlc_role_Test
        }
    }
}