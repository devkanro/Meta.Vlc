# xZune.Vlc

## Introduction

xZune.Vlc 是一个 LibVlc 封装库的 .NET 实现,封装了大部分的 LibVlc 的功能,该项目主要是为了寻求一个在 WPF 上使用 Vlc 的完美的解决方案,xZune.Vlc 提供一个原生的 WPF 播放控件(xZune.Vlc.Wpf),该控件采用 InteropBitmap 与共享内存进行播放视频,是一个原生的 WPF 控件,不存在 HwndHost 的空域问题.

xZune.Vlc is an LibVlc solution for .NET, it has encapsulated most of functionalities of LibVlc. This project aims to find a perfect solution for using Vlc on WPF. xZune.Vlc provides an native WPF control(xZune.Vlc.Wpf), this control achieves video playback by utilizing InteropBitmap and shared memory. Since it’s a native WPF control, it doesn't suffer from HwndHost’s airspace issue.

## Quick Start

在您的项目中快速使用 xZune.Vlc:

01.在项目中添加对 xZune.Vlc 的程序集的引用.
```
xZune.Vlc.dll
xZune.Vlc.Wpf.dll
```

02.在项目的属性中设置 LibVlc 库的目录.
在`$(Your Project)\Properties\AssemblyInfo.cs`中添加 VlcSettingsAttribute.
```CSharp
//设置 LibVlc 的目录
[assembly: VlcSettings(@"..\..\..\LibVlc")] 

//设置 LibVlc 初始化时的选项
[assembly: VlcSettings(@"..\..\..\LibVlc",new String[] { "-I", "dummy", "--ignore-config", "--no-video-title" })]
```
>参考:
>https://github.com/higankanshi/xZune.Vlc/blob/master/xZune.Vlc.Wpf.Sample/Properties/AssemblyInfo.cs

03.在 Xaml 中加入 VlcPlayer 控件.
```XAML
<wpf:VlcPlayer xmlns:wpf="clr-namespace:xZune.Vlc.Wpf;assembly=xZune.Vlc.Wpf" x:Name="vlcPlayer"/>
```

04.载入媒体并播放.
```CSharp
vlcPlayer.LoadMedia(@"C:\VlcTest.mp4"); //载入本地文件
//vlcPlayer.LoadMedia(@"H:\"); //载入DVD光盘
//vlcPlayer.LoadMedia(new Uri("http://127.0.0.1")); //载入网络流
vlcPlayer.Play(); //Play the media 播放媒体
```

05.停止媒体与释放资源.

可以使用`await vlcPlayer.Stop()`在一个异步方法内停止视频播放,该操作是一个异步操作,需要 50ms 以上的延迟来完成停止操作,请务必使用`await`关键字保证 vlcPlayer 的停止播放结束后再载入新的视频,以免发生死锁.

可以在程序结束时调用`vlcPlayer.Dispose()`与`xZune.Vlc.Wpf.ApiManager.ReleaseAll()`释放所有资源.


Quick start xZune.Vlc in your project:

01.Add the references of xZune.Vlc to your project.
```
xZune.Vlc.dll
xZune.Vlc.Wpf.dll
```

02.Set the path of LibVlc in properties of your project.
Add a VlcSettingsAttribute in `$(Your Project)\Properties\AssemblyInfo.cs`. 
```CSharp
//Set the path of LibVlc
[assembly: VlcSettings(@"..\..\..\LibVlc")] 

//Set the path and options of LibVlc
[assembly: VlcSettings(@"..\..\..\LibVlc",new String[] { "-I", "dummy", "--ignore-config", "--no-video-title" })]
```
>See:
>https://github.com/higankanshi/xZune.Vlc/blob/master/xZune.Vlc.Wpf.Sample/Properties/AssemblyInfo.cs

03.Add the VlcPlayer Control in your Xaml
```XAML
<wpf:VlcPlayer xmlns:wpf="clr-namespace:xZune.Vlc.Wpf;assembly=xZune.Vlc.Wpf" x:Name="vlcPlayer"/>
```

04.Load and Play the media.
```CSharp
vlcPlayer.LoadMedia(@"C:\VlcTest.mp4"); //Load a local file
//vlcPlayer.LoadMedia(@"H:\"); //Load a DVD
//vlcPlayer.LoadMedia(new Uri("http://127.0.0.1")); //Load a Network Stream
vlcPlayer.Play(); //Play the media
```

05.Stop the media and Release the resources.

You can use `await vlcPlayer.Stop()`to stop a media in a async method,this operation is a async operation,we need at least 50ms to stop the media.Please use the `await` KeyWord to keep load a new media after stop old media over,in order to avoid a death lock.

You can call the `vlcPlayer.Dispose()` and `xZune.Vlc.Wpf.ApiManager.ReleaseAll()` to release the resource when you exit.
