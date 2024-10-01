
using LiteNetLib;
using LiteNetLib.Utils;
using StickGameServer.Shared.Util;

namespace StickGameServer.Shared.Packet.Packets
{
    public struct ClientboundBallPacketDS
    {
        public Vec3f ballPos;
    };

    public class ClientboundBallPacket : StaticPacket<ClientboundBallPacketDS>
    {
        public override byte Id => 0;

        public override PacketBound Bound => PacketBound.Client;

        public override void Handle(NetPeer netPeer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {

        }

        public override void Send(NetPeer netPeer, ClientboundBallPacketDS dataStruct)
        {
            NetDataWriter dataWriter = new(); // TODO: probably make this not new every time not sure
            //dataWriter.Put(Id);
            dataWriter.PutArray(dataStruct.ballPos.ToFloatArray());
            SendPacket(netPeer, DeliveryMethod.ReliableOrdered, dataWriter);
        }
    }
}
