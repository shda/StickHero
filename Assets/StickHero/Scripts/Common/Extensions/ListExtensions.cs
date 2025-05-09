using System;
using System.Collections.Generic;
using System.Linq;

namespace CoffeeShopGame.Scripts.Common.Extensions
{
    public static class ListExtensions
    {
        public static bool EmptyOnNull<T>(this IEnumerable<T> enumerator)
        {
            if (enumerator == null)
                return true;
            return !enumerator.Any();
        }

        public static void Foreach<T>(this IEnumerable<T> enumerable , Action<T> action)
        {
            if (enumerable != null)
            {
                foreach (var val in enumerable)
                {
                    action?.Invoke(val);
                }
            }
        }

        public static T[] Shuffle<T> (this T[] array)
        {
            var rng = new Random();
            var n = array.Length;
            while (n > 1) 
            {
                var k = rng.Next(n--);
                (array[n], array[k]) = (array[k], array[n]);
            }

            return array;
        }

        public static T GetRandomElement<T>(this IList<T> array, T current)
        {
            if (array.Count > 1)
            {
                T randomElement = default;
                do
                {
                    var r = UnityEngine.Random.Range(0, array.Count);
                    randomElement = array[r];
                } while (Equals(randomElement, current));

                current = randomElement;
            }

            return current;
        }
        
        
        public static T GetNextElement<T>(this IList<T> array, T current)
        {
            if (array != null &&
                current != null &&
                array.Count > 0)
            {
                for (int i = 0; i < array.Count; i++)
                {
                    if (Equals(array[i], current))
                    {
                        int nextIndex = 0;
                        nextIndex = i + 1;

                        if (nextIndex >= array.Count)
                        {
                            return array[0];
                        }

                        return array[nextIndex];

                    }
                }
            }

            return current;
        }

        public static T GetPreviousElement<T>(this IList<T> array, T current)
        {
            if (array != null &&
                current != null &&
                array.Count > 0)
            {
                for (int i = 0; i < array.Count; i++)
                {
                    if (Equals(array[i], current))
                    {
                        int nextIndex = 0;
                        nextIndex = i - 1;

                        if (nextIndex < 0)
                        {
                            return array[array.Count - 1];
                        }

                        return array[nextIndex];
                    }
                }
            }

            return current;
        }
    }
}