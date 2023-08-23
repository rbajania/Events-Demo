using EventsAPI.Core.Entities;

namespace EventsAPI.Core.Entities
{
    public class Event : BaseEntity
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Type { get; set; }
        public string Timezone { get; set; }
        public string Description { get; set; }
    }
}