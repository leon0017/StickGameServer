
using LiteNetLib;
using StickGameServer.Shared.Util;
using System;

namespace StickGameServer.Shared.Packet.Packets
{
    public struct ServerboundMovementPacketDS
    {
        Vec3f headPos;
        Vec3f leftArmPos;
        Vec3f rightArmPos;
    };

    public class ServerboundMovementPacket : StaticPacket<ServerboundMovementPacketDS>
    {
        public override byte Id => 0;

        public override PacketBound Bound => PacketBound.Server;

        public override void Handle(NetPeer netPeer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            float[] headPos = reader.GetFloatArray();
            float[] leftArmPos = reader.GetFloatArray();
            float[] rightArmPos = reader.GetFloatArray();
            Console.WriteLine("headpos: " + string.Join(", ", headPos));
            Console.WriteLine("leftarmpos: " + string.Join(", ", leftArmPos));
            Console.WriteLine("rightarmpos: " + string.Join(", ", rightArmPos));
        }

        public override void Send(NetPeer netPeer, ServerboundMovementPacketDS dataStruct)
        {

        }
    }
}
