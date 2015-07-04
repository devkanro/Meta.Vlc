using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using xZune.Vlc.Interop;
using xZune.Vlc.Interop.Core.Error;

namespace xZune.Vlc
{
    public static class VlcError
    {
        /// <summary>
        /// 载入 LibVlc 的 Error 模块,该方法会在 <see cref="Vlc.LoadLibVlc"/> 中自动被调用
        /// </summary>
        /// <param name="libHandle"></param>
        /// <param name="libVersion"></param>
        public static void LoadLibVlc(IntPtr libHandle, Version libVersion, String devString)
        {
            if (!IsLibLoaded)
            {
                ErrorMessageFunction = new LibVlcFunction<ErrorMessage>(libHandle, libVersion, devString);
                CleanErrorFunction = new LibVlcFunction<CleanError>(libHandle, libVersion, devString);
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

        static LibVlcFunction<ErrorMessage> ErrorMessageFunction;
        static LibVlcFunction<CleanError> CleanErrorFunction;
        
        public static String GetErrorMessage()
        {
            return InteropHelper.PtrToString(ErrorMessageFunction.Delegate());
        }

        public static void CleanError()
        {
            CleanErrorFunction.Delegate();
        }
    }
}
