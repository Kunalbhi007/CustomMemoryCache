using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMemoryCache.MemoryCache
{
    public interface IMemoryCache<TKey, TValue>
    {
        public void AddItem(TKey key, TValue value);
        public TValue RetriveItem(TKey key);
    }
}
