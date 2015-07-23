using System;

namespace xZune.Vlc
{
    public interface IVlcObject : IDisposable
    {
        IntPtr InstancePointer { get; }
    }
}
