using System;

namespace Genso.Astrology.Library
{
    /// <summary>
    /// Holds cache key & and its value when saved to file on disk
    /// Note: Use class over struct for performance
    /// </summary>
    [Serializable()]
    public class CacheHolder
    {
        public readonly CacheKey Key;
        public readonly object Value;

        public CacheHolder(CacheKey key, object value)
        {
            Key = key;
            Value = value;
        }

    }
}