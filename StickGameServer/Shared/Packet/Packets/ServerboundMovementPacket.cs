using System;
using LiteNetLib;
using LiteNetLib.Utils;
using StickGameServer.Shared.Game;
using StickGameServer.Shared.Util;

namespace StickGameServer.Shared.Packet.Packets
{
    public struct ServerboundMovementPacketDS
    {
        public Vec3f headPos;
        public Vec3f leftHandPos;
        public Vec3f rightHandPos;
        public Quat4f headQuat;
        public Quat4f leftHandQuat;
        public Quat4f rightHandQuat;
    }

    public class ServerboundMovementPacket : StaticPacket<ServerboundMovementPacketDS>
    {
        public override byte Id => 0;

        public override PacketBound Bound => PacketBound.Server;

        public override void Handle(NetPeer netPeer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
#if !STICK_CLIENT
            ClientboundPlayerMovePacketDS movementPacketDs = new()
            {
                playerGuid = netPeer.player.temporaryGuid,
                headPos = new Vec3f(reader.GetFloatArray()),
                leftHandPos = new Vec3f(reader.GetFloatArray()),
                rightHandPos = new Vec3f(reader.GetFloatArray()),
                headQuat = new Quat4f(reader.GetFloatArray()),
                leftHandQuat = new Quat4f(reader.GetFloatArray()),
                rightHandQuat = new Quat4f(reader.GetFloatArray()),
            };

            foreach (var player in Program.players)
            {
                if (player.connection != netPeer) // Don't send back to sender
                {
                    PacketRegistry.CLIENTBOUND_PLAYER_MOVE_PACKET.Send(player.connection, movementPacketDs);
                }
            }
#endif
        }

        public override void Send(NetPeer netPeer, ServerboundMovementPacketDS dataStruct)
        {
#if STICK_CLIENT
            NetDataWriter writer = new();
            writer.Put(Id);
            writer.PutArray(dataStruct.headPos.ToFloatArray());
            writer.PutArray(dataStruct.leftHandPos.ToFloatArray());
            writer.PutArray(dataStruct.rightHandPos.ToFloatArray());
            writer.PutArray(dataStruct.headQuat.ToFloatArray());
            writer.PutArray(dataStruct.leftHandQuat.ToFloatArray());
            writer.PutArray(dataStruct.rightHandQuat.ToFloatArray());
            SendPacket(netPeer, DeliveryMethod.ReliableOrdered, writer);
#endif
        }
    }
}