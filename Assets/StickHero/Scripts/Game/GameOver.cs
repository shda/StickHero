using System;
using UnityEngine;

namespace StickHero.Scripts.Game
{
    public class GameOver : MonoBehaviour
    {
        public Action OnRestart;
        public void OnPressRestart()
        {
            OnRestart?.Invoke();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}