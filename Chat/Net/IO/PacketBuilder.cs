using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_Client.Net.IO
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

        public void WriteMessage(string msg) 
        {
            var msgLength = msg.Length*2;
            _ms.Write(BitConverter.GetBytes(msgLength));
            _ms.Write(Encoding.Default.GetBytes(msg));
        }

        public byte[] GetPacketBytes() 
        {
            return _ms.ToArray();
        }
    }
}
