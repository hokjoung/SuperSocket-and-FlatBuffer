using FlatBuffers;
using GameObject;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using System;

namespace NetworkEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            MyServer server = new MyServer();

            ServerConfig serverConfig = new ServerConfig
            {
                Ip = "127.0.0.1",//테스트할때만 로컬 ip를 넣는다.
                Port = 11000
            };
            server.Setup(serverConfig);

            server.NewSessionConnected += new SessionHandler<MySession>(appServer_NewSessionConnected);


            server.NewRequestReceived += new RequestHandler<MySession, MyRequestInfo>(appServer_NewRequestReceived);
            
            server.Start();

            Console.WriteLine("The server started successfully, press key 'q' to stop it!");

            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }

            Console.WriteLine();
            //Stop the appServer
            server.Stop();

            Console.WriteLine("The server was stopped!");
        }


        static void appServer_NewRequestReceived(MySession session, MyRequestInfo requestInfo)
        {
            Console.WriteLine("Received something");
            ByteBuffer buffer = new ByteBuffer(requestInfo.Body);
            var monster = Monster.GetRootAsMonster(buffer);
            Console.WriteLine("HP: {0}", monster.Hp);
        }

        static void appServer_NewSessionConnected(MySession session)
        {
            Console.WriteLine("Client Accepted");
        }
    }
}
