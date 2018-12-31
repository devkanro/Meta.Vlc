// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: Callback.cs
// Version: 20181231

using System.Runtime.InteropServices;

namespace Meta.Vlc.Interop.Core
{
    public unsafe delegate void ExitHandler(void* data);

    namespace Event
    {
        /// <summary>
        ///     Callback function notification
        /// </summary>
        /// <param name="p_event">the event triggering the callback</param>
        /// <param name="data"></param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_callback_t(libvlc_event_t* p_event, void* data);
    }

    namespace Logging
    {
        /**
         * Callback prototype for LibVLC log message handler.
         *
         * \param data data pointer as given to libvlc_log_set()
         * \param level message level (@ref libvlc_log_level)
         * \param ctx message context (meta-information about the message)
         * \param fmt printf() format string (as defined by ISO C11)
         * \param args variable argument list for the format
         * \note Log message handlers <b>must</b> be thread-safe.
         * \warning The message context pointer, the format string parameters and the
         *          variable arguments are only valid until the callback returns.
         */

        //public unsafe delegate void libvlc_log_cb(void* data, int level, IntPtr ctx,
        //    byte* fmt, va_list args);
    }
}