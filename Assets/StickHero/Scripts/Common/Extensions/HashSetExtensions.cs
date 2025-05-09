using System;
using System.Collections.Generic;
using System.Linq;

namespace CoffeeShopGame.Scripts.Common.Extensions
{
    public static class HashSetExtensions
    {
        public static bool IsEqualsOtherHash<T>(this HashSet<T> first , HashSet<T> second)
        {
            if (first == null || second == null)
            {
                return false;
            }
            
            if (ReferenceEquals(second, first))
            {
                return true;
            }

            if (first.Count != second.Count)
            {
                return false;
            }
            
            var intersect = first.Intersect(second);
            return intersect.Count() == first.Count;
        }
        
        public static HashSet<TKey> ToHashSet<TSource, TKey>(this IEnumerable<TSource> sources , Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> hashSet = new HashSet<TKey>();

            foreach (var source in sources)
            {
                hashSet.Add(keySelector.Invoke(source));
            }
            
            return hashSet;
        }
    }
}