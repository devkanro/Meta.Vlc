// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: LibVlcManager.cs
// Version: 20181231

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Meta.Vlc.Interop;
using Meta.Vlc.Interop.Core;

namespace Meta.Vlc
{
    /// <summary>
    ///     LibVlc dlls manager, load LibVlc and initialize LibVlc to use. Some public method also in this class, like
    ///     <see cref="Free" /> method.
    /// </summary>
    public static unsafe class LibVlcManager
    {
        private static LibVlcFunction<libvlc_get_version> _getVersionFunction;
        private static LibVlcFunction<libvlc_get_compiler> _getCompilerFunction;
        private static LibVlcFunction<libvlc_get_changeset> _getChangeSetFunction;
        private static LibVlcFunction<libvlc_free> _freeFunction;

        private static readonly Dictionary<Type, Object> _cache =
            new Dictionary<Type, Object>();

        /// <summary>
        ///     LibVlc loaded or not.
        /// </summary>
        public static bool IsLibLoaded { get; private set; }

        /// <summary>
        ///     Handle of libvlc.dll.
        /// </summary>
        public static IntPtr LibVlcHandle { get; private set; }

        /// <summary>
        ///     Handle of libvlccore.dll.
        /// </summary>
        public static IntPtr LibVlcVCoreHandle { get; private set; }

        /// <summary>
        ///     Directory of LibVlc dlls.
        /// </summary>
        public static string LibVlcDirectory { get; set; }

        /// <summary>
        ///     Version infomation of LibVlc.
        /// </summary>
        public static LibVlcVersion LibVlcVersion { get; set; }

        /// <summary>
        ///     Load LibVlc dlls, and mapping all function.
        /// </summary>
        /// <param name="libVlcDirectory">directory of LibVlc</param>
        /// <exception cref="LibVlcLoadLibraryException">
        ///     Can't load LibVlc dlls, check the platform and LibVlc target platform
        ///     (should be same, x86 or x64).
        /// </exception>
        /// <exception cref="TypeLoadException">A custom attribute type cannot be loaded. </exception>
        /// <exception cref="NoLibVlcFunctionAttributeException">
        ///     For LibVlcFunction, need LibVlcFunctionAttribute to get Infomation
        ///     of function.
        /// </exception>
        /// <exception cref="FunctionNotFoundException">Can't find function in dll.</exception>
        /// <exception cref="VersionStringParseException">Can't parse libvlc version string, it must like "2.2.0-Meta Weatherwax".</exception>
        /// <exception cref="OverflowException">
        ///     At least one component of version represents a number greater than
        ///     <see cref="F:System.Int32.MaxValue" />.
        /// </exception>
        public static void LoadLibVlc(string libVlcDirectory = null)
        {
            if (IsLibLoaded) return;

            LibVlcDirectory = libVlcDirectory == null ? "" : libVlcDirectory;

            try
            {
                var libcore = new FileInfo(Path.Combine(LibVlcDirectory, "libvlccore.dll"));
                var libvlc = new FileInfo(Path.Combine(LibVlcDirectory, "libvlc.dll"));
                LibVlcVCoreHandle = Win32Api.LoadLibrary(libcore.FullName);
                LibVlcHandle = Win32Api.LoadLibrary(libvlc.FullName);
            }
            catch (Win32Exception e)
            {
                throw new LibVlcLoadLibraryException(e);
            }

            _getVersionFunction = new LibVlcFunction<libvlc_get_version>(LibVlcHandle, null);
            _getCompilerFunction = new LibVlcFunction<libvlc_get_compiler>(LibVlcHandle, null);
            _getChangeSetFunction = new LibVlcFunction<libvlc_get_changeset>(LibVlcHandle, null);
            _freeFunction = new LibVlcFunction<libvlc_free>(LibVlcHandle, null);


            LibVlcVersion = new LibVlcVersion(GetVersion());
            var functionArgs = new object[] {LibVlcHandle, LibVlcVersion};

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                var attrs = type.GetCustomAttributes(typeof(LibVlcFunctionAttribute), false);
                if (attrs.Length == 0) continue;

                var functionType = typeof(LibVlcFunction<>).MakeGenericType(type);
                var function = Activator.CreateInstance(functionType, functionArgs);
                _cache.Add(type, function);
            }
        }

        /// <summary>
        ///     Get version string of LibVlc.
        /// </summary>
        /// <returns></returns>
        public static string GetVersion()
        {
            return InteropHelper.PtrToString(_getVersionFunction.Delegate());
        }

        /// <summary>
        ///     Get compiler infomation of LibVlc.
        /// </summary>
        /// <returns></returns>
        public static string GetCompiler()
        {
            return InteropHelper.PtrToString(_getCompilerFunction.Delegate());
        }

        /// <summary>
        ///     Get changeset of LibVlc.
        /// </summary>
        /// <returns></returns>
        public static string GetChangeSet()
        {
            return InteropHelper.PtrToString(_getChangeSetFunction.Delegate());
        }

        /// <summary>
        ///     Frees an heap allocation returned by a LibVLC function, like ANSI C free() method.
        /// </summary>
        /// <param name="pointer">the pointer of object to be released </param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public static void Free(void* pointer)
        {
            _freeFunction.Delegate(pointer);
        }

        public static T GetFunctionDelegate<T>() where T : Delegate
        {
            return ((LibVlcFunction<T>)_cache[typeof(T)]).Delegate;
        }
    }
}