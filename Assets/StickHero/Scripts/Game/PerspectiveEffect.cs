using UnityEngine;

namespace StickHero.Scripts.Game
{
    public class PerspectiveEffect : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float offsetMoving;
        
        private Vector3? lastPosition;

        private void Update()
        {
            if (lastPosition == null)
            {
                lastPosition =  cameraTransform.position;
                return;
            }
            
            transform.localPosition += (lastPosition.Value - cameraTransform.position) * offsetMoving;
            lastPosition =  cameraTransform.position;
        }
    }
}