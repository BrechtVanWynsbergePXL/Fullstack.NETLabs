using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Events
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }
}
