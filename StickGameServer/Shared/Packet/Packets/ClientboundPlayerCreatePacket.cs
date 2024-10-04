using System;
using LiteNetLib;
using LiteNetLib.Utils;
using StickGameServer.Shared.Game;
using StickGameServer.Shared.Util;

namespace StickGameServer.Shared.Packet.Packets
{
    public struct ClientboundPlayerCreatePacketDS
    {
        public string username;
        public PlayerColor playerColor;
        public Guid temporaryGuid;
    }
    
    public class ClientboundPlayerCreatePacket : StaticPacket<ClientboundPlayerCreatePacketDS>
    {
        public override byte Id => 0;
        public override PacketBound Bound => PacketBound.Client;
        
        public override void Handle(NetPeer netPeer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
#if STICK_CLIENT
            var username = reader.GetString();
            var playerColor = (PlayerColor)reader.GetInt();
            var guid = reader.GetGuid();

            SharedLog.Info($"New player: [guid: {guid}, username: {username}]");
            
            StickGame.PlayerManager.AddPlayer(new Player(StickGame.NetClient.serverConnection, username, playerColor, guid));
#endif
        }

        public override void Send(NetPeer netPeer, ClientboundPlayerCreatePacketDS dataStruct)
        {
            NetDataWriter writer = new();
            writer.Put(Id);
            writer.Put(dataStruct.username);
            writer.Put((int) dataStruct.playerColor);
            writer.Put(dataStruct.temporaryGuid);
            SendPacket(netPeer, DeliveryMethod.ReliableOrdered, writer);
        }
    }
}