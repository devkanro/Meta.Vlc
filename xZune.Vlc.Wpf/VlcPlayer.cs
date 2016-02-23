// Project: xZune.Vlc (https://github.com/higankanshi/xZune.Vlc)
// Filename: VlcPlayer.cs
// Version: 20160214

using System;
using System.ComponentModel;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using xZune.Vlc.Interop.MediaPlayer;
using xZune.Vlc.Wpf.Annotations;
using MediaState = xZune.Vlc.Interop.Media.MediaState;

namespace xZune.Vlc.Wpf
{
    /// <summary>
    ///     VLC media player.
    /// </summary>
    public partial class VlcPlayer : Control, IDisposable, INotifyPropertyChanged
    {
        #region --- Fields ---

        //TODO: maybe make all fields private or protected (for descendant classes to access)?

        private VideoLockCallback _lockCallback;
        private VideoUnlockCallback _unlockCallback;
        private VideoDisplayCallback _displayCallback;
        private VideoFormatCallback _formatCallback;
        private VideoCleanupCallback _cleanupCallback;
        //private AudioPlayCallback _audioPlayCallback;
        //private AudioPauseCallback _audioPauseCallback;
        //private AudioResumeCallback _audioResumeCallback;
        //private AudioFlushCallback _audioFlushCallback;
        //private AudioDrainCallback _audioDrainCallback;
        //private AudioSetupCallback _audioSetupCallback;
        //private AudioCleanupCallback _audioCleanupCallback;
        //private AudioSetVolumeCallback _audioSetVolumeCallback;

        private GCHandle _lockCallbackHandle;
        private GCHandle _unlockCallbackHandle;
        private GCHandle _displayCallbackHandle;
        private GCHandle _formatCallbackHandle;
        private GCHandle _cleanupCallbackHandle;
        //private GCHandle _audioPlayCallbackHandle;
        //private GCHandle _audioPauseCallbackHandle;
        //private GCHandle _audioResumeCallbackHandle;
        //private GCHandle _audioFlushCallbackHandle;
        //private GCHandle _audioDrainCallbackHandle;
        //private GCHandle _audioSetupCallbackHandle;
        //private GCHandle _audioCleanupCallbackHandle;
        //private GCHandle _audioSetVolumeCallbackHandle;

        //TakeSnapshot//
        private SnapshotContext _snapshotContext;

        private VideoDisplayContext _context;
        private int _checkCount;
        private bool _isDVD;
        private bool _isStopping;

        //Dispose//
        private bool _disposed;
        private bool _disposing;

        #endregion --- Fields ---

        #region --- Initialization ---

        static VlcPlayer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (VlcPlayer),
                new FrameworkPropertyMetadata(typeof (VlcPlayer)));
        }

        protected override void OnInitialized(EventArgs e)
        {
            ScaleTransform = new ScaleTransform(1, 1);
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                String libVlcPath = null;
                String[] libVlcOption = null;

                var vlcSettings =
                    Assembly.GetEntryAssembly()
                        .GetCustomAttributes(typeof (VlcSettingsAttribute), false);

                if (vlcSettings.Length > 0)
                {
                    var vlcSettingsAttribute = vlcSettings[0] as VlcSettingsAttribute;

                    if (vlcSettingsAttribute != null && vlcSettingsAttribute.LibVlcPath != null)
                    {
                        libVlcPath = Path.IsPathRooted(vlcSettingsAttribute.LibVlcPath)
                            ? vlcSettingsAttribute.LibVlcPath
                            : Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)
                                .CombinePath(vlcSettingsAttribute.LibVlcPath);
                    }

                    if (vlcSettingsAttribute != null && vlcSettingsAttribute.VlcOption != null)
                        libVlcOption = vlcSettingsAttribute.VlcOption;
                }

                if (LibVlcPath != null)
                    libVlcPath = Path.IsPathRooted(LibVlcPath)
                        ? LibVlcPath
                        : Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)
                            .CombinePath(LibVlcPath);

                if (VlcOption != null)
                    libVlcOption = VlcOption;

                if (libVlcPath != null)
                    Initialize(libVlcPath, libVlcOption);
            }
            base.OnInitialized(e);
        }

        /// <summary>
        ///     Initialize VLC player with path of LibVlc.
        /// </summary>
        /// <param name="libVlcPath"></param>
        public void Initialize(String libVlcPath)
        {
            Initialize(libVlcPath, "-I", "dummy", "--ignore-config", "--no-video-title", "--file-logging",
                "--logfile=log.txt", "--verbose=2", "--no-sub-autodetect-file");
        }

        /// <summary>
        ///     Initialize VLC player with path of LibVlc and options.
        /// </summary>
        /// <param name="libVlcPath"></param>
        /// <param name="libVlcOption"></param>
        public void Initialize(String libVlcPath, params String[] libVlcOption)
        {
            if (!ApiManager.IsInitialized)
            {
                ApiManager.Initialize(libVlcPath, libVlcOption);
            }

            switch (CreateMode)
            {
                case PlayerCreateMode.NewVlcInstance:
                    var vlc = new Vlc(libVlcOption);
                    VlcMediaPlayer = vlc.CreateMediaPlayer();
                    ApiManager.Vlcs.Add(vlc);
                    break;

                case PlayerCreateMode.Default:
                default:
                    VlcMediaPlayer = ApiManager.DefaultVlc.CreateMediaPlayer();
                    break;
            }

            if (VlcMediaPlayer != null)
            {
                VlcMediaPlayer.PositionChanged += VlcMediaPlayerPositionChanged;
                VlcMediaPlayer.TimeChanged += VlcMediaPlayerTimeChanged;
                VlcMediaPlayer.Playing += VlcMediaPlayerStateChanged;
                VlcMediaPlayer.Paused += VlcMediaPlayerStateChanged;
                VlcMediaPlayer.Stoped += VlcMediaPlayerStateChanged;
                VlcMediaPlayer.Opening += VlcMediaPlayerStateChanged;
                VlcMediaPlayer.Buffering += VlcMediaPlayerStateChanged;
                VlcMediaPlayer.EndReached += VlcMediaPlayerStateChanged;
                VlcMediaPlayer.SeekableChanged += VlcMediaPlayerSeekableChanged;
                VlcMediaPlayer.LengthChanged += VlcMediaPlayerLengthChanged;

                _lockCallback = VideoLockCallback;
                _unlockCallback = VideoUnlockCallback;
                _displayCallback = VideoDisplayCallback;
                _formatCallback = VideoFormatCallback;
                _cleanupCallback = VideoCleanupCallback;
                //_audioSetupCallback = AudioFormatSetupCallback;
                //_audioCleanupCallback = AudioFormatCleanupCallback;
                //_audioPlayCallback = AudioPlayCallback;
                //_audioPauseCallback = AudioPauseCallback;
                //_audioResumeCallback = AudioResumeCallback;
                //_audioFlushCallback = AudioFlushCallback;
                //_audioDrainCallback = AudioDrainCallback;
                //_audioSetVolumeCallback = AudioSetVolumeCallback;

                _lockCallbackHandle = GCHandle.Alloc(_lockCallback);
                _unlockCallbackHandle = GCHandle.Alloc(_unlockCallback);
                _displayCallbackHandle = GCHandle.Alloc(_displayCallback);
                _formatCallbackHandle = GCHandle.Alloc(_formatCallback);
                _cleanupCallbackHandle = GCHandle.Alloc(_cleanupCallback);
                //_audioSetupCallbackHandle = GCHandle.Alloc(_audioSetupCallback);
                //_audioCleanupCallbackHandle = GCHandle.Alloc(_audioCleanupCallback);
                //_audioPlayCallbackHandle = GCHandle.Alloc(_audioPlayCallback);
                //_audioPauseCallbackHandle = GCHandle.Alloc(_audioPauseCallback);
                //_audioResumeCallbackHandle = GCHandle.Alloc(_audioResumeCallback);
                //_audioFlushCallbackHandle = GCHandle.Alloc(_audioFlushCallback);
                //_audioDrainCallbackHandle = GCHandle.Alloc(_audioDrainCallback);
                //_audioSetVolumeCallbackHandle = GCHandle.Alloc(_audioSetVolumeCallback);

                VlcMediaPlayer.SetVideoDecodeCallback(_lockCallback, _unlockCallback, _displayCallback, IntPtr.Zero);
                VlcMediaPlayer.SetVideoFormatCallback(_formatCallback, _cleanupCallback);
                //VlcMediaPlayer.SetAudioCallback(_audioPlayCallback, _audioPauseCallback, _audioResumeCallback, _audioFlushCallback, _audioDrainCallback);
                //VlcMediaPlayer.SetAudioFormatCallback(_audioSetupCallback, _audioCleanupCallback);
                //VlcMediaPlayer.SetAudioVolumeCallback(_audioSetVolumeCallback);
            }
        }

        #endregion --- Initialization ---

        #region --- Cleanup ---

        /// <summary>
        ///     Cleanup the player used resource.
        /// </summary>
        /// <param name="disposing"></param>
        protected void Dispose(bool disposing)
        {
            if (_disposed || disposing)
            {
                return;
            }

            _disposing = true;

            BeginStop(() =>
            {
                if (VlcMediaPlayer != null)
                {
                    if (VlcMediaPlayer.Media != null)
                    {
                        VlcMediaPlayer.Media.Dispose();
                    }
                    VlcMediaPlayer.Dispose();
                }

                _lockCallbackHandle.Free();
                _unlockCallbackHandle.Free();
                _displayCallbackHandle.Free();
                _formatCallbackHandle.Free();
                _cleanupCallbackHandle.Free();
                //_audioSetupCallbackHandle.Free();
                //_audioCleanupCallbackHandle.Free();
                //_audioPlayCallbackHandle.Free();
                //_audioPauseCallbackHandle.Free();
                //_audioResumeCallbackHandle.Free();
                //_audioFlushCallbackHandle.Free();
                //_audioDrainCallbackHandle.Free();
                //_audioSetVolumeCallbackHandle.Free();

                _disposed = true;
                _disposing = false;
            });
        }

        /// <summary>
        ///     Cleanup the player used resource.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion --- Cleanup ---

        #region --- Methods ---

        #region LoadMedia

        //note: if you pass a string instead of a Uri, LoadMedia will see if it is an absolute Uri, else will treat it as a file path
        /// <summary>
        ///     Load a media by file path.
        /// </summary>
        /// <param name="path"></param>
        public void LoadMedia(String path)
        {
            Uri uri;
            if (Uri.TryCreate(path, UriKind.Absolute, out uri))
            {
                LoadMedia(uri);
                return;
            }

            if (!(File.Exists(path) || Path.GetFullPath(path).IsDriveRootDirectory()))
                throw new FileNotFoundException(String.Format("Not found: {0}", path), path);

            if (VlcMediaPlayer == null) return;

            if (VlcMediaPlayer.Media != null)
                VlcMediaPlayer.Media.Dispose();

            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }

            VlcMediaPlayer.Media = VlcMediaPlayer.VlcInstance.CreateMediaFromPath(path);
            VlcMediaPlayer.Media.ParseAsync();
            _isDVD = VlcMediaPlayer.Media.Mrl.IsDriveRootDirectory();
        }

        /// <summary>
        ///     Load a media by uri.
        /// </summary>
        /// <param name="uri"></param>
        public void LoadMedia(Uri uri)
        {
            if (VlcMediaPlayer == null) return;

            if (VlcMediaPlayer.Media != null) VlcMediaPlayer.Media.Dispose();

            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }

            VlcMediaPlayer.Media = VlcMediaPlayer.VlcInstance.CreateMediaFromLocation(uri.ToString());
            VlcMediaPlayer.Media.ParseAsync();
            _isDVD = VlcMediaPlayer.Media.Mrl.IsDriveRootDirectory();
        }

        /// <summary>
        ///     Load a media by file path and options.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="options"></param>
        public void LoadMediaWithOptions(String path, params String[] options)
        {
            if (!(File.Exists(path) || Path.GetFullPath(path).IsDriveRootDirectory()))
                throw new FileNotFoundException(String.Format("Not found: {0}", path), path);

            if (VlcMediaPlayer == null) return;

            if (VlcMediaPlayer.Media != null)
                VlcMediaPlayer.Media.Dispose();

            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }

            VlcMediaPlayer.Media = VlcMediaPlayer.VlcInstance.CreateMediaFromPath(path);
            VlcMediaPlayer.Media.AddOption(options);
            VlcMediaPlayer.Media.ParseAsync();
            _isDVD = VlcMediaPlayer.Media.Mrl.IsDriveRootDirectory();
        }

        /// <summary>
        ///     Load a media by uri and options.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="options"></param>
        public void LoadMediaWithOptions(Uri uri, params String[] options)
        {
            if (VlcMediaPlayer == null) return;

            if (VlcMediaPlayer.Media != null)
                VlcMediaPlayer.Media.Dispose();

            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }

            VlcMediaPlayer.Media = VlcMediaPlayer.VlcInstance.CreateMediaFromLocation(uri.ToString());
            VlcMediaPlayer.Media.AddOption(options);
            VlcMediaPlayer.Media.ParseAsync();
            _isDVD = VlcMediaPlayer.Media.Mrl.IsDriveRootDirectory();
        }

        #endregion LoadMedia

        #region Play/Pause

        /// <summary>
        ///     Play media.
        /// </summary>
        public void Play()
        {
            if (VlcMediaPlayer == null) return;

            if (_context != null)
            {
                VideoSource = _context.Image;
            }

            VlcMediaPlayer.Play();
        }

        /// <summary>
        ///     Pause or resume media.
        /// </summary>
        public void PauseOrResume()
        {
            if (VlcMediaPlayer != null)
                VlcMediaPlayer.PauseOrResume();
        }

        #endregion Play/Pause

        #region Stop

        /// <summary>
        ///     Stop media, this method can't be called on UI thread, you must async call it.
        /// </summary>
        public void Stop()
        {
            _isStopping = true;
            VlcMediaPlayer.Stop();
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => { VideoSource = null; }));
            _isStopping = false;
        }

        /// <summary>
        ///     Easy aync call stop method.
        /// </summary>
        /// <param name="callback"></param>
        public void BeginStop(Action callback = null)
        {
            Action action = () =>
            {
                Stop();
                if (callback != null)
                {
                    callback();
                }
            };
            action.EasyInvoke();
        }

        #endregion Stop

        /// <summary>
        ///     Add options to media.
        /// </summary>
        /// <param name="option"></param>
        public void AddOption(params String[] option)
        {
            if ((VlcMediaPlayer != null) && VlcMediaPlayer.Media != null)
                VlcMediaPlayer.Media.AddOption(option);
        }

        /// <summary>
        ///     Show next frame.
        /// </summary>
        public void NextFrame()
        {
            if (VlcMediaPlayer != null)
                VlcMediaPlayer.NextFrame();
        }

        /// <summary>
        ///     Inactive with DVD menu.
        /// </summary>
        /// <param name="mode"></param>
        public void Navigate(NavigateMode mode)
        {
            if (VlcMediaPlayer != null)
                VlcMediaPlayer.Navigate(mode);
        }

        /// <summary>
        ///     Toggle mute mode.
        /// </summary>
        public void ToggleMute()
        {
            if (VlcMediaPlayer != null)
                VlcMediaPlayer.ToggleMute();
        }

        /// <summary>
        ///     Take a snapshot.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="format"></param>
        /// <param name="quality"></param>
        public void TakeSnapshot(String path, SnapshotFormat format, int quality)
        {
            if (VlcMediaPlayer != null)
                switch (VlcMediaPlayer.State)
                {
                    case MediaState.NothingSpecial:
                    case MediaState.Opening:
                    case MediaState.Buffering:
                    case MediaState.Stopped:
                    case MediaState.Ended:
                    case MediaState.Error:
                        break;

                    case MediaState.Playing:
                    case MediaState.Paused:
                        _snapshotContext = new SnapshotContext(path, format, quality);
                        break;
                }
        }

        /// <summary>
        ///     Gets a list of potential audio output devices.
        /// </summary>
        /// <returns></returns>
        public AudioDeviceList EnumAudioDeviceList()
        {
            return VlcMediaPlayer.EnumAudioDeviceList();
        }

        /// <summary>
        ///     Gets a list of audio output devices for a given audio output module.
        /// </summary>
        /// <param name="audioOutput"></param>
        /// <returns></returns>
        public AudioDeviceList GetAudioDeviceList(AudioOutput audioOutput)
        {
            return VlcMediaPlayer.GetAudioDeviceList(audioOutput);
        }

        /// <summary>
        ///     Gets the list of available audio output modules.
        /// </summary>
        /// <returns></returns>
        public AudioOutputList GetAudioOutputList()
        {
            return VlcMediaPlayer.GetAudioOutputList();
        }

        /// <summary>
        ///     Selects an audio output module.
        ///     Any change will take be effect only after playback is stopped and restarted. Audio output cannot be changed while
        ///     playing.
        /// </summary>
        /// <param name="audioOutput"></param>
        /// <returns></returns>
        public bool SetAudioOutput(AudioOutput audioOutput)
        {
            return VlcMediaPlayer.SetAudioOutput(audioOutput);
        }

        /// <summary>
        ///     Get the current audio output device identifier.
        /// </summary>
        public String GetAudioDevice()
        {
            return VlcMediaPlayer.GetAudioDevice();
        }

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
        public void SetAudioDevice(AudioOutput audioOutput, AudioDevice audioDevice)
        {
            VlcMediaPlayer.SetAudioDevice(audioOutput, audioDevice);
        }

        #endregion --- Methods ---

        #region --- NotifyPropertyChanged ---

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> expr)
        {
            if (_disposing) return;
            if (_isStopping) return;

            if (PropertyChanged != null)
            {
                var bodyExpr = expr.Body as MemberExpression;
                var propInfo = bodyExpr.Member as PropertyInfo;
                var propName = propInfo.Name;
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        #endregion --- NotifyPropertyChanged ---
    }

    /// <summary>
    ///     VlcPlayer create mode.
    /// </summary>
    public enum PlayerCreateMode
    {
        /// <summary>
        ///     Create a new <see cref="VlcPlayer" /> instance with default <see cref="Vlc" /> instance.
        /// </summary>
        Default,

        /// <summary>
        ///     Create a new <see cref="VlcPlayer" /> instance with a new <see cref="Vlc" /> instance.
        /// </summary>
        NewVlcInstance
    }
}