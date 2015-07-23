using System;
using System.Runtime.InteropServices;

namespace xZune.Vlc.Interop.VLM
{
    /// <summary>
    /// 释放给定的 LibVlc 的实例相关的 VLM 实例
    /// </summary>
    /// <param name="instance"></param>
    [LibVlcFunction("libvlc_vlm_release")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void Releaseinstance(IntPtr instance);


    /// <summary>
    /// 添加广播和一个输入
    /// </summary>
    /// <param name="instance">VLM 实例指针</param>
    /// <param name="broadcastName">广播名</param>
    /// <param name="inputMRL">输入媒体资源地址</param>
    /// <param name="outputMRl">输出媒体资源地址</param>
    /// <param name="options"></param>
    /// <param name="stringOptions"></param>
    /// <param name="boolNewBorodcast">设置一个值允许打开新的新的广播</param>
    /// <param name="ifLoopBroadcast">是否广播循环播放</param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_vlm_add_broadcast")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int NewBroadCastInput(IntPtr instance,string broadcastName,string inputMRL,string outputMRl,int options,IntPtr stringOptions,int boolNewBorodcast, int ifLoopBroadcast);


    /// <summary>
    /// 添加视频点播和一个输入
    /// </summary>
    /// <param name="instance">VLM 实例指针</param>
    /// <param name="mediaWork"></param>
    /// <param name="MRLinput"></param>
    /// <param name="numberOptions"></param>
    /// <param name="addOptions"></param>
    /// <param name="boolNewVod"></param>
    /// <param name="vodMuxer"></param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_vlm_add_vod")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int NewVodInput(IntPtr instance, string mediaWork, string inputMRL, int numberOptions, IntPtr addOptions, int boolNewVod,string vodMuxer);


    /// <summary>
    /// 删除媒体(视频点播或广播)
    /// </summary>
    /// <param name="instance">VLM 实例指针</param>
    /// <param name="delBroadcastName"></param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_vlm_del_media")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int DelBoroadcastOrOvd(IntPtr instance, string delBroadcastName);


    /// <summary>
    /// 启用或禁用媒体(视频点播或广播)
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="mediaWork"></param>
    /// <param name="boolNewBorodcast"></param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_vlm_set_enabled")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int MediaSwitch(IntPtr instance, string mediaWork, int boolNewBorodcast);


    /// <summary>
    /// 设置媒体输出
    /// </summary>
    /// <param name="instance">VLM 实例指针</param>
    /// <param name="broadcastWork"></param>
    /// <param name="outputMRl"></param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_vlm_set_output")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SetMediaOutput(IntPtr instance, string mediaWork, string outputMRl);


    /// <summary>
    /// 设置媒体的 MRL 输入
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="mediaWork"></param>
    /// <param name="inputMRl"></param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_vlm_set_input")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SetMediaInput(IntPtr instance, string mediaWork, string inputMRl);


    /// <summary>
    /// 增加一个媒体的 MRL 输入
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="mediaWork"></param>
    /// <param name="inputMRl"></param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_vlm_add_output")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int AddMediaInput(IntPtr instance, string mediaWork, string inputMRl);


    /// <summary>
    /// 设置媒体循环状态
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="mediaWork"></param>
    /// <param name="newStatus">媒体新的状态</param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_vlm_set_loop")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SetMediaLoop(IntPtr instance, string mediaWork, int newStatus);


    /// <summary>
    /// 设置媒体的 Vod Muxer
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="mediaWork"></param>
    /// <param name="newMuxer"></param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_vlm_set_mux ")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SetVodMuxer(IntPtr instance, String mediaWork, String newMuxer);


    /// <summary>
    /// 编辑媒体参数
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="newBroadcastName"></param>
    /// <param name="inPutMRL"></param>
    /// <param name="outPutMRL"></param>
    /// <param name="numberOptains"></param>
    /// <param name="addOptains"></param>
    /// <param name="boolNewBoradcast"></param>
    /// <param name="ifLoopBroadcast"></param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_vlm_change_media")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int EditMediaParas(IntPtr instance, String newBroadcastName, String inPutMRL, String outPutMRL, int numberOptains, IntPtr addOptains, int boolNewBoradcast, int ifLoopBroadcast);
    

    /// <summary>
    /// 播放指定媒体
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="mediaName">指定的媒体的名字</param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_vlm_play_media")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int PlayNamedBoradcast(IntPtr instance, string mediaName);


    /// <summary>
    /// 停止指定的媒体
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="mediaName">指定的媒体的名字</param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_vlm_stop_media")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int StopNamedBoradcast(IntPtr instance, string mediaName);

    /// <summary>
    /// 暂停指定的媒体
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="mediaName">指定的媒体的名字</param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_vlm_pause_media")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int PauseNamedBoradcast(IntPtr instance, string mediaName);

    /// <summary>
    /// 在指定的广播中寻找
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="mediaName">指定的媒体的名字</param>
    /// <param name="seekPercent">寻找进度的百分比数值</param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_vlm_seek_media")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SeekInNamedBoradcast(IntPtr instance, string mediaName,float seekPercent);


    /// <summary>
    /// 以 JOSN 字符串的形式返回一个关于媒体的信息
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="namedMediaName"></param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_vlm_show_media")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate String ReturnJosnMessage(IntPtr instance, string namedMediaName);


    /// <summary>
    /// 通过名称或 ID 获取媒体实例的位置
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="MediaName"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_vlm_get_media_instance_position")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate float GetMediaPosition(IntPtr instance, string mediaName,int id);


    /// <summary>
    /// 通过名称或 ID 获取媒体实例的时间
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="mediaName"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_vlm_get_media_instance_time")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetMediaTime(IntPtr instance, string mediaName, int id);

    /// <summary>
    /// 通过名称或 ID 获取媒体实例的长度
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="mediaName"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_vlm_get_media_instance_length")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetMediaLength(IntPtr instance, string mediaName, int id);

    /// <summary>
    /// 通过名称或 ID 获取媒体实例的退率
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="mediaName"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_vlm_get_media_instance_rate")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetMediaBackRate(IntPtr instance, string mediaName, int id);

    /// <summary>
    /// 从 Vim Media 中得到 Libvlc 事件管理器
    /// </summary>
    /// <param name="libvlcInstance"></param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_vlm_get_event_manager")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GetMediaEventManager(IntPtr instance);
}


