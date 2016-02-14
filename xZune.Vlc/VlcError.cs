// Project: xZune.Vlc (https://github.com/higankanshi/xZune.Vlc)
// Filename: VlcError.cs
// Version: 20160214

using System;
using xZune.Vlc.Interop;
using xZune.Vlc.Interop.Core.Error;

namespace xZune.Vlc
{
    public static class VlcError
    {
        private static LibVlcFunction<ErrorMessage> _errorMessageFunction;
        private static LibVlcFunction<CleanError> _cleanErrorFunction;

        /// <summary>
        ///     LibVlc error module loaded or not.
        /// </summary>
        public static bool IsLibLoaded { get; private set; }

        internal static void LoadLibVlc()
        {
            if (!IsLibLoaded)
            {
                _errorMessageFunction = new LibVlcFunction<ErrorMessage>();
                _cleanErrorFunction = new LibVlcFunction<CleanError>();
                IsLibLoaded = true;
            }
        }

        /// <summary>
        ///     Get a readable error message.
        /// </summary>
        /// <returns>return a readable LibVlc error message, if there are no error will return <see cref="null" /></returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public static String GetErrorMessage()
        {
            return InteropHelper.PtrToString(_errorMessageFunction.Delegate());
        }

        /// <summary>
        ///     Clear error message of current thread.
        /// </summary>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public static void CleanError()
        {
            _cleanErrorFunction.Delegate();
        }
    }
}