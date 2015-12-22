using System;
using System.Linq;

namespace xZune.Vlc.Wpf
{
    /// <summary>
    /// Some extension method.
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// Return this value, if one of objs is null return safeValue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="safeValue"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static T SafeValueWhenNull<T>(this T value, T safeValue, params object[] objs)
        {
            if (objs.All(o => o != null))
            {
                return value;
            }
            else
            {
                return safeValue;
            }
        }
        
        /// <summary>
        /// Return the value from the selector, unless the object is null. Then return the default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TRet"></typeparam>
        /// <param name="value"></param>
        /// <param name="selector"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TRet DefaultValueWhenNull<T,TRet>(this T value, Func<T,TRet> selector, TRet defaultValue = default(TRet))
        {
            return value == null ? defaultValue : selector(value);
        }
    }
}