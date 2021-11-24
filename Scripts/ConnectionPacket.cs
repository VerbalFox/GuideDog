using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class ConnectionPacket : Packet
{
    public ConnectionPacket() {
        type = PacketType.ConnectionRequest;
    }

    public byte[] Serialise() {
        var array = 
        BitConverter.GetBytes(((Int32)type)); // 4 byte

        return array.ToArray();
    }
    
    public void Deserialise(byte[] stream) {
        int index = 0;
        
        type = (PacketType)BitConverter.ToInt32(stream, index);
    }
}