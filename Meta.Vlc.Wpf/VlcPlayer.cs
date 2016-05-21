// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: VlcPlayer.cs
// Version: 20160327

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Meta.Vlc.Interop.MediaPlayer;
using Meta.Vlc.Wpf.Annotations;
using MediaState = Meta.Vlc.Interop.Media.MediaState;

namespace Meta.Vlc.Wpf
{
    /// <summary>
    ///     VLC media player.
    /// </summary>
    [TemplatePart(Name = "Image", Type = typeof (ThreadSeparatedImage))]
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
        private VlcMedia _oldMedia;

        //Dispose//
        private bool _disposed;

        private bool _disposing;

        #endregion --- Fields ---

        #region --- Initialization ---

        static VlcPlayer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (VlcPlayer),
                new FrameworkPropertyMetadata(typeof (VlcPlayer)));

            HorizontalContentAlignmentProperty.OverrideMetadata(typeof (VlcPlayer),
                new FrameworkPropertyMetadata(HorizontalAlignment.Stretch, (o, args) =>
                {
                    var @this = o as VlcPlayer;

                    if (@this.DisplayThreadDispatcher != null)
                    {
                        @this.Image.HorizontalContentAlignment = (HorizontalAlignment) args.NewValue;
                    }
                }));

            VerticalContentAlignmentProperty.OverrideMetadata(typeof (VlcPlayer),
                new FrameworkPropertyMetadata(VerticalAlignment.Stretch, (o, args) =>
                {
                    var @this = o as VlcPlayer;

                    if (@this.DisplayThreadDispatcher != null)
                    {
                        @this.Image.VerticalContentAlignment = (VerticalAlignment) args.NewValue;
                    }
                }));
        }

        /// <summary>
        ///     Create a <see cref="VlcPlayer" /> for XAML, you should not use it to create player in your C# code, if you don't
        ///     want to add it to XAML visual three. use <see cref="VlcPlayer(bool)" /> and <see cref="VlcPlayer(Dispatcher)" /> to
        ///     instead.
        /// </summary>
        public VlcPlayer()
        {
        }

        /// <summary>
        ///     Create a <see cref="VlcPlayer" /> for C# code, if you want to display video with
        ///     <see cref="ThreadSeparatedImage" />, please set <see cref="displayWithThreadSeparatedImage" /> to true, player will
        ///     generate <see cref="VideoSource" /> on thread of it.
        /// </summary>
        /// <param name="displayWithThreadSeparatedImage">Do you want to display video with <see cref="ThreadSeparatedImage" />?</param>
        public VlcPlayer(bool displayWithThreadSeparatedImage) : this()
        {
            if (displayWithThreadSeparatedImage)
            {
                _customDisplayThreadDispatcher = ThreadSeparatedImage.CommonDispatcher;
            }
            else
            {
                _customDisplayThreadDispatcher = Application.Current.Dispatcher;
            }
        }

        /// <summary>
        ///     Create a <see cref="VlcPlayer" /> for C# code, use player will generate <see cref="VideoSource" /> on specified
        ///     thread.
        /// </summary>
        /// <param name="customDisplayThreadDispatcher">
        ///     The dispatcher of thread which you want to generate
        ///     <see cref="VideoSource" /> on.
        /// </param>
        public VlcPlayer(Dispatcher customDisplayThreadDispatcher) : this()
        {
            if (customDisplayThreadDispatcher == null)
                customDisplayThreadDispatcher = Application.Current.Dispatcher;

            _customDisplayThreadDispatcher = customDisplayThreadDispatcher;
        }

        protected override void OnInitialized(EventArgs e)
        {
            var designMode = DesignerProperties.GetIsInDesignMode(this);

            string libVlcPath = null;
            var libVlcOption = VlcOption;

            if (LibVlcPath != null)
            {
                libVlcPath = Path.IsPathRooted(LibVlcPath)
                    ? LibVlcPath
                    : Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)
                        .CombinePath(LibVlcPath);
            }

            if (!designMode)
            {
                if (libVlcPath != null)
                {
                    if (libVlcOption == null)
                    {
                        Initialize(libVlcPath);
                    }
                    else
                    {
                        Initialize(libVlcPath, libVlcOption);
                    }
                }
                else
                {
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
                                : Path.GetDirectoryName(
                                    Process.GetCurrentProcess().MainModule.FileName)
                                    .CombinePath(vlcSettingsAttribute.LibVlcPath);
                        }

                        if (vlcSettingsAttribute != null && vlcSettingsAttribute.VlcOption != null)
                            libVlcOption = vlcSettingsAttribute.VlcOption;
                    }

                    Initialize(libVlcPath, libVlcOption);
                }
            }

            if (VisualParent == null)
            {
            }
            else
            {
                Loaded += OnLoaded;
            }

            base.OnInitialized(e);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Image.HorizontalContentAlignment = HorizontalContentAlignment;
            Image.VerticalContentAlignment = VerticalContentAlignment;
            Image.Stretch = Stretch;
            Image.StretchDirection = StretchDirection;
        }

        /// <summary>
        ///     Initialize VLC player with path of LibVlc.
        /// </summary>
        /// <param name="libVlcPath"></param>
        public void Initialize(string libVlcPath)
        {
            Initialize(libVlcPath, "-I", "dummy", "--ignore-config", "--no-video-title", "--file-logging",
                "--logfile=log.txt", "--verbose=2", "--no-sub-autodetect-file");
        }

        /// <summary>
        ///     Initialize VLC player with path of LibVlc and options.
        /// </summary>
        /// <param name="libVlcPath"></param>
        /// <param name="libVlcOption"></param>
        public void Initialize(string libVlcPath, params string[] libVlcOption)
        {
            if (!ApiManager.IsInitialized)
            {
                ApiManager.Initialize(libVlcPath, libVlcOption);
            }

            if (VlcMediaPlayer != null)
            {
                throw new InvalidOperationException("VlcPlayer is been initialized.");
            }

            switch (CreateMode)
            {
                case PlayerCreateMode.NewVlcInstance:
                    Vlc = new Vlc(libVlcOption);
                    ApiManager.Vlcs.Add(Vlc);
                    break;

                case PlayerCreateMode.Default:
                default:
                    Vlc = ApiManager.DefaultVlc;
                    break;
            }

            VlcMediaPlayer player = Vlc.CreateMediaPlayer();

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

            if (player == null)
            {
                throw new Exception("Vlc media player initialize fail.");
            }

            AttachPlayer(player);
        }

        public void RebuildPlayer()
        {
            if (VlcMediaPlayer == null)
            {
                throw new InvalidOperationException("VlcPlayer is not initialize.");
            }
            Stop();

            var newPlayer = Vlc.CreateMediaPlayer();
            
            newPlayer.AudioOutputChannel = VlcMediaPlayer.AudioOutputChannel;
            newPlayer.Brightness = VlcMediaPlayer.Brightness;
            newPlayer.Chapter = VlcMediaPlayer.Chapter;
            newPlayer.Contrast = VlcMediaPlayer.Contrast;
            newPlayer.Gamma = VlcMediaPlayer.Gamma;
            newPlayer.Hue = VlcMediaPlayer.Hue;
            //newPlayer.IsAdjustEnable = VlcMediaPlayer.IsAdjustEnable;
            newPlayer.IsMute = VlcMediaPlayer.IsMute;
            newPlayer.Rate = VlcMediaPlayer.Rate;
            newPlayer.Saturation = VlcMediaPlayer.Saturation;
            newPlayer.Scale = VlcMediaPlayer.Scale;
            newPlayer.Subtitle = VlcMediaPlayer.Subtitle;
            newPlayer.SubtitleDelay = VlcMediaPlayer.SubtitleDelay;
            newPlayer.Volume = VlcMediaPlayer.Volume;

            if (AudioEqualizer != null)
                newPlayer.SetEqualizer(AudioEqualizer);

            AttachPlayer(newPlayer);
        }

        private void AttachPlayer(VlcMediaPlayer mediaPlayer)
        {
            if (VlcMediaPlayer != null)
            {
                VlcMediaPlayer.PositionChanged -= VlcMediaPlayerPositionChanged;
                VlcMediaPlayer.TimeChanged -= VlcMediaPlayerTimeChanged;
                VlcMediaPlayer.EndReached -= VlcMediaPlayerEndReached;
                VlcMediaPlayer.SeekableChanged -= VlcMediaPlayerSeekableChanged;
                VlcMediaPlayer.LengthChanged -= VlcMediaPlayerLengthChanged;
                VlcMediaPlayer.MediaChanged -= VlcMediaPlayerMediaChanged;

                VlcMediaPlayer.SetVideoDecodeCallback(null, null, null, IntPtr.Zero);
                VlcMediaPlayer.SetVideoFormatCallback(null, null);
                //VlcMediaPlayer.SetAudioCallback(null, null, null, null, null);
                //VlcMediaPlayer.SetAudioFormatCallback(null, null);
                //VlcMediaPlayer.SetAudioVolumeCallback(null);

                VlcMediaPlayer.Dispose();
            }

            VlcMediaPlayer = mediaPlayer;

            if (VlcMediaPlayer != null)
            {
                VlcMediaPlayer.PositionChanged += VlcMediaPlayerPositionChanged;
                VlcMediaPlayer.TimeChanged += VlcMediaPlayerTimeChanged;
                VlcMediaPlayer.EndReached += VlcMediaPlayerEndReached;
                VlcMediaPlayer.SeekableChanged += VlcMediaPlayerSeekableChanged;
                VlcMediaPlayer.LengthChanged += VlcMediaPlayerLengthChanged;
                VlcMediaPlayer.MediaChanged += VlcMediaPlayerMediaChanged;

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
            if (_disposed || _disposing)
            {
                return;
            }

            _disposing = true;

            Stop();

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
        public void LoadMedia(string path)
        {
            if (VlcMediaPlayer == null)
                throw new InvalidOperationException("VlcMediaPlayer doesn't have initialize.");

            Uri uri;
            if (Uri.TryCreate(path, UriKind.Absolute, out uri))
            {
                LoadMedia(uri);
                return;
            }

            if (!(File.Exists(path) || Path.GetFullPath(path).IsDriveRootDirectory()))
                throw new FileNotFoundException(string.Format("Not found: {0}", path), path);

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
            if (VlcMediaPlayer == null)
                throw new InvalidOperationException("VlcMediaPlayer doesn't have initialize.");

            if (VlcMediaPlayer.Media != null) VlcMediaPlayer.Media.Dispose();

            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }

            VlcMediaPlayer.Media = VlcMediaPlayer.VlcInstance.CreateMediaFromLocation(uri.ToHttpEncodeString());
            VlcMediaPlayer.Media.ParseAsync();

            _isDVD = VlcMediaPlayer.Media.Mrl.IsDriveRootDirectory();
        }

        /// <summary>
        ///     Load a media by file path and options.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="options"></param>
        public void LoadMediaWithOptions(string path, params string[] options)
        {
            if (VlcMediaPlayer == null)
                throw new InvalidOperationException("VlcMediaPlayer doesn't have initialize.");

            if (!(File.Exists(path) || Path.GetFullPath(path).IsDriveRootDirectory()))
                throw new FileNotFoundException(string.Format("Not found: {0}", path), path);
            
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
        public void LoadMediaWithOptions(Uri uri, params string[] options)
        {
            if (VlcMediaPlayer == null)
                throw new InvalidOperationException("VlcMediaPlayer doesn't have initialize.");

            if (VlcMediaPlayer.Media != null)
                VlcMediaPlayer.Media.Dispose();

            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }

            VlcMediaPlayer.Media = VlcMediaPlayer.VlcInstance.CreateMediaFromLocation(uri.ToHttpEncodeString());
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
            if (VlcMediaPlayer == null)
                throw new InvalidOperationException("VlcMediaPlayer doesn't have initialize.");

            if (_context != null)
            {
                VideoSource = _context.Image;
            }
            VlcMediaPlayer.Play();
        }

        /// <summary>
        ///     Pause media.
        /// </summary>
        public void Pause()
        {
            if (VlcMediaPlayer == null)
                throw new InvalidOperationException("VlcMediaPlayer doesn't have initialize.");

            VlcMediaPlayer.Pause();
        }

        /// <summary>
        ///     Resume media.
        /// </summary>
        public void Resume()
        {
            if (VlcMediaPlayer == null)
                throw new InvalidOperationException("VlcMediaPlayer doesn't have initialize.");

            VlcMediaPlayer.Resume();
        }

        /// <summary>
        ///     Pause or resume media.
        /// </summary>
        public void PauseOrResume()
        {
            if (VlcMediaPlayer == null)
                throw new InvalidOperationException("VlcMediaPlayer doesn't have initialize.");

            VlcMediaPlayer.PauseOrResume();
        }

        /// <summary>
        ///     Replay media.
        /// </summary>
        public void Replay()
        {
            if (VlcMediaPlayer == null)
                throw new InvalidOperationException("VlcMediaPlayer doesn't have initialize.");

            StopInternal();
            VlcMediaPlayer.Play();
        }

        #endregion Play/Pause

        #region Stop

        private void StopInternal()
        {
            _isStopping = true;
            VlcMediaPlayer.Stop();
            _isStopping = false;
        }

        /// <summary>
        ///     Stop media.
        /// </summary>
        public void Stop()
        {
            if (VlcMediaPlayer == null) throw new InvalidOperationException("VlcMediaPlayer doesn't have initialize.");

            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => { VideoSource = null; }));
            StopInternal();
        }

        #endregion Stop

        /// <summary>
        ///     Add options to media.
        /// </summary>
        /// <param name="option"></param>
        public void AddOption(params string[] option)
        {
            if (VlcMediaPlayer == null)
                throw new InvalidOperationException("VlcMediaPlayer doesn't have initialize.");
            
            if(VlcMediaPlayer.Media == null)
                throw new InvalidOperationException("No media be loaded.");

                VlcMediaPlayer.Media.AddOption(option);
        }

        /// <summary>
        ///     Show next frame.
        /// </summary>
        public void NextFrame()
        {

            if (VlcMediaPlayer == null)
                throw new InvalidOperationException("VlcMediaPlayer doesn't have initialize.");

            VlcMediaPlayer.NextFrame();
        }

        /// <summary>
        ///     Inactive with DVD menu.
        /// </summary>
        /// <param name="mode"></param>
        public void Navigate(NavigateMode mode)
        {

            if (VlcMediaPlayer == null)
                throw new InvalidOperationException("VlcMediaPlayer doesn't have initialize.");

            VlcMediaPlayer.Navigate(mode);
        }

        /// <summary>
        ///     Toggle mute mode.
        /// </summary>
        public void ToggleMute()
        {

            if (VlcMediaPlayer == null)
                throw new InvalidOperationException("VlcMediaPlayer doesn't have initialize.");

            VlcMediaPlayer.ToggleMute();
        }

        /// <summary>
        ///     Take a snapshot.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="format"></param>
        /// <param name="quality"></param>
        public void TakeSnapshot(string path, SnapshotFormat format, int quality)
        {

            if (VlcMediaPlayer == null)
                throw new InvalidOperationException("VlcMediaPlayer doesn't have initialize.");

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
            if (VlcMediaPlayer == null)
                throw new InvalidOperationException("VlcMediaPlayer doesn't have initialize.");
            
            return VlcMediaPlayer.EnumAudioDeviceList();
        }

        /// <summary>
        ///     Gets a list of audio output devices for a given audio output module.
        /// </summary>
        /// <param name="audioOutput"></param>
        /// <returns></returns>
        public AudioDeviceList GetAudioDeviceList(AudioOutput audioOutput)
        {
            if (VlcMediaPlayer == null)
                throw new InvalidOperationException("VlcMediaPlayer doesn't have initialize.");

            return VlcMediaPlayer.GetAudioDeviceList(audioOutput);
        }

        /// <summary>
        ///     Gets the list of available audio output modules.
        /// </summary>
        /// <returns></returns>
        public AudioOutputList GetAudioOutputList()
        {
            if (VlcMediaPlayer == null)
                throw new InvalidOperationException("VlcMediaPlayer doesn't have initialize.");

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
            if (VlcMediaPlayer == null)
                throw new InvalidOperationException("VlcMediaPlayer doesn't have initialize.");

            return VlcMediaPlayer.SetAudioOutput(audioOutput);
        }

        /// <summary>
        ///     Get the current audio output device identifier.
        /// </summary>
        public string GetAudioDevice()
        {
            if (VlcMediaPlayer == null)
                throw new InvalidOperationException("VlcMediaPlayer doesn't have initialize.");

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
            if (VlcMediaPlayer == null)
                throw new InvalidOperationException("VlcMediaPlayer doesn't have initialize.");

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

            var bodyExpr = expr.Body as MemberExpression;
            var propInfo = bodyExpr.Member as PropertyInfo;
            var propName = propInfo.Name;

            var displayThreadDispatcher = DisplayThreadDispatcher;
            if (displayThreadDispatcher != null)
            {
                displayThreadDispatcher.BeginInvoke(
                    new Action(() =>
                    {
                        if (_disposing) return;
                        if (_isStopping) return;

                        var onPropertyChanged = PropertyChanged;
                        if (onPropertyChanged != null)
                            onPropertyChanged(this, new PropertyChangedEventArgs(propName));
                    }));
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