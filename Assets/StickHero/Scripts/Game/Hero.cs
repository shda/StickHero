using UnityEngine;

namespace StickHero.Scripts.Game
{
    public class Hero : MonoBehaviour
    {
        private readonly int upStickAnimation = Animator.StringToHash("UpStick");
        private readonly int movingAnimation = Animator.StringToHash("Moving");
        private readonly int fallAnimation = Animator.StringToHash("Fall");

        [SerializeField] private Animator animator;

        public void SetIdle()
        {
            animator.SetBool(fallAnimation, false);
            animator.SetBool(movingAnimation, false);
            animator.SetBool(upStickAnimation, false);
        }

        public void SetMoving()
        {
            animator.SetBool(movingAnimation, true);
        }

        public void SetFall()
        {
            animator.SetBool(fallAnimation, true);
        }

        public void SetUpStickAnimation()
        {
            animator.SetBool(upStickAnimation, true);
        }
        
    }
}