using System;
using UnityEngine;
using UnityEngine.UI;

namespace WaveProject.UI
{
    public class RemovePlateUIView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        public void AddListener(Action action)
        {
            _button.onClick.AddListener(() => action());
        }
    }
}