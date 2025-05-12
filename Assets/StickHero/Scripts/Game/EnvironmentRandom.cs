using System;
using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StickHero.Scripts.Game
{
    public class EnvironmentRandom : MonoBehaviour
    {
        [SerializeField] private int maxCount = 100;
        [SerializeField] private Vector2 stepMaxMinX = new(1,0.5f);
        [SerializeField] private Vector2 stepMaxMinY = new(1,0.5f);
        
        [SerializeField] private Sprite[] sprites;
        
        [Button]
        public void CreateItems()
        {
            RemoveOldItems();

            float lastX = 0;
            
            var randomSprites = new List<Sprite>(sprites);
            ShuffleList(randomSprites);
            
            for (int i = 0; i < maxCount; i++)
            {
                var x = lastX + Random.Range(stepMaxMinX.x, stepMaxMinX.y);
                var y = Random.Range(stepMaxMinY.x, stepMaxMinY.y);
                
                var newGameObject = new GameObject($"{i}");
                newGameObject.transform.SetParent(transform);  
                newGameObject.transform.localPosition = new Vector3(x, y, 0);

                var sr = newGameObject.AddComponent<SpriteRenderer>();
                sr.sprite = randomSprites[i % sprites.Length];

                if (i % sprites.Length == 0)
                {
                    ShuffleList(randomSprites);
                }
                
                lastX = x;
            }
        }
        
        private void ShuffleList<T>(List<T> list)
        {
            for (var i = 0; i < list.Count - 1; i++)
            {
                var randomIndex = Random.Range(i, list.Count);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }

        private void RemoveOldItems()
        {
            var allChildrens = transform.Cast<Transform>().ToArray();
            foreach (var children in allChildrens)
            {
                DestroyImmediate(children.gameObject);
            }
        }
    }
}