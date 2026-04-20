using UnityEngine;
using WaveProject.Utility;

namespace WaveProject.Interaction
{
    public class MoveBetweenPointsInteractable : DirectionInteractable
    {
        [SerializeField] public Transform _leftPoint;
        [SerializeField] public Transform _rightPoint;
        [SerializeField] public float precisionModifier = 1.0f;
        private float defaultValue;

        public void SetDefaultPosition(float time)
        {
            SetPosition(time);
        }
        
        public void SetDefaultValue()
        {
            var start = _leftPoint.position;
            var end = _rightPoint.position;
            
            var currentValue = Utils.InverseLerp(start, end, transform.position);
            defaultValue = currentValue;
            
            var valuePercentage = currentValue;

            TotalDeltaDistance = valuePercentage / Sensitivity;
        }

        public override void CustomUpdate(Vector2 delta)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                UpdateDeltaDistance(delta/precisionModifier);
            }
            else
            {
                UpdateDeltaDistance(delta);
            }
            
            var time = TotalDeltaDistance * Sensitivity;
            SetPosition(time);
            
            if (Input.GetMouseButtonUp(0))
            {
                FinishChanging();
                Debug.Log(time);
            }
        }

        protected virtual void SetPosition(float time)
        {
            var start = _leftPoint.position;
            var end = _rightPoint.position;
            
            transform.position = Vector3.Lerp(start, end, time);
        }
    }
}