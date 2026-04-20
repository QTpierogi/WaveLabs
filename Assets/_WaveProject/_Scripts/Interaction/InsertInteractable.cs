using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WaveProject.Interaction
{
    public class InsertInteractable : MoveBetweenPointsInteractable
    {
        public bool _removing;
        [field: SerializeField] public UnityEvent Clicked { get; protected set; }
        [SerializeField] public Transform _basePoint;

        public Transform _correctPoint;

        private Color _currentColor;
        public bool _buttonMode = true;


        public override void CustomUpdate(Vector2 delta)
        {
            if(_buttonMode)
            {
                Clicked?.Invoke();
                Debug.Log("I'm button");
                _buttonMode = false;
            }
    
            if (Input.GetMouseButton(0) == false) return;

            SetColor();

            UpdateDeltaDistance(delta);

            var time = TotalDeltaDistance * Sensitivity;
            SetPosition(time);


            if (IsCorrectPlace())
            {
                FinishChanging();
                _buttonMode = true;
                SetColor();
            }
        }

        public void ResetToDefault()
        {
            TotalDeltaDistance = 0;
        }

        private void SetColor()
        {
            var color = IsCorrectPlace() ? Color.green : Color.red;
            if (_buttonMode) color = Color.cyan;
            if (color == _currentColor)
                return;

            _currentColor = color;

            Outline.OutlineColor = _currentColor;
        }

        private bool IsCorrectPlace() => Mathf.Abs(_correctPoint.localPosition.z - transform.localPosition.z) <= 0.0001f;

        public void MoveUp(float moveDuration, TweenCallback callback)
        {
            transform
                .DOMove(_rightPoint.position, moveDuration)
                .SetEase(Ease.InOutExpo)
                .OnComplete(callback);
        }

        public void MoveDown(float moveDuration)
        {
            transform
                .DOMove(_basePoint.position, moveDuration)
                .SetEase(Ease.InOutExpo);
        }
    }
}

