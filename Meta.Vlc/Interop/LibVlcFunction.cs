// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: LibVlcFunction.cs
// Version: 20181231

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Meta.Vlc.Interop
{
    /// <summary>
    ///     A dynamic mapper of LibVlc functions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LibVlcFunction<T> where T : Delegate
    {
        private readonly T _functionDelegate;

        private readonly LibVlcVersion _version;

        /// <summary>
        ///     Load a LibVlc function from unmanaged to managed.
        /// </summary>
        /// <exception cref="TypeLoadException">A custom attribute type cannot be loaded. </exception>
        /// <exception cref="NoLibVlcFunctionAttributeException">
        ///     For LibVlcFunction, need LibVlcFunctionAttribute to get Infomation
        ///     of function.
        /// </exception>
        /// <exception cref="FunctionNotFoundException">Can't find function in dll.</exception>
        public LibVlcFunction(IntPtr libHandle, LibVlcVersion version)
        {
            IsEnable = false;
            _version = version;

            var attrs = typeof(T).GetCustomAttributes(typeof(LibVlcFunctionAttribute), false);

            foreach (var item in attrs)
                if (item is LibVlcFunctionAttribute)
                {
                    FunctionInfomation = item as LibVlcFunctionAttribute;
                    break;
                }

            if (FunctionInfomation == null) throw new NoLibVlcFunctionAttributeException();

            if (_version == null || _version.IsFunctionAvailable(FunctionInfomation))
            {
                IntPtr procAddress;
                try
                {
                    procAddress = Win32Api.GetProcAddress(libHandle, FunctionInfomation.FunctionName.Trim());
                }
                catch (Win32Exception e)
                {
                    throw new FunctionNotFoundException(FunctionInfomation, _version, e);
                }

                var del = Marshal.GetDelegateForFunctionPointer(procAddress, typeof(T));
                _functionDelegate = (T) Convert.ChangeType(del, typeof(T));
                IsEnable = true;
            }
        }

        /// <summary>
        ///     Get this <see cref="LibVlcFunction{T}" /> is available or not.
        /// </summary>
        public bool IsEnable { get; }

        /// <summary>
        ///     Get infomation of this <see cref="LibVlcFunction{T}" />.
        /// </summary>
        public LibVlcFunctionAttribute FunctionInfomation { get; }

        /// <summary>
        ///     Get delegate of this <see cref="LibVlcFunction{T}" />, if <see cref="IsEnable" /> is false, this method will throw
        ///     exception.
        /// </summary>
        /// <exception cref="FunctionNotAvailableException" accessor="get">This function isn't available on current version LibVlc.</exception>
        public T Delegate
        {
            get
            {
                if (!IsEnable) throw new FunctionNotAvailableException(FunctionInfomation, _version);
                return _functionDelegate;
            }
        }
    }
}