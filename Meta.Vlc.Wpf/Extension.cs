// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: Extension.cs
// Version: 20160312

using System;
using System.IO;
using System.Text;
using System.Windows.Media;

namespace Meta.Vlc.Wpf
{
    /// <summary>
    ///     Some extension method.
    /// </summary>
    public static class Extension
    {
        /// <summary>
        ///     Return the value from the selector, unless the object is null. Then return the default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TRet"></typeparam>
        /// <param name="value"></param>
        /// <param name="selector"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TRet DefaultValueWhenNull<T, TRet>(this T value, Func<T, TRet> selector,
            TRet defaultValue = default(TRet))
        {
            return value == null ? defaultValue : selector(value);
        }

        public static T DefaultValueWhenTrue<T>(this T value, bool boolean, T defaultValue = default(T))
        {
            return boolean ? defaultValue : value;
        }

        /// <summary>
        ///     Combine path to src path.
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="extPath"></param>
        /// <returns></returns>
        public static string CombinePath(this string srcPath, string extPath)
        {
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(srcPath, extPath));
            return dir.FullName;
        }

        /// <summary>
        ///     Check a path is a drive root directory or not.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsDriveRootDirectory(this string path)
        {
            Uri uri = path.ToUri();
            if (uri != null && uri.IsFile)
            {
                DirectoryInfo dir = new DirectoryInfo(uri.LocalPath);

                return dir.Root.FullName == dir.FullName;
            }
            return false;
        }

        /// <summary>
        ///     Convert a path to uri.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Uri ToUri(this string str)
        {
            Uri result = null;
            Uri.TryCreate(str.FormatPath(), UriKind.Absolute, out result);
            return result;
        }

        /// <summary>
        ///     Format a path with '/' and ends with '/'.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FormatPath(this string path)
        {
            path = path.Replace('\\', '/');
            if (!path.EndsWith("/"))
            {
                path += '/';
            }
            return path;
        }

        public static PixelFormat GetPixelFormat(this ChromaType chroma)
        {
            switch (chroma)
            {
                case ChromaType.RV15:
                    return PixelFormats.Bgr555;

                case ChromaType.RV16:
                    return PixelFormats.Bgr565;

                case ChromaType.RV24:
                    return PixelFormats.Bgr24;

                case ChromaType.RV32:
                    return PixelFormats.Bgr32;

                case ChromaType.RGBA:
                    return PixelFormats.Bgra32;

                default:
                    throw new NotSupportedException(String.Format("Not support pixel format: {0}", chroma));
            }
        }

        /// <summary>
        ///     Quickly async invoke a action.
        /// </summary>
        /// <param name="action"></param>
        public static void EasyInvoke(this Action action)
        {
            action.BeginInvoke(ar => { action.EndInvoke(ar); }, null);
        }

        /// <summary>
        ///     Get the HTTP encoded string of a Uri.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>The encoded string.</returns>
        public static String ToHttpEncodeString(this Uri uri)
        {
            String uriString = uri.ToString();
            StringBuilder resultBuilder = new StringBuilder(512);

            if (String.IsNullOrEmpty(uriString))
            {
                return uriString;
            }

            foreach (var ch in uriString)
            {
                switch (ch)
                {
                    case ' ':
                        resultBuilder.Append("%20");
                        break;

                    case '%':
                        resultBuilder.Append("%25");
                        break;

                    default:
                        resultBuilder.Append(ch);
                        break;
                }
            }

            return resultBuilder.ToString();
        }
    }
}