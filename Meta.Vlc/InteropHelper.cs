// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: InteropHelper.cs
// Version: 20160214

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Meta.Vlc
{
    /// <summary>
    ///     Some helper method of interopping with unmanaged dlls.
    /// </summary>
    public static class InteropHelper
    {
        /// <summary>
        ///     Convert a pointer of string to manmaged <see cref="String" />.
        /// </summary>
        /// <param name="ptr">pointer of string</param>
        /// <param name="count">count of string, -1 mean auto check the end char</param>
        /// <param name="toBeFree">free this pointer when convert over</param>
        /// <param name="encoding">encoding of string</param>
        /// <returns>result string</returns>
        public static String PtrToString(IntPtr ptr, int count = -1, bool toBeFree = false, Encoding encoding = null)
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
                LibVlcManager.Free(ptr);
            }

            return encoding.GetString(buffer.ToArray());
        }

        /// <summary>
        ///     Pinned a <see cref="String" /> to get pointer of this, you should call <see cref="GCHandle.Free" /> when all is
        ///     over.
        /// </summary>
        /// <param name="str">string you need pinned</param>
        /// <returns>GCHandle of <see cref="String" />, you can call <see cref="GCHandle.AddrOfPinnedObject" /> to get pointer.</returns>
        public static GCHandle StringToPtr(String str)
        {
            var handle = GCHandle.Alloc(Encoding.UTF8.GetBytes(str), GCHandleType.Pinned);
            return handle;
        }

        /// <summary>
        ///     Convert a pointer array to <see cref="String" /> array.
        /// </summary>
        /// <param name="ptrs">pointer array</param>
        /// <param name="length">length of pointer array</param>
        /// <returns><see cref="String" /> array</returns>
        public static String[] PtrsToStringArray(IntPtr[] ptrs, int length)
        {
            String[] result = new String[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = ptrs[i] == IntPtr.Zero ? null : PtrToString(ptrs[i]);
            }
            return result;
        }

        /// <summary>
        ///     Get a pointer of <see cref="String" /> array.
        /// </summary>
        /// <param name="strings"><see cref="String" /> array</param>
        /// <returns>pointer of <see cref="String" /> array</returns>
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