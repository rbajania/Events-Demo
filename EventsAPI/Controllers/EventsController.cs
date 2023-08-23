using Microsoft.AspNetCore.Mvc;
using EventsAPI.Core.Entities;
using EventsAPI.Core.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;
using EventsAPI.WebUi.Validation;
using System;

namespace EventsAPI.WebUi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // GET api/events
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Event))]
        public ActionResult<IEnumerable<Event>> GetAllEvents()
        {
            var eventsList = _eventService.GetAllEvents().ToList();
            return Ok(eventsList);
        }

        // POST: api/events/addevent
        [HttpPost]
        [ValidateModel]
        [Route("CreateEvent")]
        public IActionResult AddEvent([FromBody] Event evt)
        {
            var type = new Event()
            {
                Id = Guid.NewGuid(),
                Title = evt.Title,
                StartDate = evt.StartDate,
                EndDate = evt.EndDate,
                Type = evt.Type,
                Timezone = evt.Timezone,
                Description = evt.Description,
                AddedOn = DateTime.Now,
                LastModifiedOn = DateTime.Now
            };
            var eventAlreadyExists = _eventService.GetEventByName(evt.Title);
            if (eventAlreadyExists != null)
            {
                return BadRequest("Event already exists");
            }

            _eventService.CreateEvent(evt);

            return Ok(evt);
        }

        // PUT: api/events/editevent
        [HttpPut]
        [ValidateModel]
        [Route("EditEvent")]
        public IActionResult EditEvent([FromBody] Event evt)
        {
            var dbEvent = _eventService.GetEventByName(evt.Title);
            if (dbEvent == null)
            {
                return NotFound();
            }

            dbEvent.Title = evt.Title;
            dbEvent.StartDate = evt.StartDate;
            dbEvent.EndDate = evt.EndDate;
            dbEvent.Type = evt.Type;
            dbEvent.Timezone = evt.Timezone;
            dbEvent.Description = evt.Description;
            dbEvent.LastModifiedOn = DateTime.Now;

            _eventService.EditEvent(dbEvent);

            return Ok(dbEvent);
        }

        // DELETE: api/events/deleteevent/1
        [HttpDelete]
        [Route("deleteevent/{title}")]
        public IActionResult Delete(string title)
        {
            var eventInDb = _eventService.GetEventByName(title);

            if (eventInDb == null)
            {
                return NotFound();
            }
            _eventService.RemoveEvent(eventInDb.Id);

            return NoContent();
        }
    }
}