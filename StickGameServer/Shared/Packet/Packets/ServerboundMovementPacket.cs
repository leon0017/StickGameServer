using System;
using LiteNetLib;
using StickGameServer.Shared.Game;
using StickGameServer.Shared.Util;

namespace StickGameServer.Shared.Packet.Packets
{
    public struct ServerboundMovementPacketDS
    {
        public Vec3f headPos;
        public Vec3f leftArmPos;
        public Vec3f rightArmPos;
    }

    public class ServerboundMovementPacket : StaticPacket<ServerboundMovementPacketDS>
    {
        public override byte Id => 0;

        public override PacketBound Bound => PacketBound.Server;

        public override void Handle(NetPeer netPeer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
#if !STICK_CLIENT
            float[] headPos = reader.GetFloatArray();
            float[] leftArmPos = reader.GetFloatArray();
            float[] rightArmPos = reader.GetFloatArray();
            Console.WriteLine("headpos: " + string.Join(", ", headPos));

            foreach (Player player in Program.players)
            {
                if (player.connection != netPeer)
                {
                    ClientboundBallPacketDS packetDs = new ClientboundBallPacketDS();
                    packetDs.ballPos = new Vec3f(headPos);
                    PacketRegistry.CLIENTBOUND_BALL_PACKET.Send(player.connection, packetDs);
                }
            }
#endif
        }

        public override void Send(NetPeer netPeer, ServerboundMovementPacketDS dataStruct)
        {
#if STICK_CLIENT
            NetDataWriter writer = new();
            writer.Put(Id);
            writer.PutArray(new [] {dataStruct.headPos.x, dataStruct.headPos.y, dataStruct.headPos.z});
            writer.PutArray(new[] { 0, 0, 0 });
            writer.PutArray(new[] { 0, 0, 0 });
            SendPacket(netPeer, DeliveryMethod.ReliableOrdered, writer);
#endif
        }
    }
}