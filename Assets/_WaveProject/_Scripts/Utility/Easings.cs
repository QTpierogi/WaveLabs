using UnityEngine;

namespace WaveProject.Utility
{
    public static class Easings
    {
        public static float OutCirc(float x)
        {
            x = Mathf.Clamp01(x);
            return Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2));
        }
    }
}