// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: ChromaType.cs
// Version: 20160214

namespace Meta.Vlc.Wpf
{
    /// <summary>
    ///     Pixel chroma type.
    /// </summary>
    public enum ChromaType : uint
    {
        /// <summary>
        ///     5 bit for each RGB channel, no alpha channel, BGRA5550(15bit / pixel).
        /// </summary>
        RV15 = ('R' << 00) |
               ('V' << 08) |
               ('1' << 16) |
               ('5' << 24),

        /// <summary>
        ///     5 bit Red, 6 bit Green and 5 bit Blue, no alpha channel, BGRA5650(16bit / pixel).
        /// </summary>
        RV16 = ('R' << 00) |
               ('V' << 08) |
               ('1' << 16) |
               ('6' << 24),

        /// <summary>
        ///     8 bit for each RGB channel, no alpha channel, BGRA8880(24bit / pixel).
        /// </summary>
        RV24 = ('R' << 00) |
               ('V' << 08) |
               ('2' << 16) |
               ('4' << 24),

        /// <summary>
        ///     8 bit per RGB channel and 8 bit unused, no alpha channel, BGRA8880(32bit / pixel).
        /// </summary>
        RV32 = ('R' << 00) |
               ('V' << 08) |
               ('3' << 16) |
               ('2' << 24),

        /// <summary>
        ///     8 bit for each BGRA channel, RGBA8888(32bit / pixel).
        /// </summary>
        RGBA = ('R' << 00) |
               ('G' << 08) |
               ('B' << 16) |
               ('A' << 24),

        /// <summary>
        ///     12 bits per pixel planar format with Y plane followed by V and U planes.
        /// </summary>
        YV12 = ('Y' << 00) |
               ('V' << 08) |
               ('1' << 16) |
               ('2' << 24),

        /// <summary>
        ///     Same as YV12 but V and U are swapped.
        /// </summary>
        I420 = ('I' << 00) |
               ('4' << 08) |
               ('2' << 16) |
               ('0' << 24),

        /// <summary>
        ///     12 bits per pixel planar format with Y plane and interleaved UV plane.
        /// </summary>
        NV12 = ('N' << 00) |
               ('V' << 08) |
               ('1' << 16) |
               ('2' << 24),

        /// <summary>
        ///     16 bits per pixel packed YUYV array.
        /// </summary>
        YUY2 = ('Y' << 00) |
               ('U' << 08) |
               ('Y' << 16) |
               ('2' << 24),

        /// <summary>
        ///     16 bits per pixel packed UYVY array.
        /// </summary>
        UYVY = ('U' << 00) |
               ('Y' << 08) |
               ('V' << 16) |
               ('Y' << 24),

        /// <summary>
        ///     Same as I420, mainly used with MJPG codecs.
        /// </summary>
        J420 = ('J' << 00) |
               ('4' << 08) |
               ('2' << 16) |
               ('0' << 24),

        /// <summary>
        ///     Same as YUY2, mainly used with MJPG codecs.
        /// </summary>
        J422 = ('I' << 00) |
               ('4' << 08) |
               ('2' << 16) |
               ('2' << 24)
    }
}