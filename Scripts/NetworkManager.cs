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
    public bool isHost = false;
    public bool isReady = false;
    public bool isRemoteClientReady = false;
    public bool connected = false;
    public bool gameStarting = false;
    private float lobbyStatusPacketTimer = 0;
    private float timeSyncPacketTimer = 0;

    private SceneSwitcher switcher;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        switcher = GetNode<SceneSwitcher>("/root/root/SceneSwitcher");
        
    }

    public void OpenSocket(int port) {
        udpClient = new UdpClient(port);

        Task.Run(Receive);
    }

    public void Connect(string ip, int port) {
        udpClient.Connect(ip, port);
        connected = true;
    }

    public void SendConnectionRequest() {
        ConnectionPacket connectionPacket = new ConnectionPacket();
        
        Byte[] sendBytes = connectionPacket.Serialise();
        
        udpClient.Send(sendBytes, sendBytes.Length);
    }

    public void SendLobbyStatus() {
        LobbyStatusPacket lobbyStatusPacket = new LobbyStatusPacket();
        
        lobbyStatusPacket.isReady = isReady;

        Byte[] sendBytes = lobbyStatusPacket.Serialise();
        
        udpClient.Send(sendBytes, sendBytes.Length);
    }

    public void SendTimeSync() {
        float timestamp = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) * 0.001f;
        
        TimeSyncServerPacket timeSyncPacket = new TimeSyncServerPacket();
        
        timeSyncPacket.serverTime = timeElapsed;
        timeSyncPacket.serverTimestamp = timestamp;
        
        Byte[] sendBytes = timeSyncPacket.Serialise();
        
        udpClient.Send(sendBytes, sendBytes.Length);
    }

    public void SendStartLevel() {
        StartLevelPacket timeSyncPacket = new StartLevelPacket();
        
        timeSyncPacket.serverTime = timeElapsed + 5;
        
        Byte[] sendBytes = timeSyncPacket.Serialise();
        
        udpClient.Send(sendBytes, sendBytes.Length);
        if (!gameStarting) {
            Task.Run(() => LoadGame(timeElapsed + 5));
            gameStarting = true;
        }
    }

    private void LoadGame(double startTime) {
        while (timeElapsed < startTime) {

        }
        
        switcher.LoadGame();
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
                    break;
                case PacketType.LobbyStatus:
                    LobbyStatusPacket lobbyStatusPacket = new LobbyStatusPacket();
                    lobbyStatusPacket.Deserialise(receivedResults.Buffer);

                    isRemoteClientReady = lobbyStatusPacket.isReady;
                    break;
                case PacketType.GameStart:
                    StartLevelPacket startLevelPacket = new StartLevelPacket();
                    startLevelPacket.Deserialise(receivedResults.Buffer);
                    
                    if (!gameStarting) {
                        Task.Run(() => LoadGame(startLevelPacket.serverTime));
                        gameStarting = true;
                    }
                    break;
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        timeElapsed += delta;

        if (connected) {
            lobbyStatusPacketTimer += delta;
            timeSyncPacketTimer += delta;
        }

        if (lobbyStatusPacketTimer > 0.1f) {
            lobbyStatusPacketTimer -= 0.1f;
            SendLobbyStatus();
        }

        if (timeSyncPacketTimer > 1) {
            timeSyncPacketTimer -= 1;
            SendTimeSync();
        }

        if (!gameStarting && isReady && isRemoteClientReady && isHost) {
            SendStartLevel();
        }
    }
    
}