using LiteNetLib;
using LiteNetLib.Utils;

namespace StickGameServer.Shared.Packet
{
    /// <summary>
    /// Either incomingPacket or outgoingPacket will be null, the other is the type of packet.
    /// Can't find a better way to do this at the moment.
    /// </summary>
    public struct UnprocessedPacket(UnprocessedIncomingPacket? incomingPacket, UnprocessedOutgoingPacket? outgoingPacket)
    {
        public UnprocessedIncomingPacket? incomingPacket = incomingPacket;
        public UnprocessedOutgoingPacket? outgoingPacket = outgoingPacket;
    }

    public struct UnprocessedIncomingPacket(NetPeer peer, DeliveryMethod deliveryMethod, NetPacketReader reader)
    {
        public NetPeer peer = peer;
        public DeliveryMethod deliveryMethod = deliveryMethod;
        public NetPacketReader reader = reader;
    }

    public struct UnprocessedOutgoingPacket(NetPeer peer, DeliveryMethod deliveryMethod, NetDataWriter writer)
    {
        public NetPeer peer = peer;
        public DeliveryMethod deliveryMethod = deliveryMethod;
        public NetDataWriter writer = writer;
    }

    public abstract class StaticPacketBase
    {
        public abstract byte Id { get; }
        public abstract PacketBound Bound { get; }
        public abstract void Handle(NetPeer netPeer, NetPacketReader reader, DeliveryMethod deliveryMethod);
        public abstract void Send(NetPeer netPeer, object dataStruct);

        protected void SendPacket(NetPeer peer, DeliveryMethod deliveryMethod, NetDataWriter writer)
        {
            PacketQueue.SendPacket(peer, deliveryMethod, writer);
        }
    }

    /// <summary>
    /// One instance of Packet class for each packet for the Program.
    /// </summary>
    /// <typeparam name="DS">Data structure of the packet for sending</typeparam>
    public abstract class StaticPacket<DS> : StaticPacketBase
    {
        public override void Send(NetPeer netPeer, object dataStruct)
        {
            Send(netPeer, (DS)dataStruct);
        }

        public abstract void Send(NetPeer netPeer, DS dataStruct);
    }
}
