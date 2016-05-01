// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: LibVlcFunction.cs
// Version: 20160214

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Meta.Vlc.Interop
{
    /// <summary>
    ///     A dynamic mapper of LibVlc functions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LibVlcFunction<T>
    {
        private readonly T _functionDelegate;

        /// <summary>
        ///     Load a LibVlc function from unmanaged to managed.
        /// </summary>
        /// <exception cref="TypeLoadException">A custom attribute type cannot be loaded. </exception>
        /// <exception cref="NoLibVlcFunctionAttributeException">
        ///     For LibVlcFunction, need LibVlcFunctionAttribute to get Infomation
        ///     of function.
        /// </exception>
        /// <exception cref="FunctionNotFoundException">Can't find function in dll.</exception>
        public LibVlcFunction()
        {
            IsEnable = false;

            object[] attrs = typeof (T).GetCustomAttributes(typeof (LibVlcFunctionAttribute), false);

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

            if (LibVlcManager.LibVlcVersion == null ||
                LibVlcManager.LibVlcVersion.IsFunctionAvailable(FunctionInfomation))
            {
                IntPtr procAddress;
                try
                {
                    procAddress = Win32Api.GetProcAddress(LibVlcManager.LibVlcHandle,
                        FunctionInfomation.FunctionName.Trim());
                }
                catch (Win32Exception e)
                {
                    throw new FunctionNotFoundException(FunctionInfomation, LibVlcManager.LibVlcVersion, e);
                }

                var del = Marshal.GetDelegateForFunctionPointer(procAddress, typeof (T));
                _functionDelegate = (T) Convert.ChangeType(del, typeof (T));
                IsEnable = true;
            }
        }

        /// <summary>
        ///     Get this <see cref="LibVlcFunction{T}" /> is available or not.
        /// </summary>
        public bool IsEnable { get; private set; }

        /// <summary>
        ///     Get infomation of this <see cref="LibVlcFunction{T}" />.
        /// </summary>
        public LibVlcFunctionAttribute FunctionInfomation { get; private set; }

        /// <summary>
        ///     Get delegate of this <see cref="LibVlcFunction{T}" />, if <see cref="IsEnable" /> is false, this method will throw
        ///     exception.
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
}