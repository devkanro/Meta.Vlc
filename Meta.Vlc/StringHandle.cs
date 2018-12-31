// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: StringHandle.cs
// Version: 20181231

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Meta.Vlc
{
    public unsafe class StringHandle : IDisposable
    {
        private bool _disposedValue;
        private readonly GCHandle? _handle;

        public StringHandle(string value, Encoding encoding = null)
        {
            if (value == null)
            {
                _handle = null;
                return;
            }

            if (encoding == null) encoding = Encoding.UTF8;

            _handle = GCHandle.Alloc(encoding.GetBytes(value), GCHandleType.Pinned);
        }

        public IntPtr Pointer => _handle?.AddrOfPinnedObject() ?? IntPtr.Zero;

        public byte* UnsafePointer => (byte*) Pointer.ToPointer();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                }

                _handle?.Free();
                _disposedValue = true;
            }
        }

        ~StringHandle()
        {
            Dispose(false);
        }
    }
}