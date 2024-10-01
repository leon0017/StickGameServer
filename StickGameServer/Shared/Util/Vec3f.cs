namespace StickGameServer.Shared.Util
{
    public struct Vec3f(float x, float y, float z)
    {
        public float x = x;
        public float y = y;
        public float z = z;

        public Vec3f(double x, double y, double z) : this((float)x, (float)y, (float)z)
        {
        }

        public float[] ToFloatArray()
        {
            return [x, y, z];
        }

        public override string ToString()
        {
            return $"Vec3f[{x}, {y}, {z}]";
        }
    }
}
