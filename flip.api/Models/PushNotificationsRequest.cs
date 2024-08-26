using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace flip.api.Models
{
    public class PushNotificationRequest
    {
        public string to { get; set; }
        public PNotification notification { get; set; }
        public DataNotification data { get; set; }

        public class PNotification
        {
            public string title { get; set; }
            public string text { get; set; }
        }

        public class DataNotification
        {
            public long id_notificacion { get; set; }
            public string link { get; set; }
            public int conversationId { get; set; }
        }

        public PushNotificationRequest()
        {
            notification = new PNotification();
            data = new DataNotification();
        }
    }
}
