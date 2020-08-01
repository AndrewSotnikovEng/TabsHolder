using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabsHolder
{
    public static class MessengerStatic
    {
        public static event Action<object> Bus;

        public static void Send(object data)
            => Bus?.Invoke(data);


        public static event Action<object> CloseAddTabWindow;

        public static void NotifyAttTabClosing(object data)
        {
            CloseAddTabWindow?.Invoke(data);
        }





    }
}

