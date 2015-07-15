using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace xZune.Vlc
{
    public class TrackDescription : IDisposable
    {
        /// <summary>
        /// 通过指针来初始化 TrackDescription
        /// </summary>
        /// <param name="pointer">提供的指针</param>
        public TrackDescription(IntPtr pointer)
        {
            Dictionary<int, String> itemsList = new Dictionary<int, String>();

            while (pointer != IntPtr.Zero)
            {
                var trackDescriptionStruct = (Interop.MediaPlayer.TrackDescription)Marshal.PtrToStructure(pointer, typeof(Interop.MediaPlayer.TrackDescription));
                itemsList.Add(trackDescriptionStruct.Id, trackDescriptionStruct.Name);
                pointer = trackDescriptionStruct.Next;
            }

            Items = itemsList;
        }

        /// <summary>
        /// 获取这个 TrackDescription 的指针
        /// </summary>
        public IntPtr Pointer { get; private set; }

        /// <summary>
        /// 获取这个 TrackDescription 包含的子项
        /// </summary>
        public Dictionary<int, String> Items { get; private set; }

        /// <summary>
        /// 获取这个 TrackDescription 包含的子项的个数
        /// </summary>
        public int Count
        {
            get
            {
                return Items.Count;
            }
        }

        /// <summary>
        /// 释放当前的 ModuleDescription 资源
        /// </summary>
        public void Dispose()
        {
            VlcMediaPlayer.ReleaseTrackDescription(this);
            Items = null;
            Pointer = IntPtr.Zero;
        }
    }
}
