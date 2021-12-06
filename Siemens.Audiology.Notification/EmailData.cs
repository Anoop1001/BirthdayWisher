using System.Collections.Generic;

namespace Siemens.Audiology.Notification
{
    public class EmailData
    {
        public string From { get; set; }
        public IEnumerable<string> To { get; set; }
        public IEnumerable<string> Cc { get; set; }
        public IEnumerable<string> Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
        public IEnumerable<Attachment> Attachments {get;set;}
    }
}
