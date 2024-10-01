using LiteNetLib;
using LiteNetLib.Utils;

namespace StickGameServer.Shared.Packet
{
    public class PacketQueue
    {
        private static List<UnprocessedPacket> unprocessedPackets = [];

        public static void Unqueue()
        {
            foreach (UnprocessedPacket unprocessedPacket in unprocessedPackets)
            {
                if (unprocessedPacket.incomingPacket != null)
                {
                    ProcessIncomingPacket((UnprocessedIncomingPacket)unprocessedPacket.incomingPacket);
                }
                else if (unprocessedPacket.outgoingPacket != null)
                {
                    ProcessOutgoingPacket((UnprocessedOutgoingPacket)unprocessedPacket.outgoingPacket);
                }
                else
                {
                    throw new NotImplementedException("incoming and outgoing packet both null?");
                }
            }

            unprocessedPackets.Clear();
        }

        private static void ProcessIncomingPacket(UnprocessedIncomingPacket incomingPacket)
        {
            if (incomingPacket.peer == null)
            {
                return;
            }

            NetPacketReader reader = incomingPacket.reader;
            byte packetId = reader.GetByte();

            StaticPacketBase staticPacketBase;
#if STICK_CLIENT
            staticPacketBase = PacketRegistry.GetServerboundPacketBaseFromId(packetId);
#else
            staticPacketBase = PacketRegistry.GetClientboundPacketBaseFromId(packetId);
#endif

            staticPacketBase.Handle(incomingPacket.peer, reader, incomingPacket.deliveryMethod);

            reader.Recycle();
        }

        private static void ProcessOutgoingPacket(UnprocessedOutgoingPacket outgoingPacket)
        {
            if (outgoingPacket.peer == null)
            {
                return;
            }

            outgoingPacket.peer.Send(outgoingPacket.writer, outgoingPacket.deliveryMethod);
        }

        public static void HandleIncomingPacket(NetPeer peer, NetPacketReader reader, byte channelReader, DeliveryMethod deliveryMethod)
        {
            UnprocessedIncomingPacket unprocessedIncomingPacket = new(peer, deliveryMethod, reader);
            UnprocessedPacket unprocessedPacket = new(unprocessedIncomingPacket, null);
            unprocessedPackets.Add(unprocessedPacket);
        }

        public static void SendPacket(NetPeer peer, DeliveryMethod deliveryMethod, NetDataWriter writer)
        {
            UnprocessedOutgoingPacket unprocessedOutgoingPacket = new(peer, deliveryMethod, writer);
            UnprocessedPacket unprocessedPacket = new(null, unprocessedOutgoingPacket);
            unprocessedPackets.Add(unprocessedPacket);
        }
    }
}
