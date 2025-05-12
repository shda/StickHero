using UnityEngine;
using UnityEngine.SceneManagement;

namespace StickHero.Scripts.Game
{
    public class SwitchLevel : MonoBehaviour
    {
        public void Switch(int levelIndex)
        {
            GlobalSettings.LevelIndex = levelIndex;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}