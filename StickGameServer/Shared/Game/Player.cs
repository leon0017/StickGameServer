using System;
using LiteNetLib;

namespace StickGameServer.Shared.Game
{
    public class Player
    {
        public readonly Guid temporaryGuid;
        private readonly int cachedGuidHashCode;
        public readonly NetPeer connection;
        public readonly string username;
        public readonly PlayerColor playerColor;
        public int ping;
#if STICK_CLIENT
        public UnityEngine.GameObject gameObject;
        public UnityEngine.Transform ballHead;
        public UnityEngine.Transform leftHand;
        public UnityEngine.Transform rightHand;
        public UnityEngine.Transform leftHandGeomLhand;
        public UnityEngine.Transform rightHandGeomRhand;
        public UnityEngine.Animator leftHandAnimator;
        public UnityEngine.Animator rightHandAnimator;
#endif
        
        public Player(NetPeer connection, string username, PlayerColor playerColor, Guid? temporaryGuid=null)
        {
            this.temporaryGuid = temporaryGuid ?? Guid.NewGuid();
            cachedGuidHashCode = temporaryGuid.GetHashCode();
            
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