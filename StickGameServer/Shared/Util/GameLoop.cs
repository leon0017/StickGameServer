
using System;
using System.Diagnostics;
using System.Threading;

namespace StickGameServer.Shared.Util
{

    // TODO: In Unity, GameLoop is non-blocking, but outside of Unity it is blocking
    public class GameLoop
    {
        private const long WARN_DELAY_MS = 3000;

        private Stopwatch stopwatch = new Stopwatch();
        private double accumulatedTimeMs = 0.0;
        private string loopName;
        private double tickDurationMs;
        private long nextWarnAt = 0;

        void Init(string loopName, int tps)
        {
            this.loopName = loopName;
            tickDurationMs = 1000.0 / tps;
        }

#if STICK_CLIENT
        public GameLoop(UnityEngine.MonoBehaviour monoBehaviour, string loopName, int tps, string tickMethodName)
        {
            Init(loopName, tps);
            monoBehaviour.InvokeRepeating(tickMethodName, 0f, 1f / (tps * 1.25f)); // Mul repeat freq by 1.25 times to ensure timer doesn't slip
        }
#else
        public GameLoop(string loopName, int tps, Action tick)
        {
            Init(loopName, tps);
            RunWithoutUnity(tick);
        }
#endif

        void LoopLogic(Action tick)
        {
            double elapsedTimeMs = stopwatch.Elapsed.TotalMilliseconds;

            stopwatch.Restart();

            accumulatedTimeMs += elapsedTimeMs;

            while (accumulatedTimeMs >= tickDurationMs)
            {
                tick();
                accumulatedTimeMs -= tickDurationMs;

                if (accumulatedTimeMs > (tickDurationMs * 4.0) && DateTimeOffset.Now.ToUnixTimeMilliseconds() >= nextWarnAt)
                {
                    SharedLog.Severe($"Loop '{loopName}' is running {(long)accumulatedTimeMs}ms behind!");
                    nextWarnAt = DateTimeOffset.Now.ToUnixTimeMilliseconds() + WARN_DELAY_MS;
                }
            }

#if !STICK_CLIENT
            Thread.Sleep(1); // Avoid busy waiting
#endif
        }

#if !STICK_CLIENT
        void RunWithoutUnity(Action tick)
        {
            stopwatch.Start();

            while (true)
            {
                LoopLogic(tick);
            }
        }
#endif

#if STICK_CLIENT
        public void UnityHelper(Action tick)
        {
            if (stopwatch == null)
            {
                stopwatch = new();
                stopwatch.Start();
            }

            LoopLogic(tick);
        }
#endif

    }
}
