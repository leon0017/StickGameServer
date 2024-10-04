using System;
using LiteNetLib;
using LiteNetLib.Utils;

namespace StickGameServer.Shared.Packet.Packets
{
    public struct ClientboundPlayerDestroyPacketDS
    {
        public Guid temporaryGuid;
    };
    
    public class ClientboundPlayerDestroyPacket : StaticPacket<ClientboundPlayerDestroyPacketDS>
    {
        public override byte Id => 2;
        public override PacketBound Bound => PacketBound.Client;
        
        public override void Handle(NetPeer netPeer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
#if STICK_CLIENT
            var guid = reader.GetGuid();
            
            var player = StickGame.PlayerManager.playersDict[guid];
            if (player == null)
            {
                return;
            }

            UnityEngine.Object.Destroy(player.gameObject);
#endif
        }

        public override void Send(NetPeer netPeer, ClientboundPlayerDestroyPacketDS dataStruct)
        {
            NetDataWriter writer = new();
            writer.Put(Id);
            writer.Put(dataStruct.temporaryGuid);
            SendPacket(netPeer, DeliveryMethod.ReliableOrdered, writer);
        }
    }
}