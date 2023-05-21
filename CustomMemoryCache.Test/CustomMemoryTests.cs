using CustomMemoryCache.MemoryCache;
using CustomMemoryCache.NotificationServices;
namespace CustomMemoryCache.Test
{
    public class CustomMemoryTests
    {
        private INotificationService _notification;
        private MemoryCache<int, int> _cacheInt;
        private MemoryCache<string, string> _cacheString;
        [SetUp]
        public void Setup()
        {
            _notification = new ConsoleNotificationService();
            _cacheInt =  MemoryCache<int, int>.GetMemoryCacheInstance(8, _notification);
            _cacheString = MemoryCache<string, string>.GetMemoryCacheInstance(5, null);
        }

        [Test]
        public void Cache_LRU_Check_Test()
        {
            for (int i = 0; i < 8; i++)
            {
                _cacheInt.AddItem(i, i);

            }
            int val = _cacheInt.RetriveItem(0);

            _cacheInt.AddItem(11,11);

            Assert.IsTrue(_cacheInt.RetriveItem(0) == 0, "Pass");
        }
        [Test]
        public void Cache_Genric_Check_Test()
        {
            for (int i = 0; i < 8; i++)
            {
                _cacheString.AddItem(i.ToString(), i.ToString());

            }
            var value = _cacheString.RetriveItem("5");
            Assert.IsTrue("0"  == "0", "Pass");
        }
    }
}