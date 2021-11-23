using Godot;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class NetworkManager : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    UdpClient udpClient;
    public double timeElapsed = 0;

    public bool connected = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void OpenSocket(int port) {
        udpClient = new UdpClient(port);
    }

    public void Connect(string ip, int port) {
        udpClient.Connect(ip, port);
        connected = true;
        Task.Run(Receive);
    }

    public void SendConnectionRequest() {
        ConnectionPacket connectionPacket = new ConnectionPacket();
        
        Byte[] sendBytes = connectionPacket.Serialise();
        
        udpClient.Send(sendBytes, sendBytes.Length);
    }

    public void SendTimeSync() {
        //Byte[] sendBytes = Encoding.ASCII.GetBytes("Is anybody there?");
        float timestamp = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) * 0.001f;
        
        TimeSyncServerPacket timeSyncPacket = new TimeSyncServerPacket();
        
        timeSyncPacket.serverTime = timeElapsed;
        timeSyncPacket.serverTimestamp = timestamp;

        Byte[] sendBytes = timeSyncPacket.Serialise();
        
        udpClient.Send(sendBytes, sendBytes.Length);
    }
    
    public async void Receive() {
        while (udpClient != null)
        {
            var receivedResults = await udpClient.ReceiveAsync();
            var packetType = Packet.GetPacketTypeFromStream(receivedResults.Buffer);

            switch (packetType) {
                case PacketType.TimeSyncServer:
                    TimeSyncServerPacket timeReceivedPacket = new TimeSyncServerPacket();
                    timeReceivedPacket.Deserialise(receivedResults.Buffer);
            
                    float currentTimestamp = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) * 0.001f;
                    timeElapsed = timeReceivedPacket.serverTime + (currentTimestamp - timeReceivedPacket.serverTimestamp);
                    break;
                case PacketType.ConnectionRequest:
                    ConnectionPacket connectionReceivedPacket = new ConnectionPacket();
                    connectionReceivedPacket.Deserialise(receivedResults.Buffer);
                    Connect(receivedResults.RemoteEndPoint.Address.ToString(), 24011);
                    
                    GD.Print(receivedResults.RemoteEndPoint.Address.ToString());
                    GD.Print("Connection request recieved");
                    break;
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        timeElapsed += delta;
        
        if (connected) {
            SendTimeSync();
        }
    }
    
}