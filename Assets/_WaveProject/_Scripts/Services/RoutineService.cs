using System;
using System.Collections;
using UnityEngine;

namespace WaveProject.Services
{
    public class RoutineService : MonoBehaviour, IService
    {
        private void Awake()
        {
            //DontDestroyOnLoad(this);
        }

        public void WaitTime(float time, MonoBehaviour owner = null, Action callback = null)
        {
            var ownerObject = owner == null ? this : owner;
            ownerObject.StartCoroutine(WaitForTime(time, callback));
        }

        private IEnumerator WaitForTime(float time, Action callback = null)
        {
            yield return new WaitForSeconds(time);
            callback?.Invoke();
        }
    }
}