using SQLite;
using System;

namespace Siemens.Audiology.BirthdayWisher.Data.Models
{
    public class BirthdayInformation
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
        [Unique]
        public string Email { get; set; }
        public string GId { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public string BackgroundImage { get; set; }
        public bool IsActive { get; set; }
    }
}
