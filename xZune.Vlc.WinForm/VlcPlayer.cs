using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace xZune.Vlc.WinForm
{
    public partial class VlcPlayer: UserControl, INotifyPropertyChanged
    {
        public VlcPlayer(String libvlcPath) : this()
        {
            ApiManager.LibVlcPath = libvlcPath;
        }

        public VlcPlayer()
        {
            InitializeComponent();
            
        }

        static String CombinePath(String path1, String path2)
        {
            string result = string.Empty;

            if (!Path.IsPathRooted(path2))
            {
                Regex regex = new Regex(@"^\\|([..]+)");
                int backUp = regex.Matches(path2).Count;
                List<string> pathes = path1.Split('\\').ToList();
                pathes.RemoveRange(pathes.Count - backUp, backUp);
                regex = new Regex(@"^\\|([a-zA-Z0-9]+)");
                MatchCollection matches = regex.Matches(path2);
                foreach (Match match in matches)
                {
                    pathes.Add(match.Value);
                }
                pathes[0] = Path.GetPathRoot(path1);
                foreach (string p in pathes)
                {
                    result = Path.Combine(result, p);
                }
            }
            return result;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if(!this.DesignMode)
            {
                if (!ApiManager.IsInited)
                {
                    if (LibVlcPath != null)
                    {
                        if (Path.IsPathRooted(LibVlcPath))
                        {
                            ApiManager.LibVlcPath = LibVlcPath;
                        }
                        else
                        {
                            ApiManager.LibVlcPath = CombinePath(ApiManager.LibVlcPath, LibVlcPath);
                        }
                        if (ApiManager.LibVlcPath[ApiManager.LibVlcPath.Length - 1] != '\\' && ApiManager.LibVlcPath[ApiManager.LibVlcPath.Length - 1] != '/')
                        {
                            ApiManager.LibVlcPath += "\\";
                        }
                    }
                    ApiManager.Init();
                }
                VlcMediaPlayer = ApiManager.Vlc.CreateMediaPlayer();
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
                VlcMediaPlayer.Hwnd = this.Handle;
            }
        }

        #region 属性变更通知
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChange([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void SetProperty(ref object field, object newValue, [CallerMemberName]string propertyName = "")
        {
            if (field != newValue)
            {
                field = newValue;
                NotifyPropertyChange(propertyName);
            }
        }

        protected void SetProperty<T>(ref T field, T newValue, [CallerMemberName]string propertyName = "")
        {
            if (!object.Equals(field, newValue))
            {
                field = newValue;
                NotifyPropertyChange(propertyName);
            }
        }
        #endregion

        #region 只读属性 VlcMediaPlayer
        public VlcMediaPlayer VlcMediaPlayer { get; private set; }
        #endregion

        #region 只读属性 IsSeekable
        private void VlcMediaPlayerSeekableChanged(object sender, EventArgs e)
        {
            IsSeekable = VlcMediaPlayer.IsSeekable;
        }

        private bool isSeekable;

        /// <summary>
        /// 获取一个值,该值表示是否允许调整进度条
        /// </summary>
        public bool IsSeekable
        {
            get { return isSeekable; }
            private set { SetProperty<bool>(ref isSeekable, value); }
        }
        #endregion

        #region 只读属性 State
        private void VlcMediaPlayerStateChanged(object sender, EventArgs e)
        {
            State = VlcMediaPlayer.State;
        }

        private xZune.Vlc.Interop.Media.MediaState state;

        /// <summary>
        /// 获取一个值,该值表示当前的媒体状态
        /// </summary>
        public xZune.Vlc.Interop.Media.MediaState State
        {
            get { return state; }
            private set { SetProperty<xZune.Vlc.Interop.Media.MediaState>(ref state, value); }
        }
        #endregion

        #region 只读属性 Length

        private void VlcMediaPlayerLengthChanged(object sender, EventArgs e)
        {
            Length = VlcMediaPlayer.Length;
        }

        private TimeSpan length;

        /// <summary>
        /// 获取一个值,该值表示当前的媒体的长度
        /// </summary>
        public TimeSpan Length
        {
            get { return length; }
            set { SetProperty<TimeSpan>(ref length, value); }
        }
        #endregion

        #region 属性 Time
        private void VlcMediaPlayerTimeChanged(object sender, EventArgs e)
        {
            TimeChanged?.Invoke(this, new EventArgs());
        }

        public event EventHandler TimeChanged;

        /// <summary>
        /// 获取或设置一个值,该值表示当前的时间进度
        /// </summary>
        public TimeSpan Time
        {
            get { return VlcMediaPlayer.Time; }
            set { VlcMediaPlayer.Time = value; }
        }
        #endregion
        
        #region 属性 Position
        private void VlcMediaPlayerPositionChanged(object sender, EventArgs e)
        {
            PositionChanged?.Invoke(this, new EventArgs());
        }

        public event EventHandler PositionChanged;

        /// <summary>
        /// 获取或设置一个值,该值表示当前的播放进度
        /// </summary>
        public float Position
        {
            get { return VlcMediaPlayer.Position; }
            set { VlcMediaPlayer.Position = value; }
        }
        #endregion

        #region 属性 LibVlcPath
        public string LibVlcPath { get; set; }
        #endregion

        public void LoadMedia(String path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(String.Format("找不到媒体文件:{0}", path), path);
            }
            VlcMediaPlayer.Media?.Dispose();
            VlcMediaPlayer.Media = ApiManager.Vlc.CreateMediaFormPath(path);
            VlcMediaPlayer.Media.ParseAsync();
        }

        public void LoadMedia(Uri uri)
        {
            VlcMediaPlayer?.Stop();
            VlcMediaPlayer.Media?.Dispose();
            VlcMediaPlayer.Media = ApiManager.Vlc.CreateMediaFormLocation(uri.ToString());
        }

        public void Play()
        {
            VlcMediaPlayer.Play();
        }

        public void PauseOrResume()
        {
            VlcMediaPlayer.PauseOrResume();
        }

        public void AddOption(String option)
        {
            VlcMediaPlayer.Media?.AddOption(option);
        }

        public void NextFrame()
        {
            VlcMediaPlayer.NextFrame();
        }
    }
}
