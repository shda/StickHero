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

        private readonly List<Platform> platforms = new();

        public List<Platform> CreatePlatforms()
        {
            ClearPlatforms();
            float lastPos = 0;
            int lastIndex = -1;

            for (int i = 0; i < maxCountPlatforms; i++)
            {
                int index = GetRandomPlatformIndex(lastIndex, i == 0);
                lastIndex = index;

                var newPlatform = InstantiatePlatform(platformPrefabs[index], i, lastPos);
                lastPos += platformPrefabs[index].GetSize() + 10.0f;

                platforms.Add(newPlatform);
            }

            return platforms;
        }

        private void ClearPlatforms()
        {
            foreach (var platform in rootPlatforms.GetComponentsInChildren<Platform>())
            {
                DestroyImmediate(platform.gameObject);
            }

            platforms.Clear();
        }

        private int GetRandomPlatformIndex(int lastIndex, bool isFirstPlatform)
        {
            if (isFirstPlatform) return 0;

            int index;
            do
            {
                index = Random.Range(0, platformPrefabs.Length);
            } while (index == lastIndex);

            return index;
        }

        private Platform InstantiatePlatform(Platform prefab, int index, float position)
        {
            var newPlatform = Instantiate(prefab, rootPlatforms);
            newPlatform.name = $"{index} - {prefab.name}";
            newPlatform.transform.rotation = Quaternion.identity;
            newPlatform.transform.localPosition = new Vector3(position, 0, 0);
            return newPlatform;
        }
    }
}