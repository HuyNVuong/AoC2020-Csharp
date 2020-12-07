using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Helper
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrAddExisting<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (key == null)
                return default;
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, value);

            return dictionary[key];
        }

        public static void AddOrUpdateExisting<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] = value;
            else
                dictionary.Add(key, value);
        }

        public static Dictionary<TKey, TValue> ShallowClone<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return dictionary.ToDictionary(entry => entry.Key,
                entry => entry.Value);
        }

        public static Dictionary<TKey, TValue> DeepCopy<TKey, TValue>(this Dictionary<TKey, TValue> dictionary) where TValue : ICloneable
        {
            var clone = new Dictionary<TKey, TValue>(dictionary.Count, dictionary.Comparer);
            foreach (var (key, value) in dictionary)
            {
                clone.Add(key, (TValue)value.Clone());
            }
            return clone;
        }

        public static void PutIfAbsent<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, value);
        }
    }
}
