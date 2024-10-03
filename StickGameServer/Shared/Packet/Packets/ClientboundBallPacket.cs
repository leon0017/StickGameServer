
using LiteNetLib;
using LiteNetLib.Utils;
using StickGameServer.Shared.Util;
using System;
#if STICK_CLIENT
using UnityEngine;
#endif

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

#if STICK_CLIENT
        private GameObject clientBall;
        private GameObject clientBallInterpolated;

        public ClientboundBallPacket()
        {
            clientBall = GameObject.Find("Ball");
            clientBallInterpolated = GameObject.Find("Ball Interpolated");
        }
#endif

        long prev = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        public override void Handle(NetPeer netPeer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
#if STICK_CLIENT
            // vec3f
            float[] ballPos = reader.GetFloatArray();

            Vector3 currentPos = clientBall.transform.position;

            //oldBallPos = new Vector3(currentPos.x, currentPos.y, currentPos.z);
            //newBallPos = new Vector3(ballPos[0], ballPos[1], ballPos[2]);

            clientBall.transform.position = new Vector3(ballPos[0], ballPos[1], ballPos[2]);

            //lerpStartTime = Time.time;
            //isLerping = true;

            SharedLog.Info("" + (DateTimeOffset.Now.ToUnixTimeMilliseconds() - prev));
            prev = DateTimeOffset.Now.ToUnixTimeMilliseconds();
#endif
        }

        public override void Send(NetPeer netPeer, ClientboundBallPacketDS dataStruct)
        {
#if !STICK_CLIENT
            NetDataWriter dataWriter = new(); // TODO: probably make this not new every time not sure
            dataWriter.Put(Id);
            dataWriter.PutArray(dataStruct.ballPos.ToFloatArray());
            SendPacket(netPeer, DeliveryMethod.ReliableOrdered, dataWriter);
#endif
        }
    }
}
