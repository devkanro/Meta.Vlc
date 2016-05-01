// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: AssemblyInfo.cs
// Version: 20160312

using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using Meta.Vlc;

[assembly: AssemblyTitle("Meta.Vlc.Wpf.Sample")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("higan")]
[assembly: AssemblyProduct("Meta.Vlc.Wpf.Sample")]
[assembly: AssemblyCopyright("Copyright ©  2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: VlcSettings(@"..\..\libvlc", "-I", "--dummy-quiet", "--ignore-config", "--no-video-title", "--no-sub-autodetect-file")]
//[assembly: VlcSettings(@"..\..\libvlc", "-I dummy", "--ignore-config", "--no-video-title", "--file-logging", "--logfile=log.txt", "--verbose=2", "--no-sub-autodetect-file" //,"--rtsp-tcp"
//    )]
//note: you may need to add the option --rtsp-tcp to pass RTSP through a VPN (e.g if you want to access some IP Camera behind a firewall by tunneling to its local network via VPN)

[assembly: ComVisible(false)]
//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None,
    ResourceDictionaryLocation.SourceAssembly
    )]
[assembly: AssemblyVersion("16.05.01.0")]
[assembly: AssemblyFileVersion("16.05.01.0")]