//Project: xZune.Vlc (https://github.com/higankanshi/xZune.Vlc)
//Filename: VlcPlayer.Properties.cs
//Version: 20151220

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
            get { return VlcMediaPlayer.Position.DefaultValueWhenNull(VlcMediaPlayer); }
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
            get { return VlcMediaPlayer.Time.DefaultValueWhenNull(VlcMediaPlayer); }
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
            get { return VlcMediaPlayer.Fps.DefaultValueWhenNull(VlcMediaPlayer); }
        }

        #endregion FPS

        #region IsMute

        /// <summary>
        /// Get or set state of mute.
        /// </summary>
        public bool IsMute
        {
            get { return VlcMediaPlayer.IsMute.DefaultValueWhenNull(VlcMediaPlayer); }
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

        /// <summary>
        /// Get or set volume of media.
        /// </summary>
        public int Volume
        {
            get { return VlcMediaPlayer.Volume.SafeValueWhenNull(100, VlcMediaPlayer); }
            set
            {
                if (Volume == value || VlcMediaPlayer == null) return;
                VlcMediaPlayer.Volume = value;

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
            get { return VlcMediaPlayer.AudioOutputChannel.SafeValueWhenNull(AudioOutputChannel.Error, VlcMediaPlayer); }
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
            get { return VlcMediaPlayer.AudioTrackCount.DefaultValueWhenNull(VlcMediaPlayer); }
        }

        #endregion AudioTrackCount

        #region AudioTrack

        /// <summary>
        /// Get or set track index of audio.
        /// </summary>
        public int AudioTrack
        {
            get { return VlcMediaPlayer.AudioTrack.SafeValueWhenNull(-1, VlcMediaPlayer); } //note: assuming a 0-based index
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
            get { return (VlcMediaPlayer != null) ? VlcMediaPlayer.AudioTrackDescription : null; }
        }

        #endregion AudioTrackDescription

        #region Rate

        /// <summary>
        /// Get or set rate of media.
        /// </summary>
        public float Rate
        {
            get { return (VlcMediaPlayer != null) ? VlcMediaPlayer.Rate : 0; }
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
            get { return VlcMediaPlayer.Title.SafeValueWhenNull(-1, VlcMediaPlayer); } //note: assuming a 0-based index
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
            get { return VlcMediaPlayer.TitleCount.DefaultValueWhenNull(VlcMediaPlayer); }
        }

        #endregion TitleCount

        #region Chapter

        /// <summary>
        /// Get or set chapter index of media.
        /// </summary>
        public int Chapter
        {
            get { return VlcMediaPlayer.Chapter.SafeValueWhenNull(-1, VlcMediaPlayer); } //note: assuming a 0-based index
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
            get { return VlcMediaPlayer.ChapterCount.DefaultValueWhenNull(VlcMediaPlayer); }
        }

        #endregion ChapterCount

        #region IsSeekable

        /// <summary>
        /// Checks if media is seekable.
        /// </summary>
        public bool IsSeekable
        {
            get { return VlcMediaPlayer.IsSeekable.SafeValueWhenNull(true, VlcMediaPlayer); }
        }

        #endregion IsSeekable

        #region State

        /// <summary>
        /// Get state of media.
        /// </summary>
        public MediaState State
        {
            get { return VlcMediaPlayer.State.DefaultValueWhenNull(VlcMediaPlayer); }
        }

        #endregion State

        #region Length

        /// <summary>
        /// Get length of media.
        /// </summary>
        public TimeSpan Length
        {
            get { return VlcMediaPlayer.Length.DefaultValueWhenNull(VlcMediaPlayer); }
        }

        #endregion Length

        #region VlcMediaPlayer

        /// <summary>
        /// Get internal VlcMediaPlayer, it is best not to use this, unless you need to customize advanced features.
        /// </summary>
        public VlcMediaPlayer VlcMediaPlayer { get; private set; }

        #endregion VlcMediaPlayer
    }
}