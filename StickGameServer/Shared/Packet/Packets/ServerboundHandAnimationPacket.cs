using LiteNetLib;
using LiteNetLib.Utils;

namespace StickGameServer.Shared.Packet.Packets
{
    public struct ServerboundHandAnimationPacketDS
    {
        public float leftHandTrigger;
        public float leftHandGrip;
        public float rightHandTrigger;
        public float rightHandGrip;
    }
    
    public class ServerboundHandAnimationPacket : StaticPacket<ServerboundHandAnimationPacketDS>
    {
        public override byte Id => 1;
        public override PacketBound Bound => PacketBound.Server;
        
        public override void Handle(NetPeer netPeer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
#if !STICK_CLIENT
            ClientboundHandAnimationPacketDS handAnimationDs = new()
            {
                playerGuid = netPeer.player.temporaryGuid,
                packetBase =
                {
                    leftHandTrigger = reader.GetFloat(),
                    leftHandGrip = reader.GetFloat(),
                    rightHandTrigger = reader.GetFloat(),
                    rightHandGrip = reader.GetFloat(),
                }
            };

            foreach (var player in Program.players)
            {
                if (player.connection != netPeer) // Don't send back to sender
                {
                    PacketRegistry.CLIENTBOUND_HAND_ANIMATION_PACKET.Send(player.connection, handAnimationDs);
                }
            }
#endif
        }

        public override void Send(NetPeer netPeer, ServerboundHandAnimationPacketDS dataStruct)
        {
#if STICK_CLIENT
            NetDataWriter writer = new();
            writer.Put(Id);
            writer.Put(dataStruct.leftHandTrigger);
            writer.Put(dataStruct.leftHandGrip);
            writer.Put(dataStruct.rightHandTrigger);
            writer.Put(dataStruct.rightHandGrip);
            SendPacket(netPeer, DeliveryMethod.ReliableOrdered, writer);
#endif
        }
    }
}