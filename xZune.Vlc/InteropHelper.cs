using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace xZune.Vlc
{
    public static class InteropHelper
    {
        public static String PtrToString(IntPtr ptr)
        {
            if(ptr == IntPtr.Zero)
            {
                return null;
            }

            int offset = 0;
            byte tmp = Marshal.ReadByte(ptr, offset);
            List<byte> buffer = new List<byte>(1024);

            while (tmp != 0)
            {
                buffer.Add(tmp);
                offset++;
                tmp = Marshal.ReadByte(ptr, offset);
            }

            return Encoding.UTF8.GetString(buffer.ToArray());
        }

        public static GCHandle StringToPtr(String str)
        {
            var handle = GCHandle.Alloc(Encoding.UTF8.GetByteCount(str), GCHandleType.Pinned);
            return handle;
        }

        public static String[] PtrsToStringArray(IntPtr[] ptrs, int length)
        {
            String[] result = new String[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = ptrs[i] == IntPtr.Zero ? null : PtrToString(ptrs[i]);
            }
            return result;
        }

        public static IntPtr StringArrayToPtr(String[] strings)
        {
            IntPtr[] ptrs = new IntPtr[strings.Length];

            for (int i = 0; i < strings.Length; i++)
            {
                ptrs[i] = Marshal.StringToHGlobalAnsi(strings[i]);
            }

            return Marshal.UnsafeAddrOfPinnedArrayElement(ptrs, 0);
        }
    }
}
