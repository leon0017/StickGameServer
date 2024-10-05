using LiteNetLib;
using LiteNetLib.Utils;
using StickGameServer.Shared.Game;

namespace StickGameServer.Shared.Packet.Packets
{
    public struct ClientboundHelloServerPacketDS
    {
        public PlayerColor playerColor;
    }
    
    public class ClientboundHelloServerPacket : StaticPacket<ClientboundHelloServerPacketDS>
    {
        public override byte Id => 4;
        public override PacketBound Bound => PacketBound.Client;
        public override void Handle(NetPeer netPeer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
#if STICK_CLIENT
            var material = StickGame.PlayerManager.GetMaterialFromPlayerColor((PlayerColor)reader.GetByte());
            StickGame.HelloServer.instance.SetPlayerMaterial(material);
#endif
        }

        public override void Send(NetPeer netPeer, ClientboundHelloServerPacketDS dataStruct)
        {
            NetDataWriter writer = new();
            writer.Put(Id);
            writer.Put((byte)dataStruct.playerColor);
            SendPacket(netPeer, DeliveryMethod.ReliableOrdered, writer);
        }
    }
}