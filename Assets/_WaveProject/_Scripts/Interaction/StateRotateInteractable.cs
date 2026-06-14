using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveProject.Utility;

namespace WaveProject.Interaction
{
    public class StateRotateInteractable : DirectionInteractable
    {
        private float _minValue;
        private float _maxValue;
        public List<float> states;
        public int currentState;
        private float stateAngle;

        [field: SerializeField] public float AngleRange { get; protected set; } = 90;

        public virtual void SetDefaultValue(float currentValue)
        {
            stateAngle = AngleRange / states.Count;
            _minValue = 0;
            _maxValue = states.Count - 1;

            //Transform.localRotation = Utils.GetRotationInRange(currentValue, _minValue, _maxValue,
            //        -AngleRange, AngleRange, ExitAxis);

            Transform.localRotation = Utils.GetRotationForState(currentValue, stateAngle, ExitAxis);
            currentState = (int)currentValue;

            var percentage = Utils.Remap(currentValue, _minValue, _maxValue, 0, 1);


            TotalDeltaDistance = percentage / Sensitivity;
        }

        public float GetValue()
        {
            //currentState = Mathf.RoundToInt(Utils.GetValueByRotationInRange(Transform.rotation, -AngleRange,
            //    AngleRange, _minValue, _maxValue, ExitAxis));
            return states[currentState];
        }

        public override void CustomUpdate(Vector2 delta)
        {
            UpdateDeltaDistance(delta);

            Transform.localRotation = Utils.GetRotationInRange(TotalDeltaDistance * Sensitivity, 0, 1,
                -AngleRange, AngleRange, ExitAxis);
            currentState = Mathf.RoundToInt(Mathf.Clamp(Utils.GetValueByRotationInRange(Transform.rotation, -AngleRange,
                AngleRange, _minValue, _maxValue, ExitAxis), _minValue, _maxValue));

            if (Input.GetMouseButtonUp(0))
            {
                Transform.localRotation = Utils.GetRotationForState(currentState, stateAngle, ExitAxis);
                FinishChanging();
            }
        }
    }
}
