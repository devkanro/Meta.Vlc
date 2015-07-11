using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace xZune.Vlc
{
    public class ModuleDescription : IDisposable
    {
        /// <summary>
        /// 通过指针来初始化 ModuleDescription
        /// </summary>
        /// <param name="pointer">提供的指针</param>
        public ModuleDescription(IntPtr pointer)
        {
            List<ModuleDescriptionItem> itemsList = new List<ModuleDescriptionItem>();
            
            while (pointer != IntPtr.Zero)
            {
                var moduleDescriptionStruct = (Interop.Core.ModuleDescription)Marshal.PtrToStructure(pointer, typeof(Interop.Core.ModuleDescription));
                itemsList.Add(new ModuleDescriptionItem(moduleDescriptionStruct));
                pointer = moduleDescriptionStruct.Next;
            }

            Items = itemsList.ToArray();
        }

        /// <summary>
        /// 获取这个 ModuleDescription 的指针
        /// </summary>
        public IntPtr Pointer { get; private set; }

        /// <summary>
        /// 获取这个 ModuleDescription 包含的子项
        /// </summary>
        public ModuleDescriptionItem[] Items { get; private set; }

        /// <summary>
        /// 获取这个 ModuleDescription 包含的子项的个数
        /// </summary>
        public int Count
        {
            get
            {
                return Items.Length;
            }
        }

        /// <summary>
        /// 释放当前的 ModuleDescription 资源
        /// </summary>
        public void Dispose()
        {
            Vlc.ReleaseModuleDescription(this);
            Items = null;
            Pointer = IntPtr.Zero;
        }
    }

    public class ModuleDescriptionItem
    {
        public ModuleDescriptionItem(Interop.Core.ModuleDescription @struct)
        {
            Name = @struct.Name;
            ShortName = @struct.ShortName;
            LongName = @struct.LongName;
            Description = @struct.Help;
        }

        public String Name { get; private set; }
        public String ShortName { get; private set; }
        public String LongName { get; private set; }
        public String Description { get; private set; }
    }
}
