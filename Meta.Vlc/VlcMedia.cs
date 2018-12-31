// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: VlcMedia.cs
// Version: 20181231

using System;
using System.Collections.Generic;
using Meta.Vlc.Event;
using Meta.Vlc.Interop.Media;

namespace Meta.Vlc
{
    /// <summary>
    ///     The API wrapper of LibVlc media.
    /// </summary>
    public unsafe class VlcMedia : IVlcObjectWithEvent
    {
        private readonly MediaStats _stats = new MediaStats();

        private VlcMedia(IVlcObject parentVlcObject, void* pointer)
        {
            VlcInstance = parentVlcObject.VlcInstance;
            InstancePointer = pointer;
            VlcObjectManager.Add(this);

            EventManager = new VlcEventManager(this,
                LibVlcManager.GetFunctionDelegate<libvlc_media_event_manager>().Invoke(InstancePointer));

            EventManager.Attach(EventType.MediaMetaChanged);
            EventManager.Attach(EventType.MediaSubItemAdded);
            EventManager.Attach(EventType.MediaDurationChanged);
            EventManager.Attach(EventType.MediaParsedChanged);
            EventManager.Attach(EventType.MediaFreed);
            EventManager.Attach(EventType.MediaStateChanged);
            EventManager.Attach(EventType.MediaSubItemTreeAdded);

            EventManager.VlcEventFired += OnVlcEventFired;
        }

        /// <summary>
        ///     Get duration of media descriptor object item.
        /// </summary>
        public TimeSpan Duration =>
            new TimeSpan(LibVlcManager.GetFunctionDelegate<libvlc_media_get_duration>().Invoke(InstancePointer) *
                         10000);

        /// <summary>
        ///     Get the media resource locator (mrl) from a media descriptor object.
        /// </summary>
        public string Mrl =>
            InteropHelper.PtrToString(LibVlcManager.GetFunctionDelegate<libvlc_media_get_mrl>()
                .Invoke(InstancePointer));

        /// <summary>
        ///     Get current state of media descriptor object.
        /// </summary>
        public MediaState State
        {
            get
            {
                if (InstancePointer == null)
                    return MediaState.NothingSpecial;

                var state = (MediaState) LibVlcManager.GetFunctionDelegate<libvlc_media_get_state>()
                    .Invoke(InstancePointer);
                if (state == MediaState.Error) Error = VlcError.GetErrorMessage();
                return state;
            }
        }

        public string Error { get; private set; }

        /// <summary>
        ///     Get the current statistics about the media
        /// </summary>
        public MediaStats Stats
        {
            get
            {
                fixed (libvlc_media_stats_t* pointer = &_stats.Struct)
                {
                    if (LibVlcManager.GetFunctionDelegate<libvlc_media_get_stats>().Invoke(InstancePointer, pointer))
                        return _stats;
                }

                throw new Exception("Statistics about the media is unavailable");
            }
        }

        /// <summary>
        ///     Get media descriptor's user_data. user_data is specialized data
        ///     accessed by the host application, VLC.framework uses it as a pointer to
        ///     an native object that references a <see cref="VlcMedia" /> pointer.
        /// </summary>
        public IntPtr UserData
        {
            get => new IntPtr(LibVlcManager.GetFunctionDelegate<libvlc_media_get_user_data>().Invoke(InstancePointer));

            set => LibVlcManager.GetFunctionDelegate<libvlc_media_set_user_data>()
                .Invoke(InstancePointer, value.ToPointer());
        }

        [Obsolete("Use 'ParsedStatus' instead")]
        public bool IsParsed =>
            LibVlcManager.GetFunctionDelegate<libvlc_media_get_parsed_status>().Invoke(InstancePointer) ==
            libvlc_media_parsed_status_t.libvlc_media_parsed_status_done;

        /// <summary>
        ///     Get Parsed status for media descriptor object.
        /// </summary>
        public MediaParsedStatus ParsedStatus => (MediaParsedStatus) LibVlcManager
            .GetFunctionDelegate<libvlc_media_get_parsed_status>().Invoke(InstancePointer);

        /// <summary>
        ///     Get sub items of media descriptor object. This will increment
        ///     the reference count of supplied media descriptor object. Use
        ///     libvlc_media_list_release to decrement the reference counting.
        /// </summary>
        public IntPtr SubItems =>
            new IntPtr(LibVlcManager.GetFunctionDelegate<libvlc_media_subitems>().Invoke(InstancePointer));

        public void* InstancePointer { get; private set; }

        public Vlc VlcInstance { get; }

        public VlcEventManager EventManager { get; }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        private void OnVlcEventFired(object sender, VlcEventArgs e)
        {
            switch (e.Type)
            {
                case EventType.MediaMetaChanged:
                    MetaChanged?.Invoke(this,
                        new MediaMetaChangedEventArgs((MediaMetaType) e.EventArgs->media_meta_changed.meta_type));
                    break;
                case EventType.MediaSubItemAdded:
                    SubItemAdded?.Invoke(this,
                        new MediaSubItemAddedEventArgs(e.EventArgs->media_subitem_added.new_child));
                    break;
                case EventType.MediaDurationChanged:
                    DurationChanged?.Invoke(this,
                        new MediaDurationChangedEventArgs(e.EventArgs->media_duration_changed.new_duration));
                    break;
                case EventType.MediaParsedChanged:
                    ParsedChanged?.Invoke(this,
                        new MediaParsedStatusChangedEventArgs(
                            (MediaParsedStatus) e.EventArgs->media_parsed_changed.new_status));
                    break;
                case EventType.MediaFreed:
                    Freed?.Invoke(this, new MediaFreedEventArgs(e.EventArgs->media_freed.md));
                    break;
                case EventType.MediaStateChanged:
                    StateChanged?.Invoke(this,
                        new MediaStateChangedEventArgs((MediaState) e.EventArgs->media_state_changed.new_state));
                    break;
                case EventType.MediaSubItemTreeAdded:
                    SubItemTreeAdded?.Invoke(this,
                        new MediaSubItemAddedEventArgs(e.EventArgs->media_subitemtree_added.md));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public event EventHandler<MediaMetaChangedEventArgs> MetaChanged;
        public event EventHandler<MediaSubItemAddedEventArgs> SubItemAdded;
        public event EventHandler<MediaDurationChangedEventArgs> DurationChanged;
        public event EventHandler<MediaParsedStatusChangedEventArgs> ParsedChanged;
        public event EventHandler<MediaFreedEventArgs> Freed;
        public event EventHandler<MediaStateChangedEventArgs> StateChanged;
        public event EventHandler<MediaSubItemAddedEventArgs> SubItemTreeAdded;

        /// <summary>
        ///     Create a media as an empty node with a given name
        /// </summary>
        public static VlcMedia CreateAsNewNode(Vlc vlc, string name)
        {
            using (var handle = new StringHandle(name))
            {
                var pointer = LibVlcManager.GetFunctionDelegate<libvlc_media_new_as_node>()
                    .Invoke(vlc.InstancePointer, handle.UnsafePointer);

                if (pointer == null) throw new LibVlcException("Fail to create media.");

                return new VlcMedia(vlc, pointer);
            }
        }

        /// <summary>
        ///     Create a media with a certain given media resource location,
        ///     for instance a valid URL.
        ///     <para />
        ///     To refer to a local file with this function,
        ///     the file://... URI syntax <b>must</b> be used (see IETF RFC3986).
        ///     We recommend using <see cref="CreateFormPath" /> instead when dealing with local files.
        /// </summary>
        public static VlcMedia CreateFormLocation(Vlc vlc, string url)
        {
            using (var handle = new StringHandle(url))
            {
                var pointer = LibVlcManager.GetFunctionDelegate<libvlc_media_new_location>()
                    .Invoke(vlc.InstancePointer, handle.UnsafePointer);

                if (pointer == null) throw new LibVlcException("Fail to create media.");

                return new VlcMedia(vlc, pointer);
            }
        }

        /// <summary>
        ///     Create a media for a certain file path.
        /// </summary>
        public static VlcMedia CreateFormPath(Vlc vlc, string path)
        {
            using (var handle = new StringHandle(path))
            {
                var pointer = LibVlcManager.GetFunctionDelegate<libvlc_media_new_path>()
                    .Invoke(vlc.InstancePointer, handle.UnsafePointer);

                if (pointer == null) throw new LibVlcException("Fail to create media.");

                return new VlcMedia(vlc, pointer);
            }
        }

        /// <summary>
        ///     Add an option to the media.
        ///     <para />
        ///     This option will be used to determine how the <see cref="VlcMediaPlayer" /> will
        ///     read the media. This allows to use VLC's advanced
        ///     reading/streaming options on a per-media basis.
        /// </summary>
        /// <param name="options"></param>
        public void AddOption(params string[] options)
        {
            foreach (var option in options)
                using (var handle = new StringHandle(option))
                {
                    LibVlcManager.GetFunctionDelegate<libvlc_media_add_option>()
                        .Invoke(InstancePointer, handle.UnsafePointer);
                }
        }

        /// <summary>
        ///     Add an option to the media with configurable flags.
        ///     <para />
        ///     This option will be used to determine how the <see cref="VlcMediaPlayer" /> will
        ///     read the media. This allows to use VLC's advanced
        ///     reading/streaming options on a per-media basis.
        ///     <para />
        ///     The options are detailed in vlc --long-help, for instance
        ///     "--sout-all". Note that all options are not usable on medias:
        ///     specifically, due to architectural issues, video-related options
        ///     such as text renderer options cannot be set on a single media. They
        ///     must be set on the whole libvlc instance instead.
        /// </summary>
        public void AddOption(string option, uint flag)
        {
            using (var handle = new StringHandle(option))
            {
                LibVlcManager.GetFunctionDelegate<libvlc_media_add_option_flag>()
                    .Invoke(InstancePointer, handle.UnsafePointer, flag);
            }
        }

        /// <summary>
        ///     Duplicate a media descriptor object.
        /// </summary>
        public VlcMedia Duplicate()
        {
            return new VlcMedia(this,
                LibVlcManager.GetFunctionDelegate<libvlc_media_duplicate>().Invoke(InstancePointer));
        }

        /// <summary>
        ///     Get codec description from media elementary stream
        /// </summary>
        /// <param name="type">
        ///     <see cref="MediaTrack.Type" />
        /// </param>
        /// <param name="codec"><see cref="MediaTrack.OriginalFourcc" /> or <see cref="MediaTrack.Codec" /></param>
        public static string GetCodecDescription(TrackType type, uint codec)
        {
            return InteropHelper.PtrToString(LibVlcManager.GetFunctionDelegate<libvlc_media_get_codec_description>()
                .Invoke((libvlc_track_type_t) type, codec));
        }

        /// <summary>
        ///     Read the meta of the media.
        ///     <para />
        ///     If the media has not yet been parsed this will return NULL.
        /// </summary>
        public string GetMeta(MediaMetaType type)
        {
            return InteropHelper.PtrToString(LibVlcManager.GetFunctionDelegate<libvlc_media_get_meta>()
                .Invoke(InstancePointer, (libvlc_meta_t) type));
        }

        /// <summary>
        ///     Get media descriptor's elementary streams description
        ///     <para />
        ///     Note, you need to call <see cref="ParseWithOptionAsync" /> or play the media at least once
        ///     before calling this function.
        ///     Not doing this will result in an empty list.
        /// </summary>
        public List<MediaTrack> GetTrackInfo()
        {
            libvlc_media_track_t** pointer;
            var count = LibVlcManager.GetFunctionDelegate<libvlc_media_tracks_get>().Invoke(InstancePointer, &pointer);

            using (var list = new MediaTrackList((void**) pointer, count))
            {
                return new List<MediaTrack>(list);
            }
        }

        /// <summary>
        ///     Parse the media asynchronously with options.
        ///     <para />
        ///     This fetches (local or network) art, meta data and/or tracks information.
        ///     <para />
        ///     To track when this is over you can listen to <see cref="ParsedChanged" />
        ///     event. However if this functions returns an error, you will not receive any
        ///     events.
        ///     <para />
        ///     It uses a flag to specify parse options (see <see cref="MediaParseOption" />). All
        ///     these flags can be combined. By default, media is parsed if it's a local
        ///     file.
        /// </summary>
        /// <param name="option">parse options</param>
        /// <param name="timeout">
        ///     maximum time allowed to preparse the media. If -1, the
        ///     default "preparse-timeout" option will be used as a timeout. If 0, it will
        ///     wait indefinitely. If > 0, the timeout will be used (in milliseconds).
        /// </param>
        /// <remarks>Parsing can be aborted with <see cref="StopParse" />.</remarks>
        public void ParseWithOption(MediaParseOption option, int timeout = 0)
        {
            if (LibVlcManager.GetFunctionDelegate<libvlc_media_parse_with_options>()
                    .Invoke(InstancePointer, (libvlc_media_parse_flag_t) option, timeout) != 0)
                throw new LibVlcException("Fail to parse media");
        }

        /// <summary>
        ///     Stop the parsing of the media
        ///     <para />
        ///     When the media parsing is stopped, the <see cref="ParsedChanged" /> event will
        ///     be sent with the <see cref="MediaParsedStatus.Timeout" /> status.
        /// </summary>
        public void StopParse()
        {
            LibVlcManager.GetFunctionDelegate<libvlc_media_parse_stop>().Invoke(InstancePointer);
        }

        /// <summary>
        ///     Save the meta previously set
        /// </summary>
        /// <returns>true if the write operation was successful</returns>
        public bool SaveMeta()
        {
            return LibVlcManager.GetFunctionDelegate<libvlc_media_save_meta>().Invoke(InstancePointer);
        }

        /// <summary>
        ///     Set the meta of the media (this function will not save the meta, call
        ///     <see cref="SaveMeta" /> in order to save the meta)
        /// </summary>
        public void SetMeta(MediaMetaType type, string data)
        {
            using (var handle = new StringHandle(data))
            {
                LibVlcManager.GetFunctionDelegate<libvlc_media_set_meta>()
                    .Invoke(InstancePointer, (libvlc_meta_t) type, handle.UnsafePointer);
            }
        }

        public VlcMediaPlayer CreateMediaPlayer()
        {
            return VlcMediaPlayer.CreateFormMedia(this);
        }

        private void ReleaseUnmanagedResources()
        {
            VlcObjectManager.Remove(this);
            EventManager.VlcEventFired -= OnVlcEventFired;
            EventManager.Dispose();
            LibVlcManager.GetFunctionDelegate<libvlc_media_release>().Invoke(InstancePointer);
            InstancePointer = null;
        }

        ~VlcMedia()
        {
            ReleaseUnmanagedResources();
        }
    }
}