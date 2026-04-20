using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WaveProject
{
    public class ConnectorChoiceUIVIew : MonoBehaviour
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _smoothButton;
        [SerializeField] private Button _steppedButton;

        public void Init(Action chooseSmoothConnector, Action chooseSteppedConnector, Action back)
        {
            _backButton.onClick.AddListener(() => back());
            _smoothButton.onClick.AddListener(() => chooseSmoothConnector());
            _steppedButton.onClick.AddListener(() => chooseSteppedConnector());
        }
    }
}
