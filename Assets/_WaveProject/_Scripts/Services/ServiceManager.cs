using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace WaveProject.Services
{
    public static class ServiceManager
    {
        private static readonly Dictionary<Type,IService> Services = new();
        
        public static bool TryAddService(IService service)
        {
            if (Services.TryAdd(service.GetType(), service))
            {
                Debug.Log("Services count: " + Services.Count);
                return true; 
            }
            
            var additionalMessage = String.Empty;
            
            var oldService = Services[service.GetType()];

            if (service is MonoBehaviour mono)
            {
                additionalMessage = "Current service was deleted";
                Object.Destroy(mono.gameObject);
            }
            Debug.Log($"Error, the service {service.GetType()} already exists! {additionalMessage}");
            Debug.Log("Services count: " + Services.Count);
            return false;
        }

        public static bool TryGetService<T>(out T service) where T : IService
        {
            if (Services.TryGetValue(typeof(T), out var serviceType))
            {
                service = (T)serviceType;
                return true;
            }

            service = default;
            Debug.Log($"Service - {typeof(T)} not created yet");
            
            return false;
        }

        public static void ClearServices()
        {
            Services.Clear();
        }
    }
}