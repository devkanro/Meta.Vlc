# xZune.Vlc

## Introduction 

xZune.Vlc 是一个 LibVlc 封装库的 .NET 实现,封装了大部分的 LibVlc 的功能,该项目主要是为了寻求一个在 WPF 上使用 Vlc 的完美的解决方案,xZune.Vlc 提供一个原生的 WPF 播放控件(xZune.Vlc.Wpf),该控件采用 InteropBitmap 与共享内存进行播放视频,是一个原生的 WPF 控件,不存在 HwndHost 的空域问题.  

_xZune.Vlc is an LibVlc solution for .NET, it has encapsulated most of functionalities of LibVlc. This project aims to find a perfect solution for using Vlc on WPF. xZune.Vlc provides an native WPF control(xZune.Vlc.Wpf), this control achieves video playback by utilizing InteropBitmap and shared memory. Since it’s a native WPF control, it doesn't suffer from HwndHost’s airspace issue._  

## NuGet Packages

Install [xZune.Vlc](https://www.nuget.org/packages/xZune.Vlc/)
```Powershell
PM> Install-Package xZune.Vlc 
```

Install [xZune.Vlc.Wpf](https://www.nuget.org/packages/xZune.Vlc.Wpf/)
```Powershell
PM> Install-Package xZune.Vlc.Wpf 
```

Install [xZune.Vlc.Lib](https://www.nuget.org/packages/xZune.Vlc.Lib/)
```Powershell
PM> Install-Package xZune.Vlc.Lib
```
>xZune.Vlc.Lib 包含所有 LibVlc 的文件，你可以在 xZune.Vlc 与 xZune.Vlc.Wpf 中使用这些 Dll。  
>LibVlc 的版本：  
>2.2.0-xZune Weatherwax  
>   
>_xZune.Vlc.Lib include all files of LibVlc. You can use this dlls for xZune.Vlc and xZune.Vlc.Wpf._   
>_LibVlc Version:_   
>_2.2.0-xZune Weatherwax_   

## Build Status

Branch(分支) | Status(状态) | Description(描述)
--- | --- | ---
master | [![Build Status](https://ci.appveyor.com/api/projects/status/q76jlj04n40h2ygg/branch/master?svg=true)](https://ci.appveyor.com/project/higankanshi/xzune-vlc/branch/master) | xZune.Vlc

## .Net Support
.NET Version | xZune.Vlc | xZune.Vlc.Wpf | xZune.Vlc.Wpf.Sample
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
> 本次更改更倾向于底层代码优化与清理，如果你主要使用 xZune.Vlc 而不是 xZune.Vlc.Wpf，你可能需要做比较多的更改来兼容这次变更。  
> 如果你只是使用 xZune.Vlc.Wpf 只是部分涉及到 `TrackDescription` 结构，`MediaTrack` 结构，或是其他结构的 API 有小部分变更。   
>  
> _This commit is only changed and clear up some lowest layer code, if you using xZune.Vlc not xZune.Vlc.Wpf, maybe you should changed many code to compatible this commit._  
> _If you using xZune.Vlc.Wpf, only some APIs about `TrackDescription` struct, `MediaTrack` struct or other struct have changed._

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

在您的项目中快速使用 xZune.Vlc：  
_Quick start xZune.Vlc in your project:_  

**01.在项目中添加对 xZune.Vlc 的程序集的引用。**    
_**01.Add the references of xZune.Vlc to your project.**_  
```
xZune.Vlc.dll
xZune.Vlc.Wpf.dll
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
>https://github.com/higankanshi/xZune.Vlc/blob/master/xZune.Vlc.Wpf.Sample/Properties/AssemblyInfo.cs

**03.在 Xaml 中加入 VlcPlayer 控件。**   
_**03.Add the VlcPlayer Control in your Xaml**_  
```XAML
<wpf:VlcPlayer xmlns:wpf="clr-namespace:xZune.Vlc.Wpf;assembly=xZune.Vlc.Wpf" x:Name="vlcPlayer"/>
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

`vlcPlayer.Stop()`在 UI 线程被调用会导致出现死锁，所以它不能在 UI 线程被调用，你需要将所有和其他一起的操作异步进行。   
`vlcPlayer.BeginStop()`提供简单的回调模型的异步操作，需要所有结束后的媒体操作都在回调中进行。    

_If you call `vlcPlayer.Stop()` on UI thread, it will case deadlock, so you should call it and other media operations what be called after stop by async._  
_`vlcPlayer.BeginStop()` provide a easy way call stop on UI thread, but you should use the callback._  
__
```CSharp
// 在 .NET 4.5 中使用 Stop
// Use Stop with .NET 4.5

Task.Run(() => 
{
  VlcPlayer.Stop();
  VlcPlayer.Play(); // 或者其他媒体操作。         Or other media operations
}
```
```CSharp
// 在 .NET 4.0(及以下) 中使用 Stop
// Use Stop with .NET 4.0(and lower)

Action stopAction = () => 
{
  VlcPlayer.Stop();
  VlcPlayer.Play(); // 或者其他媒体操作。         Or other media operations
}

stopAction.EasyInvoke();
```
```CSharp
// 使用 BeginStop
// Use BeginStop

VlcPlayer.BeginStop(()=>
{
  VlcPlayer.Play(); // 或者其他媒体操作。         Or other media operations
});
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
> `xZune.Vlc.VlcMediaPlayer` 不会引用均衡器实例，仅会复制其值，但是 AudioEqualizer 提供了属性变更通知，你可以使用其属性变更事件来重新设置均衡器。   
> `xZune.Vlc.Wpf.VlcPlayer` 会引用均衡器实例，并且监听了当前均衡器的属性变更事件，当当前均衡器有变更会自动的为 VlcMediaPlayer 重新设置均衡器。  
>   
> The `xZune.Vlc.VlcMediaPlayer` **will not** keep a reference to the supplied equalizer so you need set it again after you changed some value of `AudioEqualizer`, we provide PropertyChanged event for `AudioEqualizer` you can use this to reset equalizer.   
> The `xZune.Vlc.Wpf.VlcPlayer` **will** keep a reference to the supplied equalizer, when you changed some value of `AudioEqualizer`, it will auto reset equalizer for `VlcMediaPlayer`.  

  
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
xZune.Vlc 使用了某些 VLC 的拓展功能，我们目前提供 LibVlc(2.2.0-xZune) 32bit 版本。如果您需要在更低或者更高的 VLC 版本上使用 xZune.Vlc，您可能需要自己编译 VLC for xZune.Vlc，以保证 xZune.Vlc 拓展功能可用。  
*xZune.Vlc has used some "Expansions" for VLC, we are providing LibVlc(2.2.0-xZune) 32bit version now.But you want to use xZune.Vlc with later or lower version,you could need compile VLC for xZune.Vlc by yourself,to ensure that the expansions are available.*  

查看 [Compile VLC for xZune.Vlc](https://github.com/higankanshi/xZune.Vlc/wiki/Compile-VLC-for-xZune.Vlc) 来获取编译 xZune 专用的 VLC 教程。   
*See [Compile VLC for xZune.Vlc](https://github.com/higankanshi/xZune.Vlc/wiki/Compile-VLC-for-xZune.Vlc) to get some infomation about compile VLC for xZune.*   

## Other Project  
**已经在使用 xZune.Vlc 的项目：**  
_**Already in use xZune.Vlc:**_  

**[FoscamController](http://foscamcontroller.codeplex.com/)**    
This is a Library and Demo WPF program for controlling MJPEG and HD models of Foscam IP PTZ (Pan-Tilt-Zoom) Cameras and displaying MJPEG and H.264/RTSP video.   

**[FiloStreamer](https://github.com/TheThing/FiloStreamer)**  
FiloStreamer is a simple GUI application frontend on top of ffmpeg, designed to simplify and ease configuring and setting up local live stream from a Blackmagic Decklink card source.     

**[SenpaiCopy](https://github.com/coczero/SenpaiCopy)**  
SenpaiCopy is a simple tool to copy image and video files from a source folder to one or more other folders. It supports a variety of image and video formats, with more to come in the future. It also allows for quickly searching an image on google. SenpaiCopy is made for people with huge amounts of folders to sort images into.  

**[Popcorn](https://github.com/bbougot/Popcorn)**    
An application which aims to provide a simple interface to watch any movie.  

**xZune 系列：**  
_**xZune Suit:**_  

**[xZune.Visualizer](https://github.com/higankanshi/xZune.Visualizer)**  
Zune 风格的音频可视化控件。  
*Zune style audio visualizer.*  
