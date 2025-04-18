﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Abstractions
{
    public  interface IEventPublisher
    {
        void Publish<TEvent>(TEvent @event) where TEvent : class;
    }
}
