using UnityEngine;
using WaveProject.UserInput;

namespace WaveProject.Interaction
{
    public abstract class DirectionInteractable : Interactable
    {
        [field: SerializeField] public InputAxis InputAxis { get; private set;}
        [field: SerializeField] public Vector3 ExitAxis { get; private set;} = Vector3.right;
        
        protected override void UpdateDeltaDistance(Vector2 delta)
        {
            TotalDeltaDistance += InputAxis == InputAxis.Horizontal ? delta.x : delta.y;
            TotalDeltaDistance = Mathf.Clamp(TotalDeltaDistance, 0, MaxTotalDeltaDistance);
        }
    }
}