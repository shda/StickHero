using UnityEngine;

namespace StickHero.Scripts.Game
{
    [ExecuteAlways]
    public class FixCameraWidth : MonoBehaviour
    {
        [SerializeField] private float desired = 32f;
        [SerializeField] private Camera camera;
        
        private int _lastScreenWidth;
        private int _lastScreenHeight;
        private float _lastDesiredWidth;
        
        private void Update()
        {
            if(camera == null)
                return;
            
            if(_lastScreenWidth == Screen.width && 
               _lastScreenHeight == Screen.height && 
               Mathf.Approximately(_lastDesiredWidth, desired))
                return;

            _lastDesiredWidth = desired;
            _lastScreenWidth = Screen.width;
            _lastScreenHeight = Screen.height;
            
            var desiredHorizontalSize = desired / 2f;
            var aspectRatio = (float)Screen.width / Screen.height;
            camera.orthographicSize = desiredHorizontalSize / aspectRatio;
        }
    }
}