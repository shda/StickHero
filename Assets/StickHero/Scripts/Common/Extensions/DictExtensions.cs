using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CoffeeShopGame.Scripts.Common.Extensions
{
    public static class DictExtensions
    {
        public static TValue GetValueOrDefault<TKey ,TValue>(this IDictionary<TKey ,TValue> sources , TKey key)
        {
            if (sources == null)
                return default;
            
            sources.TryGetValue(key, out var value);
            return value;
        }
        
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dict , IEnumerable<TValue> values , Func<TValue , TKey> OnAdd)
        {
            if (values != null && dict != null)
            {
                foreach (var value in values)
                {
                    var key = OnAdd(value);
                    if (!dict.ContainsKey(key))
                    {
                        dict.Add(key , value);
                    }
                }
            }
        }
        
        public static void DebugLogPrintValues<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var pair in dict)
            {
                sb.AppendLine($"Key:\"{pair.Key}\" Value: \"{pair.Value}\"");
            }
            
            //Debug.Log($"{sb}");
        }
    }
}