// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: TrackDescription.cs
// Version: 20181231

using System;
using Meta.Vlc.Interop.MediaPlayer;

namespace Meta.Vlc
{
    /// <summary>
    ///     A wrapper for <see cref="libvlc_track_description_t" /> struct.
    /// </summary>
    public unsafe class TrackDescription
    {
        internal TrackDescription(libvlc_track_description_t* pointer)
        {
            if (pointer == null) return;
            Name = InteropHelper.PtrToString(pointer->psz_name);
            Id = pointer->i_id;
        }

        public string Name { get; }

        public int Id { get; }
    }

    /// <summary>
    ///     A list wrapper for <see cref="libvlc_track_description_t" /> linked list struct.
    /// </summary>
    public unsafe class TrackDescriptionList : VlcUnmanagedLinkedList<TrackDescription>
    {
        public TrackDescriptionList(void* pointer) : base(pointer)
        {
        }

        protected override TrackDescription CreateItem(void* data)
        {
            return new TrackDescription((libvlc_track_description_t*) data);
        }

        protected override void* NextItem(void* data)
        {
            return ((libvlc_track_description_t*) data)->p_next;
        }

        protected override void Release(void* data)
        {
            LibVlcManager.GetFunctionDelegate<libvlc_track_description_list_release>()
                .Invoke((libvlc_track_description_t*) data);
        }
    }
}