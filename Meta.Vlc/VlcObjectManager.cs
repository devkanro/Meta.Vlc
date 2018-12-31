// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: VlcObjectManager.cs
// Version: 20181231

using System;
using System.Collections.Generic;

namespace Meta.Vlc
{
    internal static unsafe class VlcObjectManager
    {
        private static readonly Dictionary<IntPtr, IVlcObject> HandleDic = new Dictionary<IntPtr, IVlcObject>();

        public static IVlcObject GetVlcObject(IntPtr pointer)
        {
            return HandleDic.ContainsKey(pointer) ? HandleDic[pointer] : null;
        }

        public static IVlcObject GetVlcObject(void* pointer)
        {
            return GetVlcObject(new IntPtr(pointer));
        }

        public static void Add(IVlcObject vlcObject)
        {
            var pointer = new IntPtr(vlcObject.InstancePointer);
            if (!HandleDic.ContainsKey(pointer)) HandleDic.Add(pointer, vlcObject);
        }

        public static void Remove(IVlcObject vlcObject)
        {
            var pointer = new IntPtr(vlcObject.InstancePointer);
            if (HandleDic.ContainsKey(pointer)) HandleDic.Remove(pointer);
        }
    }
}