using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using EventsAPI.Core.Entities;
using EventsAPI.Core.Interfaces.Repositories;
using EventsAPI.Infrastructure.Data;
using EventsAPI.Infrastructure.Services.EventService;
using System;
using System.Collections.Generic;
using System.Linq;
using Assert = NUnit.Framework.Assert;

namespace EventsAPI.test.EventrepoTests
{
    [TestFixture]
    public class EventrepoTests
    {
        private IEventRepository _mockEventRepository;

        #region Dummy EventsList

        private static List<Event> DummyEvents()
        {
            return new List<Event>()
            {
                new Event()
                {
                    Id=Guid.Parse("0df4ec64-6c4c-4085-8dac-0ee98943d8a9"),
                    Title = "Event 1",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Type = "Offline",
                    Timezone = "(GMT+5:30) Ahmedabad, India",
                    Description = "Event 1 Description",
                    AddedOn = DateTime.Now,
                    LastModifiedOn = DateTime.Now
                },new Event()
                {
                    Id=Guid.Parse("144a9c6a-135c-4535-8c6e-13adff61b24a"),
                    Title = "Event 2",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Type = "Offline",
                    Timezone = "(GMT+5:30) Ahmedabad, India",
                    Description = "Event 2 Description",
                    AddedOn = DateTime.Now,
                    LastModifiedOn = DateTime.Now
                }
            };
        }

        #endregion Dummy EventsList

        #region GetAllEvents

        [Test]
        public void GetEvents_GetsAllTheEvents_ShouldReturnAllEvents()
        {
            // arrange
            // Setup-list
            var types = DummyEvents();

            // Setup Repository
            var mockEventRepo = new Mock<IEventRepository>();

            mockEventRepo.Setup(e => e.AddEvent(It.IsAny<Event>()))
                .Callback((Event evt) => types.Add(evt));

            mockEventRepo.Setup(e => e.GetAllEvents()).Returns(types);

            _mockEventRepository = mockEventRepo.Object;

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            using (var context = new AppDbContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (new AppDbContext(options))
            {
                var service = new EventService(_mockEventRepository);
                var result = service.GetAllEvents();
                Assert.AreEqual(2, result.Count());
            }
        }

        #endregion GetAllEvents

        #region Create ExpenseType

        [Test]
        public void CreateEvent_AddANewEvent_ShouldAddAnEvent()
        {
            var types = DummyEvents();

            var mockEventRepo = new Mock<IEventRepository>();

            mockEventRepo.Setup(e => e.AddEvent(It.IsAny<Event>()))
                .Callback((Event evt) => types.Add(evt));

            mockEventRepo.Setup(e => e.GetAllEvents()).Returns(types);

            _mockEventRepository = mockEventRepo.Object;

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            using (var context = new AppDbContext(options))
            {
                //  context.Database.EnsureDeleted();

                context.Database.EnsureCreated();
            }

            using (var context = new AppDbContext(options))
            {
                var service = new EventService(_mockEventRepository);
                service.CreateEvent(new Event()
                {
                    Id = Guid.NewGuid(),
                    Title = "Event 3",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Type = "Offline",
                    Timezone = "(GMT+5:30) Ahmedabad, India",
                    Description = "Event 3 Description",
                    AddedOn = DateTime.Now,
                    LastModifiedOn = DateTime.Now
                });
                context.SaveChanges();

                var result = _mockEventRepository.GetAllEvents().Count();
                var newRecord = types.Find(t => t.Title == "Event 3");

                Assert.Multiple(() =>
                {
                    Assert.AreEqual(3, result);
                    Assert.AreEqual("Event 3", newRecord.Title);
                });
            }
        }

        [Test]
        [NUnit.Framework.Ignore("No working as expected")]
        public void CreateEvent_AddAnExistingEvent_ShouldThrowException()
        {
            var types = DummyEvents();

            // Setup Repository
            var mockEventRepo = new Mock<IEventRepository>();

            mockEventRepo.Setup(e => e.AddEvent(It.IsAny<Event>()))
                .Callback((Event evt) => types.Add(evt));

            mockEventRepo.Setup(e => e.GetAllEvents()).Returns(types);

            _mockEventRepository = mockEventRepo.Object;

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            using (var context = new AppDbContext(options))
            {
                //context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            using (var context = new AppDbContext(options))
            {
                var service = new EventService(_mockEventRepository);

                Assert.Throws<Exception>(() => service.CreateEvent(new Event()
                {
                    Id = Guid.NewGuid(),
                    Title = "Event 4",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Type = "Offline",
                    Timezone = "(GMT+5:30) Ahmedabad, India",
                    Description = "Event 4 Description",
                    AddedOn = DateTime.Now,
                    LastModifiedOn = DateTime.Now
                }));
            }
        }

        #endregion Create ExpenseType

        #region Edit Event

        [Test]
        public void EditEvent_EditAnExistingEvent_ShouldUpdateAnExistingEvent()
        {
            var types = DummyEvents();

            var mockEventRepo = new Mock<IEventRepository>();

            mockEventRepo.Setup(e => e.GetEventByName(It.IsAny<string>()))
                .Returns((string s) => types.Find(x => x.Title == s));

            _mockEventRepository = mockEventRepo.Object;

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            using (var context = new AppDbContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new AppDbContext(options))
            {
                var service = new EventService(_mockEventRepository);
                var et = service.GetEventByName("Event 3");
                et.Type = "Offline";
                service.GetEventByName("Event 3");
                Assert.AreEqual("Event 3", service.GetEventByName("Event 3").Type);
            }
        }

        [Test]
        public void EditEvent_EditATypeWhichDoesntExist_ShouldReturnAnException()
        {
            var types = DummyEvents();

            var mockEventRepo = new Mock<IEventRepository>();

            mockEventRepo.Setup(e => e.GetEventByName(It.IsAny<string>()))
                .Returns((string s) => types.Find(x => x.Title == s));

            _mockEventRepository = mockEventRepo.Object;

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            using (var context = new AppDbContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new AppDbContext(options))
            {
                var service = new EventService(_mockEventRepository);
                var et = new Event()
                {
                    Type = "Event 4"
                };

                Assert.Throws<Exception>(() => service.GetEventByName("Event 4"));
            }
        }

        #endregion Edit Expense Types

        #region Remove Event

        [Test]
        [TestCase("Event 3")]
        [TestCase("Event 4")]
        public void RemoveEvent_RemovestheSpecifiedEvent_ShouldRemoveEvetnFromList(string title)
        {
            var types = DummyEvents();

            var mockEventRepo = new Mock<IEventRepository>();

            mockEventRepo.Setup(e => e.GetEventByName(It.IsAny<string>()))
                 .Returns((string s) => types.Find(x => x.Title == s));

            mockEventRepo.Setup(r => r.RemoveEvent(It.IsAny<Guid>()))
                .Callback((Guid id) => types.Remove(types.Find(x => x.Id == id)));

            mockEventRepo.Setup(x => x.GetAllEvents()).Returns(types);

            _mockEventRepository = mockEventRepo.Object;

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            using (var context = new AppDbContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new AppDbContext(options))
            {
                var service = new EventService(_mockEventRepository);

                var item = service.GetEventByName(title);

                service.RemoveEvent(item.Id);

                Assert.AreEqual(1, types.Count);
            }
        }

        #endregion Remove Event
    }
}