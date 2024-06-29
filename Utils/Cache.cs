namespace M16API.Utils
{
    public class Cache<TKey, TValue>
    {
        private readonly int maxSize;
        private readonly SortedDictionary<TKey, TValue> cache;
        
        public Cache(int maxSize)
        {
            this.maxSize = maxSize;
            this.cache = new SortedDictionary<TKey, TValue>();
        }

        public void Add(TKey key, TValue value)
        {
            cache[key] = value;
            if (cache.Count > maxSize)
            {
                cache.Remove(cache.Keys.First());
            }
        }

        public bool ContainsKey(TKey key)
        {
            return cache.ContainsKey(key);
        }

        public TValue GetValueOrDefault(TKey key, TValue value)
        {
            return cache.GetValueOrDefault(key, value);
        }

        public IEnumerable<TValue> GetAllValues()
        {
            return cache.Values;
        }
    }
}
