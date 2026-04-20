using System;
using UnityEngine;
using UnityEngine.UI;

namespace WaveProject
{
    public class RemoveInsertUIView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        public void AddListener(Action action)
        {
            _button.onClick.AddListener(() => action());
        }
    }
}
