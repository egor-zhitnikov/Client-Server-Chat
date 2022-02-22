﻿using Chat_Client.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public event Action messageRecievedEvent;
        public event Action userDisconnectEvent;
        public Server() 
        {
            _client = new TcpClient();
        }

        public void ConnectToServer(string username) 
        {
            if (!_client.Connected) 
            {
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("25.70.56.21"), 80);
                _client.Connect(endpoint);
                PacketReader = new PacketReader(_client.GetStream());

                if (!string.IsNullOrEmpty(username)) 
                {
                    var connectPacket = new PacketBuilder();
                    connectPacket.WriteOpCode(0);
                    connectPacket.WriteMessage(username);
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
                        case 5:
                            messageRecievedEvent?.Invoke();
                            break;
                        case 10:
                            userDisconnectEvent?.Invoke();
                            break;
                        default:
                            Console.WriteLine("Shit , here we go again");
                            break;
                    }
                }
            });
        }

        public void SendMessageToServer(string message) 
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteMessage(message);
            _client.Client.Send(messagePacket.GetPacketBytes());

        }
    }
}
