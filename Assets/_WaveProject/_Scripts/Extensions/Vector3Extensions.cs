using UnityEngine;

namespace WaveProject.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 Multiply(this Vector3 first, Vector3 second)
        {
            return new Vector3(first.x * second.x, first.y * second.y, first.z * second.z);
        }
    }
}