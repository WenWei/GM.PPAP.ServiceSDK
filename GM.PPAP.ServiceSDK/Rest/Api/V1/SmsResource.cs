using GM.PPAP.ServiceSDK.Base;
using GM.PPAP.ServiceSDK.Clients;
using GM.PPAP.ServiceSDK.Exceptions;
using GM.PPAP.ServiceSDK.Http;
using Newtonsoft.Json;

namespace GM.PPAP.ServiceSDK.Rest.Api.V1
{
    public class SmsResource : Resource
    {
        private static Request BuildCreateRequest(CreateSmsOptions options, IGmServiceRestClient client)
        {
            return new Request(
                HttpMethod.Post,
                Rest.Domain.Api,
                "/v1/sms",
                postParams: options.GetParams()
            );
        }

        public static SmsResource Create(CreateSmsOptions options, IGmServiceRestClient client = null)
        {
            client = client ?? GmService.GetServiceRestClient();
            var response = client.Request(BuildCreateRequest(options, client));
            return FromJson(response.Content);
        }

        public static SmsResource Create(Types.PhoneNumber to,
            string msg = null,
            string pathAccountSid = null,
            IGmServiceRestClient client = null)
        {
            var options = new CreateSmsOptions(to, msg);
            return Create(options, client);
        }

        public static SmsResource FromJson(string json)
        {
            // Convert all checked exceptions to Runtime
            try
            {
                return JsonConvert.DeserializeObject<SmsResource>(json);
            }
            catch (JsonException e)
            {
                throw new ApiException(e.Message, e);
            }
        }

        /// <summary>
        /// Twilio sms response trace code
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }
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
