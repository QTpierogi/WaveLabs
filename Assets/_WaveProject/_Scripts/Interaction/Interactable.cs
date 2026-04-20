using System;
using UnityEngine;
using WaveProject.Configs;
using WaveProject.UserInput;

namespace WaveProject.Interaction
{
    [RequireComponent(typeof(Outline), typeof(Collider))]
    public abstract class Interactable : MonoBehaviour, IInputSubscriber, ISelectable
    {
        [field: SerializeField] public Outline Outline { get; private set; }
        
        protected float Sensitivity;
        protected float TotalDeltaDistance;
        protected float MaxTotalDeltaDistance => 1 / Sensitivity;
        
        public event Action ChangingFinished;
        
        protected void OnValidate()
        {
            Outline ??= GetComponent<Outline>();
            
            var newLayer = LayerMask.NameToLayer(IInputSubscriber.LAYER);
            if (gameObject.layer != newLayer)
            {
                gameObject.layer = newLayer;
            }
        }

        public virtual void Init()
        {
            LoadData();
            Deselect();
        }

        private void LoadData() => Sensitivity = InteractionSettings.Data.Sensitivity;

        public Transform Transform => transform;
        public virtual bool OneClickInteracting => false;

        public virtual void Enable() { }

        public virtual void Disable() { }

        public abstract void CustomUpdate(Vector2 delta);

        public virtual void Select()
        {
            //Outline.enabled = true;
            var baseColor = Outline.OutlineColor;
            Outline.OutlineColor = new Color(baseColor.r, Outline.OutlineColor.g, Outline.OutlineColor.b, 1.0f);
        }

        public virtual void Deselect()
        {
            //Outline.enabled = false;

            var baseColor = Outline.OutlineColor;
            Outline.OutlineColor = new Color(baseColor.r, Outline.OutlineColor.g, Outline.OutlineColor.b, 0.35f);
        }
        
        protected virtual void UpdateDeltaDistance(Vector2 delta)
        {
            TotalDeltaDistance += delta.x + delta.y;
            TotalDeltaDistance = Mathf.Clamp(TotalDeltaDistance, 0, MaxTotalDeltaDistance);
        }

        protected void FinishChanging()
        {
            ChangingFinished?.Invoke();
        }
    }
}