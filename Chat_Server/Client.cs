using Chat_Server.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat_Server
{
    class Client
    {
        public string Username { get; set; }
        public Guid UID { get; set; }
        public TcpClient ClientSocket { get; set; }

        PacketReader _packetReader;

        public Client(TcpClient client)
        {
            ClientSocket = client;
            UID = Guid.NewGuid();
            _packetReader = new PacketReader(ClientSocket.GetStream());


            Username = _packetReader.ReadMessage();
            Console.WriteLine($"{DateTime.Now} User: {Username} has connected.");

            Task.Run(() => Process());
        }
        void Process() 
        {
            var nickname = new string(Username.Where(char.IsLetter).ToArray());
            while (true) 
            {
                try
                {
                    var opcode = _packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 5:
                            var msg = _packetReader.ReadMessage();
                            Console.WriteLine($"{DateTime.Now} [{nickname}] Message: {msg}");
                            Program.BroadcastMessage($"[{DateTime.Now}]: [{nickname}]: {msg}",msg.Length);
                            break;
                        default:

                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"{nickname} Disconnected.");
                    Program.BroadcastDisconnect(UID.ToString());
                    ClientSocket.Close();
                    break;
                }
            }
        }
    }
}
