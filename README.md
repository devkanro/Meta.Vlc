# Meta.Vlc

## Introduction 

Meta.Vlc 是一个 LibVlc 封装库的 .NET 实现,封装了大部分的 LibVlc 的功能,该项目主要是为了寻求一个在 WPF 上使用 Vlc 的完美的解决方案,Meta.Vlc 提供一个原生的 WPF 播放控件(Meta.Vlc.Wpf),该控件采用 InteropBitmap 与共享内存进行播放视频,是一个原生的 WPF 控件,不存在 HwndHost 的空域问题.  

_Meta.Vlc is an LibVlc solution for .NET, it has encapsulated most of functionalities of LibVlc. This project aims to find a perfect solution for using Vlc on WPF. Meta.Vlc provides an native WPF control(Meta.Vlc.Wpf), this control achieves video playback by utilizing InteropBitmap and shared memory. Since it’s a native WPF control, it doesn't suffer from HwndHost’s airspace issue._  

[![Build Status](https://ci.appveyor.com/api/projects/status/q76jlj04n40h2ygg/branch/master?svg=true)](https://ci.appveyor.com/project/higankanshi/Meta-vlc/branch/master) [![Join the chat at https://gitter.im/higankanshi/Meta.Vlc](https://badges.gitter.im/higankanshi/Meta.Vlc.svg)](https://gitter.im/higankanshi/Meta.Vlc?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge) 

## NuGet Packages

Install [Meta.Vlc](https://www.nuget.org/packages/Meta.Vlc/)
```Powershell
PM> Install-Package Meta.Vlc 
```

Install [Meta.Vlc.Wpf](https://www.nuget.org/packages/Meta.Vlc.Wpf/)
```Powershell
PM> Install-Package Meta.Vlc.Wpf 
```

Install [Meta.Vlc.Lib](https://www.nuget.org/packages/Meta.Vlc.Lib/)
```Powershell
PM> Install-Package Meta.Vlc.Lib
```
>Meta.Vlc.Lib 包含所有 LibVlc 的文件，你可以在 Meta.Vlc 与 Meta.Vlc.Wpf 中使用这些 Dll。  
>LibVlc 的版本：  
>2.2.0-Meta Weatherwax  
>   
>_Meta.Vlc.Lib include all files of LibVlc. You can use this dlls for Meta.Vlc and Meta.Vlc.Wpf._   
>_LibVlc Version:_   
>_2.2.0-Meta Weatherwax_  

## Api Documentation 

See the api documentation of [Meta.Vlc](http://higan.me/Meta.Vlc/api/index.html).

## .Net Support
.NET Version | Meta.Vlc | Meta.Vlc.Wpf | Meta.Vlc.Wpf.Sample
------------ | ------------- | ------------- | -------------
.NET 2.0 | :o: | :x: | :x:
.NET 3.0 | :o: | :x: | :x:
.NET 3.5 | :o: | :o: | :o:
.NET 4.0 | :o: | :o: | :o:
.NET 4.5 | :o: | :o: | :o:

>:o: :兼容/Compatible     
>:x: :不兼容/Incompatible       
>:interrobang: :部分功能可能不可用/Some functions may not be available  

## Change Log

### 2016/05/01
SHA1:90c45ed791b96e9da65b926cc9738cbd4840988e

**01.更改项目代号 xZune 为 Meta。**  
_**01.Change the code name to "Meta".**_  
项目代号与更命名控件的变更，仅需要全局替换 "xZune" 为 "Meta"，即可兼容此次变更。  
_Project code name and root name space have been changed, you can replace "xZune" to "Meta" to compatible this commit._  

旧版本 Nuget 包仍然被提供，在这里查看旧版本 Nuget 包：  
_Old version Nuget package still be provided, check those:_
[xZune.Vlc](https://www.nuget.org/packages/xZune.Vlc/)  
[xZune.Vlc.Wpf](https://www.nuget.org/packages/xZune.Vlc.Wpf/)  
[xZune.Vlc.Lib](https://www.nuget.org/packages/xZune.Vlc.Lib/)  

### 2016/04/17  
SHA1:3a4d2cc7ca1a1c1026047a07494641d4c8b38e3d  

**01.添加 Video Adjust 系列 API。**  
_**01.Add adjust APIs for video.**_  
在 VlcMediaPlayer 中使用下列过滤器相关的 API 来为播放器提供过滤器。  
_Use those video adjust APIs in VlcMediaPlayer to control adjust._  
```
Property              VlcMediaPlayer.IsAdjustEnable          get/set   bool
Property              VlcMediaPlayer.Contrast                get/set   float
Property              VlcMediaPlayer.Brightness              get/set   float
Property              VlcMediaPlayer.Hue                     get/set   float  
Property              VlcMediaPlayer.Saturation              get/set   float  
Property              VlcMediaPlayer.Gamma                   get/set   float  
```

### 2016/04/13  
SHA1:7ffd3eb8616a6a364591809890688bda84b36501  

**01.添加 Subtitle 系列 API。**  
_**01.Add subtitle APIs.**_  
在 VlcMediaPlayer 中使用下列字幕相关的 API 来控制播放器的字幕行为。  
_Use those subtitle APIs in VlcMediaPlayer to control subtitle._  
```
Property              VlcMediaPlayer.Subtitle                  get/set   int
Property              VlcMediaPlayer.SubtitleDelay             get/set   long
Property              VlcMediaPlayer.SubtitleCount             get       int
Property              VlcMediaPlayer.SubtitleDescription       get       TrackDescription
Method                VlcMediaPlayer.SetSubtitleFile(String)             bool
```

### 2016/03/28
SHA1:72b6e0fa70d34c64b700e9bdeda8f9ca61731b9f  

**01.可为其他控件生成 VideoSource。**  
_**01.VideoSource can be used in other control.**_  
由于呈现线程的改变，VideoSource 将属于呈现线程，而非默认 UI 线程，因此 VideoSource 不在能被默认 UI 线程的元素使用，但是我们为在其他控件使用 VideoSource 准备其他方案。
_Due the change of display thread, VideoSource will belong to display thread, not the default UI thread. So VideoSource can't be used by UI element of default UI thread, but we provide another way to use VideoSource for other control._  

查看 [Use VlcPlayer with other controls](https://github.com/higankanshi/Meta.Vlc/wiki/Use-VlcPlayer-with-other-controls) 来获取如何在其他控件中使用 VideoSource 的教程。  
_See [Use VlcPlayer with other controls](https://github.com/higankanshi/Meta.Vlc/wiki/Use-VlcPlayer-with-other-controls) to get more information about use VlcPlayer with other conttols._

### 2016/03/25

**01.多线程的图片呈现方式。**  
_**01.Multi-Thread video display support. .**_  
SHA1:5fff42c9f4a8c37b65107e23fdfd85ca9b42bfa4  
现在我们将用于呈现视频的 Image 控件转移到高优先度的线程，降低由于 UI 操作（更新 UI 等）导致 UI 线程卡死从而导致视频卡顿。  
但是目前最多能接受大约 500ms 的卡顿不会影响到视频播放，长时间的 UI 线程卡死仍然会导致视频卡顿，但是解码与呈现线程已经不和 UI 线程有交互。  
导致这种情况的原因不明，初步断定为由于文件读写操作与 UI 线程有交互。  
_Now we move the Image control which used for display video to highest thread, it will reduce stuck when UI thread is busy._  
_But if UI thread is busy for long time, video still will stuck, but decode thread is not associated with UI thread any more._  
_No idea about this, but I think the video file IO is  associated with UI thread._ 

**02.VlcPlayer 处理 VLC 事件不在主 UI 线程同步处理。**  
_**02.VlcPlayer handle event of VLC no longer block the main UI thread.**_  
SHA1:052ed5ec6c16fa43a3c50b8ec594c3176973b97e  
完善 VLC 线程与 UI 线程之间的交互，现在 VlcPlayer 在处理 VLC 事件时不会在主 UI 线程上同步。  
解决了当主 UI 线程卡死时，视频将暂停播放的问题，现在当主 UI 线程卡死时，视频仍然能够继续播放不受影响。  
_To improve the interaction between VLC threads and UI thread, VlcPlayer handle event of LibVlc will not sync invoke in main UI thread._  
_Solved video will be pause when main UI thread is stuck. Now even the main UI thread is stuck, the display will play go on._  

**03.移除 BeginStop 方法。**  
_**03.Remove the BeginStop method.**_  
SHA1:052ed5ec6c16fa43a3c50b8ec594c3176973b97e  
由于线程交互的完善，Stop 方法不再会阻塞线程导致死锁，因此 BeginStop 方法不再被需要。  
_Due to the improvement of thread interaction, Stop method will block the thread no longer, BeginStop method are no longer needed._  

### 2016/02/14
SHA1:185dfe8d8c713c1ff10fadaaf0f97af2ecc3aabc

**01.添加 VLM 系列 APIs。**  
_**01.Add VLM APIs.**_  
你可以使用 `Vlc` 类提供的 `Vlm*()` 系列方法来使用 VLM 功能。  
_You can use the `Vlm*()` methods of `Vlc` class to use VLM functions._

### 2016/02/13
SHA1:6ec4d6aa5823702af6cafe8cb3d4c0ec3e58602a

**01.提供更多结构体封装。**  
_**01.Wrap more sturct.**_  
提供全新的各种结构体封装方式，更易于使用。  
_We changed the wrapper of each struct, make more easy to use._

**02.更改 LibVlc 初始化方式。**  
_**02.Changed the LibVlc initialize mode.**_  
提供新的 `LibVlcManager` 类来管理 LibVlc 的初始化，让初始化逻辑更清晰。  
_New `LibVlcManager` class to manage initialization of LibVlc._

**03.添加更多的英文注释。**
_**03.Add more English comment.**_

> **注意(_Note_):**  
> 本次更改更倾向于底层代码优化与清理，如果你主要使用 Meta.Vlc 而不是 Meta.Vlc.Wpf，你可能需要做比较多的更改来兼容这次变更。  
> 如果你只是使用 Meta.Vlc.Wpf 只是部分涉及到 `TrackDescription` 结构，`MediaTrack` 结构，或是其他结构的 API 有小部分变更。   
>  
> _This commit is only changed and clear up some lowest layer code, if you using Meta.Vlc not Meta.Vlc.Wpf, maybe you should changed many code to compatible this commit._  
> _If you using Meta.Vlc.Wpf, only some APIs about `TrackDescription` struct, `MediaTrack` struct or other struct have changed._

### 2016/02/13
SHA1:b5b6a020e041a0d9e9b1e911debdd606b4911971

**01.支持音频设备系列 API。**  
_**01.Support audio dvice APIs**._  
新添加的 `VlcPlayer.SetAudioDevice(AudioOutput audioOutput, AudioDevice audioDevice)` 方法可以由指定的音频设备播放媒体的音频。  
_New audio dvice API `VlcPlayer.SetAudioDevice(AudioOutput audioOutput, AudioDevice audioDevice)` can select a audio device to display audio of media._  

**02.多 VLC 实例支持。**  
_**02.Mutil VLC instance support.**_  
可以通过 `VlcPlayer.CreateMode` 属性来设置创建 VlcMediaPlayer 时是使用默认 VLC 实例或者新建一个 VLC 实例。  
_You can set create VlcMediaPlayer with default VLC instance or create a new VLC instance by `VlcPlayer.CreateMode` property._

## Quick Start

在您的项目中快速使用 Meta.Vlc：  
_Quick start Meta.Vlc in your project:_  

**01.在项目中添加对 Meta.Vlc 的程序集的引用。**    
_**01.Add the references of Meta.Vlc to your project.**_  
```
Meta.Vlc.dll
Meta.Vlc.Wpf.dll
```

**02.在项目的属性中设置 LibVlc 库的目录。**  
_**02.Set the path of LibVlc in properties of your project.**_     
在`$(Your Project)\Properties\AssemblyInfo.cs`中添加 VlcSettingsAttribute。  
_Add a VlcSettingsAttribute in `$(Your Project)\Properties\AssemblyInfo.cs`._  
```CSharp
//设置 LibVlc 的目录
//Set the path of LibVlc
[assembly: VlcSettings(@"..\..\..\LibVlc")] 

//设置 LibVlc 初始化时的选项
//Set the path and options of LibVlc
[assembly: VlcSettings(@"..\..\..\LibVlc", "-I", "dummy", "--ignore-config", "--no-video-title" )]
```
>参考:  
>_See:_  
>https://github.com/higankanshi/Meta.Vlc/blob/master/Meta.Vlc.Wpf.Sample/Properties/AssemblyInfo.cs

**03.在 Xaml 中加入 VlcPlayer 控件。**   
_**03.Add the VlcPlayer Control in your Xaml**_  
```XAML
<wpf:VlcPlayer xmlns:wpf="clr-namespace:Meta.Vlc.Wpf;assembly=Meta.Vlc.Wpf" x:Name="vlcPlayer"/>
```

**04.载入媒体并播放。**    
_**04.Load and Play the media.**_    
```CSharp
vlcPlayer.LoadMedia(@"C:\VlcTest.mp4");             //载入本地文件    Load a local file
//vlcPlayer.LoadMedia(@"H:\");                      //载入DVD光盘     Load a DVD
//vlcPlayer.LoadMedia(new Uri("http://127.0.0.1")); //载入网络流      Load a Network Stream
vlcPlayer.Play();                                   //播放媒体        Play the media
```

**05.停止媒体与释放资源。**     
_**05.Stop the media and Release the resources.**_  

`vlcPlayer.Stop()`可以用于停止媒体的播放。
_`vlcPlayer.Stop()` used for stop the Player._  

```CSharp
VlcPlayer.Stop();
VlcPlayer.Play(); // 重载媒体。         Replay media.
```

可以在程序结束时调用`vlcPlayer.Dispose()`释放所有资源。   
_You can call the `vlcPlayer.Dispose()`to release the resource when you exit._  

**06.为音频输出使用均衡器(需要 LibVlc 2.2.0 以上)。**  
_**06.Add equalizer for audio(Need LibVlc 2.2.0 and higher).**_  

使用 AudioEqualizer 为音频提供均衡器，并提供 18 种预置均衡器与 10 个可自定义放大数值的频带。  
_Now we can use AudioEqualizer to provide equalizer for VlcPlayer, have 18 preset equalizers and 10 frequency bands._   
```CSharp
//使用预置的均衡器设置初始化均衡器。
//use preset equalizer setting to initilaize AudioEqualizer.

AudioEqualizer ae = new AudioEqualizer(PresetAudioEqualizerType.Classical);
Player.AudioEqualizer = ae;


//使用默认设置初始化均衡器，并为 10 个频带赋值。
//use default setting to initilaize AudioEqualizer and set every frequency bands.

AudioEqualizer ae = new AudioEqualizer();
ae.Preamp = 12;
ae[0] = -1.11022E-15f;
ae[1] = -1.11022E-15f;
ae[2] = -1.11022E-15f;
ae[3] = -1.11022E-15f;
ae[4] = -1.11022E-15f;
ae[5] = -1.11022E-15f;
ae[6] = -7.2f;
ae[7] = -7.2f;
ae[8] = -7.2f;
ae[9] = -9.6f;
Player.AudioEqualizer = ae;
```
>**注意(_Note_)：**  
> `Meta.Vlc.VlcMediaPlayer` 不会引用均衡器实例，仅会复制其值，但是 AudioEqualizer 提供了属性变更通知，你可以使用其属性变更事件来重新设置均衡器。   
> `Meta.Vlc.Wpf.VlcPlayer` 会引用均衡器实例，并且监听了当前均衡器的属性变更事件，当当前均衡器有变更会自动的为 VlcMediaPlayer 重新设置均衡器。  
>   
> The `Meta.Vlc.VlcMediaPlayer` **will not** keep a reference to the supplied equalizer so you need set it again after you changed some value of `AudioEqualizer`, we provide PropertyChanged event for `AudioEqualizer` you can use this to reset equalizer.   
> The `Meta.Vlc.Wpf.VlcPlayer` **will** keep a reference to the supplied equalizer, when you changed some value of `AudioEqualizer`, it will auto reset equalizer for `VlcMediaPlayer`.  

  
**07.循环播放视频。**    
_**07.loop the media.**_    
通过设置 EndBehavior 来设置当媒体播放完毕后的动作,支持 Nothing,Stop,Repeat 三种模式。    
当设置为 Nothing 时，会什么都不做，屏幕仍然会显示最后一帧的画面，你需要先将播放器停止才能重新播放视频。  
当设置为 Stop 时，会自动停止视频，屏幕会被清空，并且可以直接使用播放方法重新播放视频，这是默认的行为。    
当设置为 Repeat 时，会自动的重新播放视频。  

*You can set the EndBehavior property to set behavior when media ended. Support Nothing,Stop,Repeat mode.*   
*If you set it to Nothing, player will do nothing after media ended, you need stop it, and you can play again.*    
*If you set it to Stop, player will set to stop.*    
*If you set it to Repeat, player will atuo play again media.*    

**08.特殊的 VLC 功能。**   
_**08.Some other extension for VLC.**_   
Meta.Vlc 使用了某些 VLC 的拓展功能，我们目前提供 LibVlc(2.2.0-Meta) 32bit 版本。如果您需要在更低或者更高的 VLC 版本上使用 Meta.Vlc，您可能需要自己编译 VLC for Meta.Vlc，以保证 Meta.Vlc 拓展功能可用。  
*Meta.Vlc has used some "Expansions" for VLC, we are providing LibVlc(2.2.0-Meta) 32bit version now.But you want to use Meta.Vlc with later or lower version,you could need compile VLC for Meta.Vlc by yourself,to ensure that the expansions are available.*  

查看 [Compile VLC for Meta.Vlc](https://github.com/higankanshi/Meta.Vlc/wiki/Compile-VLC-for-Meta.Vlc) 来获取编译 Meta 专用的 VLC 教程。   
*See [Compile VLC for Meta.Vlc](https://github.com/higankanshi/Meta.Vlc/wiki/Compile-VLC-for-Meta.Vlc) to get some infomation about compile VLC for Meta.*   

## Other Project  
**已经在使用 Meta.Vlc 的项目：**  
_**Already in use Meta.Vlc:**_  

**[FoscamController](http://foscamcontroller.codeplex.com/)**    
This is a Library and Demo WPF program for controlling MJPEG and HD models of Foscam IP PTZ (Pan-Tilt-Zoom) Cameras and displaying MJPEG and H.264/RTSP video.   

**[FiloStreamer](https://github.com/TheThing/FiloStreamer)**  
FiloStreamer is a simple GUI application frontend on top of ffmpeg, designed to simplify and ease configuring and setting up local live stream from a Blackmagic Decklink card source.     

**[SenpaiCopy](https://github.com/coczero/SenpaiCopy)**  
SenpaiCopy is a simple tool to copy image and video files from a source folder to one or more other folders. It supports a variety of image and video formats, with more to come in the future. It also allows for quickly searching an image on google. SenpaiCopy is made for people with huge amounts of folders to sort images into.  

**[Popcorn](https://github.com/bbougot/Popcorn)**    
An application which aims to provide a simple interface to watch any movie.  

**[Videre](https://github.com/Wolf-Code/Videre)**    
A simple, yet effective video player.

**[TwitchViewer](https://github.com/rguerraalmeida/TwitchViewer)**    
A simple Twitch client that uses Vlc and Livestream to watch twitch streams. 


**Meta 媒体套件：**   
_**Meta Media Suit:**_  
 
**[Meta.Bass](https://github.com/higankanshi/Meta.Bass)**  
Meta.Bass 是 [Bass](http://www.un4seen.com/bass.html) 库的 .NET 封装实现，用于多种格式的音频播放与解码。  
Meta.Bass is a Bass library wrapper for .NET, used to play/decode mutil format audio.   

**[Meta.Visualizer](https://github.com/higankanshi/Meta.Visualizer)**  
Zune 风格的音频可视化控件。  
*Zune style audio visualizer.*    
