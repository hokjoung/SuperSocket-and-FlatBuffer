using SuperSocket.Common;
using SuperSocket.Facility.Protocol;
using System;
using System.Text;

namespace NetworkEngine
{
    class MyReceiveFilter : FixedHeaderReceiveFilter<MyRequestInfo>
    {
        public MyReceiveFilter() : base(8)
        {
        }

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            for(int i = offset; i < offset + 8; i++)
            {
                Console.Write(header[i]);
            }
            //header와 key의 인덱스가 틀렸다. 
            return BitConverter.ToInt32(header, offset + 4);
        }

        protected override MyRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            return new MyRequestInfo(Encoding.UTF8.GetString(header.Array, header.Offset, 4), bodyBuffer.CloneRange(offset, length));
        }

//         public override MyRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
//                 {
//         
//                     return new MyRequestInfo();
//                 }
//         
//                 protected override MyRequestInfo ProcessMatchedRequest(byte[] buffer, int offset, int length, bool toBeCopied)
//                 {
//                     return MyRequestInfo()
//                 }
    }
}
