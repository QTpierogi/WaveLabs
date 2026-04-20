using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using WaveProject.Services;
using WaveProject.UserInput;

namespace WaveProject
{
    public class HelpUIView : MonoBehaviour
    {
        [SerializeField] private Button _helpButton;
        [SerializeField] private Button _helpHideMenu;
        [SerializeField] private RectTransform _menu;

        [SerializeField] private float _xOpenedPosition = -60;
        [SerializeField] private float _animationDuration = 1.5f;
        [SerializeField] private Ease _animationEase;

        private InputController _inputController;

        private float _xHidedPosition;
        private bool _opened;
        private bool _inProcess;
        private Sequence _animationSequence;

        private void OnEnable()
        {
            _helpButton.onClick.AddListener(ToggleShowMenu);
            _helpHideMenu.onClick.AddListener(exitButton);
        }

        private void OnDisable()
        {
            _helpButton.onClick.RemoveListener(ToggleShowMenu);
            _helpHideMenu.onClick.RemoveListener(exitButton);
        }

        private IEnumerator Start()
        {
            ServiceManager.TryGetService(out InputController inputController);
            _inputController = inputController;

            _xHidedPosition = _menu.transform.localPosition.x;

            _helpHideMenu.gameObject.SetActive(_opened);

            yield return null;
        }

        private void exitButton()
        {
            if (!_opened)
                return;
            ToggleShowMenu();
        }

        private void ToggleShowMenu()
        {
            if (_inProcess) return;

            _inProcess = true;
            _opened = !_opened;

            _helpHideMenu.gameObject.SetActive(_opened);

            _animationSequence?.Kill();
            _animationSequence = DOTween.Sequence();

            _animationSequence.Insert(0,
                _menu
                    .DOAnchorPosX(_opened ? _xOpenedPosition : _xHidedPosition, _animationDuration)
                    .SetEase(_animationEase));

            _animationSequence.OnKill(() => _inProcess = false);
        }
    }
}
