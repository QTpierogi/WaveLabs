using UnityEngine;
using UnityEngine.Events;

namespace WaveProject.Interaction
{
    internal class InteractableButton : Interactable
    {
        [field: SerializeField] public UnityEvent Clicked { get; protected set; }
        
        public override void CustomUpdate(Vector2 delta)
        {
            Clicked?.Invoke();
            FinishChanging();
        }
    }
}