using Microsoft.EntityFrameworkCore;
using EventsAPI.Core.Entities;
using EventsAPI.Core.Interfaces.Repositories;
using EventsAPI.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventsAPI.Infrastructure.Data.Repositories
{
    public class EventRepository : IEventRepository
    {
        private bool _disposed = false;

        protected AppDbContext Context;

        public EventRepository(AppDbContext appDbContext)
        {
            Context = appDbContext;
        }

        /// <summary>
        /// Adds the Event
        /// </summary>
        /// <param name="evt"></param>
        public void AddEvent(Event evt)
        {
            Context.Events.Add(evt);

            SaveChangesToDb();
        }

        /// <summary>
        /// Edits Event
        /// </summary>
        /// <param name="evt"></param>
        public void EditEvent(Event evt)
        {
            var getEvent = Context.Events.SingleOrDefault(e => e.Id == evt.Id);

            if (getEvent == null)
            {
                throw new InvalidOperationException("Event with the given Id doesn't exist.");
            }

            Context.Entry((evt)).State = EntityState.Modified;

            SaveChangesToDb();
        }

        public void RemoveEvent(Guid id)
        {
            var evt = Context.Events.SingleOrDefault(e => e.Id == id);
            if (evt != null)
            {
                Context.Events.Remove(evt);
                SaveChangesToDb();
            }
        }

        /// <summary>
        /// Gets all the Events from the DB
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Event> GetAllEvents()
        {
            return Context.Events.ToList();
        }

        /// <summary>
        /// Get Event By Id
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public Event GetEventByName(string eventName)
        {
            return Context.Events.SingleOrDefault(e => e.Title == eventName);
        }

        public Event Find(Guid id)
        {
            return Context.Events.Single(x => x.Id == id);
        }

        /// <summary>
        /// Commits all the transactions to the database
        /// </summary>
        public void SaveChangesToDb()
        {
            Context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}