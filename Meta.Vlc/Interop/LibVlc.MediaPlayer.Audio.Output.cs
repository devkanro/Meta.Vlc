// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: LibVlc.MediaPlayer.Audio.Output.cs
// Version: 20160214

using System;
using System.Runtime.InteropServices;

namespace Meta.Vlc.Interop.MediaPlayer
{
    /// <summary>
    ///     Description for audio output device.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AudioDevice
    {
        /// <summary>
        ///     Next entry in list.
        /// </summary>
        public IntPtr Next;

        /// <summary>
        ///     Device identifier string.
        /// </summary>
        public IntPtr Device;

        /// <summary>
        ///     User-friendly device description.
        /// </summary>
        public IntPtr Description;
    }

    /// <summary>
    ///     Description for audio output.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct AudioOutput
    {
        public IntPtr Name;

        public IntPtr Description;

        public IntPtr Next;
    }

    /// <summary>
    ///     Gets a list of potential audio output devices.
    /// </summary>
    /// <param name="mediaPlayer">media player</param>
    /// <returns>
    ///     A NULL-terminated linked list of potential audio output devices. It must be freed with
    ///     <see cref="ReleaseAudioDeviceList" />
    /// </returns>
    [LibVlcFunction("libvlc_audio_output_device_enum", "2.2.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr EnumAudioDeviceList(IntPtr mediaPlayer);

    /// <summary>
    ///     Frees a list of available audio output devices.
    /// </summary>
    /// <param name="audioDeviceList">list with audio outputs for release </param>
    [LibVlcFunction("libvlc_audio_output_device_list_release", "2.1.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ReleaseAudioDeviceList(IntPtr audioDeviceList);

    /// <summary>
    ///     Gets a list of audio output devices for a given audio output module.
    /// </summary>
    /// <param name="instance">libvlc instance</param>
    /// <param name="audioOutput">audio output name (as returned by <see cref="GetAudioOutputList" />)</param>
    /// <returns>
    ///     A NULL-terminated linked list of potential audio output devices. It must be freed with
    ///     <see cref="ReleaseAudioDeviceList" />
    /// </returns>
    [LibVlcFunction("libvlc_audio_output_device_list_get", "2.1.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GetAudioDeviceList(IntPtr instance, IntPtr audioOutput);

    /// <summary>
    ///     Gets the list of available audio output modules.
    /// </summary>
    /// <param name="instance">libvlc instance</param>
    /// <returns>list of available audio outputs. It must be freed with  <see cref="ReleaseAudioOutputList" /></returns>
    [LibVlcFunction("libvlc_audio_output_list_get")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GetAudioOutputList(IntPtr instance);

    /// <summary>
    ///     Frees the list of available audio output modules.
    /// </summary>
    /// <param name="audioOutputList">list with audio outputs for release </param>
    [LibVlcFunction("libvlc_audio_output_list_release")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ReleaseAudioOutputList(IntPtr audioOutputList);

    /// <summary>
    ///     Selects an audio output module.
    ///     Any change will take be effect only after playback is stopped and restarted. Audio output cannot be changed while
    ///     playing.
    /// </summary>
    /// <param name="mediaPlayer">media player </param>
    /// <param name="name">name of audio output, use <see cref="AudioOutput.Name" /></param>
    /// <returns>0 if function succeded, -1 on error </returns>
    [LibVlcFunction("libvlc_audio_output_set")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SetAudioOutput(IntPtr mediaPlayer, IntPtr name);

    /// <summary>
    ///     Configures an explicit audio output device. If the module paramater is NULL,
    ///     audio output will be moved to the device specified by the device identifier string immediately.
    ///     This is the recommended usage. A list of adequate potential device strings can be obtained with
    ///     <see cref="EnumAudioDeviceList" />.
    ///     However passing NULL is supported in LibVLC version 2.2.0 and later only; in earlier versions, this function would
    ///     have no effects when the module parameter was NULL.
    ///     If the module parameter is not NULL, the device parameter of the corresponding audio output, if it exists, will be
    ///     set to the specified string.
    ///     Note that some audio output modules do not have such a parameter (notably MMDevice and PulseAudio).
    ///     A list of adequate potential device strings can be obtained with <see cref="GetAudioDeviceList" />.
    /// </summary>
    /// <param name="mediaPlayer">media player </param>
    /// <param name="module">
    ///     If NULL, current audio output module. if non-NULL, name of audio output module (
    ///     <see cref="AudioOutput.Name" />)
    /// </param>
    /// <param name="deviceId">device identifier string </param>
    /// <returns>Nothing. Errors are ignored (this is a design issue).</returns>
    [LibVlcFunction("libvlc_audio_output_device_set")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetAudioDevice(IntPtr mediaPlayer, IntPtr module, IntPtr deviceId);

    /// <summary>
    ///     Get the current audio output device identifier.
    /// </summary>
    /// <param name="mediaPlayer">media player </param>
    /// <returns>
    ///     the current audio output device identifier NULL if no device is selected or in case of error (the result must
    ///     be released with <see cref="Free" />).
    /// </returns>
    [LibVlcFunction("libvlc_audio_output_device_get", "3.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GetAudioDevice(IntPtr mediaPlayer);
}