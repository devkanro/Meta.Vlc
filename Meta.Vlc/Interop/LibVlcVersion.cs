// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: LibVlcVersion.cs
// Version: 20160216

using System;
using System.Text.RegularExpressions;

namespace Meta.Vlc.Interop
{
    /// <summary>
    ///     Version infomation of LibVlc.
    /// </summary>
    public class LibVlcVersion
    {
        private static String[] matchExpressions =
        {
            @"^([0-9.]*)-([\S]*)(?: ([\S]*))?",
            @"^([0-9.]*) ([^(]*)(?:\(([\S]*)\))?"
        };

        /// <summary>
        ///     Create LibVlcVersion from version string, it must like "2.2.0-Meta Weatherwax".
        /// </summary>
        /// <param name="versionString">version string</param>
        /// <exception cref="VersionStringParseException">Can't parse libvlc version string, it must like "2.2.0-Meta Weatherwax".</exception>
        /// <exception cref="OverflowException">
        ///     At least one component of version represents a number greater than
        ///     <see cref="Int32.MaxValue" />.
        /// </exception>
        public LibVlcVersion(String versionString)
        {
            Match match = null;

            foreach (var expression in matchExpressions)
            {
                var tmpMatch = Regex.Match(versionString.Trim(), expression);

                if (tmpMatch.Success)
                {
                    match = tmpMatch;
                    break;
                }
            }

            if (match == null)
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
                    DevString = match.Groups[2].Value.Trim();
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
        ///     Version of LibVlc.
        /// </summary>
        public Version Version { get; private set; }

        /// <summary>
        ///     DevString of LibVlc.
        /// </summary>
        public String DevString { get; private set; }

        /// <summary>
        ///     Code name of LibVlc.
        /// </summary>
        public String CodeName { get; private set; }

        /// <summary>
        ///     Check a function is available for this version.
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