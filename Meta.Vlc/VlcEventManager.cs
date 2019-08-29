// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: VlcEventManager.cs
// Version: 20181231

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Meta.Vlc.Interop.Core.Event;

namespace Meta.Vlc
{
    /// <summary>
    ///     A manager of LibVlc event system.
    /// </summary>
    public unsafe class VlcEventManager : IVlcObject
    {
        private readonly HashSet<EventType> _attachedEvents = new HashSet<EventType>();
        private readonly libvlc_callback_t _onVlcEventFired;
        private GCHandle _onVlcEventFiredHandle;

        /// <summary>
        ///     Create a event manager with parent Vlc object and pointer of event manager.
        /// </summary>
        /// <param name="parentVlcObject"></param>
        /// <param name="pointer"></param>
        public VlcEventManager(IVlcObject parentVlcObject, void* pointer)
        {
            VlcInstance = parentVlcObject.VlcInstance;
            InstancePointer = pointer;
            VlcObjectManager.Add(this);

            _onVlcEventFired = OnVlcEventFired;
            _onVlcEventFiredHandle = GCHandle.Alloc(_onVlcEventFired);
        }

        /// <summary>
        ///     Pointer of this event manager.
        /// </summary>
        public void* InstancePointer { get; }

        /// <summary>
        ///     A relation <see cref="Vlc" /> of this object.
        /// </summary>
        public Vlc VlcInstance { get; }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Attach a event with a callback.
        /// </summary>
        /// <param name="type">event type</param>
        public void Attach(EventType type)
        {
            if (_attachedEvents.Contains(type)) return;

            _attachedEvents.Add(type);
            LibVlcManager.GetFunctionDelegate<libvlc_event_attach>()
                .Invoke(InstancePointer, (libvlc_event_e) type, _onVlcEventFired, null);
        }

        /// <summary>
        ///     Deattach a event with a callback.
        /// </summary>
        /// <param name="type">event type</param>
        public void Detach(EventType type)
        {
            if (!_attachedEvents.Contains(type)) return;

            _attachedEvents.Remove(type);
            LibVlcManager.GetFunctionDelegate<libvlc_event_detach>()
                .Invoke(InstancePointer, (libvlc_event_e) type, _onVlcEventFired, null);
        }

        public event EventHandler<VlcEventArgs> VlcEventFired;

        private void OnVlcEventFired(libvlc_event_t* p_event, void* data)
        {
            VlcEventFired?.Invoke(this, new VlcEventArgs((EventType) p_event->type, p_event));
        }

        /// <summary>
        ///     Get event type name.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetEventTypeName(EventType type)
        {
            return InteropHelper.PtrToString(LibVlcManager.GetFunctionDelegate<libvlc_event_type_name>()
                .Invoke((libvlc_event_e) type));
        }

        private void ReleaseUnmanagedResources()
        {
            foreach (var eventType in new List<EventType>(_attachedEvents)) Detach(eventType);
            if (_onVlcEventFiredHandle.IsAllocated) _onVlcEventFiredHandle.Free();
        }

        ~VlcEventManager()
        {
            ReleaseUnmanagedResources();
        }
    }

    public unsafe class VlcEventArgs : EventArgs
    {
        internal VlcEventArgs(EventType type, libvlc_event_t* eventArgs)
        {
            EventArgs = eventArgs;
            Type = type;
        }

        public libvlc_event_t* EventArgs { get; }

        public EventType Type { get; }
    }
}
