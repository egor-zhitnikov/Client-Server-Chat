﻿using Chat_Client.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat_Client.Net
{
    class Server
    {
        TcpClient _client;
        PacketBuilder _packetBuilder;
        public PacketReader PacketReader;

        public event Action connectedEvent;
        public Server() 
        {
            _client = new TcpClient();
        }

        public void ConnectToServer(string username) 
        {
            if (!_client.Connected) 
            {
                _client.Connect("127.0.0.1",7891);
                PacketReader = new PacketReader(_client.GetStream());

                if (!string.IsNullOrEmpty(username)) 
                {
                    var connectPacket = new PacketBuilder();
                    connectPacket.WriteOpCode(0);
                    connectPacket.WriteString(username);
                    _client.Client.Send(connectPacket.GetPacketBytes());
                }
                ReadPackets();
            }
        }

        private void ReadPackets() 
        {
            Task.Run(() => {
                while (true) 
                {
                    var opcode = PacketReader.ReadByte();
                    switch (opcode) 
                    {
                        case 1:
                            connectedEvent?.Invoke();
                            break;
                        default:
                            Console.WriteLine("Shit , here we go again");
                            break;
                    }
                }
            });
        }
    }
}