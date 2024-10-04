using System;
using LiteNetLib;
using LiteNetLib.Utils;
using StickGameServer.Shared.Util;

namespace StickGameServer.Shared.Packet.Packets
{
    public struct ClientboundHandAnimationPacketDS
    {
        public Guid playerGuid;
        public ServerboundHandAnimationPacketDS packetBase;
    }
    
    public class ClientboundHandAnimationPacket : StaticPacket<ClientboundHandAnimationPacketDS>
    {
        public override byte Id => 3;
        public override PacketBound Bound => PacketBound.Client;
        
        public override void Handle(NetPeer netPeer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
#if STICK_CLIENT
            var player = StickGame.PlayerManager.GetPlayer(reader.GetGuid());

            if (player == null)
            {
                return;
            }
            
            var leftTriggerValue = reader.GetFloat();
            var leftGripValue = reader.GetFloat();
            var rightTriggerValue = reader.GetFloat();
            var rightGripValue = reader.GetFloat();

            if (player.leftHandAnimator != null)
            {
                player.leftHandAnimator.SetFloat("Trigger", leftTriggerValue);
                player.leftHandAnimator.SetFloat("Grip", leftGripValue);
            }
            
            if (player.rightHandAnimator != null)
            {
                player.rightHandAnimator.SetFloat("Trigger", rightTriggerValue);
                player.rightHandAnimator.SetFloat("Grip", rightGripValue);
            }
#endif
        }

        public override void Send(NetPeer netPeer, ClientboundHandAnimationPacketDS dataStruct)
        {
            NetDataWriter writer = new();
            writer.Put(Id);
            writer.Put(dataStruct.playerGuid);
            writer.Put(dataStruct.packetBase.leftHandTrigger);
            writer.Put(dataStruct.packetBase.leftHandGrip);
            writer.Put(dataStruct.packetBase.rightHandTrigger);
            writer.Put(dataStruct.packetBase.rightHandGrip);
            SendPacket(netPeer, DeliveryMethod.ReliableSequenced, writer);
        }
    }
}