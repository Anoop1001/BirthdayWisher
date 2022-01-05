using System;

namespace Siemens.Audiology.BirthdayWisher.Data.Models
{
    public class BirthdayInformation
    {
        public string Name { get; set; }
        public string GId { get; set; }
        public DateTimeOffset BirthDate { get; set; }
    }
}
