using Godot;
using System;

public enum PacketType
{
    ConnectionRequest,
    ConnectionHealth,
    LobbyStatus,
    TimeSyncServer,
    PositionUpdate,
    GameStart
}
public class Packet
{ 
    protected PacketType type;
    public static PacketType GetPacketTypeFromStream(byte[] stream) {
        PacketType type = (PacketType)BitConverter.ToInt32(stream, 0);
        return type;
    }
}