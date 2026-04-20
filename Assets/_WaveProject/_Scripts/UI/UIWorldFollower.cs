using UnityEngine;

namespace WaveProject.UI
{
    public class UIWorldFollower : MonoBehaviour
    {
        [SerializeField] private Transform _followPoint;
        [SerializeField] private Vector3 _offset;
        
        private Camera _camera;

        public void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            var worldPos = _followPoint.position + _offset;
            var screenPoint = _camera.WorldToScreenPoint(worldPos);
            
            transform.position = new Vector2(screenPoint.x, screenPoint.y);
        }
    }
}