using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class LobbyStatusPacket : Packet
{
    public LobbyStatusPacket() {
        type = PacketType.LobbyStatus;
    }

    public byte[] Serialise() {
        var array = 
                BitConverter.GetBytes(((Int32)type)) // 4 byte
        .Concat(BitConverter.GetBytes(isReady)); // 8 byte

        return array.ToArray();
    }
    
    public void Deserialise(byte[] stream) {
        int index = 0;
        
        type = (PacketType)BitConverter.ToInt32(stream, index);     index += sizeof(Int32);
        isReady = BitConverter.ToBoolean(stream, index);            index += sizeof(bool);
    }
    
    public bool isReady;
}