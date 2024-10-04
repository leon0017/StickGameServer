namespace StickGameServer.Shared.Util
{
    public struct Vec3f
    {
        public float x;
        public float y;
        public float z;

        public Vec3f(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vec3f(double x, double y, double z)
        {
            this.x = (float) x;
            this.y = (float) y;
            this.z = (float) z;
        }

        public Vec3f(float[] arr)
        {
            x = arr[0];
            y = arr[1];
            z = arr[2];
        }

#if STICK_CLIENT
        public Vec3f(UnityEngine.Vector3 vector3)
        {
            x = vector3.x;
            y = vector3.y;
            z = vector3.z;
        }
#endif

        public float[] ToFloatArray()
        {
            return new float[3] { x, y, z };
        }

#if STICK_CLIENT
        public UnityEngine.Vector3 ToVector3()
        {
            return new UnityEngine.Vector3(x, y, z);
        }
#endif

        public override string ToString()
        {
            return $"Vec3f[{x}, {y}, {z}]";
        }
    }
}