using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WaveProject
{
    public class VR_Switch : MonoBehaviour
    {
        private Toggle toggle;

        private void Awake()
        {
            toggle = GetComponent<Toggle>();

            if (VR_ModeManager.Instance.EnableVR)
                toggle.isOn = true;
        }

        public void SwitchToMainMenu()
        {
            if (VR_ModeManager.Instance.EnableVR)
                VR_ModeManager.Instance.SwitchMode();
        }

        public void SwitchMode()
        {
            if (toggle.isOn == VR_ModeManager.Instance.EnableVR)
                return;

            VR_ModeManager.Instance.SwitchMode();
        }
    }
}
