using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMemoryCache.MemoryCache
{
    public class MemoryCacheItem<TKey, TValue>
    {
        private TKey _key;
        private TValue _value;
        private MemoryCacheItem<TKey, TValue> _prev;
        private MemoryCacheItem<TKey, TValue> _next;

        public MemoryCacheItem(TKey key, TValue value)
        {
            _key = key;
            _value = value;
        }


        public TKey Key
        {
            get
            {
                return _key;
            }
        }

        public TValue Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }
        public MemoryCacheItem<TKey, TValue> PreviousNode
        {
            get
            {
                return _prev;
            }

            set
            {
                _prev = value;
            }
        }

        public MemoryCacheItem<TKey, TValue> NextNode
        {
            get
            {
                return _next;
            }

            set
            {
                _next = value;
            }
        }
    }
}
