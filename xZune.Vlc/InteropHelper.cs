using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace xZune.Vlc
{
    public static class InteropHelper
    {
        public static String PtrToString(IntPtr ptr , int count = -1, bool toBeFree = false, Encoding  encoding = null)
        {
            if (ptr == IntPtr.Zero)
            {
                return null;
            }
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            List<byte> buffer = new List<byte>(1024);

            if (count == -1)
            {
                int offset = 0;
                byte tmp = Marshal.ReadByte(ptr, offset);
                while (tmp != 0)
                {
                    buffer.Add(tmp);
                    offset++;
                    tmp = Marshal.ReadByte(ptr, offset);
                }
            }
            else
            {
                byte tmp = 0;
                for (int i = 0; i < count; i++)
                {
                    tmp = Marshal.ReadByte(ptr, i);
                    buffer.Add(tmp);
                }
            }
            

            if (toBeFree)
            {
                Vlc.Free(ptr);
            }

            return encoding.GetString(buffer.ToArray());
        }

        public static GCHandle StringToPtr(String str)
        {
            var handle = GCHandle.Alloc(Encoding.UTF8.GetBytes(str), GCHandleType.Pinned);
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