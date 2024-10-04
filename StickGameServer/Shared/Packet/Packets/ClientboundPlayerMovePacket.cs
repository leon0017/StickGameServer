using System;
using LiteNetLib;
using LiteNetLib.Utils;
using StickGameServer.Shared.Util;

namespace StickGameServer.Shared.Packet.Packets
{
    public struct ClientboundPlayerMovePacketDS
    {
        public Guid playerGuid;
        public Vec3f headPos;
        public Vec3f leftHandPos;
        public Vec3f rightHandPos;
        public Quat4f headQuat;
        public Quat4f leftHandQuat;
        public Quat4f rightHandQuat;
    }
    
    public class ClientboundPlayerMovePacket : StaticPacket<ClientboundPlayerMovePacketDS>
    {
        public override byte Id => 1;
        public override PacketBound Bound => PacketBound.Client;
        
        public override void Handle(NetPeer netPeer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
#if STICK_CLIENT
            var guid = reader.GetGuid();
            var player = StickGame.PlayerManager.GetPlayer(guid);

            if (player == null)
            {
                return;
            }
            
            player.ballHead.transform.position = new Vec3f(reader.GetFloatArray()).ToVector3();
            player.leftHand.transform.position = new Vec3f(reader.GetFloatArray()).ToVector3();
            player.rightHand.transform.position = new Vec3f(reader.GetFloatArray()).ToVector3();
            player.ballHead.transform.rotation = new Quat4f(reader.GetFloatArray()).ToQuaternion();
            player.leftHand.transform.rotation = new Quat4f(reader.GetFloatArray()).ToQuaternion();
            player.rightHand.transform.rotation = new Quat4f(reader.GetFloatArray()).ToQuaternion();
#endif
        }

        public override void Send(NetPeer netPeer, ClientboundPlayerMovePacketDS dataStruct)
        {
            NetDataWriter writer = new();
            writer.Put(Id);
            writer.Put(dataStruct.playerGuid);
            writer.PutArray(dataStruct.headPos.ToFloatArray());
            writer.PutArray(dataStruct.leftHandPos.ToFloatArray());
            writer.PutArray(dataStruct.rightHandPos.ToFloatArray());
            writer.PutArray(dataStruct.headQuat.ToFloatArray());
            writer.PutArray(dataStruct.leftHandQuat.ToFloatArray());
            writer.PutArray(dataStruct.rightHandQuat.ToFloatArray());
            SendPacket(netPeer, DeliveryMethod.ReliableOrdered, writer);
        }
    }
}