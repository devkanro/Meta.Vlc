// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: Callback.cs
// Version: 20181231

using System;
using System.Runtime.InteropServices;

namespace Meta.Vlc.Interop.Media
{
    /// <summary>
    ///     Callback prototype to close a custom bitstream input media.
    /// </summary>
    /// <param name="opaque">private pointer as set by the <see cref="Media.libvlc_media_open_cb" /> callback</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void libvlc_media_close_cb(void* opaque);

    /// <summary>
    ///     Callback prototype to seek a custom bitstream input media.
    /// </summary>
    /// <param name="opaque">private pointer as set by the <see cref="Media.libvlc_media_open_cb" /> callback</param>
    /// <param name="offset">absolute byte offset to seek to</param>
    /// <returns>0 on success, -1 on error.</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int libvlc_media_seek_cb(void* opaque, ulong offset);

    /// <summary>
    ///     Callback prototype to read data from a custom bitstream input media.
    /// </summary>
    /// <param name="opaque">private pointer as set by the <see cref="Media.libvlc_media_open_cb" /> callback</param>
    /// <param name="buf">start address of the buffer to read data into</param>
    /// <param name="len">bytes length of the buffer</param>
    /// <remarks>
    ///     If no data is immediately available, then the callback should sleep.
    ///     <para />
    ///     The application is responsible for avoiding deadlock situations.
    ///     In particular, the callback should return an error if playback is stopped;
    ///     if it does not return, then <see cref="libvlc_media_player_stop" />() will never return.
    /// </remarks>
    /// <returns>strictly positive number of bytes read, 0 on end-of-stream, or -1 on non-recoverable error</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate UIntPtr libvlc_media_read_cb(void* opaque, byte* buf, IntPtr len);

    /// <summary>
    ///     Callback prototype to open a custom bitstream input media.
    ///     <para />
    ///     The same media item can be opened multiple times. Each time, this callback
    ///     is invoked. It should allocate and initialize any instance-specific
    ///     resources, then store them in *datap. The instance resources can be freed
    ///     in the <see cref="Media.libvlc_media_close_cb(void*)" /> callback.
    /// </summary>
    /// <param name="opaque">private pointer as passed to <see cref="Media.libvlc_media_new_callbacks" />()</param>
    /// <param name="datap">storage space for a private data pointer [OUT]</param>
    /// <param name="sizep">byte length of the bitstream or UINT64_MAX if unknown [OUT]</param>
    /// <remarks>For convenience, *datap is initially NULL and *sizep is initially 0.</remarks>
    /// <returns>
    ///     0 on success, non-zero on error. In case of failure, the other
    ///     callbacks will not be invoked and any value stored in *datap and *sizep is
    ///     discarded.
    /// </returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int libvlc_media_open_cb(void* opaque, void** datap, ulong* sizep);
}