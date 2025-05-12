using UnityEngine;

namespace StickHero.Scripts.Game
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private EnvironmentRandom environmentRandom;
        [SerializeField] private PlatformsGenerator platformsGenerator;
        public PlatformsGenerator PlatformsGenerator => platformsGenerator;
        
        public void InitLevel()
        {
            environmentRandom?.CreateItems();
            PlatformsGenerator?.CreatePlatforms();
        }
    }
}