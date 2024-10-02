using LiteNetLib;
using LiteNetLib.Utils;

namespace StickGameServer.Shared.Packet
{
    /// <summary>
    /// Either incomingPacket or outgoingPacket will be null, the other is the type of packet.
    /// Can't find a better way to do this at the moment.
    /// </summary>
    public struct UnprocessedPacket
    {
        public UnprocessedIncomingPacket? incomingPacket;
        public UnprocessedOutgoingPacket? outgoingPacket;

        public UnprocessedPacket(UnprocessedIncomingPacket? incomingPacket, UnprocessedOutgoingPacket? outgoingPacket)

        {
            this.incomingPacket = incomingPacket;
            this.outgoingPacket = outgoingPacket;
        }
    }

    public struct UnprocessedIncomingPacket
    {
        public NetPeer peer;
        public DeliveryMethod deliveryMethod;
        public NetPacketReader reader;

        public UnprocessedIncomingPacket(NetPeer peer, DeliveryMethod deliveryMethod, NetPacketReader reader)
        {
            this.peer = peer;
            this.deliveryMethod = deliveryMethod;
            this.reader = reader;
        }
    }

    public struct UnprocessedOutgoingPacket
    {
        public NetPeer peer;
        public DeliveryMethod deliveryMethod;
        public NetDataWriter writer;

        public UnprocessedOutgoingPacket(NetPeer peer, DeliveryMethod deliveryMethod, NetDataWriter writer)
        {
            this.peer = peer;
            this.deliveryMethod = deliveryMethod;
            this.writer = writer;
        }
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
