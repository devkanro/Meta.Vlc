using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using xZune.Vlc.Interop;
using xZune.Vlc.Interop.Core.Events;

namespace xZune.Vlc
{
    public class VlcEventManager : IVlcObject
    {
        static VlcEventManager()
        {
            IsLibLoaded = false;
        }

        /// <summary>
        /// 载入 LibVlc 的 Event 模块,该方法会在 <see cref="Vlc.LoadLibVlc"/> 中自动被调用
        /// </summary>
        /// <param name="libHandle"></param>
        /// <param name="libVersion"></param>
        public static void LoadLibVlc(IntPtr libHandle, Version libVersion, String devString)
        {
            if(!IsLibLoaded)
            {
                EventAttachFunction = new LibVlcFunction<EventAttach>(libHandle, libVersion, devString);
                EventDetachFunction = new LibVlcFunction<EventDetach>(libHandle, libVersion, devString);
                GetTypeNameFunction = new LibVlcFunction<GetTypeName>(libHandle, libVersion, devString);
                IsLibLoaded = true;
            }
        }

        /// <summary>
        /// 获取一个值,该值指示当前模块是否被载入
        /// </summary>
        public static bool IsLibLoaded
        {
            get;
            private set;
        }

        private static LibVlcFunction<EventAttach> EventAttachFunction;
        private static LibVlcFunction<EventDetach> EventDetachFunction;
        private static LibVlcFunction<GetTypeName> GetTypeNameFunction;

        public VlcEventManager(IntPtr pointer)
        {
            InstancePointer = pointer;
            HandleManager.Add(this);
        }

        public IntPtr InstancePointer { get; private set; }

        public void Attach(EventTypes type, LibVlcEventCallBack callback , IntPtr userData)
        {
            EventAttachFunction.Delegate(InstancePointer, type, callback, userData);
        }

        public void Detach(EventTypes type, LibVlcEventCallBack callback, IntPtr userData)
        {
            EventDetachFunction.Delegate(InstancePointer, type, callback, userData);
        }

        public static String GetEventTypeName(EventTypes type)
        {
            return InteropHelper.PtrToString(GetTypeNameFunction.Delegate(type));
        }

        public void Dispose()
        {
            HandleManager.Remove(this);
            Vlc.Free(InstancePointer);
            InstancePointer = IntPtr.Zero;
        }
    }
}
