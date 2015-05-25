using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xZune.Vlc
{
    static class HandleManager
    {
        static Dictionary<IntPtr, IVlcObject> handleDic = new Dictionary<IntPtr, IVlcObject>();

        public static IVlcObject GetVlcObject(IntPtr pointer)
        {
            if(handleDic.ContainsKey(pointer))
            {
                return handleDic[pointer];
            }
            else
            {
                return null;
            }
        }

        public static void Add(IVlcObject vlcObject)
        {
            if (!handleDic.ContainsKey(vlcObject.InstancePointer))
            {
                handleDic.Add(vlcObject.InstancePointer, vlcObject);
            }
        }

        public static void Remove(IVlcObject vlcObject)
        {
            if (handleDic.ContainsKey(vlcObject.InstancePointer))
            {
                handleDic.Remove(vlcObject.InstancePointer);
            }
        }
    }

}
