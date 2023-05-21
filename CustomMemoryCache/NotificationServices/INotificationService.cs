using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMemoryCache.NotificationServices
{
    public interface INotificationService
    {
       public void Notify(string message);
    }
}
