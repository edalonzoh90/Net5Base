using System.Collections.Generic;
using System.IO;

namespace BOT.DATA.Models
{
    public class MailNotificationModel
    {
        //public string from { get; set; }
        public string to { get; set; }
        public string cc { get; set; }
        public string subject { get; set; }
        public string header { get; set; }
        public string body { get; set; }
        public string footer { get; set; }
        //public string description { get; set; }
        public List<MailAttachmentModel> attachments { get; set; }
    }

    public class MailAttachmentModel
    {
        public MemoryStream file { get; set; }
        public string fileName { get; set; }
        public string extension { get; set; }
    }

}
