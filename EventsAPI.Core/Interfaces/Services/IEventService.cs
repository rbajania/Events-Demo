using EventsAPI.Core.Entities;
using System;
using System.Collections.Generic;

namespace EventsAPI.Core.Interfaces.Services
{
    public interface IEventService
    {
        void CreateEvent(Event evt);

        void EditEvent(Event evt);

        void RemoveEvent(Guid id);

        IEnumerable<Event> GetAllEvents();

        Event GetEventByName(string eventName);

        bool ValidateEvent(Event eventToValidate);

        bool DoesTheEventExists(string evt);

        bool Find(Guid id);
    }
}