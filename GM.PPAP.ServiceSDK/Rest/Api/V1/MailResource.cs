using System.Collections.Generic;
using System.Reflection;
using GM.PPAP.ServiceSDK.Base;
using GM.PPAP.ServiceSDK.Clients;
using GM.PPAP.ServiceSDK.Exceptions;
using GM.PPAP.ServiceSDK.Http;
using GM.PPAP.ServiceSDK.Types;
using Newtonsoft.Json;

namespace GM.PPAP.ServiceSDK.Rest.Api.V1
{
    public class MailResource: Resource
    {
        private static Request BuildCreateRequest(CreateMailOptions options, IGmServiceRestClient client)
        {
            return new Request(
                HttpMethod.Post,
                Rest.Domain.Api,
                "/v1/mail",
                postParams: options.GetParams()
            );
        }

        public static MailResource Create(CreateMailOptions options, IGmServiceRestClient client = null)
        {
            client = client ?? GmService.GetServiceRestClient();
            var response = client.Request(BuildCreateRequest(options, client));
            return FromJson(response.Content);
        }

        public static MailResource Create(IEnumerable<MailAddress> to,
                                          string subject,
                                          string body,
                                          bool isHtml = false,
                                          IEnumerable<MailAddress> cc = null,
                                          IEnumerable<MailAddress> bcc = null,
                                          IGmServiceRestClient client = null)
        {
            var options = new CreateMailOptions(to, subject, body, isHtml, cc, bcc);
            return Create(options, client);
        }

        public static MailResource Create(MailAddress to,
            string subject,
            string body,
            bool isHtml = false,
            IGmServiceRestClient client = null)
        {
            var options = new CreateMailOptions(new []{to}, subject, body, isHtml);
            return Create(options, client);
        }

        public static MailResource FromJson(string json)
        {
            // Convert all checked exceptions to Runtime
            try
            {
                return JsonConvert.DeserializeObject<MailResource>(json);
            }
            catch (JsonException e)
            {
                throw new ApiException(e.Message, e);
            }
        }

        /// <summary>
        /// Email trace code
        /// </summary>
        [JsonProperty("code")]
        public int? Code { get; set; }
        /// <summary>
        /// The error code associated with the message
        /// </summary>
        [JsonProperty("error_code")]
        public int? ErrorCode { get; set; }
        /// <summary>
        /// Human readable description of the ErrorCode
        /// </summary>
        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }
    }
}
