using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace xZune.Vlc.Interop
{
    public class LibVlcFunction<T>
    {
        /// <summary>
        /// 使用提供的 LibVlc 库句柄初始化一个 LibVlcFunction,不指定库版本
        /// </summary>
        /// <param name="libHandle">提供的 LibVlc 库句柄</param>
        public LibVlcFunction(IntPtr libHandle) : this(libHandle, null, null)
        {

        }

        /// <summary>
        /// 使用提供的指定版本的 LibVlc 库句柄初始化一个 LibVlcFunction
        /// </summary>
        /// <param name="libHandle">提供的 LibVlc 库句柄</param>
        /// <param name="libVersion">库版本</param>
        public LibVlcFunction(IntPtr libHandle,Version libVersion,String Dev)
        {
            IsEnable = false;
            object[] attrs = typeof(T).GetCustomAttributes(typeof(LibVlcFunctionAttribute), false);

            LibVlcFunctionAttribute functionInfo = null;
            foreach (var item in attrs)
            {
                if(item is LibVlcFunctionAttribute)
                {
                    functionInfo = item as LibVlcFunctionAttribute;
                    break;
                }
            }

            if(functionInfo == null)
            {
                throw new Exception("对于 LibVlcFunction,需要添加 LibVlcFunctionAttribute 才能正常读取函数");
            }

            FunctionName = functionInfo.FunctionName;
            if((libVersion == null || ((functionInfo.MinVersion == null || functionInfo.MinVersion <= libVersion) && (functionInfo.MaxVersion == null || functionInfo.MaxVersion >= libVersion))) && (functionInfo.Dev == null || functionInfo.Dev == Dev))
            {
                IsEnable = true;

                IntPtr procAddress;
                try
                {
                    procAddress = Win32Api.GetProcAddress(libHandle, FunctionName);
                }
                catch (Win32Exception e)
                {
                    throw new Exception(String.Format("在提供的 LibVlc 中找不到函数 {0}", FunctionName), e);
                }

                var del = Marshal.GetDelegateForFunctionPointer(procAddress, typeof(T));
                functionDelegate = (T)Convert.ChangeType(del, typeof(T));
            }
        }

        /// <summary>
        /// 获取一个值,该值指示该 <see cref="LibVlcFunction{T}"/> 是否正确载入,并且可用
        /// </summary>
        public bool IsEnable { get; private set; }

        /// <summary>
        /// 获取一个字符串,表示该函数在 LibVlc 中的名称,类似于 "libvlc_get_version"
        /// </summary>
        public String FunctionName { get; private set; }


        private T functionDelegate;

        /// <summary>
        /// 获取当前该 LibVlcFunction 的委托,当 IsEnable 属性为 False 时会抛出异常
        /// </summary>
        public T Delegate
        {
            get
            {
                if(!IsEnable)
                {
                    throw new Exception(String.Format("该函数不可用,请确保当前 LibVlc 版本中{0}函数可用", FunctionName));
                }
                return functionDelegate;
            }
        }
    }
}
