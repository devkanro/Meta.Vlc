using System;

using xZune.Vlc.Interop;
using xZune.Vlc.Interop.Core.Error;

namespace xZune.Vlc
{
    public static class VlcError
    {
        /// <summary>
        /// 载入 LibVlc 的 Error 模块,该方法会在 <see cref="Vlc.LoadLibVlc()"/> 中自动被调用
        /// </summary>
        /// <param name="libHandle"></param>
        /// <param name="libVersion"></param>
        /// <param name="devString"></param>
        public static void LoadLibVlc(IntPtr libHandle, Version libVersion, String devString)
        {
            if (!IsLibLoaded)
            {
                _errorMessageFunction = new LibVlcFunction<ErrorMessage>(libHandle, libVersion, devString);
                _cleanErrorFunction = new LibVlcFunction<CleanError>(libHandle, libVersion, devString);
                IsLibLoaded = true;
            }
        }

        /// <summary>
        /// 获取一个值,该值指示当前模块是否被载入
        /// </summary>
        public static bool IsLibLoaded
        {
            get;
            private set;
        }

        static LibVlcFunction<ErrorMessage> _errorMessageFunction;
        static LibVlcFunction<CleanError> _cleanErrorFunction;

        /// <summary>
        /// 获取一个可读的 LibVlc 错误信息
        /// </summary>
        /// <returns>返回一个可读的 LibVlc 错误信息,如果没有错误信息将返回 NULL</returns>
        public static String GetErrorMessage()
        {
            return InteropHelper.PtrToString(_errorMessageFunction.Delegate());
        }

        /// <summary>
        /// 清除当前线程的 LibVlc 的错误信息
        /// </summary>
        public static void CleanError()
        {
            _cleanErrorFunction.Delegate();
        }
    }
}
