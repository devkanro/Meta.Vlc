//Project: xZune.Vlc (https://github.com/higankanshi/xZune.Vlc)
//Filename: LibVlcFunction.cs
//Version: 20160213

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace xZune.Vlc.Interop
{
    /// <summary>
    /// A dynamic mapper of LibVlc functions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LibVlcFunction<T>
    {
        /// <summary>
        /// Load a LibVlc function from unmanaged to managed.
        /// </summary>
        /// <exception cref="TypeLoadException">A custom attribute type cannot be loaded. </exception>
        /// <exception cref="NoLibVlcFunctionAttributeException">For LibVlcFunction, need LibVlcFunctionAttribute to get Infomation of function.</exception>
        /// <exception cref="FunctionNotFoundException">Can't find function in dll.</exception>
        public LibVlcFunction()
        {
            IsEnable = false;

            object[] attrs = typeof(T).GetCustomAttributes(typeof(LibVlcFunctionAttribute), false);
            
            foreach (var item in attrs)
            {
                if (item is LibVlcFunctionAttribute)
                {
                    FunctionInfomation = item as LibVlcFunctionAttribute;
                    break;
                }
            }

            if (FunctionInfomation == null)
            {
                throw new NoLibVlcFunctionAttributeException();
            }

            if (LibVlcManager.LibVlcVersion == null || LibVlcManager.LibVlcVersion.IsFunctionAvailable(FunctionInfomation))
            {
                IsEnable = true;

                IntPtr procAddress;
                try
                {
                    procAddress = Win32Api.GetProcAddress(LibVlcManager.LibVlcHandle, FunctionInfomation.FunctionName.Trim());
                }
                catch (Win32Exception e)
                {
                    throw new FunctionNotFoundException(FunctionInfomation, LibVlcManager.LibVlcVersion, e);
                }

                var del = Marshal.GetDelegateForFunctionPointer(procAddress, typeof(T));
                _functionDelegate = (T)Convert.ChangeType(del, typeof(T));
            }
        }

        /// <summary>
        /// Get this <see cref="LibVlcFunction{T}"/> is available or not.
        /// </summary>
        public bool IsEnable { get; private set; }

        /// <summary>
        /// Get infomation of this <see cref="LibVlcFunction{T}"/>.
        /// </summary>
        public LibVlcFunctionAttribute FunctionInfomation { get; private set; }

        private readonly T _functionDelegate;

        /// <summary>
        /// Get delegate of this <see cref="LibVlcFunction{T}"/>, if <see cref="IsEnable"/> is false, this method will throw exception.
        /// </summary>
        /// <exception cref="FunctionNotAvailableException" accessor="get">This function isn't available on current version LibVlc.</exception>
        public T Delegate
        {
            get
            {
                if (!IsEnable)
                {
                    throw new FunctionNotAvailableException(FunctionInfomation, LibVlcManager.LibVlcVersion);
                }
                return _functionDelegate;
            }
        }
    }

    /// <summary>
    /// Version infomation of LibVlc.
    /// </summary>
    public class LibVlcVersion
    {
        /// <summary>
        /// Create LibVlcVersion from version string, it must like "2.2.0-xZune Weatherwax".
        /// </summary>
        /// <param name="versionString">version string</param>
        /// <exception cref="VersionStringParseException">Can't parse libvlc version string, it must like "2.2.0-xZune Weatherwax".</exception>
        /// <exception cref="OverflowException">At least one component of version represents a number greater than <see cref="Int32.MaxValue" />.</exception>
        public LibVlcVersion(String versionString)
        {
            var match = Regex.Match(versionString.Trim(), @"^([0-9.]*)-([\S]*)(?: ([\S]*))?");
            if (!match.Success)
            {
                throw new VersionStringParseException(versionString);
            }

            switch (match.Groups.Count)
            {
                case 3:
                    Version = new Version(match.Groups[1].Value);
                    DevString = match.Groups[2].Value;
                    break;
                case 4:
                    Version = new Version(match.Groups[1].Value);
                    DevString = match.Groups[2].Value;
                    if (match.Groups[3].Success)
                    {
                        CodeName = match.Groups[3].Value;
                    }
                    break;
                default:
                    throw new VersionStringParseException(versionString);
            }
        }

        /// <summary>
        /// Version of LibVlc.
        /// </summary>
        public Version Version { get; private set; }

        /// <summary>
        /// DevString of LibVlc.
        /// </summary>
        public String DevString { get; private set; }

        /// <summary>
        /// Code name of LibVlc.
        /// </summary>
        public String CodeName { get; private set; }

        /// <summary>
        /// Check a function is available for this version.
        /// </summary>
        /// <param name="functionInfo"></param>
        /// <returns></returns>
        public bool IsFunctionAvailable(LibVlcFunctionAttribute functionInfo)
        {
            var result = true;

            if (functionInfo.MinVersion != null)
            {
                result = functionInfo.MinVersion < Version;
            }

            if (functionInfo.MaxVersion != null)
            {
                result = result && Version < functionInfo.MaxVersion;
            }

            if (functionInfo.Dev != null)
            {
                result = result && DevString == functionInfo.Dev;
            }

            return result;
        }
    }
}