using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Receiver
{
    public class Settings
    {
        public static string ConnectionString = "Endpoint=sb://servicebusexample.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=/ovqS+wqlZysn+/8FSxnQCii6WRLUl7/2Qq2ZZDo5yI=";
        public static string QueueName = "Errors";
    }
}
