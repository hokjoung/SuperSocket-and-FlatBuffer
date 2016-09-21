using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkEngine
{
    class MyRequestInfo : BinaryRequestInfo
    {
        public MyRequestInfo(string key, byte[] body) : base(key, body)
        {
        }
    }
}
