using System;
using LiteNetLib;

namespace StickGameServer.Shared.Game
{
    public class Player
    {
        public readonly Guid localGuid;
        private readonly int cachedGuidHashCode;
        public readonly NetPeer connection;
        public readonly string username;
        public readonly PlayerColor playerColor;
        public int ping;
        
        public Player(NetPeer connection, string username, PlayerColor playerColor)
        {
            localGuid = Guid.NewGuid();
            cachedGuidHashCode = localGuid.GetHashCode();
            
            this.connection = connection;
            this.username = username;
            this.playerColor = playerColor;
        }
        
        public override int GetHashCode()
        {
            return cachedGuidHashCode;
        }
        
    }
}