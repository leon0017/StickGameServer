using System;
using StickGameServer.Shared.Packet.Packets;

namespace StickGameServer.Shared.Packet
{
    public static class PacketRegistry
    {
        public static readonly ServerboundMovementPacket SERVERBOUND_MOVEMENT_PACKET = new();
        public static readonly ServerboundHandAnimationPacket SERVERBOUND_HAND_ANIMATION_PACKET = new();
        public static readonly ClientboundPlayerCreatePacket CLIENTBOUND_PLAYER_CREATE_PACKET = new();
        public static readonly ClientboundPlayerMovePacket CLIENTBOUND_PLAYER_MOVE_PACKET = new();
        public static readonly ClientboundPlayerDestroyPacket CLIENTBOUND_PLAYER_DESTROY_PACKET = new();
        public static readonly ClientboundHandAnimationPacket CLIENTBOUND_HAND_ANIMATION_PACKET = new();

        private static readonly StaticPacketBase[] serverboundPackets = new StaticPacketBase[256];
        private static readonly StaticPacketBase[] clientboundPackets = new StaticPacketBase[256];

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
            InitPacket(SERVERBOUND_HAND_ANIMATION_PACKET);

            InitPacket(CLIENTBOUND_PLAYER_CREATE_PACKET);
            InitPacket(CLIENTBOUND_PLAYER_MOVE_PACKET);
            InitPacket(CLIENTBOUND_PLAYER_DESTROY_PACKET);
            InitPacket(CLIENTBOUND_HAND_ANIMATION_PACKET);
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