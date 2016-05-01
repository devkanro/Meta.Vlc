// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: Exceptions.cs
// Version: 20160214

using System;
using Meta.Vlc.Interop;

namespace Meta.Vlc
{
    /// <summary>
    ///     A base class of LibVlc exceptions.
    /// </summary>
    public class LibVlcException : Exception
    {
        /// <summary>
        ///     Create exception with a message.
        /// </summary>
        /// <param name="message">exception message</param>
        public LibVlcException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Create exception with a message and a inner exception.
        /// </summary>
        /// <param name="message">exception message</param>
        /// <param name="innerException">inner exception</param>
        public LibVlcException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    /// <summary>
    ///     If a LibVlc function don't have <see cref="LibVlcFunctionAttribute" />, this exception will be throwed.
    /// </summary>
    public class NoLibVlcFunctionAttributeException : LibVlcException
    {
        /// <summary>
        ///     Create a <see cref="NoLibVlcFunctionAttributeException" />.
        /// </summary>
        public NoLibVlcFunctionAttributeException()
            : base("For LibVlcFunction, need LibVlcFunctionAttribute to get Infomation of function.")
        {
        }
    }

    /// <summary>
    ///     If a function can't be found in LibVlc dlls, this exception will be throwed, maybe we should check the LibVlc
    ///     version what the function need.
    /// </summary>
    public class FunctionNotFoundException : LibVlcException
    {
        /// <summary>
        ///     Create a  <see cref="FunctionNotFoundException" /> with function's infomation and LibVlc's version.
        /// </summary>
        /// <param name="functionInfo">infomation of function</param>
        /// <param name="libvlcVersion">version of LibVlc</param>
        public FunctionNotFoundException(LibVlcFunctionAttribute functionInfo, LibVlcVersion libvlcVersion)
            : this(functionInfo, libvlcVersion, null)
        {
        }

        /// <summary>
        ///     Create a  <see cref="FunctionNotFoundException" /> with function's infomation, LibVlc's version and a inner
        ///     exception.
        /// </summary>
        /// <param name="functionInfo">infomation of function</param>
        /// <param name="libvlcVersion">version of LibVlc</param>
        /// <param name="innerException">inner exception</param>
        public FunctionNotFoundException(LibVlcFunctionAttribute functionInfo, LibVlcVersion libvlcVersion,
            Exception innerException)
            : base(String.Format("Can't find function \"{0}\" in dll.", functionInfo.FunctionName), innerException)
        {
            FunctionInfomation = functionInfo;
            LibVlcVersion = libvlcVersion;
        }

        /// <summary>
        ///     Infomation of function what not found.
        /// </summary>
        public LibVlcFunctionAttribute FunctionInfomation { get; private set; }

        /// <summary>
        ///     Versiong infomation of current LibVlc.
        /// </summary>
        public LibVlcVersion LibVlcVersion { get; private set; }
    }

    /// <summary>
    ///     If a function is not available in current version LibVlc, but you call this, the exception will be throwed.
    /// </summary>
    public class FunctionNotAvailableException : LibVlcException
    {
        /// <summary>
        ///     Create a <see cref="FunctionNotAvailableException" /> with function's infomation and LibVlc's version.
        /// </summary>
        /// <param name="functionInfo">infomation of function</param>
        /// <param name="libvlcVersion">version of LibVlc</param>
        public FunctionNotAvailableException(LibVlcFunctionAttribute functionInfo, LibVlcVersion libvlcVersion)
            : base(
                String.Format("Function \"{0}\" isn't available on current version LibVlc.", functionInfo.FunctionName))
        {
            FunctionInfomation = functionInfo;
            LibVlcVersion = libvlcVersion;
        }

        /// <summary>
        ///     Infomation of function what not found.
        /// </summary>
        public LibVlcFunctionAttribute FunctionInfomation { get; private set; }

        /// <summary>
        ///     Versiong infomation of current LibVlc.
        /// </summary>
        public LibVlcVersion LibVlcVersion { get; private set; }
    }

    /// <summary>
    ///     If a version string parse failed, this exception will be throwed.
    /// </summary>
    public class VersionStringParseException : LibVlcException
    {
        /// <summary>
        ///     Create a <see cref="VersionStringParseException" /> with parse failed version string.
        /// </summary>
        /// <param name="versionString"></param>
        public VersionStringParseException(String versionString)
            : base(
                String.Format("Can't parse libvlc version string \"{0}\", it must be like \"2.2.0-Meta Weatherwax\".",
                    versionString))
        {
            VersionString = versionString;
        }

        /// <summary>
        ///     Parse failed version string.
        /// </summary>
        public String VersionString { get; private set; }
    }

    /// <summary>
    ///     If some exception throwed when loading LibVlc, this exception will be throwed. Maybe you should check the LibVlc
    ///     target platform and your app target platform.
    /// </summary>
    public class LibVlcLoadLibraryException : LibVlcException
    {
        /// <summary>
        ///     Create a <see cref="LibVlcLoadLibraryException" />.
        /// </summary>
        public LibVlcLoadLibraryException() : this(null)
        {
        }

        /// <summary>
        ///     Create a <see cref="LibVlcLoadLibraryException" /> with a inner exception.
        /// </summary>
        public LibVlcLoadLibraryException(Exception innerException)
            : base(
                "Can't load LibVlc dlls, check the platform and LibVlc target platform (should be same, x86 or x64).",
                innerException)
        {
        }
    }

    /// <summary>
    ///     If create a new Vlc instence return NULL, this exception will be throwed. Maybe you should check your Vlc options.
    /// </summary>
    public class VlcCreateFailException : LibVlcException
    {
        /// <summary>
        ///     Create a <see cref="VlcCreateFailException" />.
        /// </summary>
        public VlcCreateFailException() : this(null)
        {
        }

        /// <summary>
        ///     Create a <see cref="VlcCreateFailException" /> with some message.
        /// </summary>
        public VlcCreateFailException(String message)
            : base("Can't create a Vlc instence, check your Vlc options." + (message == null ? "" : String.Format(" Maybe those message \"{0}\" mean sometion for you.",message)))
        {
        }
    }
}