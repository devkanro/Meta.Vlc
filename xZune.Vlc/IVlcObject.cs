using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xZune.Vlc
{
    public interface IVlcObject : IDisposable
    {
        IntPtr InstancePointer { get; }
    }
}
