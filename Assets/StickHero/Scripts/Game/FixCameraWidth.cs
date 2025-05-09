using System;
using UnityEngine;

namespace StickHero.Scripts.Game
{
    [ExecuteAlways]
    public class FixCameraWidth : MonoBehaviour
    {
        [SerializeField] private float desiredWidth = 32f;
        [SerializeField] private float offestY = 1.0f;
        [SerializeField] private Camera camera;
        [SerializeField] private Vector3 startPosition;

        private int _lastScreenWidth;
        private int _lastScreenHeight;
        private float _lastDesiredWidth;
        
        private void Update()
        {
            if(camera == null)
                return;
            
            if(_lastScreenWidth == Screen.width && 
               _lastScreenHeight == Screen.height && 
               Mathf.Approximately(_lastDesiredWidth, desiredWidth))
                return;

            _lastDesiredWidth = desiredWidth;
            _lastScreenWidth = Screen.width;
            _lastScreenHeight = Screen.height;
            
            var desiredHorizontalSize = desiredWidth / 2f;
            var aspectRatio = (float)Screen.width / Screen.height;
            camera.orthographicSize = desiredHorizontalSize / aspectRatio;
        }
    }
}