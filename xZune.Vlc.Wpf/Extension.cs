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
        /// Return this value, if one of objs is null return default(T).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static T DefaultValueWhenNull<T>(this T value, params object[] objs)
        {
            if (objs.All(o => o != null))
            {
                return value;
            }
            else
            {
                return default(T);
            }
        }
    }
}