using System;
using System.ComponentModel.DataAnnotations;

namespace EventsAPI.Core.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        public DateTime? AddedOn { get; set; }
    }
}