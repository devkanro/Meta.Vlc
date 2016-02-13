//Project: xZune.Vlc (https://github.com/higankanshi/xZune.Vlc)
//Filename: VlcEventManager.cs
//Version: 20160213

using System;

using xZune.Vlc.Interop;
using xZune.Vlc.Interop.Core.Events;

namespace xZune.Vlc
{
    /// <summary>
    /// A manager of LibVlc event system.
    /// </summary>
    public class VlcEventManager : IVlcObject
    {
        static VlcEventManager()
        {
            IsLibLoaded = false;
        }
        
        internal static void LoadLibVlc()
        {
            if (!IsLibLoaded)
            {
                _eventAttachFunction = new LibVlcFunction<EventAttach>();
                _eventDetachFunction = new LibVlcFunction<EventDetach>();
                _getTypeNameFunction = new LibVlcFunction<GetTypeName>();
                IsLibLoaded = true;
            }
        }

        /// <summary>
        /// LibVlc event module loaded or not.
        /// </summary>
        public static bool IsLibLoaded
        {
            get;
            private set;
        }

        private static LibVlcFunction<EventAttach> _eventAttachFunction;
        private static LibVlcFunction<EventDetach> _eventDetachFunction;
        private static LibVlcFunction<GetTypeName> _getTypeNameFunction;

        /// <summary>
        /// Create a event manager with parent Vlc object and pointer of event manager.
        /// </summary>
        /// <param name="parentVlcObject"></param>
        /// <param name="pointer"></param>
        public VlcEventManager(IVlcObject parentVlcObject, IntPtr pointer)
        {
            VlcInstance = parentVlcObject.VlcInstance;
            InstancePointer = pointer;
            HandleManager.Add(this);
        }

        /// <summary>
        /// Pointer of this event manager.
        /// </summary>
        public IntPtr InstancePointer { get; private set; }

        /// <summary>
        /// A relation <see cref="Vlc"/> of this object.
        /// </summary>
        public Vlc VlcInstance { get; private set; }

        /// <summary>
        /// Attach a event with a callback.
        /// </summary>
        /// <param name="type">event type</param>
        /// <param name="callback">callback which will be called when event case</param>
        /// <param name="userData">some custom data</param>
        public void Attach(EventTypes type, LibVlcEventCallBack callback, IntPtr userData)
        {
            _eventAttachFunction.Delegate(InstancePointer, type, callback, userData);
        }

        /// <summary>
        /// Deattach a event with a callback.
        /// </summary>
        /// <param name="type">event type</param>
        /// <param name="callback">callback which will be called when event case</param>
        /// <param name="userData">some custom data</param>
        public void Detach(EventTypes type, LibVlcEventCallBack callback, IntPtr userData)
        {
            _eventDetachFunction.Delegate(InstancePointer, type, callback, userData);
        }

        /// <summary>
        /// Get event type name.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static String GetEventTypeName(EventTypes type)
        {
            return InteropHelper.PtrToString(_getTypeNameFunction.Delegate(type));
        }

        /// <summary>
        /// Release this event manager.
        /// </summary>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public void Dispose()
        {
            HandleManager.Remove(this);
            LibVlcManager.Free(InstancePointer);
            InstancePointer = IntPtr.Zero;
        }
    }
}