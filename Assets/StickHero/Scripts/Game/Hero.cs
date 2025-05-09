using UnityEngine;

namespace StickHero.Scripts.Game
{
    public class Hero : MonoBehaviour
    {
        private Vector3 positionHero;
        
        public void SavePosition()
        {
            positionHero = transform.position;
        }
        
        public void ResetPosition()
        {
            transform.position =  positionHero;
        }
    }
}