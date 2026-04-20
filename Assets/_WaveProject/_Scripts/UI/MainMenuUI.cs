using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WaveProject.Services;
using WaveProject.UserInput;

namespace WaveProject.UI
{
    public class MainMenu : MonoBehaviour
    {

        [SerializeField] private Transform[] _menuList;

        [SerializeField] private TMP_Dropdown _qualityDropdown;
        [SerializeField] private TMP_Dropdown _resolutionDropdown;
        [SerializeField] private TMP_Dropdown _fullscreenModeDropdown;

        [SerializeField] private Button _manualButton;
        [SerializeField] private Button _exitButton;

        private InputController _inputController;

        
        private readonly string _manualPath = Path.Combine(Application.streamingAssetsPath, "manual.pdf");

        private void OnEnable()
        {
            
            _qualityDropdown.onValueChanged.AddListener(SetQualityLevel);
            _resolutionDropdown.onValueChanged.AddListener(SetResolution);
            _fullscreenModeDropdown.onValueChanged.AddListener(SetFullscreenMode);

            _manualButton.onClick.AddListener(OpenManual);
            _exitButton.onClick.AddListener(Application.Quit);
        }

        private void OnDisable()
        {
            
            _qualityDropdown.onValueChanged.RemoveListener(SetQualityLevel);
            _resolutionDropdown.onValueChanged.RemoveListener(SetResolution);
            _fullscreenModeDropdown.onValueChanged.RemoveListener(SetFullscreenMode);

            _manualButton.onClick.RemoveListener(OpenManual);
            _exitButton.onClick.RemoveListener(Application.Quit);
        }

        private IEnumerator Start()
        {
            ServiceManager.TryGetService(out InputController inputController);
            _inputController = inputController;

            LoadQuality();
            LoadResolution();
            LoadFullscreenMode();

            yield return HideButtonsIfWeb();
        }

        private IEnumerator HideButtonsIfWeb()
        {
#if UNITY_WEBGL
                    _resolutionDropdown.gameObject.SetActive(false);
                    _exitButton.gameObject.SetActive(false);

                    yield return null;
#else
            yield return null; 
#endif
        }

        private void LoadQuality()
        {
            _qualityDropdown.options = new List<TMP_Dropdown.OptionData>();
            foreach (var qualityLevel in QualitySettings.names)
            {
                _qualityDropdown.options.Add(new TMP_Dropdown.OptionData(qualityLevel));
            }
            
            _qualityDropdown.value = QualitySettings.GetQualityLevel();
        }

        private void LoadResolution()
        {
           _resolutionDropdown.options = new List<TMP_Dropdown.OptionData>();
           foreach (var resolution in Screen.resolutions)
           {
               _resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.ToString()));
           }

           _resolutionDropdown.value = Screen.resolutions
               .ToList()
               .FindIndex(resolution => resolution.width == Screen.width && resolution.height == Screen.height);
        }

        private void LoadFullscreenMode()
        {
            _fullscreenModeDropdown.options = new List<TMP_Dropdown.OptionData> { new("Fullscreen"), new("Windowed") };
            _fullscreenModeDropdown.value = Screen.fullScreenMode == FullScreenMode.FullScreenWindow ? 0 : 1;
        }

        private void SetQualityLevel(int value)
        {
            QualitySettings.SetQualityLevel(value, true);
        }

        private void SetResolution(int value)
        {
            var resolution = Screen.resolutions[value];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
        }

        private void SetFullscreenMode(int value)
        {
            var mode = value == 0 ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
            Screen.fullScreenMode = mode;
        }

        private void OpenManual()
        {
            Application.OpenURL(_manualPath);
        }

        public void LoadLevel(int levelId)
        {
            //DOTween.KillAll();
            SceneManager.LoadScene(levelId);
        }

        public void SetActiveMenu(Transform activeMenu)
        {
            activeMenu.gameObject.SetActive(true);
            foreach(var menu in _menuList)
                if (menu != activeMenu)
                    menu.gameObject.SetActive(false); 
        }
    }
}