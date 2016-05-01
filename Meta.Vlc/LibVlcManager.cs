// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: LibVlcManager.cs
// Version: 20160214

using System;
using System.ComponentModel;
using System.IO;
using Meta.Vlc.Interop;
using Meta.Vlc.Interop.Core;
using Meta.Vlc.Interop.Media;
using Meta.Vlc.Interop.MediaPlayer;

namespace Meta.Vlc
{
    /// <summary>
    ///     LibVlc dlls manager, load LibVlc and initialize LibVlc to use. Some public method also in this class, like
    ///     <see cref="Free" /> method.
    /// </summary>
    public static class LibVlcManager
    {
        private static LibVlcFunction<GetVersion> _getVersionFunction;
        private static LibVlcFunction<GetCompiler> _getCompilerFunction;
        private static LibVlcFunction<GetChangeset> _getChangesetFunction;
        private static LibVlcFunction<Free> _freeFunction;
        private static LibVlcFunction<ReleaseLibVlcModuleDescription> _releaseLibVlcModuleDescriptionFunction;
        private static LibVlcFunction<ReleaseTrackDescription> _releaseTrackDescriptionFunction;
        private static LibVlcFunction<ReleaseAudioDeviceList> _releaseAudioDeviceListFunction;
        private static LibVlcFunction<ReleaseAudioOutputList> _releaseAudioOutputListFunction;
        private static LibVlcFunction<ReleaseTracks> _releaseTracksFunction;

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
        public static String LibVlcDirectory { get; set; }

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
        public static void LoadLibVlc(String libVlcDirectory = null)
        {
            LibVlcDirectory = libVlcDirectory == null ? "" : libVlcDirectory;

            if (IsLibLoaded) return;

            try
            {
                FileInfo libcore = new FileInfo(Path.Combine(LibVlcDirectory, "libvlccore.dll"));
                FileInfo libvlc = new FileInfo(Path.Combine(LibVlcDirectory, "libvlc.dll"));
                LibVlcVCoreHandle = Win32Api.LoadLibrary(libcore.FullName);
                LibVlcHandle = Win32Api.LoadLibrary(libvlc.FullName);
            }
            catch (Win32Exception e)
            {
                throw new LibVlcLoadLibraryException(e);
            }

            _getVersionFunction = new LibVlcFunction<GetVersion>();
            LibVlcVersion = new LibVlcVersion(GetVersion());

            _getCompilerFunction = new LibVlcFunction<GetCompiler>();
            _getChangesetFunction = new LibVlcFunction<GetChangeset>();
            _freeFunction = new LibVlcFunction<Free>();
            _releaseLibVlcModuleDescriptionFunction = new LibVlcFunction<ReleaseLibVlcModuleDescription>();
            _releaseAudioOutputListFunction = new LibVlcFunction<ReleaseAudioOutputList>();
            _releaseAudioDeviceListFunction = new LibVlcFunction<ReleaseAudioDeviceList>();
            _releaseTrackDescriptionFunction = new LibVlcFunction<ReleaseTrackDescription>();
            _releaseTracksFunction = new LibVlcFunction<ReleaseTracks>();

            Vlc.LoadLibVlc();
            VlcError.LoadLibVlc();
            VlcEventManager.LoadLibVlc();
            VlcMedia.LoadLibVlc();
            VlcMediaPlayer.LoadLibVlc();
            AudioEqualizer.LoadLibVlc();
        }

        /// <summary>
        ///     Get version string of LibVlc.
        /// </summary>
        /// <returns></returns>
        public static String GetVersion()
        {
            return InteropHelper.PtrToString(_getVersionFunction.Delegate());
        }

        /// <summary>
        ///     Get compiler infomation of LibVlc.
        /// </summary>
        /// <returns></returns>
        public static String GetCompiler()
        {
            return InteropHelper.PtrToString(_getCompilerFunction.Delegate());
        }

        /// <summary>
        ///     Get changeset of LibVlc.
        /// </summary>
        /// <returns></returns>
        public static String GetChangeset()
        {
            return InteropHelper.PtrToString(_getChangesetFunction.Delegate());
        }

        /// <summary>
        ///     Frees an heap allocation returned by a LibVLC function, like ANSI C free() method.
        /// </summary>
        /// <param name="pointer">the pointer of object to be released </param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public static void Free(IntPtr pointer)
        {
            _freeFunction.Delegate(pointer);
        }

        /// <summary>
        ///     Release a list of module descriptions.
        /// </summary>
        /// <param name="moduleDescriptionList">the list to be released </param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public static void ReleaseModuleDescriptionList(IntPtr moduleDescriptionList)
        {
            _releaseLibVlcModuleDescriptionFunction.Delegate(moduleDescriptionList);
        }

        /// <summary>
        ///     Frees the list of available audio output modules.
        /// </summary>
        /// <param name="pointer">a pointer of first <see cref="Interop.MediaPlayer.AudioOutput" />. </param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public static void ReleaseAudioOutputList(IntPtr pointer)
        {
            _releaseAudioOutputListFunction.Delegate(pointer);
        }

        /// <summary>
        ///     Frees a list of available audio output devices.
        /// </summary>
        /// <param name="pointer">a pointer of first <see cref="Interop.MediaPlayer.AudioDevice" />. </param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public static void ReleaseAudioDeviceList(IntPtr pointer)
        {
            _releaseAudioDeviceListFunction.Delegate(pointer);
        }

        /// <summary>
        ///     Release (free) pointer of <see cref="TrackDescriptionList" />.
        /// </summary>
        /// <param name="pointer"></param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public static void ReleaseTrackDescriptionList(IntPtr pointer)
        {
            _releaseTrackDescriptionFunction.Delegate(pointer);
        }

        /// <summary>
        ///     Release media descriptor's elementary streams description array.
        /// </summary>
        /// <param name="pointer">pointer tracks info array to release </param>
        /// <param name="count">number of elements in the array </param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public static void ReleaseTracks(IntPtr pointer, uint count)
        {
            _releaseTracksFunction.Delegate(pointer, count);
        }
    }
}