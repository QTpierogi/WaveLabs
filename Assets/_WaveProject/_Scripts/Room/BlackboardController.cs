using System;
using System.IO;
using TMPro;
using UnityEngine;
using WaveProject.Services;
using WaveProject.UserInput;

namespace WaveProject.Room
{
    public class BlackboardController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _boardText;

        private InputController _inputController;

        private void OnEnable()
        {
            _boardText.onSelect.AddListener(OnTextSelected);
            _boardText.onDeselect.AddListener(OnTextDeselected);
        }
        private void OnDisable()
        {
            _boardText.onSelect.RemoveListener(OnTextSelected);
            _boardText.onDeselect.RemoveListener(OnTextDeselected);
        }

        private void Start()
        {
            ServiceManager.TryGetService(out InputController inputController);
            _inputController = inputController;
            
            LoadText();
        }

        private void LoadText()
        {
            var result = ReadString();

            if (string.IsNullOrEmpty(result) == false)
            {
                _boardText.text = result;
            }
        }

        private void OnTextSelected(string _)
        {
            _inputController.BlockMovement(true);
        }

        private void OnTextDeselected(string _)
        {
            _inputController.BlockMovement(false);
        }

        private static string ReadString()
        {
            var path = Path.Combine(Application.streamingAssetsPath, "description.txt");
            
            if (File.Exists(path) == false)
            {
                return string.Empty;
            }
            
            using var reader = new StreamReader(path);
            return reader.ReadToEnd();
        }
    }
}