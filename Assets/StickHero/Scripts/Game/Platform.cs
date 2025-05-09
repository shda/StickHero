using System;
using UnityEngine;

namespace StickHero.Scripts.Game
{
    public class Platform : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D boxCollider;

        public float GetSize()
        {
            var bounds = boxCollider.bounds;
            return bounds.size.x;
        }
        
        public Vector3 GetTopLeftPosition()
        {
            var bounds = boxCollider.bounds;
            return new Vector2(bounds.min.x, bounds.max.y);;
        }

        public Vector3 GetTopRightPosition()
        {
            var bounds = boxCollider.bounds;
            return new Vector2(bounds.max.x , bounds.max.y);
        }

        private void Update()
        {
            Debug.Log(GetTopRightPosition());
        }
    }
}
