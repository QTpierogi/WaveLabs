using UnityEngine;

namespace WaveProject.Extensions
{
    public static class MonoExtensions
    {
        public static T Get<T>(this MonoBehaviour mono) where T : MonoBehaviour
        {
            var simpleGo = new GameObject
            {
                name = typeof(T).Name
            };

            var component = simpleGo.AddComponent<T>();
            return component;
        }
    }
}