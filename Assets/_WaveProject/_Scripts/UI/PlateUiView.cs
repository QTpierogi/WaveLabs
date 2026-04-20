using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WaveProject.UI
{
    public class PlateUiView : MonoBehaviour
    {
        [Space] 
        [SerializeField] private Slider _lengthSlider;
        [SerializeField] private Slider _thicknessSlider;
        [SerializeField] private Slider _resistanceSlider;

        [Space] 
        [SerializeField] private TextMeshProUGUI _titleText;
        
        [Space] 
        [SerializeField] private TMP_InputField _lengthText;
        [SerializeField] private TMP_InputField _thicknessText;
        [SerializeField] private TMP_InputField _resistanceText;
        
        [Space] 
        [SerializeField] private GameObject _resistance;
        
        [Space] 
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _createButton;
        
        private bool _lengthConfigured;
        private bool _thicknessConfigured;
        private bool _resistanceConfigured;

        public event Action<float> LengthChanged;
        public event Action<float> ThicknessChanged;
        public event Action<float> ResistanceChanged;

        public void Init(Action create, Action back,
            float lengthMinValue,
            float lengthMaxValue,
            float thicknessMinValue,
            float thicknessMaxValue,
            float resistanceMinValue,
            float resistanceMaxValue)
        {
            _lengthSlider.minValue = lengthMinValue;
            _lengthSlider.maxValue = lengthMaxValue;
            _lengthSlider.value = 0;

            _thicknessSlider.minValue = thicknessMinValue;
            _thicknessSlider.maxValue = thicknessMaxValue;
            _thicknessSlider.value = 0;

            _resistanceSlider.minValue = resistanceMinValue;
            _resistanceSlider.maxValue = resistanceMaxValue;
            _resistanceSlider.value = 0;

            _lengthSlider.onValueChanged.AddListener(ChangeLength);
            _thicknessSlider.onValueChanged.AddListener(ChangeThickness);
            _resistanceSlider.onValueChanged.AddListener(ChangeResistance);
            
            _lengthText.onValueChanged.AddListener(value => ChangeLength(float.Parse(value)));
            _thicknessText.onValueChanged.AddListener(value => ChangeThickness(float.Parse(value)));
            _resistanceText.onValueChanged.AddListener(value => ChangeResistance(float.Parse(value)));

            _backButton.onClick.AddListener(() => back());
            _createButton.onClick.AddListener(() => create());
            
            _createButton.interactable = false;
        }

        private void ChangeLength(float value)
        {
            value = Mathf.Clamp(value, _lengthSlider.minValue, _lengthSlider.maxValue);
            
            _lengthSlider.value = value;
            _lengthConfigured = true;
            LengthChanged?.Invoke(ChangeParameter(_lengthText, value));
        }

        private void ChangeThickness(float time)
        {
            time = Mathf.Clamp(time, _thicknessSlider.minValue, _thicknessSlider.maxValue);
            
            _thicknessSlider.value = time;
            _thicknessConfigured = true;
            ThicknessChanged?.Invoke(ChangeParameter(_thicknessText, time));
        }

        private void ChangeResistance(float time)
        {
            time = Mathf.Clamp(time, _resistanceSlider.minValue, _resistanceSlider.maxValue);
            
            _resistanceSlider.value = time;
            _resistanceConfigured = true;
            ResistanceChanged?.Invoke(ChangeParameter(_resistanceText, time));
        }

        private static float ChangeParameter(TMP_InputField text, float value)
        {
            text.text = value.ToString("F");
            return value;
        }

        public void SelectMetalPlate()
        {
            _resistance.gameObject.SetActive(false);
            _createButton.interactable = false;

            _titleText.text = "Размеры металлической пластинки";

            StartCoroutine(WaitAllConfigured(true));
        }

        public void SelectDielectricPlate()
        {
            _resistance.gameObject.SetActive(true);
            _createButton.interactable = false;
            
            _titleText.text = "Параметры диэлектрической пластинки";

            StartCoroutine(WaitAllConfigured(false));
        }

        private IEnumerator WaitAllConfigured(bool isMetal)
        {
            if (isMetal)
            {
                yield return new WaitWhile(() => _lengthConfigured == false ||
                                                 _thicknessConfigured == false);
            }
            else
            {
                yield return new WaitWhile(() => _lengthConfigured == false ||
                                                 _thicknessConfigured == false ||
                                                 _resistanceConfigured == false);
            }
            
            _createButton.interactable = true;
        }
    }
}