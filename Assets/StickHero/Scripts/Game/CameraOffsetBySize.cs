using UnityEngine;

namespace StickHero.Scripts.Game
{
    [ExecuteAlways]
    public class CameraOffsetBySize : MonoBehaviour
    {
        [SerializeField] private float startOffetX = 1.0f;
        [SerializeField] private Vector3 offset;
        [SerializeField] private Camera camera;
        
        private int _lastScreenWidth;
        private int _lastScreenHeight;
        private float _lastOffset;
        
        private void Update()
        {
            if(camera == null)
                return;
            
            if(_lastScreenWidth == Screen.width && 
               _lastScreenHeight == Screen.height)
                return;
            
            _lastScreenWidth = Screen.width;
            _lastScreenHeight = Screen.height;
            
            
            Camera cam = camera;

            float left = cam.transform.position.x - cam.orthographicSize * cam.aspect;
            float bottom = cam.transform.position.y - cam.orthographicSize;

            Vector3 worldBottomLeft = new Vector3(left, bottom, 0);

            transform.localPosition = new Vector3(
                offset.x + left,
                offset.y + bottom,
                transform.localPosition.z);
            Debug.Log("Нижний левый угол (ортография): " + worldBottomLeft);
            
            
           // var aspectRatio = (float)Screen.width / Screen.height;
           // transform.localPosition = new Vector3(startOffetX + offset * aspectRatio, transform.localPosition.y, transform.localPosition.z);
        }
    }
}