using UnityEngine;
using UnityEngine.Events;

namespace WaveProject.Interaction
{
    internal class Toggle : Interactable
    {
        [SerializeField] private UnityEvent<bool> _toggled;
        
        public override bool OneClickInteracting => true;

        public UnityEvent<bool> Toggled => _toggled;

        protected bool TurnOn;

        public virtual void SetDefaultToggledState(bool value)
        {
            TurnOn = value;
        }

        public override void CustomUpdate(Vector2 delta)
        {
            TurnOn = !TurnOn;
            
            Toggled?.Invoke(TurnOn);
            FinishChanging();
        }
    }
}