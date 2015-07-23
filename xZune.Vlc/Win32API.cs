using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace xZune.Vlc
{
    public static class Win32Api
    {

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi ,EntryPoint = "LoadLibrary")]
        private static extern IntPtr LoadLibraryStatic(string lpFileName);

        /// <summary>
        /// 进程调用 LoadLibrary 以显式链接到 DLL,如果函数执行成功,它会将指定的 DLL 映射到调用进程的地址空间中并返回该 DLL 的句柄,此句柄可以与其他函数(如 GetProcAddress 和 FreeLibrary)一起在显式链接中使用
        /// LoadLibrary 将尝试使用用于隐式链接的相同搜索序列来查找 DLL.如果系统无法找到所需的 DLL 或者入口点函数返回 FALSE.则 LoadLibrary 将抛出异常.如果对 LoadLibrary 的调用所指定的 DLL 模块已映射到调用进程的地址空间中,则该函数将返回该 DLL 的句柄并递增模块的引用数
        /// </summary>
        /// <param name="lpFileName">DLL 模块地址</param>
        /// <returns>返回 DLL 模块句柄,如果出错将抛出异常</returns>
        public static IntPtr LoadLibrary(string lpFileName)
        {
            if(!System.IO.File.Exists(lpFileName))
            {
                throw new Exception(String.Format("模块文件不存在:{0}", lpFileName));
            }
            var result = LoadLibraryStatic(lpFileName);
            if (result != IntPtr.Zero) return result;
            var error = GetLastError();
            if (error == 0)
            {
                throw new Win32Exception("无法载入指定的模块,未知错误.");
            }
            else
            {
                throw new Win32Exception(error, "无法载入指定的模块.");
            }
        }

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true, EntryPoint = "GetProcAddress")]
        private static extern IntPtr GetProcAddressStatic(IntPtr hModule, string lpProcName);

        /// <summary>
        /// 显式链接到 DLL 的进程调用 GetProcAddress 来获取 DLL 导出函数的地址,由于是通过指针调用 DLL 函数并且没有编译时类型检查,需确保函数的参数是正确的,以便不会超出在堆栈上分配的内存和不会导致访问冲突
        /// </summary>
        /// <param name="hModule">DLL 模块句柄</param>
        /// <param name="lpProcName">调用的函数名</param>
        /// <returns>返回函数地址</returns>
        public static IntPtr GetProcAddress(IntPtr hModule, string lpProcName)
        {
            IntPtr result = GetProcAddressStatic(hModule, lpProcName);
            if (result == IntPtr.Zero)
            {
                int error = GetLastError();
                if (error == 0)
                {
                    throw new Win32Exception("无法获取函数地址,未知错误.");
                }
                else
                {
                    throw new Win32Exception(error, "无法获取函数地址.");
                }
            }
            return result;
        }

        /// <summary>
        /// 不再需要 DLL 模块时,显式链接到 DLL 的进程调用 FreeLibrary 函数.此函数递减模块的引用数,如果引用数为零,此函数便从进程的地址空间中取消模块的映射
        /// </summary>
        /// <param name="hModule">DLL 模块句柄</param>
        /// <returns>如果成功会返回 true ,否则会返回 false,请通过 GetLastError 获取更多信息</returns>
        [DllImport("kernel32", SetLastError = true)]
        public static extern bool FreeLibrary(IntPtr hModule);

        /// <summary>
        /// 创建一个新的文件映射内核对象
        /// </summary>
        /// <param name="hFile">指定欲在其中创建映射的一个文件句柄,为0xFFFFFFFF则表示创建一个内存文件映射</param>
        /// <param name="lpAttributes">它指明返回的句柄是否可以被子进程所继承,使用 NULL 表示使用默认安全设置</param>
        /// <param name="flProtect">指定文件映射对象的页面保护</param>
        /// <param name="dwMaximumSizeHigh">表示映射文件大小的高32位</param>
        /// <param name="dwMaximumSizeLow">表示映射文件大小的低32位</param>
        /// <param name="lpName">指定文件映射对象的名字,如果为 NULL 则会创建一个无名称的文件映射对象</param>
        /// <returns>返回文件映射对象指针,如果错误将返回 NULL,请通过 GetLastError 获取更多信息</returns>
        [DllImport("kernel32", SetLastError = true)]
        public static extern IntPtr CreateFileMapping(IntPtr hFile, IntPtr lpAttributes, PageAccess flProtect, int dwMaximumSizeHigh, int dwMaximumSizeLow, string lpName);

        /// <summary>
        /// 将一个文件映射对象映射到当前应用程序的地址空间
        /// </summary>
        /// <param name="hFileMappingObject">文件映射对象的句柄</param>
        /// <param name="dwDesiredAccess">映射对象的文件数据的访问方式,而且同样要与 CreateFileMapping 函数所设置的保护属性相匹配</param>
        /// <param name="dwFileOffsetHigh">表示文件映射起始偏移的高32位</param>
        /// <param name="dwFileOffsetLow">表示文件映射起始偏移的低32位</param>
        /// <param name="dwNumberOfBytesToMap">指定映射文件的字节数</param>
        /// <returns>返回文件映射在内存中的起始地址,如果错误将返回 NULL,请通过 GetLastError 获取更多信息</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject, FileMapAccess dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, uint dwNumberOfBytesToMap);

        [DllImport("kernel32", SetLastError = true)]
        public static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

        /// <summary>
        /// 关闭一个内核对象.其中包括文件,文件映射,进程,线程,安全和同步对象等
        /// </summary>
        /// <param name="handle">欲关闭的一个对象的句柄</param>
        /// <returns>如果成功会返回 true ,否则会返回 false,请通过 GetLastError 获取更多信息</returns>
        [DllImport("kernel32", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);


        [DllImport("kernel32")]
        public static extern int GetLastError();
    }

    public enum PageAccess
    {
        NoAccess = 0x01,
        ReadOnly = 0x02,
        ReadWrite = 0x04,
        WriteCopy = 0x08,
        Execute = 0x10,
        ExecuteRead = 0x20,
        ExecuteReadWrite = 0x40,
        ExecuteWriteCopy = 0x80,
        Guard = 0x100,
        NoCache = 0x200,
        WriteCombine = 0x400
    }

    public enum FileMapAccess : uint
    {
        Write = 0x00000002,
        Read = 0x00000004,
        AllAccess = 0x000f001f,
        Copy = 0x00000001,
        Execute = 0x00000020
    }
}
