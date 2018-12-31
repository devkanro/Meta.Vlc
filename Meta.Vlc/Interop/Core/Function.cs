// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: Function.cs
// Version: 20181231

using System;
using System.Runtime.InteropServices;

namespace Meta.Vlc.Interop.Core
{
    namespace Error
    {
        /// <summary>
        ///     A human-readable error message for the last LibVLC error in the calling
        ///     thread. The resulting string is valid until another error occurs (at least
        ///     until the next LibVLC call).
        /// </summary>
        /// <remarks>This will be NULL if there was no error.</remarks>
        /// <returns></returns>
        [LibVlcFunction(nameof(libvlc_errmsg))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate byte* libvlc_errmsg();

        /// <summary>
        ///     Clears the LibVLC error status for the current thread. This is optional.
        ///     By default, the error status is automatically overridden when a new error
        ///     occurs, and destroyed when the thread exits.
        /// </summary>
        [LibVlcFunction(nameof(libvlc_clearerr))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate void libvlc_clearerr();

        /// <summary>
        ///     Sets the LibVLC error status and message for the current thread.
        ///     Any previous error is overridden.
        /// </summary>
        /// <param name="fmt">the format string</param>
        /// <param name="ap">the arguments</param>
        /// <returns>a nul terminated string in any case</returns>
        [LibVlcFunction(nameof(libvlc_vprinterr))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate byte* libvlc_vprinterr(byte* fmt, IntPtr ap);

        /// <summary>
        ///     Sets the LibVLC error status and message for the current thread.
        ///     Any previous error is overridden.
        /// </summary>
        /// <param name="fmt">the format string</param>
        /// <param name="args">the arguments</param>
        /// <returns>a nul terminated string in any case</returns>
        [LibVlcFunction(nameof(libvlc_printerr))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate byte* libvlc_printerr(byte* fmt, ArgIterator args);
    }

    /// <summary>
    ///     Create and initialize a libvlc instance.
    ///     This functions accept a list of "command line" arguments similar to the
    ///     main(). These arguments affect the LibVLC instance default configuration.
    /// </summary>
    /// <param name="argc">the number of arguments (should be 0)</param>
    /// <param name="argv">the number of arguments (should be NULL)</param>
    /// <remarks>
    ///     LibVLC may create threads. Therefore, any thread-unsafe process
    ///     initialization must be performed before calling libvlc_new(). In particular
    ///     and where applicable:
    ///     - setlocale() and textdomain(),
    ///     - setenv(), unsetenv() and putenv(),
    ///     - with the X11 display system, XInitThreads()
    ///     (see also libvlc_media_player_set_xwindow()) and
    ///     - on Microsoft Windows, SetErrorMode().
    ///     - sigprocmask() shall never be invoked; pthread_sigmask() can be used.
    ///     On POSIX systems, the SIGCHLD signal <b>must not</b> be ignored, i.e. the
    ///     signal handler must set to SIG_DFL or a function pointer, not SIG_IGN.
    ///     Also while LibVLC is active, the wait() function shall not be called, and
    ///     any call to waitpid() shall use a strictly positive value for the first
    ///     parameter (i.e. the PID). Failure to follow those rules may lead to a
    ///     deadlock or a busy loop.
    ///     On Microsoft Windows, setting the default DLL directories to SYSTEM32
    ///     exclusively is strongly recommended for security reasons:
    ///     <para />
    ///     SetDefaultDllDirectories(LOAD_LIBRARY_SEARCH_SYSTEM32);
    ///     <para />
    ///     <para />
    ///     Arguments are meant to be passed from the command line to LibVLC, just like
    ///     VLC media player does. The list of valid arguments depends on the LibVLC
    ///     version, the operating system and platform, and set of available LibVLC
    ///     plugins. Invalid or unsupported arguments will cause the function to fail
    ///     (i.e. return NULL). Also, some arguments may alter the behaviour or
    ///     otherwise interfere with other LibVLC functions.
    ///     <para />
    ///     There is absolutely no warranty or promise of forward, backward and
    ///     cross-platform compatibility with regards to <see cref="libvlc_new" /> arguments.
    ///     We recommend that you do not use them, other than when debugging.
    /// </remarks>
    /// <returns>the libvlc instance or NULL in case of error</returns>
    [LibVlcFunction(nameof(libvlc_new))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void* libvlc_new(int argc, IntPtr argv);

    /// <summary>
    ///     Decrement the reference count of a libvlc instance, and destroy it
    ///     if it reaches zero.
    /// </summary>
    /// <param name="p_instance">the instance to destroy</param>
    [LibVlcFunction(nameof(libvlc_release))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_release(void* p_instance);

    /// <summary>
    ///     Increments the reference count of a libvlc instance.
    ///     The initial reference count is 1 after <see cref="libvlc_new" /> returns.
    /// </summary>
    /// <param name="p_instance">the instance to reference</param>
    [LibVlcFunction(nameof(libvlc_retain))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_retain(void* p_instance);

    /// <summary>
    ///     Try to start a user interface for the libvlc instance.
    /// </summary>
    /// <param name="p_instance">the instance</param>
    /// <param name="name">interface name, or NULL for default</param>
    /// <returns>0 on success, -1 on error.</returns>
    [LibVlcFunction(nameof(libvlc_add_intf))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate int libvlc_add_intf(void* p_instance, byte* name);

    /// <summary>
    ///     Registers a callback for the LibVLC exit event. This is mostly useful if
    ///     the VLC playlist and/or at least one interface are started with
    ///     <see cref="libvlc_playlist_play" /> or <see cref="libvlc_add_intf" /> respectively.
    ///     Typically, this function will wake up your application main loop (from
    ///     another thread).
    /// </summary>
    /// <param name="p_instance">LibVLC instance</param>
    /// <param name="callback">
    ///     callback to invoke when LibVLC wants to exit, or NULL to disable the exit handler (as by
    ///     default)
    /// </param>
    /// <param name="opaque">data pointer for the callback</param>
    /// <remarks>
    ///     This function should be called before the playlist or interface are
    ///     started. Otherwise, there is a small race condition: the exit event could
    ///     be raised before the handler is registered.
    /// </remarks>
    [LibVlcFunction(nameof(libvlc_set_exit_handler))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_set_exit_handler(void* p_instance, ExitHandler callback, void* opaque);

    /// <summary>
    ///     Sets the application name. LibVLC passes this as the user agent string
    ///     when a protocol requires it.
    /// </summary>
    /// <param name="p_instance">LibVLC instance</param>
    /// <param name="name">human-readable application name, e.g. "FooBar player 1.2.3"</param>
    /// <param name="http">HTTP User Agent, e.g. "FooBar/1.2.3 Python/2.6.0"</param>
    [LibVlcFunction(nameof(libvlc_set_user_agent), "1.1.1")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_set_user_agent(void* p_instance, byte* name, byte* http);

    /// <summary>
    ///     Sets some meta-information about the application.
    /// </summary>
    /// <param name="p_instance">LibVLC instance</param>
    /// <param name="id">Java-style application identifier, e.g. "com.acme.foobar"</param>
    /// <param name="version">application version numbers, e.g. "1.2.3"</param>
    /// <param name="icon">application icon name, e.g. "foobar"</param>
    /// <seealso cref="libvlc_set_user_agent" />
    [LibVlcFunction(nameof(libvlc_set_app_id), "2.1.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_set_app_id(void* p_instance, byte* id, byte* version, byte* icon);

    /// <summary>
    ///     Retrieve libvlc version.
    ///     <para />
    ///     Example: "1.1.0-git The Luggage"
    /// </summary>
    /// <returns>a string containing the libvlc version</returns>
    [LibVlcFunction(nameof(libvlc_get_version))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate byte* libvlc_get_version();

    /// <summary>
    ///     Retrieve libvlc compiler version.
    ///     <para />
    ///     Example: "gcc version 4.2.3 (Ubuntu 4.2.3-2ubuntu6)"
    /// </summary>
    /// <returns>a string containing the libvlc compiler version</returns>
    [LibVlcFunction(nameof(libvlc_get_compiler))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate byte* libvlc_get_compiler();

    /// <summary>
    ///     Retrieve libvlc changeset.
    ///     <para />
    ///     Example: "aa9bce0bc4"
    /// </summary>
    /// <returns>a string containing the libvlc changeset</returns>
    [LibVlcFunction(nameof(libvlc_get_changeset))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate byte* libvlc_get_changeset();

    /// <summary>
    ///     Frees an heap allocation returned by a LibVLC function.
    ///     If you know you're using the same underlying C run-time as the LibVLC
    ///     implementation, then you can call ANSI C free() directly instead.
    /// </summary>
    /// <param name="ptr">the pointer</param>
    [LibVlcFunction(nameof(libvlc_free))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_free(void* ptr);

    /**
     * LibVLC emits asynchronous events.
     *
     * Several LibVLC objects (such @ref libvlc_instance_t as
     * @ref libvlc_media_player_t) generate events asynchronously. Each of them
     * provides @ref libvlc_event_manager_t event manager. You can subscribe to
     * events with libvlc_event_attach() and unsubscribe with
     * libvlc_event_detach().
     */
    namespace Event
    {
        /// <summary>
        ///     Register for an event notification.
        /// </summary>
        /// <param name="p_event_manager">
        ///     the event manager to which you want to attach to.
        ///     Generally it is obtained by vlc_my_object_event_manager() where
        ///     my_object is the object you want to listen to.
        /// </param>
        /// <param name="i_event_type">the desired event to which we want to listen</param>
        /// <param name="f_callback">the function to call when i_event_type occurs</param>
        /// <param name="user_data">user provided data to carry with the event</param>
        /// <returns>0 on success, ENOMEM on error</returns>
        [LibVlcFunction(nameof(libvlc_event_attach))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate int libvlc_event_attach(void* p_event_manager,
            libvlc_event_e i_event_type,
            libvlc_callback_t f_callback,
            void* user_data);

        /// <summary>
        ///     Unregister an event notification.
        /// </summary>
        /// <param name="p_event_manager">the event manager</param>
        /// <param name="i_event_type">the desired event to which we want to unregister</param>
        /// <param name="f_callback">the function to call when i_event_type occurs</param>
        /// <param name="p_user_data">user provided data to carry with the event</param>
        [LibVlcFunction(nameof(libvlc_event_detach))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_event_detach(void* p_event_manager,
            libvlc_event_e i_event_type,
            libvlc_callback_t f_callback,
            void* p_user_data);

        /// <summary>
        ///     Get an event's type name.
        /// </summary>
        /// <param name="event_type">the desired event</param>
        /// <returns></returns>
        [LibVlcFunction(nameof(libvlc_event_type_name))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate byte* libvlc_event_type_name(libvlc_event_e event_type);
    }

    /**
     * libvlc_log_* functions provide access to the LibVLC messages log.
     * This is used for logging and debugging.
     */
    namespace Logging
    {
        /**
         * Gets log message debug infos.
         *
         * This function retrieves self-debug information about a log message:
         * - the name of the VLC module emitting the message,
         * - the name of the source code module (i.e. file) and
         * - the line number within the source code module.
         *
         * The returned module name and file name will be NULL if unknown.
         * The returned line number will similarly be zero if unknown.
         *
         * \param ctx message context (as passed to the @ref libvlc_log_cb callback)
         * \param module module name storage (or NULL) [OUT]
         * \param file source code file name storage (or NULL) [OUT]
         * \param line source code file line number storage (or NULL) [OUT]
         * \warning The returned module name and source code file name, if non-NULL,
         * are only valid until the logging callback returns.
         *
         * \version LibVLC 2.1.0 or later
         */
        [LibVlcFunction(nameof(libvlc_log_get_context), "2.1.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_log_get_context(void* ctx,
            byte** module, byte** file, uint* line);

        /**
         * Gets log message info.
         *
         * This function retrieves meta-information about a log message:
         * - the type name of the VLC object emitting the message,
         * - the object header if any, and
         * - a temporaly-unique object identifier.
         *
         * This information is mainly meant for <b>manual</b> troubleshooting.
         *
         * The returned type name may be "generic" if unknown, but it cannot be NULL.
         * The returned header will be NULL if unset; in current versions, the header
         * is used to distinguish for VLM inputs.
         * The returned object ID will be zero if the message is not associated with
         * any VLC object.
         *
         * \param ctx message context (as passed to the @ref libvlc_log_cb callback)
         * \param name object name storage (or NULL) [OUT]
         * \param header object header (or NULL) [OUT]
         * \param line source code file line number storage (or NULL) [OUT]
         * \warning The returned module name and source code file name, if non-NULL,
         * are only valid until the logging callback returns.
         *
         * \version LibVLC 2.1.0 or later
         */
        [LibVlcFunction(nameof(libvlc_log_get_object), "2.1.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_log_get_object(void* ctx,
            byte** name, byte** header, UIntPtr* id);

        /**
         * Unsets the logging callback.
         *
         * This function deregisters the logging callback for a LibVLC instance.
         * This is rarely needed as the callback is implicitly unset when the instance
         * is destroyed.
         *
         * \note This function will wait for any pending callbacks invocation to
         * complete (causing a deadlock if called from within the callback).
         *
         * \param p_instance libvlc instance
         * \version LibVLC 2.1.0 or later
         */
        [LibVlcFunction(nameof(libvlc_log_unset), "2.1.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_log_unset(void* p_instance);

        /**
         * Sets the logging callback for a LibVLC instance.
         *
         * This function is thread-safe: it will wait for any pending callbacks
         * invocation to complete.
         *
         * \param cb callback function pointer
         * \param data opaque data pointer for the callback function
         *
         * \note Some log messages (especially debug) are emitted by LibVLC while
         * is being initialized. These messages cannot be captured with this interface.
         *
         * \warning A deadlock may occur if this function is called from the callback.
         *
         * \param p_instance libvlc instance
         * \version LibVLC 2.1.0 or later
         */
        //[LibVlcFunction(nameof(libvlc_log_set), "2.1.0")]
        //[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        //public unsafe delegate void libvlc_log_set(void* p_instance,
        //                                libvlc_log_cb cb, void* data);


        /**
         * Sets up logging to a file.
         * \param p_instance libvlc instance
         * \param stream FILE pointer opened for writing
         *         (the FILE pointer must remain valid until libvlc_log_unset())
         * \version LibVLC 2.1.0 or later
         */
        [LibVlcFunction(nameof(libvlc_log_set_file), "2.1.0")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public unsafe delegate void libvlc_log_set_file(void* p_instance, void* stream);
    }

    /// <summary>
    ///     Release a list of module descriptions.
    /// </summary>
    /// <param name="p_list">the list to be released</param>
    [LibVlcFunction(nameof(libvlc_module_description_list_release))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void libvlc_module_description_list_release(libvlc_module_description_t* p_list);

    /// <summary>
    ///     Returns a list of audio filters that are available.
    /// </summary>
    /// <param name="p_instance">libvlc instance</param>
    /// <returns>
    ///     a list of module descriptions. It should be freed with <see cref="libvlc_module_description_list_release" />.
    ///     In case of an error, NULL is returned.
    /// </returns>
    /// <seealso cref="libvlc_module_description_t" />
    /// <seealso cref="libvlc_module_description_list_release" />
    [LibVlcFunction(nameof(libvlc_audio_filter_list_get))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate libvlc_module_description_t* libvlc_audio_filter_list_get(void* p_instance);

    /// <summary>
    ///     Returns a list of video filters that are available.
    /// </summary>
    /// <param name="p_instance">libvlc instance</param>
    /// <returns>
    ///     a list of module descriptions. It should be freed with libvlc_module_description_list_release().
    ///     In case of an error, NULL is returned.
    /// </returns>
    /// <seealso cref="libvlc_module_description_t" />
    /// <seealso cref="libvlc_module_description_list_release" />
    [LibVlcFunction(nameof(libvlc_video_filter_list_get))]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate libvlc_module_description_t* libvlc_video_filter_list_get(void* p_instance);

    /**
     * These functions provide access to the LibVLC time/clock.
     */
    namespace Time
    {
        /// <summary>
        ///     Return the current time as defined by LibVLC. The unit is the microsecond.
        ///     Time increases monotonically (regardless of time zone changes and RTC
        ///     adjustements).
        ///     <para />
        ///     The origin is arbitrary but consistent across the whole system
        ///     (e.g. the system uptim, the time since the system was booted).
        /// </summary>
        /// <remarks>On systems that support it, the POSIX monotonic clock is used.</remarks>
        /// <returns></returns>
        [LibVlcFunction(nameof(libvlc_clock))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate long libvlc_clock();
    }
}