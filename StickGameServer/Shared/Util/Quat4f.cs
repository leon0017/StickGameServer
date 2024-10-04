namespace StickGameServer.Shared.Util
{
    public struct Quat4f
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public Quat4f(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Quat4f(double x, double y, double z, double w)
        {
            this.x = (float) x;
            this.y = (float) y;
            this.z = (float) z;
            this.w = (float) w;
        }

        public Quat4f(float[] arr)
        {
            x = arr[0];
            y = arr[1];
            z = arr[2];
            w = arr[3];
        }

#if STICK_CLIENT
        public Quat4f(UnityEngine.Quaternion quaternion)
        {
            x = quaternion.x;
            y = quaternion.y;
            z = quaternion.z;
            w = quaternion.w;
        }
#endif

        public float[] ToFloatArray()
        {
            return new float[4] { x, y, z, w };
        }

#if STICK_CLIENT
        public UnityEngine.Quaternion ToQuaternion()
        {
            return new UnityEngine.Quaternion(x, y, z, w);
        }
#endif

        public override string ToString()
        {
            return $"Quat4f[{x}, {y}, {z}, {w}]";
        }
    }
}