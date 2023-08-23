using EventsAPI.Core.Entities;
using EventsAPI.Core.Interfaces.Repositories;
using EventsAPI.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;

namespace EventsAPI.Infrastructure.Services.EventService
{
    /// <summary>
    /// This class acts as a glue between EventRepository and the api.
    /// </summary>
    public class EventService : IEventService
    {
        private readonly IEventRepository _repository;
        private bool _eventExists = false;

        public EventService(IEventRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Event> GetAllEvents()
        {
            return _repository.GetAllEvents().ToList();
        }

        public Event GetEventByName(string eventName)
        {
            var evt = _repository.GetEventByName(eventName);
            if (evt == null)
            {
                throw new InvalidOperationException();
            }
            return evt;
        }

        public void CreateEvent(Event evt)
        {
            if (!ValidateEvent(evt))
            {
                _repository.AddEvent(evt);
            }
            else
            {
                throw new Exception();
            }
        }

        public void EditEvent(Event evt)
        {
            if (ValidateEvent(evt))
            {
                _repository.EditEvent(evt);
            }
            else { throw new Exception(); }
        }

        public void RemoveEvent(Guid id)
        {
            var item = Find(id);

            if (item)
            {
                _repository.RemoveEvent(id);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public bool ValidateEvent(Event eventToValidate)
        {
            var isValidEvent= _repository.GetEventByName(eventToValidate.Title);
            if (isValidEvent != null)
            {
                return _eventExists = true;
            }
            else
            {
                return _eventExists = false;
            }
        }

        //TODO:Remove this method
        public bool DoesTheEventExists(string eventTitle)
        {
            var isValidEvent = _repository.GetEventByName(eventTitle);

            if (isValidEvent != null)
            {
                return _eventExists = true;
            }

            return _eventExists;
        }

        public bool Find(Guid id)
        {
            try
            {
                var item = _repository.Find(id);
                if (item == null)
                {
                    return _eventExists = false;
                }

                return _eventExists = true;
            }
            catch (Exception)
            {
                return _eventExists = false;
            }
        }
    }
}