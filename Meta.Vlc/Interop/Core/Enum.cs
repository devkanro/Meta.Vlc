// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: Enum.cs
// Version: 20181231

using Meta.Vlc.Interop.Media;

namespace Meta.Vlc.Interop.Core
{
    namespace Event
    {
        public enum libvlc_event_e
        {
            /* Append new event types at the end of a category.
             * Do not remove, insert or re-order any entry.
             * Keep this in sync with lib/event.c:libvlc_event_type_name(). */

            /// <summary>
            ///     Metadata of a \link #libvlc_media_t media item\endlink changed
            /// </summary>
            libvlc_MediaMetaChanged = 0,

            /// <summary>
            ///     Subitem was added to a \link #libvlc_media_t media item\endlink
            /// </summary>
            /// <seealso cref="libvlc_media_subitems" />
            libvlc_MediaSubItemAdded,

            /// <summary>
            ///     Duration of a \link #libvlc_media_t media item\endlink changed
            /// </summary>
            /// <seealso cref="libvlc_media_get_duration" />
            libvlc_MediaDurationChanged,

            /// <summary>
            ///     Parsing state of a \link #libvlc_media_t media item\endlink changed
            /// </summary>
            /// <seealso cref="libvlc_media_parse_with_options" />
            /// <seealso cref="libvlc_media_get_parsed_status" />
            /// <seealso cref="libvlc_media_parse_stop" />
            libvlc_MediaParsedChanged,

            /// <summary>
            ///     A \link #libvlc_media_t media item\endlink was freed
            /// </summary>
            libvlc_MediaFreed,

            /// <summary>
            ///     \link #libvlc_state_t State\endlink of the \link #libvlc_media_t media item\endlink changed
            /// </summary>
            /// <seealso cref="libvlc_media_get_state" />
            libvlc_MediaStateChanged,

            /// <summary>
            ///     Subitem tree was added to a \link #libvlc_media_t media item\endlink
            /// </summary>
            libvlc_MediaSubItemTreeAdded,

            libvlc_MediaPlayerMediaChanged = 0x100,
            libvlc_MediaPlayerNothingSpecial,
            libvlc_MediaPlayerOpening,
            libvlc_MediaPlayerBuffering,
            libvlc_MediaPlayerPlaying,
            libvlc_MediaPlayerPaused,
            libvlc_MediaPlayerStopped,
            libvlc_MediaPlayerForward,
            libvlc_MediaPlayerBackward,
            libvlc_MediaPlayerEndReached,
            libvlc_MediaPlayerEncounteredError,
            libvlc_MediaPlayerTimeChanged,
            libvlc_MediaPlayerPositionChanged,
            libvlc_MediaPlayerSeekableChanged,
            libvlc_MediaPlayerPausableChanged,
            libvlc_MediaPlayerTitleChanged,
            libvlc_MediaPlayerSnapshotTaken,
            libvlc_MediaPlayerLengthChanged,
            libvlc_MediaPlayerVout,
            libvlc_MediaPlayerScrambledChanged,
            libvlc_MediaPlayerESAdded,
            libvlc_MediaPlayerESDeleted,
            libvlc_MediaPlayerESSelected,
            libvlc_MediaPlayerCorked,
            libvlc_MediaPlayerUncorked,
            libvlc_MediaPlayerMuted,
            libvlc_MediaPlayerUnmuted,
            libvlc_MediaPlayerAudioVolume,
            libvlc_MediaPlayerAudioDevice,
            libvlc_MediaPlayerChapterChanged,

            /**
             * A \link #libvlc_media_t media item\endlink was added to a
             * \link #libvlc_media_list_t media list\endlink.
             */
            libvlc_MediaListItemAdded = 0x200,

            /**
             * A \link #libvlc_media_t media item\endlink is about to get
             * added to a \link #libvlc_media_list_t media list\endlink.
             */
            libvlc_MediaListWillAddItem,

            /**
             * A \link #libvlc_media_t media item\endlink was deleted from
             * a \link #libvlc_media_list_t media list\endlink.
             */
            libvlc_MediaListItemDeleted,

            /**
             * A \link #libvlc_media_t media item\endlink is about to get
             * deleted from a \link #libvlc_media_list_t media list\endlink.
             */
            libvlc_MediaListWillDeleteItem,

            /**
             * A \link #libvlc_media_list_t media list\endlink has reached the
             * end.
             * All \link #libvlc_media_t items\endlink were either added (in
             * case of a \ref libvlc_media_discoverer_t) or parsed (preparser).
             */
            libvlc_MediaListEndReached,

            /**
             * \deprecated No longer used.
             * This belonged to the removed libvlc_media_list_view_t
             */
            libvlc_MediaListViewItemAdded = 0x300,

            /**
             * \deprecated No longer used.
             * This belonged to the removed libvlc_media_list_view_t
             */
            libvlc_MediaListViewWillAddItem,

            /**
             * \deprecated No longer used.
             * This belonged to the removed libvlc_media_list_view_t
             */
            libvlc_MediaListViewItemDeleted,

            /**
             * \deprecated No longer used.
             * This belonged to the removed libvlc_media_list_view_t
             */
            libvlc_MediaListViewWillDeleteItem,

            /**
             * Playback of a \link #libvlc_media_list_player_t media list
             * player\endlink has started.
             */
            libvlc_MediaListPlayerPlayed = 0x400,

            /**
             * The current \link #libvlc_media_t item\endlink of a
             * \link #libvlc_media_list_player_t media list player\endlink
             * has changed to a different item.
             */
            libvlc_MediaListPlayerNextItemSet,

            /**
             * Playback of a \link #libvlc_media_list_player_t media list
             * player\endlink has stopped.
             */
            libvlc_MediaListPlayerStopped,

            /**
             * A new \link #libvlc_renderer_item_t renderer item\endlink was found by a
             * \link #libvlc_renderer_discoverer_t renderer discoverer\endlink.
             * The renderer item is valid until deleted.
             */
            libvlc_RendererDiscovererItemAdded = 0x502,

            /**
             * A previously discovered \link #libvlc_renderer_item_t renderer item\endlink
             * was deleted by a \link #libvlc_renderer_discoverer_t renderer discoverer\endlink.
             * The renderer item is no longer valid.
             */
            libvlc_RendererDiscovererItemDeleted,

            libvlc_VlmMediaAdded = 0x600,
            libvlc_VlmMediaRemoved,
            libvlc_VlmMediaChanged,
            libvlc_VlmMediaInstanceStarted,
            libvlc_VlmMediaInstanceStopped,
            libvlc_VlmMediaInstanceStatusInit,
            libvlc_VlmMediaInstanceStatusOpening,
            libvlc_VlmMediaInstanceStatusPlaying,
            libvlc_VlmMediaInstanceStatusPause,
            libvlc_VlmMediaInstanceStatusEnd,
            libvlc_VlmMediaInstanceStatusError
        }
    }

    namespace Logging
    {
        /// <summary>
        ///     Logging messages level.
        /// </summary>
        /// <remarks>
        ///     Future LibVLC versions may define new levels.
        /// </remarks>
        public enum libvlc_log_level
        {
            /// <summary>
            ///     Debug message
            /// </summary>
            LIBVLC_DEBUG = 0,

            /// <summary>
            ///     Important informational messag
            /// </summary>
            LIBVLC_NOTICE = 2,

            /// <summary>
            ///     Warning (potential error) message
            /// </summary>
            LIBVLC_WARNING = 3,

            /// <summary>
            ///     Error message
            /// </summary>
            LIBVLC_ERROR = 4
        }
    }
}