// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: VlcUnmanagedList.cs
// Version: 20181231

using System;
using System.Collections;
using System.Collections.Generic;

namespace Meta.Vlc
{
    public abstract unsafe class VlcUnmanagedList<TItem> : IUnmanagedObject, IEnumerable<TItem>
    {
        private readonly List<TItem> _list = new List<TItem>();

        private bool _disposedValue;

        public VlcUnmanagedList(void** pointer, uint count)
        {
            if (pointer == null) return;

            InstancePointer = pointer;
            for (var i = 0; i < count; i++) _list.Add(CreateItem(pointer[i]));
        }

        public int Count => _list.Count;

        public TItem this[int index] => _list[index];

        public void* InstancePointer { get; private set; }

        public IEnumerator<TItem> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void* IUnmanagedObject.InstancePointer => throw new NotImplementedException();

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract TItem CreateItem(void* data);

        protected abstract void Release(void** data);

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing) _list.Clear();

                if (InstancePointer != null)
                {
                    Release((void**) InstancePointer);
                    InstancePointer = null;
                }

                _disposedValue = true;
            }
        }

        ~VlcUnmanagedList()
        {
            Dispose(false);
        }
    }
}