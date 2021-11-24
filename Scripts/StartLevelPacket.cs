using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class StartLevelPacket : Packet
{
    public StartLevelPacket() {
        type = PacketType.GameStart;
    }

    public byte[] Serialise() {
        var array = 
                BitConverter.GetBytes(((Int32)type)) // 4 byte
        .Concat(BitConverter.GetBytes(serverTime)); // 8 byte

        return array.ToArray();
    }
    
    public void Deserialise(byte[] stream) {
        int index = 0;
        
        type = (PacketType)BitConverter.ToInt32(stream, index);     index += sizeof(Int32); // index = 0
        
        serverTime = BitConverter.ToDouble(stream, index);          index += sizeof(double); // index = 4
    }
    
    public double serverTime;
}
