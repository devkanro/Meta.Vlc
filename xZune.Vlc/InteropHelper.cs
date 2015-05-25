using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace xZune.Vlc
{
    public static class InteropHelper
    {
        public static String PtrToString(IntPtr ptr)
        {
            return ptr == IntPtr.Zero ? null : Marshal.PtrToStringAnsi(ptr);
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
