// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: IVlcObject.cs
// Version: 20181231

using System;

namespace Meta.Vlc
{
    /// <summary>
    ///     A unmanaged object.
    /// </summary>
    public unsafe interface IUnmanagedObject : IDisposable
    {
        /// <summary>
        ///     A pointer of this unmanaged object.
        /// </summary>
        void* InstancePointer { get; }
    }

    /// <summary>
    ///     A Vlc unmanaged object.
    /// </summary>
    public interface IVlcObject : IUnmanagedObject
    {
        /// <summary>
        ///     A relation <see cref="Vlc" /> of this object.
        /// </summary>
        Vlc VlcInstance { get; }
    }

    /// <summary>
    ///     A Vlc unmanaged object with Vlc event system.
    /// </summary>
    public interface IVlcObjectWithEvent : IVlcObject
    {
        /// <summary>
        ///     Vlc event manager.
        /// </summary>
        VlcEventManager EventManager { get; }
    }
}