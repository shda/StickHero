using System.Collections.Generic;
using UnityEngine;

namespace StickHero.Scripts.Game
{
    public class PlatformsGenerator : MonoBehaviour
    {
        [SerializeField] private int maxCountPlatforms = 20;
        [SerializeField] private float chanceChangeByDistance = 1.0f;
        
        [SerializeField] private Transform rootPlatforms;
        [SerializeField] private Platform[] platformPrefabs;

        private List<Platform> Platforms { get; set; } = new();
        
        public List<Platform> CreatePlatforms()
        {
            RemoveOldPlatforms();
            Platforms.Clear();
            
            float lastPos = 0;
            
            for (int i = 0; i < maxCountPlatforms; i++)
            {
                var index = Random.Range(0 , platformPrefabs.Length);

                if (i == 0)
                    index = 0;
                
                var platformPrefab = platformPrefabs[index];
                
                var newPlatform = Instantiate(platformPrefab, rootPlatforms);
                newPlatform.name = $"{i} - {newPlatform.name}";
                newPlatform.transform.rotation = Quaternion.identity;
                newPlatform.transform.localPosition = new Vector3(lastPos, 0, 0);

                lastPos += platformPrefab.GetSize() + 10.0f;
                
                Platforms.Add(newPlatform);
            }

            return Platforms;
        }

        private void RemoveOldPlatforms()
        {
            var lastPlatforms = rootPlatforms.GetComponentsInChildren<Platform>();
            foreach (var platform in lastPlatforms)
            {
                DestroyImmediate(platform.gameObject);
            }
        }
    }
}