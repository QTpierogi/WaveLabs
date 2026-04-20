using UnityEngine;
using WaveProject.UserInput;

namespace WaveProject.Interaction
{
    class InfiniteRotateInteractable : DirectionInteractable
    {
        public virtual void SetDefaultRotation(float rotationInDegree)
        {
            var exitRotation = rotationInDegree * ExitAxis;
            Transform.rotation = Quaternion.Euler(exitRotation);

            TotalDeltaDistance = Vector3.Scale(Transform.rotation.eulerAngles, ExitAxis).magnitude;
        }

        protected override void UpdateDeltaDistance(Vector2 delta)
        {
            TotalDeltaDistance += InputAxis == InputAxis.Horizontal ? delta.x : delta.y;
        }

        public float GetRotation()
        {
            var rotationEulerAngles = transform.rotation.eulerAngles;
            rotationEulerAngles.Scale(ExitAxis);
            return rotationEulerAngles.magnitude;
        }

        public override void CustomUpdate(Vector2 delta)
        {
            UpdateDeltaDistance(delta);

            Transform.localRotation = Quaternion.Euler(ExitAxis * TotalDeltaDistance);

            if (Input.GetMouseButtonUp(0))
            {
                FinishChanging();
            }
        }
    }
}