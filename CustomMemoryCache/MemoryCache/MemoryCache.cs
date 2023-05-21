using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomMemoryCache.NotificationServices;

namespace CustomMemoryCache.MemoryCache
{
    public class MemoryCache<TKey, TValue> : IMemoryCache<TKey, TValue>
    {
        private MemoryCacheItem<TKey, TValue>? _first;
        private MemoryCacheItem<TKey, TValue>? _last;
        private Dictionary<TKey, MemoryCacheItem<TKey, TValue>> _cacheMapping 
            = new Dictionary<TKey, MemoryCacheItem<TKey, TValue>>();
        private int _cacheThreshold = 5;
        static MemoryCache<TKey, TValue>? _instance;
        private static readonly object _instancelock = new object();
        INotificationService _notificationService;

        public static MemoryCache<TKey, TValue> GetMemoryCacheInstance(int cacheThreshold, INotificationService notificationService)
        {
            //This is Thread Safe
            lock (_instancelock)
            {
                if (_instance == null)
                {
                    _instance = new MemoryCache<TKey, TValue>(cacheThreshold, notificationService);
                }
            }
            return _instance;
        }

        private MemoryCache()
        {
        }
        private MemoryCache(int cacheThreshold, INotificationService notificationService)
        {
            _cacheMapping.EnsureCapacity(_cacheThreshold);
            _cacheThreshold = cacheThreshold;
            _notificationService = notificationService;
        }

        public void AddItem(TKey key, TValue value)
        {
            MemoryCacheItem<TKey, TValue> node = new MemoryCacheItem<TKey, TValue>(key, value);
            try
            {
                if (_cacheMapping.Count() >= _cacheThreshold)
                {
                    RemoveItem(_first);
                }

                AddNodeToLast(node);

                if (!_cacheMapping.ContainsKey(key))
                    _cacheMapping.Add(key, node);
                else
                    _cacheMapping[key] = node;
            }
            catch (Exception ex) 
            {   
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine($"Item Added to Cache with Key : {key} ");
        }
        public TValue RetriveItem(TKey key)
        {
            MemoryCacheItem<TKey, TValue> node;
            if (_cacheMapping.ContainsKey(key))
            {

                node = _cacheMapping[key];
                MoveItems(node);
                Console.WriteLine($"Item Retrived from Cache with key : {key}");
                return node.Value;

            }
            return default(TValue);
        }

        private void RemoveItem(MemoryCacheItem<TKey, TValue> node)
        {
            if (node == null)
            {
                return;
            }
            if (_last == node)
            {
                _last = node.PreviousNode;
            }
            if (_first == node)
            {
                _first = node.NextNode;
            }
            _cacheMapping.Remove(node.Key);
            if (_notificationService != null)
                _notificationService.Notify($"Key : {node.Key}  Value : {node.Value} deleted");
        }

        private void AddNodeToLast(MemoryCacheItem<TKey, TValue> node)
        {
            if (_last != null)
            {
                _last.NextNode = node;
                node.PreviousNode = _last;
            }
            _last = node;

            if (_first == null)
            {
                _first = node;
            }
        }

        private void MoveItems(MemoryCacheItem<TKey, TValue> node)
        {
            if (node == null && _last == node)
                return;
            if (_first == node)
            {
                _first = node.NextNode;
                node.PreviousNode = null;
            }
            else
            {
                node.PreviousNode.NextNode = node.NextNode;
                node.NextNode.PreviousNode = node.PreviousNode;

            }
            _last.NextNode = node;
            node.PreviousNode = _last;
            node.NextNode = null;
            _last = node;
        }
    }
}
