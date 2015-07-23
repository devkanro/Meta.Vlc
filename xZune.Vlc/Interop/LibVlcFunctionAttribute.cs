using System;

namespace xZune.Vlc.Interop
{
    /// <summary>
    /// 为 LibVlc 函数委托初始化提供必要的信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Delegate, AllowMultiple = true)]
    public class LibVlcFunctionAttribute : Attribute
    {
        /// <summary>
        /// 获取一个值,表示函数在 LibVlc 中的名称
        /// </summary>
        public string FunctionName { get; private set; }
        /// <summary>
        /// 获取一个值,表示支持该函数的最小 LibVlc 版本
        /// </summary>
        public Version MinVersion { get; private set; }
        /// <summary>
        /// 获取一个值,表示支持该函数的最大 LibVlc 版本
        /// </summary>
        public Version MaxVersion { get; private set; }

        /// <summary>
        /// 获取一个值,表示特定的开发版本
        /// </summary>
        public String Dev { get; private set; }

        /// <summary>
        /// 指定该委托在 LibVlc 中的函数名,不限定 LibVlc 的版本
        /// </summary>
        /// <param name="functionName">函数名</param>
        public LibVlcFunctionAttribute(string functionName)
            : this(functionName, null)
        {
        }

        /// <summary>
        /// 指定该委托在 LibVlc 中的函数名,并要求不低于指定版本的 LibVlc
        /// </summary>
        /// <param name="functionName">函数名</param>
        /// <param name="minVersion">最低支持的 LibVlc</param>
        public LibVlcFunctionAttribute(string functionName, string minVersion)
            : this(functionName, minVersion, null)
        {
        }

        /// <summary>
        /// 指定该委托在 LibVlc 中的函数名,并要求不低于指定版本的 LibVlc,也不高于指定的最大版本
        /// </summary>
        /// <param name="functionName">函数名</param>
        /// <param name="minVersion">最低支持的 LibVlc</param>
        /// <param name="maxVersion">最高支持的 LibVlc</param>
        public LibVlcFunctionAttribute(string functionName, string minVersion, string maxVersion) 
            : this(functionName, minVersion, maxVersion, null)
        {
        }

        /// <summary>
        /// 指定该委托在 LibVlc 中的函数名,并要求不低于指定版本的 LibVlc,也不高于指定的最大版本
        /// </summary>
        /// <param name="functionName">函数名</param>
        /// <param name="minVersion">最低支持的 LibVlc</param>
        /// <param name="maxVersion">最高支持的 LibVlc</param>
        /// <param name="dev">特定支持的 LibVlc 开发版本</param>
        public LibVlcFunctionAttribute(string functionName, string minVersion, string maxVersion,string dev)
        {
            FunctionName = functionName;
            if (minVersion != null)
                MinVersion = new Version(minVersion);
            if (maxVersion != null)
                MaxVersion = new Version(maxVersion);
            if (dev != null)
                Dev = dev;
        }
    }
}
