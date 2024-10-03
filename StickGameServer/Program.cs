using LiteNetLib;
using LiteNetLib.Utils;
using StickGameServer.Shared.Packet;
using StickGameServer.Shared.Packet.Packets;
using StickGameServer.Shared.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using StickGameServer.Shared.Game;

class Program : INetEventListener, INetLogger
{
    private NetManager netServer;
    private NetDataWriter dataWriter;
    private ulong ticksDone = 0;

    private const int MAX_TPS = 60;
    private static Stopwatch gameLoopStopwatch = new();
    public static HashSet<Player> players = new();

    static void Main()
    {
        new Program().ProgramMain();
    }

    void ProgramMain()
    {
        PacketRegistry.Init();

        NetDebug.Logger = this;
        dataWriter = new();
        netServer = new(this)
        {
            AutoRecycle = false
        };

        netServer.Start(43921);
        SharedLog.Info("Started server.");
        netServer.UpdateTime = (int)((1000f / MAX_TPS) * 0.7); // Max update time

        RunGameLoop();
    }

    void RunGameLoop()
    {
        ulong runs = 0;
        GameLoop gameLoop = new("gameloop", MAX_TPS, () =>
        {
            // TODO: maybe move pollnet to loop that doesnt try catch up
            netServer.PollEvents(); // @60TPS

            // compensating for GameLoop being blocking at the moment with modulo instead of 2 game loops
            if (runs % 2 == 0) // @30TPS
            {
                Tick();
                ticksDone++;
            }

            runs++;
        });
    }

    long prev = DateTimeOffset.Now.ToUnixTimeMilliseconds();
    void Tick()
    {
        // foreach (var player in players)
        // {
        //     /*if (ticksDone % 10 == 0)
        //     {*/
        //         ClientboundBallPacketDS ballPacketDS = new();
        //         Random random = new();
        //         ballPacketDS.ballPos = new Vec3f(-5.0 + (random.NextDouble() * 10.0),
        //             -5.0 + (random.NextDouble() * 10.0),
        //             -5.0 + (random.NextDouble() * 10.0)
        //         );
        //         SharedLog.Info($"MOVING BALL TO: {ballPacketDS.ballPos}");
        //         SharedLog.Info("" + (DateTimeOffset.Now.ToUnixTimeMilliseconds() - prev));
        //         prev = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        //         PacketRegistry.CLIENTBOUND_BALL_PACKET.Send(player.connection, ballPacketDS);
        //     //}
        // }

        PacketQueue.Unqueue();
        /* now done in RunPollNetLoop */
        //netServer.PollEvents(); // TODO: maybe move this and make it run more often, also maybe this running async could make things out of sync but also i cant do it sync so idk
        ticksDone++;
    }
    
    static readonly Random _random = new Random();
    static TEnum RandomEnumValue<TEnum>() where TEnum : struct, Enum
    {
        TEnum[] vals = Enum.GetValues<TEnum>();
        return vals[_random.Next(vals.Length)];
    }

    void INetEventListener.OnPeerConnected(NetPeer peer)
    {
        SharedLog.Info($"Peer connected, peer='{peer}'");
        peer.player = new Player(peer, "Player", RandomEnumValue<PlayerColor>());
        peer.setupPlayer = true;
        players.Add(peer.player);
    }

    void INetEventListener.OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
        SharedLog.Severe($"Network error endPoint='{endPoint}, socketError='{socketError}'");
    }

    void INetEventListener.OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
        reader.Recycle();
    }

    void INetEventListener.OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
        if (peer.setupPlayer)
        {
            peer.player.ping = latency;
        }
    }

    void INetEventListener.OnConnectionRequest(ConnectionRequest connectionRequest)
    {
        connectionRequest.AcceptIfKey("test_key");
    }

    void INetEventListener.OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        SharedLog.Info($"Peer disconnected peer='{peer}', disconnectInfo.Reason='{disconnectInfo.Reason}'");
        players.Remove(peer.player);
    }

    void INetEventListener.OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
    {
        PacketQueue.HandleIncomingPacket(peer, reader, channelNumber, deliveryMethod);
    }

    void INetLogger.WriteNet(NetLogLevel level, string str, params object[] args)
    {
        SharedLog.Info(str + ", " + args);
    }
}