using System;
using UnityEngine;

namespace WaveProject.Station
{
    public class MagnificationZone : MonoBehaviour
    {
        [SerializeField] private Transform _magnificationObject;
        [SerializeField] private Material _material;
        [SerializeField] private Camera _camera;

        [SerializeField] private float _magnificationFactor = .5f;
        
        
        private static readonly int ObjectScreenPosition = Shader.PropertyToID("_ObjectScreenPosition");
        private static readonly int ZoomFactor = Shader.PropertyToID("_ZoomFactor");


        private void OnValidate()
        {
            _camera ??= Camera.main;
            
            if (_magnificationObject == null)
                return;

            var sharedMaterial = _magnificationObject.GetComponent<MeshRenderer>().sharedMaterial;
            _material = sharedMaterial;
            
            sharedMaterial.SetFloat(ZoomFactor, _magnificationFactor);
        }

        private void Start()
        {
            _material.SetFloat(ZoomFactor, _magnificationFactor);
        }

        private void LateUpdate()
        {
            Vector2 screenPixels = _camera.WorldToScreenPoint(_magnificationObject.position);
            screenPixels = new Vector2(screenPixels.x / Screen.width, screenPixels.y / Screen.height);
            
            _material.SetVector(ObjectScreenPosition, screenPixels);
        }
    }
}