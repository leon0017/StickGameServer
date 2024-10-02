using StickGameServer.Shared.Packet.Packets;
using System;

namespace StickGameServer.Shared.Packet
{
    public class PacketRegistry
    {
        public static readonly ServerboundMovementPacket SERVERBOUND_MOVEMENT_PACKET = new ServerboundMovementPacket();
        public static readonly ClientboundBallPacket CLIENTBOUND_BALL_PACKET = new ClientboundBallPacket();

        private static StaticPacketBase[] serverboundPackets = new StaticPacketBase[256];
        private static StaticPacketBase[] clientboundPackets = new StaticPacketBase[256];

        public static StaticPacketBase GetServerboundPacketBaseFromId(byte id)
        {
            // TODO: check if packet id not out of bounds or null
            return serverboundPackets[id];
        }

        public static StaticPacketBase GetClientboundPacketBaseFromId(byte id)
        {
            // TODO: check if packet id not out of bounds or null
            return clientboundPackets[id];
        }

        public static void Init()
        {
            InitPacket(SERVERBOUND_MOVEMENT_PACKET);

            InitPacket(CLIENTBOUND_BALL_PACKET);
        }

        private static void InitPacket<DS>(StaticPacket<DS> staticPacket)
        {
            StaticPacketBase[] arr = staticPacket.Bound switch
            {
                PacketBound.Server => serverboundPackets,
                PacketBound.Client => clientboundPackets,
                _ => throw new NotImplementedException()
            };

            arr[staticPacket.Id] = staticPacket;
        }
    }
}
