using Core.MessageBroker;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Core.EventMessages
{
    public abstract class EventRequest
    {
        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
