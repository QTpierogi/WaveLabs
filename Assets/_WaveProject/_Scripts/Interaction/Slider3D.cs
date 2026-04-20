using UnityEngine;
using UnityEngine.Events;

namespace WaveProject.Interaction
{
    public class Slider3D : MoveBetweenPointsInteractable 
    {
        [field: SerializeField] public UnityEvent<float> OnValueChanged { get; protected set; } = new();

        public override void Init()
        {
            base.Init();
            SetPosition(0);
        }

        protected override void SetPosition(float time)
        {
            var start = _leftPoint.position;
            var end = _rightPoint.position;
            
            transform.position = Vector3.Lerp(start, end, time);
            OnValueChanged?.Invoke(time);
        }

        public void Show() => transform.parent.gameObject.SetActive(true);

        public void Hide() => transform.parent.gameObject.SetActive(false);
    }
}