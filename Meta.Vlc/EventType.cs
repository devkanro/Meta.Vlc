// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: EventType.cs
// Version: 20181231

using Meta.Vlc.Interop.Core.Event;
using Meta.Vlc.Interop.Media;

namespace Meta.Vlc
{
    public enum EventType
    {
        /// <summary>
        ///     Metadata of a <see cref="VlcMedia" /> changed
        /// </summary>
        MediaMetaChanged = libvlc_event_e.libvlc_MediaMetaChanged,

        /// <summary>
        ///     Subitem was added to a <see cref="VlcMedia" />
        /// </summary>
        /// <seealso cref="VlcMedia.Subitems" />
        MediaSubItemAdded = libvlc_event_e.libvlc_MediaSubItemAdded,

        /// <summary>
        ///     Duration of a <see cref="VlcMedia" /> changed
        /// </summary>
        /// <seealso cref="VlcMedia.Duration" />
        MediaDurationChanged = libvlc_event_e.libvlc_MediaDurationChanged,

        /// <summary>
        ///     Parsing state of a <see cref="VlcMedia" /> changed
        /// </summary>
        /// <seealso cref="VlcMedia.ParseWithOptionAsync" />
        /// <seealso cref="VlcMedia.ParseStatus" />
        /// <seealso cref="VlcMedia.ParseStop" />
        MediaParsedChanged = libvlc_event_e.libvlc_MediaParsedChanged,

        /// <summary>
        ///     A <see cref="VlcMedia" /> was freed
        /// </summary>
        MediaFreed = libvlc_event_e.libvlc_MediaFreed,

        /// <summary>
        ///     \link #libvlc_state_t State\endlink of the <see cref="VlcMedia" /> changed
        /// </summary>
        /// <seealso cref="libvlc_media_get_state" />
        MediaStateChanged = libvlc_event_e.libvlc_MediaStateChanged,

        /// <summary>
        ///     Subitem tree was added to a <see cref="VlcMedia" />
        /// </summary>
        MediaSubItemTreeAdded = libvlc_event_e.libvlc_MediaSubItemTreeAdded,

        MediaPlayerMediaChanged = libvlc_event_e.libvlc_MediaPlayerMediaChanged,
        MediaPlayerNothingSpecial = libvlc_event_e.libvlc_MediaPlayerNothingSpecial,
        MediaPlayerOpening = libvlc_event_e.libvlc_MediaPlayerOpening,
        MediaPlayerBuffering = libvlc_event_e.libvlc_MediaPlayerBuffering,
        MediaPlayerPlaying = libvlc_event_e.libvlc_MediaPlayerPlaying,
        MediaPlayerPaused = libvlc_event_e.libvlc_MediaPlayerPaused,
        MediaPlayerStopped = libvlc_event_e.libvlc_MediaPlayerStopped,
        MediaPlayerForward = libvlc_event_e.libvlc_MediaPlayerForward,
        MediaPlayerBackward = libvlc_event_e.libvlc_MediaPlayerBackward,
        MediaPlayerEndReached = libvlc_event_e.libvlc_MediaPlayerEndReached,
        MediaPlayerEncounteredError = libvlc_event_e.libvlc_MediaPlayerEncounteredError,
        MediaPlayerTimeChanged = libvlc_event_e.libvlc_MediaPlayerTimeChanged,
        MediaPlayerPositionChanged = libvlc_event_e.libvlc_MediaPlayerPositionChanged,
        MediaPlayerSeekableChanged = libvlc_event_e.libvlc_MediaPlayerSeekableChanged,
        MediaPlayerPausableChanged = libvlc_event_e.libvlc_MediaPlayerPausableChanged,
        MediaPlayerTitleChanged = libvlc_event_e.libvlc_MediaPlayerTitleChanged,
        MediaPlayerSnapshotTaken = libvlc_event_e.libvlc_MediaPlayerSnapshotTaken,
        MediaPlayerLengthChanged = libvlc_event_e.libvlc_MediaPlayerLengthChanged,
        MediaPlayerVout = libvlc_event_e.libvlc_MediaPlayerVout,
        MediaPlayerScrambledChanged = libvlc_event_e.libvlc_MediaPlayerScrambledChanged,
        MediaPlayerESAdded = libvlc_event_e.libvlc_MediaPlayerESAdded,
        MediaPlayerESDeleted = libvlc_event_e.libvlc_MediaPlayerESDeleted,
        MediaPlayerESSelected = libvlc_event_e.libvlc_MediaPlayerESSelected,
        MediaPlayerCorked = libvlc_event_e.libvlc_MediaPlayerCorked,
        MediaPlayerUncorked = libvlc_event_e.libvlc_MediaPlayerUncorked,
        MediaPlayerMuted = libvlc_event_e.libvlc_MediaPlayerMuted,
        MediaPlayerUnmuted = libvlc_event_e.libvlc_MediaPlayerUnmuted,
        MediaPlayerAudioVolume = libvlc_event_e.libvlc_MediaPlayerAudioVolume,
        MediaPlayerAudioDevice = libvlc_event_e.libvlc_MediaPlayerAudioDevice,
        MediaPlayerChapterChanged = libvlc_event_e.libvlc_MediaPlayerChapterChanged,

        /**
         * A <see cref="VlcMedia"/> was added to a
         * \link #libvlc_media_list_t media list\endlink.
         */
        MediaListItemAdded = libvlc_event_e.libvlc_MediaListItemAdded,

        /**
         * A <see cref="VlcMedia"/> is about to get
         * added to a \link #libvlc_media_list_t media list\endlink.
         */
        MediaListWillAddItem = libvlc_event_e.libvlc_MediaListWillAddItem,

        /**
         * A <see cref="VlcMedia"/> was deleted from
         * a \link #libvlc_media_list_t media list\endlink.
         */
        MediaListItemDeleted = libvlc_event_e.libvlc_MediaListItemDeleted,

        /**
         * A <see cref="VlcMedia"/> is about to get
         * deleted from a \link #libvlc_media_list_t media list\endlink.
         */
        MediaListWillDeleteItem = libvlc_event_e.libvlc_MediaListWillDeleteItem,

        /**
         * A \link #libvlc_media_list_t media list\endlink has reached the
         * end.
         * All \link #libvlc_media_t items\endlink were either added (in
         * case of a \ref libvlc_media_discoverer_t) or parsed (preparser).
         */
        MediaListEndReached = libvlc_event_e.libvlc_MediaListEndReached,

        /**
         * \deprecated No longer used.
         * This belonged to the removed libvlc_media_list_view_t
         */
        MediaListViewItemAdded = libvlc_event_e.libvlc_MediaListViewItemAdded,

        /**
         * \deprecated No longer used.
         * This belonged to the removed libvlc_media_list_view_t
         */
        MediaListViewWillAddItem = libvlc_event_e.libvlc_MediaListViewWillAddItem,

        /**
         * \deprecated No longer used.
         * This belonged to the removed libvlc_media_list_view_t
         */
        MediaListViewItemDeleted = libvlc_event_e.libvlc_MediaListViewItemDeleted,

        /**
         * \deprecated No longer used.
         * This belonged to the removed libvlc_media_list_view_t
         */
        MediaListViewWillDeleteItem = libvlc_event_e.libvlc_MediaListViewWillDeleteItem,

        /**
         * Playback of a \link #libvlc_media_list_player_t media list
         * player\endlink has started.
         */
        MediaListPlayerPlayed = libvlc_event_e.libvlc_MediaListPlayerPlayed,

        /**
         * The current \link #libvlc_media_t item\endlink of a
         * \link #libvlc_media_list_player_t media list player\endlink
         * has changed to a different item.
         */
        MediaListPlayerNextItemSet = libvlc_event_e.libvlc_MediaListPlayerNextItemSet,

        /**
         * Playback of a \link #libvlc_media_list_player_t media list
         * player\endlink has stopped.
         */
        MediaListPlayerStopped = libvlc_event_e.libvlc_MediaListPlayerStopped,

        /**
         * A new \link #libvlc_renderer_item_t renderer item\endlink was found by a
         * \link #libvlc_renderer_discoverer_t renderer discoverer\endlink.
         * The renderer item is valid until deleted.
         */
        RendererDiscovererItemAdded = libvlc_event_e.libvlc_RendererDiscovererItemAdded,

        /**
         * A previously discovered \link #libvlc_renderer_item_t renderer item\endlink
         * was deleted by a \link #libvlc_renderer_discoverer_t renderer discoverer\endlink.
         * The renderer item is no longer valid.
         */
        RendererDiscovererItemDeleted = libvlc_event_e.libvlc_RendererDiscovererItemDeleted,

        VlmMediaAdded = libvlc_event_e.libvlc_VlmMediaAdded,
        VlmMediaRemoved = libvlc_event_e.libvlc_VlmMediaRemoved,
        VlmMediaChanged = libvlc_event_e.libvlc_VlmMediaChanged,
        VlmMediaInstanceStarted = libvlc_event_e.libvlc_VlmMediaInstanceStarted,
        VlmMediaInstanceStopped = libvlc_event_e.libvlc_VlmMediaInstanceStopped,
        VlmMediaInstanceStatusInit = libvlc_event_e.libvlc_VlmMediaInstanceStatusInit,
        VlmMediaInstanceStatusOpening = libvlc_event_e.libvlc_VlmMediaInstanceStatusOpening,
        VlmMediaInstanceStatusPlaying = libvlc_event_e.libvlc_VlmMediaInstanceStatusPlaying,
        VlmMediaInstanceStatusPause = libvlc_event_e.libvlc_VlmMediaInstanceStatusPause,
        VlmMediaInstanceStatusEnd = libvlc_event_e.libvlc_VlmMediaInstanceStatusEnd,
        VlmMediaInstanceStatusError = libvlc_event_e.libvlc_VlmMediaInstanceStatusError
    }
}