using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class PositionUpdatePacket : Packet
{
    public PositionUpdatePacket() {
        type = PacketType.PositionUpdate;
    }

    public byte[] Serialise() {
        var array = 
                BitConverter.GetBytes(((Int32)type)) // 4 byte
        .Concat(BitConverter.GetBytes(posX)) // 8 byte
        .Concat(BitConverter.GetBytes(posY)) // 8 byte
        .Concat(BitConverter.GetBytes(velX)) // 8 byte
        .Concat(BitConverter.GetBytes(velY)); // 8 byte

        return array.ToArray();
    }
    
    public void Deserialise(byte[] stream) {
        int index = 0;
        
        type = (PacketType)BitConverter.ToInt32(stream, index);     index += sizeof(Int32); // index = 0
        
        posX = BitConverter.ToDouble(stream, index);          index += sizeof(double); // index = 4
        posY = BitConverter.ToDouble(stream, index);     index += sizeof(double); // index = 12
        velX = BitConverter.ToDouble(stream, index);          index += sizeof(double); // index = 4
        velY = BitConverter.ToDouble(stream, index);     index += sizeof(double); // index = 12
    }
    
    public double posX;
    public double posY;
    public double velX;
    public double velY;
}
