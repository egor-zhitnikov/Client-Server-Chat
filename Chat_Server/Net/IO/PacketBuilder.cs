using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_Server.Net.IO
{
    class PacketBuilder
    {
        MemoryStream _ms; 

        public PacketBuilder()
        {
            _ms = new MemoryStream();
        }

        public void WriteOpCode(byte opcode)
        {
            _ms.WriteByte(opcode);
        }

        public void WriteMessage(string msg,int messageLength)
        {
            var msgLength = msg.Length;
            _ms.Write(BitConverter.GetBytes(msgLength+messageLength));
            _ms.Write(Encoding.Default.GetBytes(msg));
        }

        public byte[] GetPacketBytes()
        {
            return _ms.ToArray();
        }
    }
}