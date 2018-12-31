// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: VlcError.cs
// Version: 20181231

using System;
using Meta.Vlc.Interop.Core.Error;

namespace Meta.Vlc
{
    public static unsafe class VlcError
    {
        /// <summary>
        ///     Get a readable error message.
        /// </summary>
        /// <returns>return a readable LibVlc error message, if there are no error will return <see cref="null" /></returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public static string GetErrorMessage()
        {
            return InteropHelper.PtrToString(LibVlcManager.GetFunctionDelegate<libvlc_errmsg>().Invoke());
        }

        /// <summary>
        ///     Clear error message of current thread.
        /// </summary>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public static void CleanError()
        {
            LibVlcManager.GetFunctionDelegate<libvlc_clearerr>().Invoke();
        }
    }
}