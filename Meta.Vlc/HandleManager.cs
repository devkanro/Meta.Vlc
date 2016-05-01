// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: HandleManager.cs
// Version: 20160214

using System;
using System.Collections.Generic;

namespace Meta.Vlc
{
    internal static class HandleManager
    {
        private static readonly Dictionary<IntPtr, IVlcObject> HandleDic = new Dictionary<IntPtr, IVlcObject>();

        public static IVlcObject GetVlcObject(IntPtr pointer)
        {
            if (HandleDic.ContainsKey(pointer))
            {
                return HandleDic[pointer];
            }
            return null;
        }

        public static void Add(IVlcObject vlcObject)
        {
            if (!HandleDic.ContainsKey(vlcObject.InstancePointer))
            {
                HandleDic.Add(vlcObject.InstancePointer, vlcObject);
            }
        }

        public static void Remove(IVlcObject vlcObject)
        {
            if (HandleDic.ContainsKey(vlcObject.InstancePointer))
            {
                HandleDic.Remove(vlcObject.InstancePointer);
            }
        }
    }
}