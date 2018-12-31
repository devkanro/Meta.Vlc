// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: ModuleDescription.cs
// Version: 20181231

using System;
using System.Collections;
using System.Collections.Generic;
using Meta.Vlc.Interop.Core;

namespace Meta.Vlc
{
    /// <summary>
    ///     A wrapper for <see cref="libvlc_module_description_t" /> struct.
    /// </summary>
    public unsafe class ModuleDescription
    {
        internal ModuleDescription(libvlc_module_description_t* pointer)
        {
            if (pointer == null) return;
            Name = InteropHelper.PtrToString(pointer->psz_name);
            ShortName = InteropHelper.PtrToString(pointer->psz_shortname);
            LongName = InteropHelper.PtrToString(pointer->psz_longname);
            Help = InteropHelper.PtrToString(pointer->psz_help);
        }

        public string Name { get; }

        public string ShortName { get; }

        public string LongName { get; }

        public string Help { get; }
    }

    /// <summary>
    ///     A list wrapper for <see cref="libvlc_module_description_t" /> linked list struct.
    /// </summary>
    public unsafe class ModuleDescriptionList : VlcUnmanagedLinkedList<ModuleDescription>
    {
        public ModuleDescriptionList(void* pointer) : base(pointer)
        {
        }

        protected override ModuleDescription CreateItem(void* data)
        {
            return new ModuleDescription((libvlc_module_description_t*) data);
        }

        protected override void* NextItem(void* data)
        {
            return ((libvlc_module_description_t*) data)->p_next;
        }

        protected override void Release(void* data)
        {
            LibVlcManager.GetFunctionDelegate<libvlc_module_description_list_release>()
                .Invoke((libvlc_module_description_t*) data);
        }
    }
}