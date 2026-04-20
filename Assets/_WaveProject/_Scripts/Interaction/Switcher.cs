using UnityEngine;

namespace WaveProject.Interaction
{
    internal class Switcher : Toggle
    {
        private Vector3 _defaultScale;

        public override void SetDefaultToggledState(bool value)
        {
            base.SetDefaultToggledState(value);
            _defaultScale = transform.localScale;
        }
        
        public override void CustomUpdate(Vector2 delta)
        {
            TurnOn = !TurnOn;

            var scaleFactor = new Vector3(1, TurnOn ? -1 : 1, 1);
            var newScale = _defaultScale;
            newScale.Scale(scaleFactor);
            
            transform.localScale = newScale;
            
            Toggled?.Invoke(TurnOn);
            FinishChanging();
        }
    }
}