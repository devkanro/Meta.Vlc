//Project: xZune.Vlc (https://github.com/higankanshi/xZune.Vlc)
//Filename: VlcPlayer.Properties.cs
//Version: 20160109

using System;
using System.Windows.Threading;
using xZune.Vlc.Interop.MediaPlayer;
using MediaState = xZune.Vlc.Interop.Media.MediaState;

namespace xZune.Vlc.Wpf
{
    public partial class VlcPlayer
    {
        #region Position

        /// <summary>
        /// Get or set progress of media, between 0 and 1.
        /// </summary>
        public float Position
        {
            get { return VlcMediaPlayer.DefaultValueWhenNull(x => x.Position); }
            set
            {
                if (Position == value || VlcMediaPlayer == null || !IsSeekable) return;
                VlcMediaPlayer.Position = value;
            }
        }

        #endregion Position

        #region Time

        /// <summary>
        /// Get or set current time progress of media.
        /// </summary>
        public TimeSpan Time
        {
            get { return VlcMediaPlayer.DefaultValueWhenNull(x => x.Time); }
            set
            {
                if (Time == value || VlcMediaPlayer == null || !IsSeekable) return;
                VlcMediaPlayer.Time = value;
            }
        }

        #endregion Time

        #region FPS

        /// <summary>
        /// Get FPS of media.
        /// </summary>
        public float FPS
        {
            get { return VlcMediaPlayer.DefaultValueWhenNull(x => x.Fps); }
        }

        #endregion FPS

        #region IsMute

        /// <summary>
        /// Get or set state of mute.
        /// </summary>
        public bool IsMute
        {
            get { return VlcMediaPlayer.DefaultValueWhenNull(x => x.IsMute); }
            set
            {
                if (IsMute == value || VlcMediaPlayer == null) return;
                VlcMediaPlayer.IsMute = value;

                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    OnPropertyChanged(() => IsMute);
                    if (IsMuteChanged != null)
                    {
                        IsMuteChanged(this, new EventArgs());
                    }
                }));
            }
        }

        #endregion IsMute

        #region Volume

        private int _volume = 100;

        /// <summary>
        /// Get or set volume of media.
        /// </summary>
        public int Volume
        {
            get { return _volume; }
            set
            {
                if (_volume == value || VlcMediaPlayer == null) return;
                VlcMediaPlayer.Volume = _volume = value;
                
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    OnPropertyChanged(() => Volume);
                    if (VolumeChanged != null)
                    {
                        VolumeChanged(this, new EventArgs());
                    }
                }));
            }
        }

        #endregion Volume

        #region AudioOutputChannel

        /// <summary>
        /// Get or set output channel of audio.
        /// </summary>
        public AudioOutputChannel AudioOutputChannel
        {
            get { return VlcMediaPlayer.DefaultValueWhenNull(x => x.AudioOutputChannel, AudioOutputChannel.Error); }
            set
            {
                if (AudioOutputChannel == value || VlcMediaPlayer == null) return;
                VlcMediaPlayer.AudioOutputChannel = value;
            }
        }

        #endregion AudioOutputChannel

        #region AudioTrackCount

        /// <summary>
        /// Get track count of audio.
        /// </summary>
        public int AudioTrackCount
        {
            get { return VlcMediaPlayer.DefaultValueWhenNull(x => x.AudioTrackCount); }
        }

        #endregion AudioTrackCount

        #region AudioTrack

        /// <summary>
        /// Get or set track index of audio.
        /// </summary>
        public int AudioTrack
        {
            get { return VlcMediaPlayer.DefaultValueWhenNull(x => x.AudioTrack, - 1); } //note: assuming a 0-based index
            set
            {
                if (AudioTrack == value || VlcMediaPlayer == null) return;
                VlcMediaPlayer.AudioTrack = value;
            }
        }

        #endregion AudioTrack

        #region AudioTrackDescription

        /// <summary>
        /// Get description of audio track.
        /// </summary>
        public TrackDescription AudioTrackDescription
        {
            get { return VlcMediaPlayer.DefaultValueWhenNull(x => x.AudioTrackDescription); }
        }

        #endregion AudioTrackDescription

        #region Rate

        /// <summary>
        /// Get or set rate of media.
        /// </summary>
        public float Rate
        {
            get { return VlcMediaPlayer.DefaultValueWhenNull(x => x.Rate); }
            set
            {
                if (Rate == value || VlcMediaPlayer == null) return;
                VlcMediaPlayer.Rate = value;
            }
        }

        #endregion Rate

        #region Title

        /// <summary>
        /// Get or set title index of media.
        /// </summary>
        public int Title
        {
            get { return VlcMediaPlayer.DefaultValueWhenNull(x => x.Title, -1); } //note: assuming a 0-based index
            set
            {
                if (Title == value || VlcMediaPlayer == null) return;
                VlcMediaPlayer.Title = value;
            }
        }

        #endregion Title

        #region TitleCount

        /// <summary>
        /// Get title count of media.
        /// </summary>
        public int TitleCount
        {
            get { return VlcMediaPlayer.DefaultValueWhenNull(x => x.TitleCount); }
        }

        #endregion TitleCount

        #region Chapter

        /// <summary>
        /// Get or set chapter index of media.
        /// </summary>
        public int Chapter
        {
            get { return VlcMediaPlayer.DefaultValueWhenNull(x => x.Chapter, -1); } //note: assuming a 0-based index
            set
            {
                if (Chapter == value || VlcMediaPlayer == null) return;
                VlcMediaPlayer.Chapter = value;
            }
        }

        #endregion Chapter

        #region ChapterCount

        /// <summary>
        /// Get chapter count of media.
        /// </summary>
        public int ChapterCount
        {
            get { return VlcMediaPlayer.DefaultValueWhenNull(x => x.ChapterCount); }
        }

        #endregion ChapterCount

        #region IsSeekable

        /// <summary>
        /// Checks if media is seekable.
        /// </summary>
        public bool IsSeekable
        {
            get { return VlcMediaPlayer.DefaultValueWhenNull(x => x.IsSeekable, true); }
        }

        #endregion IsSeekable

        #region State

        /// <summary>
        /// Get state of media.
        /// </summary>
        public MediaState State
        {
            get { return VlcMediaPlayer.DefaultValueWhenNull(x => x.State); }
        }

        #endregion State

        #region Length

        /// <summary>
        /// Get length of media.
        /// </summary>
        public TimeSpan Length
        {
            get { return VlcMediaPlayer.DefaultValueWhenNull(x => x.Length); }
        }

        #endregion Length

        #region VlcMediaPlayer

        /// <summary>
        /// Get internal VlcMediaPlayer, it is best not to use this, unless you need to customize advanced features.
        /// </summary>
        public VlcMediaPlayer VlcMediaPlayer { get; private set; }

        #endregion VlcMediaPlayer

        #region AudioEqualizer
        private AudioEqualizer _audioEqualizer;

        /// <summary>
        /// Get or set audio equalizer.
        /// </summary>
        public AudioEqualizer AudioEqualizer
        {
            get { return _audioEqualizer; }
            set
            {
                if (_audioEqualizer != null)
                {
                    _audioEqualizer.PropertyChanged -= AudioEqualizer_PropertyChanged;
                }
                if (value != null)
                {
                    value.PropertyChanged += AudioEqualizer_PropertyChanged;
                }
                _audioEqualizer = value;
                VlcMediaPlayer.SetEqualizer(_audioEqualizer);
            }
        }

        private void AudioEqualizer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            VlcMediaPlayer.SetEqualizer(_audioEqualizer);
        }

        #endregion

    }
}