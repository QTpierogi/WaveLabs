using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WaveProject
{
    public class InsertUIView : MonoBehaviour
    {
        [Space]
        [SerializeField] private TextMeshProUGUI _titleText;

        [Space]
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _longButton;
        [SerializeField] private Button _crossButton;

        [Space]
        [SerializeField] private TMP_Dropdown _insertTypeDropdown;
        [SerializeField] private CarriageStation _carriageStation;

        public void Init(Action insertLong, Action insertCross, Action back)
        {
            _backButton.onClick.AddListener(() => back());
            _longButton.onClick.AddListener(() => insertLong());
            _crossButton.onClick.AddListener(() => insertCross());
            _insertTypeDropdown.onValueChanged.AddListener(SetInsertType);
        }

        private void SetInsertType(int value)
        {
            bool isLoop = value == 0 ? false : true;
            _carriageStation.loopInsert = isLoop;
        }
    }
}
