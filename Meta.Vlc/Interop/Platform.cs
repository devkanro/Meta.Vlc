// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: Platform.cs
// Version: 20181231

#if AnyCPU || x86
#define ILP32
#else
#define LP64
#endif

namespace Meta.Vlc.Interop
{
    public static class Platform
    {
        public const int CharSize = 1;
        public const int ShortSize = 2;
        public const int UShortSize = ShortSize;
        public const int FloatSize = 4;
        public const int DoubleSize = 8;
#if ILP32
        public const int IntSize = 4;
        public const int LongSize = 4;
        public const int LongLongSize = 8;
        public const int IntPtrSize = 4;
#elif LP32
        public const int IntSize = 2;
        public const int LongSize = 4;
        public const int LongLongSize = 8;
        public const int IntPtrSize = 4;
#elif LP64
        public const int IntSize = 4;
        public const int LongSize = 8;
        public const int LongLongSize = 8;
        public const int IntPtrSize = 8;
#elif LLP64
        public const int IntSize = 4;
        public const int LongSize = 4;
        public const int LongLongSize = 8;
        public const int IntPtrSize = 8;
#endif
        public const int UIntSize = IntSize;
        public const int ULongSize = LongSize;
        public const int ULongLongSize = LongLongSize;
        public const int SizeTSize = IntPtrSize;
        public const int EnumSize = IntSize;
    }
}