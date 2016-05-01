// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: ModuleDescription.cs
// Version: 20160214

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Meta.Vlc
{
    /// <summary>
    ///     A warpper for <see cref="Interop.Core.ModuleDescription" /> struct.
    /// </summary>
    public class ModuleDescription
    {
        internal IntPtr _pointer;

        internal Interop.Core.ModuleDescription _struct;

        internal ModuleDescription(IntPtr pointer)
        {
            _pointer = pointer;
            if (pointer != IntPtr.Zero)
            {
                _struct =
                    (Interop.Core.ModuleDescription)
                        Marshal.PtrToStructure(pointer, typeof (Interop.Core.ModuleDescription));
                Name = InteropHelper.PtrToString(_struct.Name);
                ShortName = InteropHelper.PtrToString(_struct.ShortName);
                LongName = InteropHelper.PtrToString(_struct.LongName);
                Help = InteropHelper.PtrToString(_struct.Help);
            }
        }

        public String Name { get; private set; }

        public String ShortName { get; private set; }

        public String LongName { get; private set; }

        public String Help { get; private set; }
    }

    /// <summary>
    ///     A list warpper for <see cref="Interop.Core.ModuleDescription" /> linklist struct.
    /// </summary>
    public class ModuleDescriptionList : IDisposable, IEnumerable<ModuleDescription>, IEnumerable
    {
        private List<ModuleDescription> _list;
        private IntPtr _pointer;

        /// <summary>
        ///     Create a readonly list by a pointer of <see cref="Interop.Core.ModuleDescription" />.
        /// </summary>
        /// <param name="pointer"></param>
        public ModuleDescriptionList(IntPtr pointer)
        {
            _list = new List<ModuleDescription>();
            _pointer = pointer;

            while (pointer != IntPtr.Zero)
            {
                var ModuleDescription = new ModuleDescription(pointer);
                _list.Add(ModuleDescription);

                pointer = ModuleDescription._struct.Next;
            }
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public ModuleDescription this[int index]
        {
            get { return _list[index]; }
        }

        public void Dispose()
        {
            if (_pointer == IntPtr.Zero) return;

            LibVlcManager.ReleaseModuleDescriptionList(_pointer);
            _pointer = IntPtr.Zero;
            _list.Clear();
        }

        public IEnumerator<ModuleDescription> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}