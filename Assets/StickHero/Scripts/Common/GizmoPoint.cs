using UnityEngine;

namespace StickHero.Scripts.Common
{
    public class GizmoPoint : MonoBehaviour
    {
        public float sizePoint = 1.0f;
        public Color color = Color.gray;
        void OnDrawGizmos()
        {
            if (enabled)
            {
                var oldColor = Gizmos.color;
                Gizmos.color = color;
                Gizmos.DrawSphere(transform.position, sizePoint);
                Gizmos.color = oldColor;
            }
        }
    }
}
