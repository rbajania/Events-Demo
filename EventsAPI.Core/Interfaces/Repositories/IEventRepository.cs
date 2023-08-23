using EventsAPI.Core.Entities;
using System;
using System.Collections.Generic;

namespace EventsAPI.Core.Interfaces.Repositories
{
    public interface IEventRepository : IDisposable
    {
        void AddEvent(Event evt);

        void EditEvent(Event evt);

        void RemoveEvent(Guid id);

        IEnumerable<Event> GetAllEvents();

        Event GetEventByName(string eventName);

        Event Find(Guid id);

        void SaveChangesToDb();
    }
}