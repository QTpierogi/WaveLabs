using UnityEngine;
using WaveProject.Utility;

namespace WaveProject.Interaction
{
    internal class RotateInteractable : DirectionInteractable
    {
        private float _minValue;
        private float _maxValue;
        
        [field: SerializeField] public float AngleRange { get; protected set; } = 90;

        public virtual void SetDefaultValue(float currentValue, float minValue, float maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
            
            Transform.localRotation = Utils.GetRotationInRange(currentValue, minValue, maxValue,
                    -AngleRange, AngleRange, ExitAxis);

            var value = GetValue();
            var percentage = Utils.Remap(value, _minValue, _maxValue, 0, 1);
            

            TotalDeltaDistance = percentage / Sensitivity;
        }

        public float GetValue()
        {
            return Utils.GetValueByRotationInRange(Transform.rotation, -AngleRange,
                AngleRange, _minValue, _maxValue, ExitAxis);
        }

        public override void CustomUpdate(Vector2 delta)
        {
            UpdateDeltaDistance(delta);
            
            Transform.localRotation = Utils.GetRotationInRange(TotalDeltaDistance * Sensitivity, 0, 1,
                -AngleRange, AngleRange, ExitAxis);

            if (Input.GetMouseButtonUp(0))
            {
                FinishChanging();
            }
        }
    }
}