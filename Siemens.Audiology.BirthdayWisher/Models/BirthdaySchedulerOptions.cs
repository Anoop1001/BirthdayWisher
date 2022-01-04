namespace Siemens.Audiology.BirthdayWisher.Models
{
    public class BirthdaySchedulerOptions
    {
        public string CronExpression { get; set; }
        public string FromEmailAddress { get; set; }
        public string FromEmailCredential { get; set; }
    }
}
