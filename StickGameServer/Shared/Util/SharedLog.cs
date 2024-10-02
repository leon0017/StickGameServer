
namespace StickGameServer.Shared.Util
{
    public class SharedLog
    {
        public static void Info(string msg)
        {
#if STICK_CLIENT
            UnityEngine.Debug.Log(msg);
#else
            System.Console.WriteLine(msg);
#endif
        }

        public static void Severe(string msg)
        {
#if STICK_CLIENT
            UnityEngine.Debug.LogError(msg);
#else
            System.Console.Error.WriteLine(msg);
#endif
        }
    }
}
