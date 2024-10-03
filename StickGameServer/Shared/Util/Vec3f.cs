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

        public float[] ToFloatArray()
        {
            return new float[3] { x, y, z };
        }

        public override string ToString()
        {
            return $"Vec3f[{x}, {y}, {z}]";
        }
    }
}
