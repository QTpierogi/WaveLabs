using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WaveProject
{
    public class ChangeWiresUIView : MonoBehaviour
    {
        [Space]
        [SerializeField] private TextMeshProUGUI _titleText;

        [Space]
        [SerializeField] private Button _confirmChangeButton;
        [SerializeField] private Button _cancelChangeButton;

        public void Init(Action changeWires, Action cancel)
        {
            _confirmChangeButton.onClick.AddListener(() => changeWires());
            _cancelChangeButton.onClick.AddListener(() => cancel());
        }
    }
}
