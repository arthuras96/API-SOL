using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions.Models
{
    public class MailBody
    {
        public string contentType { get; set; }
        public string content { get; set; }
    }

    public class MailEmailAddress
    {
        public string address { get; set; }
    }

    public class Recipient
    {
        public MailEmailAddress emailAddress { get; set; }
    }

    public class Message
    {
        public string subject { get; set; }
        public MailBody body { get; set; }
        public List<Recipient> toRecipients { get; set; }
        public List<Recipient> bccRecipients { get; set; }
        public List<Attachment> attachments { get; set; }

    }

    public class GraphMailSender
    {
        public Message message { get; set; }
        public string saveToSentItems { get; set; }
    }

    public class Attachment
    {
        [JsonProperty("@odata.type")]
        public string OdataType { get; set; }
        public string name { get; set; }
        public string contentType { get; set; }
        public string contentBytes { get; set; }
    }
}
