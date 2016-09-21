using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace NetworkEngine
{
    class MyServer : AppServer<MySession, MyRequestInfo>
    {
        public MyServer() : base(new DefaultReceiveFilterFactory<MyReceiveFilter, MyRequestInfo>())
        {
        }
    }
}
