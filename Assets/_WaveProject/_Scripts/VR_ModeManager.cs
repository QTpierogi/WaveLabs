using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Management;

namespace WaveProject
{
    public class VR_ModeManager : MonoBehaviour
    {
        [SerializeField] private bool enableVR = false;

        public bool EnableVR => enableVR;

        [SerializeField] private Transform cameraOffset;

        [SerializeField] private UnityEvent vrOn;
        [SerializeField] private UnityEvent vrOff;

        private XRLoader SelectedXRLoader;
        private static VR_ModeManager instance;

        public static VR_ModeManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindFirstObjectByType<VR_ModeManager>();
                return instance;
            }
        }

        private void Awake()
        {
            var existingSession = IsSessionExist();
            if (existingSession != null)
            {
                Destroy(gameObject);
            }
            else DontDestroyOnLoad(this);
        }

        public VR_ModeManager IsSessionExist()
        {
            VR_ModeManager[] Sessions = FindObjectsByType<VR_ModeManager>(FindObjectsSortMode.None);
            foreach(VR_ModeManager session in Sessions)
            {
                if (session != this)
                    return session;
            }
            return null;
        }

        private void Start()
        {
            if (!enableVR)
                StopXR();
            cameraOffset.localPosition = Vector3.zero;
        }

        public void SwitchMode()
        {
            if (!enableVR)
                StartXR(0);
            else StopXR();
        }

        public void StartXR(int loaderIndex)
        {
            if (SelectedXRLoader == null)
                SelectedXRLoader = XRGeneralSettings.Instance.Manager.activeLoaders[loaderIndex];

            StartCoroutine(StartXRCoroutine());
        }

        IEnumerator StartXRCoroutine()
        {
            var initSuccess = SelectedXRLoader.Initialize();
            if(!initSuccess)
            {
                Debug.LogError("Error initializing selected loader");
            }
            else
            {
                yield return null;
                Debug.Log("Start XR Loader");
                var startSuccess = SelectedXRLoader.Start();
                if(!startSuccess)
                {
                    yield return null;
                    Debug.LogError("Error starting selected loader");
                    SelectedXRLoader.Deinitialize();
                }
                else
                {
                    enableVR = true; 
                    vrOn?.Invoke();
                }
            }
        }

        public void StopXR()
        {
            if (SelectedXRLoader == null)
                SelectedXRLoader = XRGeneralSettings.Instance.Manager.activeLoaders[0];

            SelectedXRLoader.Stop();
            SelectedXRLoader.Deinitialize();
            SelectedXRLoader = null;

            enableVR = false;
            vrOff?.Invoke();
        }
    }
}
