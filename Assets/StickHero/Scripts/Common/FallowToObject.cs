using UnityEngine;

namespace StickHero.Scripts.Common
{
    public class FallowToObject : MonoBehaviour
    {
        [SerializeField] private Transform fallowingTarget;
        [SerializeField] private Transform fallowObject;

        private void Update()
        {
            fallowObject.transform.position = fallowingTarget.position;
        }
    }
}