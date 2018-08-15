using System.Collections.Generic;
using System.Linq;
using System.Text;
using GM.PPAP.ServiceSDK.Types;

namespace GM.PPAP.ServiceSDK.Rest.Api.V1
{
    public class CreateMailOptions
    {
        public IEnumerable<MailAddress> To { get; }
        public IEnumerable<MailAddress> Cc { get; set; }
        public IEnumerable<MailAddress> Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }

        public CreateMailOptions(IEnumerable<MailAddress> to,
            string subject,
            string body,
            bool isHtml = false,
            IEnumerable<MailAddress> cc = null,
            IEnumerable<MailAddress> bcc = null)
        {
            To = to;
            Subject = subject;
            Body = body;
            IsHtml = isHtml;
            Cc = cc;
            Bcc = bcc;
        }

        public List<KeyValuePair<string, string>> GetParams()
        {
            var p = new List<KeyValuePair<string, string>>();
            if (To != null)
            {
                p.Add(new KeyValuePair<string, string>("To", string.Join(",", To)));
            }

            if (Cc != null)
            {
                p.Add(new KeyValuePair<string, string>("Cc", string.Join(",", Cc)));
            }

            if (Bcc != null)
            {
                p.Add(new KeyValuePair<string, string>("Bcc", string.Join(",", Bcc)));
            }

            if (Subject != null)
            {
                p.Add(new KeyValuePair<string, string>("Subject", Subject));
            }

            if (Body != null)
            {
                p.Add(new KeyValuePair<string, string>("Body", Body));
            }

            p.Add(new KeyValuePair<string, string>("IsHtml", IsHtml.ToString().ToLower()));

            return p;
        }
    }
}