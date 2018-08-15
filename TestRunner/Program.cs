using System;
using GM.PPAP.ServiceSDK.Rest.Api.V1;
using GM.PPAP.ServiceSDK.Types;

namespace TestRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            GM.PPAP.ServiceSDK.GmService.Init("Test", "Password");
            
            //SendMail();
            SendSms();
        }

        public static void SendMail()
        {
            var address = new[] { new MailAddress("cww@pm.me") };
            var subject = "test mail";
            var body = @"
<h1>test</h1>
<span style='Color:red;'>show color</span>
";

            var mailResponse = MailResource.Create(address, subject, body, true, address, address);
        }

        public static void SendSms()
        {
            var to =new PhoneNumber("+886phonenumber");
            var msg = "test from api";

            var smsResponse = SmsResource.Create(to, msg);
        }
    }
}
