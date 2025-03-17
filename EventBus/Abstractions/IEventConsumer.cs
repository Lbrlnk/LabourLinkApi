using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Abstractions
{
    public interface IEventConsumer
    {
        void Subscribe<TEvent>(string queueName, Action<TEvent> handler) where TEvent : class;
    }
}
