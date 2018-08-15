using System.Collections.Generic;

namespace GM.PPAP.ServiceSDK.Rest.Api.V1
{
    public class CreateSmsOptions
    {
        public Types.PhoneNumber To { get; }
        public string Msg { get; set; }
        public string ApplicationSid { get; set; }

        public CreateSmsOptions(Types.PhoneNumber to, string msg)
        {
            To = to;
            Msg = msg;
        }

        public List<KeyValuePair<string, string>> GetParams()
        {
            var p = new List<KeyValuePair<string, string>>();
            if (To != null)
            {
                p.Add(new KeyValuePair<string, string>("To", To.ToString()));
            }

            if (Msg != null)
            {
                p.Add(new KeyValuePair<string, string>("Msg", Msg));
            }

            return p;
        }

    }
}