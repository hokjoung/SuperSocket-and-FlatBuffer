using SuperSocket.ProtoBase;
using System;
using System.Text;

namespace Client
{
    class MyReceiveFilter : FixedHeaderReceiveFilter<PackageInfo<string, byte[]>>
    {
        public MyReceiveFilter(int headerSize) : base(headerSize)
        {
        }

        public override PackageInfo<string, byte[]> ResolvePackage(IBufferStream bufferStream)
        {
            string key = bufferStream.ReadString(4, Encoding.UTF8);
            if(bufferStream.Length > 4)
            {
                byte[] body = new byte[bufferStream.Length - 4];
                bufferStream.Skip(4).Read(body, 4, body.Length);
                return new PackageInfo<string, byte[]>(key, body);
            }

            return new PackageInfo<string, byte[]>(key, null);
        }

        protected override int GetBodyLengthFromHeader(IBufferStream bufferStream, int length)
        {
            if (bufferStream.Length <= 4)
                return 0;
            return (int)bufferStream.Length - 4;
        }

    }
}
