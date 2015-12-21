# xZune.Vlc

## Introduction 

xZune.Vlc 是一个 LibVlc 封装库的 .NET 实现,封装了大部分的 LibVlc 的功能,该项目主要是为了寻求一个在 WPF 上使用 Vlc 的完美的解决方案,xZune.Vlc 提供一个原生的 WPF 播放控件(xZune.Vlc.Wpf),该控件采用 InteropBitmap 与共享内存进行播放视频,是一个原生的 WPF 控件,不存在 HwndHost 的空域问题.  

xZune.Vlc is an LibVlc solution for .NET, it has encapsulated most of functionalities of LibVlc. This project aims to find a perfect solution for using Vlc on WPF. xZune.Vlc provides an native WPF control(xZune.Vlc.Wpf), this control achieves video playback by utilizing InteropBitmap and shared memory. Since it’s a native WPF control, it doesn't suffer from HwndHost’s airspace issue.  

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
>xZune.Vlc.Lib include all files of LibVlc. You can use this dlls for xZune.Vlc and xZune.Vlc.Wpf.   
>LibVlc Version:   
>2.2.0-xZune Weatherwax   

## Build Status

Branch(分支) | Status(状态) | Description(描述)
--- | --- | ---
master | [![Build Status](https://ci.appveyor.com/api/projects/status/q76jlj04n40h2ygg/branch/master?svg=true)](https://ci.appveyor.com/project/higankanshi/xzune-vlc/branch/master) | xZune.Vlc

## Change Log  

###12.20
SHA: 80723064078314c70089496e2793d4f261d6e983  

01.可循环播放视频
通过设置 EndBehavior 来设置当媒体播放完毕后的动作,支持 Nothing,Stop,Repeat 三种模式。  
当设置为 Repeat 时,会自动的重新播放视频。

02.提供属性变更通知
现在已为大部分的属性提供属性变更通知。

03.新的 Stop 方法
更改 Stop 模式，大致与之前差不多，但是逻辑更为清晰与严谨。  
但是密集的调用 Stop 与其他控制媒体播放的方法(例如: Play)，仍然可能会导致资源死锁。  

04.添加注释
为大多数的公开类,属性与方法提供英文注释。  

01.Support loop the media  
You can set the EndBehavior property to set behavior when media ended. Support Nothing,Stop,Repeat mode.  
You can set it to Repeat to loop the media.

02.Support INotifyPropertyChanged  
Support some properties changed notify.

03.New Stop method group  
We changed the Stop methods, it will be more rigorous logic.  
But intensive media state operation may lead to deadlock.

04.Add more comments  
We provide more english comments for public class, methods and properties. 

###10.23
SHA: fba747bd44e190a79c3456bd97bad2396fd84122  
01.自动修复 DAR  
现在可以通过读取 SAR 来获取正确的 DAR 与显示分辨率,用于解决指定 SAR 下显示比例异常。

02.添加 AspectRatio 属性   
现在可以使用`VlcPlayer.AspectRatio`属性来将视频调整至指定的横宽比。

01.Auto correct DAR  
Now we can auto correct DAR and video display size by load SAR. It used for fix sometimes video size is wrong.

02.Add AspectRatio property  
Now we can change the aspect ratio of video by `VlcPlayer.AspectRatio` property.  

###07.18  
SHA: 5d4144ba64d568b36cd789e482e9ed1281525c25  
01.更改字符集至 UTF-8  
我们更改了 xZune.Vlc 与 LibVlc 的互操作字符串字符集至 UTF-8,解决中文路径的媒体文件无法播放的问题.  
  
02.添加 Title 与 Chapter 属性    
现在可以使用 `VlcPlayer.Title`与`VlcPlayer.Chapter`属性来控制多章节的媒体.  
  
01.Change the Character set to UTF-8  
We change the Character set to UTF-8,to fix some problem in chinese system.  

02.Add Title and Chapter properties  
Now you can use `VlcPlayer.Title`and`VlcPlayer.Chapter`to control the media which has multiple chapters.  
 
###07.15
SHA: 3fe6a38934f5418e77dd4c180579d471ec697dd5  
如果你仍然在使用旧版本的 xZune.Vlc,这里列举了一些新版本的 xZune.Vlc 所拥有新的特性.  
  
01.更多的 .NET 版本支持.  
在之前我们更改了 xZune.Vlc 的代码,使其能够在多个 .NET 版本上使用,以下是相关的兼容信息:  

.NET 版本 | xZune.Vlc | xZune.Vlc.Wpf | xZune.Vlc.Wpf.Sample
------------ | ------------- | ------------- | -------------
.NET 2.0 | :o: | :x: | :x:
.NET 3.0 | :o: | :x: | :x:
.NET 3.5 | :o: | :interrobang: | :o:
.NET 4.0 | :o: | :interrobang: | :o:
.NET 4.5 | :o: | :o: | :o:

>:o: :兼容     :x: :不兼容       :interrobang: :部分功能可能不可用  
  
02.更多控件初始化方案  
现在,你可以使用 3 种方案来初始化 VlcPlayer 控件,VlcSettingsAttribute 特性 和 VlcPlayer.LibVlcPath 属性仍然可用,当你不设置这些特性与属性时,你就需要手动调用 `Vlcplayer.Initialize(libVlcPath, libVlcOption)` 来初始化控件.  
他们之间的优先级为 VlcPlayer.LibVlcPath > VlcSettingsAttribute > Vlcplayer.Initialize  

03.新的 Stop 方法组  
现在你可以通过 3 个不同的 Stop 方法来停止媒体的播放.  
>VlcPlayer.Stop() 方法:  
>该方法将 Stop 操作提交给后台线程,由后台线程来完成 Stop 操作,所以会立即返回.但是 Stop 仍然是个耗时操作,所以请勿在此方法后调用其它媒体相关操作,以免造成死锁.  
>示例:  
>```CSharp
>vlcPlayer.Stop();
>//不要在此方法后调用媒体相关操作.  
>```
   
>VlcPlayer.BeginStop(AsyncCallback) 方法:  
>该方法将 Stop 操作提交给后台线程,并提供一个 AsyncCallback 当 Stop 操作完成,将调用此 AsyncCallback.  
>示例:  
>```CSharp
>vlcPlayer.BeginStop(ar =>
>{
>	//TODO: 在这里可进行媒体相关操作.
>});
>//不要在此方法后调用媒体相关操作,请在 AsyncCallback 中异步进行.  
>```
    
>VlcPlayer.StopAsync() 方法:  
>仅支持 .NET 4.5 ,该方法是 async/await 异步操作,使用 await 关键字确保 Stop 操作完成后,继续进行下面的操作.  
>示例:  
>```CSharp
>await vlcPlayer.StopAsync();
>//TODO: 在这里可进行媒体相关操作.
>```
  
If you are still using the old version of xZune.Vlc,Here are some new features of the new version.  
  
01.More .NET version support.  
We change the code of xZune.Vlc,make it to be used on multiple versions .NET,those are compatible informations:  

.NET Version | xZune.Vlc | xZune.Vlc.Wpf | xZune.Vlc.Wpf.Sample
------------ | ------------- | ------------- | -------------
.NET 2.0 | :o: | :x: | :x:
.NET 3.0 | :o: | :x: | :x:
.NET 3.5 | :o: | :interrobang: | :o:
.NET 4.0 | :o: | :interrobang: | :o:
.NET 4.5 | :o: | :o: | :o:

>:o: :Compatible :x: :Incompatible  :interrobang: :Some functions may not be available  
  
02.More way of control initialization   
Now,you have 3 ways to initialize VlcPlayer ,VlcSettingsAttribute and VlcPlayer.LibVlcPath also is available.When you don't set those,you need call `Vlcplayer.Initialize(libVlcPath, libVlcOption)` to initialize VlcPlayer.  
Priority: VlcPlayer.LibVlcPath > VlcSettingsAttribute > Vlcplayer.Initialize  

03.New Stop methods  
Now you can use 3 different way to Stop a media.  
>VlcPlayer.Stop() method:  
>This method send Stop operation to background threading, so it will immediately return.But Stop is still a time consuming operation. So please do not call other media operations after this method, so as to avoid deadlock.  
>Sample:  
>```CSharp
>vlcPlayer.Stop();
>//Do not call other media operations.  
>```
  
>VlcPlayer.BeginStop(AsyncCallback) method:  
>This method send Stop operation to background threading, and you can set a AsyncCallback parameter. When the Stop operation completed, AsyncCallback will be called.  
>Sample:  
>```CSharp
>vlcPlayer.BeginStop(ar =>
>{
>	//TODO: You can call other media operations here.
>});
>//Do not call other media operations, please call those in AsyncCallback.  
>```
  
>VlcPlayer.StopAsync() method:
>Only .NET 4.5 support, this method is a async/await operation, use await keyword to make it async.  
>Sample:  
>```CSharp
>await vlcPlayer.StopAsync();
>//TODO: You can call other media operations here.
>```

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

可以在程序结束时调用`vlcPlayer.Dispose()`释放所有资源.  


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

You can call the `vlcPlayer.Dispose()`to release the resource when you exit.
