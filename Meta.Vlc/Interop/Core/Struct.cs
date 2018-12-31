// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: Struct.cs
// Version: 20181231

using System.Runtime.InteropServices;
using Meta.Vlc.Interop.Media;
using MediaPtr = System.IntPtr;
using VlcTime = System.Int64;

namespace Meta.Vlc.Interop.Core
{
    namespace Event
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct media_meta_changed
        {
            public libvlc_meta_t meta_type;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct media_subitem_added
        {
            public MediaPtr new_child;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct media_duration_changed
        {
            public long new_duration;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct media_parsed_changed
        {
            public libvlc_media_parsed_status_t new_status;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct media_freed
        {
            public MediaPtr md;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct media_state_changed
        {
            public libvlc_state_t new_state;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct media_subitemtree_added
        {
            public MediaPtr md;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct media_player_buffering
        {
            public float new_cache;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct media_player_chapter_changed
        {
            public int new_chapter;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct media_player_position_changed
        {
            public float new_position;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct media_player_time_changed
        {
            public long new_time;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct media_player_title_changed
        {
            public int new_title;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct media_player_seekable_changed
        {
            public int new_seekable;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct media_player_pausable_changed
        {
            public int new_pausable;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct media_player_scrambled_changed
        {
            public int new_scrambled;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct media_player_vout
        {
            public int new_count;
        }

        [StructLayout(LayoutKind.Sequential)]
        /* media list */
        public struct media_list_item_added
        {
            public MediaPtr item;
            public int index;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct media_list_will_add_item
        {
            public MediaPtr item;
            public int index;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct media_list_item_deleted
        {
            public MediaPtr item;
            public int index;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct media_list_will_delete_item
        {
            public MediaPtr item;
            public int index;
        }

        /* media list player */
        [StructLayout(LayoutKind.Sequential)]
        public struct media_list_player_next_item_set
        {
            public MediaPtr item;
        }

        /* snapshot taken */
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct media_player_snapshot_taken
        {
            public byte* psz_filename;
        }

        /* Length changed */
        [StructLayout(LayoutKind.Sequential)]
        public struct media_player_length_changed
        {
            public long new_length;
        }

        /* VLM media */
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct vlm_media_event
        {
            public byte* psz_media_name;
            public byte* psz_instance_name;
        }

        /* Extra MediaPlayer */
        [StructLayout(LayoutKind.Sequential)]
        public struct media_player_media_changed
        {
            public MediaPtr new_media;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct media_player_es_changed
        {
            public libvlc_track_type_t i_type;
            public int i_id;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct media_player_audio_volume
        {
            public float volume;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct media_player_audio_device
        {
            public byte* device;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct renderer_discoverer_item_added
        {
            public void* item;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct renderer_discoverer_item_deleted
        {
            public void* item;
        }

        [StructLayout(LayoutKind.Explicit)]
        public unsafe struct libvlc_event_t
        {
            /// <summary>
            ///     Event type
            /// </summary>
            /// <seealso cref="libvlc_event_e" />
            [FieldOffset(0)] public int type;

            /// <summary>
            ///     Object emitting the event
            /// </summary>
            [FieldOffset(Platform.IntSize)] public void* p_obj;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_meta_changed media_meta_changed;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_subitem_added media_subitem_added;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_duration_changed media_duration_changed;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_parsed_changed media_parsed_changed;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_freed media_freed;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_state_changed media_state_changed;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_subitemtree_added media_subitemtree_added;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_player_buffering media_player_buffering;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_player_chapter_changed media_player_chapter_changed;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_player_position_changed media_player_position_changed;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_player_time_changed media_player_time_changed;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_player_title_changed media_player_title_changed;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_player_seekable_changed media_player_seekable_changed;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_player_pausable_changed media_player_pausable_changed;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_player_scrambled_changed media_player_scrambled_changed;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_player_vout media_player_vout;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_list_item_added media_list_item_added;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_list_will_add_item media_list_will_add_item;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_list_item_deleted media_list_item_deleted;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_list_will_delete_item media_list_will_delete_item;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_list_player_next_item_set media_list_player_next_item_set;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_player_snapshot_taken media_player_snapshot_taken;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_player_length_changed media_player_length_changed;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public vlm_media_event vlm_media_event;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_player_media_changed media_player_media_changed;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_player_es_changed media_player_es_changed;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_player_audio_volume media_player_audio_volume;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public media_player_audio_device media_player_audio_device;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public renderer_discoverer_item_added renderer_discoverer_item_added;

            [FieldOffset(Platform.IntSize + Platform.IntPtrSize)]
            public renderer_discoverer_item_deleted renderer_discoverer_item_deleted;
        }
    }

    /// <summary>
    ///     Description of a module.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct libvlc_module_description_t
    {
        public byte* psz_name;
        public byte* psz_shortname;
        public byte* psz_longname;
        public byte* psz_help;
        public libvlc_module_description_t* p_next;
    }
}