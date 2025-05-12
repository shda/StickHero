using UnityEngine;

namespace CoffeeShopGame.Scripts.Common.Ui
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