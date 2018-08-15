using System;
using System.Net;
using System.Threading.Tasks;
using GM.PPAP.ServiceSDK.Exceptions;
using GM.PPAP.ServiceSDK.Http;
using Newtonsoft.Json;

namespace GM.PPAP.ServiceSDK.Clients
{
    public class GmServiceRestClient: IGmServiceRestClient
    {
        public string AppId { get; }
        public string Secret { get; }
        public string ServiceUrl { get; }
        public string AccountSid { get; }
        public HttpClient HttpClient { get; }

        public GmServiceRestClient(
            string appId,
            string secret,
            string serviceUrl,
            string accountSid = null,
            HttpClient httpClient = null)
        {
            AppId = appId;
            Secret = secret;
            ServiceUrl = serviceUrl;
            AccountSid = accountSid ?? appId;
            HttpClient = httpClient ?? DefaultClient();
        }

        public static void ValidateSslCertificate(HttpClient client)
        {
            Request request = new Request("GET", "api", ":8443/", null);

            try
            {
                Response response = client.MakeRequest(request);

                if (!response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    throw new CertificateValidationException(
                        "Unexpected response from certificate endpoint",
                        request,
                        response
                    );
                }
            }
            catch (CertificateValidationException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new CertificateValidationException(
                    "Connection to api.twilio.com:8443 failed",
                    e,
                    request
                );
            }
        }

        private static Response ProcessResponse(Response response)
        {
            if (response == null)
            {
                throw new ApiConnectionException("Connection Error: No response received.");
            }

            if (response.StatusCode >= HttpStatusCode.OK && response.StatusCode < HttpStatusCode.Ambiguous)
            {
                return response;
            }

            // Deserialize and throw exception
            RestException restException = null;
            try
            {
                restException = RestException.FromJson(response.Content);
            }
            catch (JsonReaderException) { /* Allow null check below to handle */ }

            if (restException == null)
            {
                throw new ApiException("Api Error: " + response.StatusCode + " - " + (response.Content ?? "[no content]"));
            }

            throw new ApiException(
                restException.Code,
                (int)response.StatusCode,
                restException.Message ?? "Unable to make request, " + response.StatusCode,
                restException.MoreInfo
            );
        }


        public Response Request(Request request)
        {
            request.SetAuth(AppId, Secret);
            request.SetServiceUrl(ServiceUrl);

            Response response;
            try
            {
                response = HttpClient.MakeRequest(request);
            }
            catch (Exception clientException)
            {
                throw new ApiConnectionException(
                    "Connection Error: " + request.Method + request.ConstructUrl(),
                    clientException
                );
            }
            return ProcessResponse(response);
        }
#if !NET35
        /// <summary>
        /// Make a request to the Twilio API
        /// </summary>
        ///
        /// <param name="request">request to make</param>
        /// <returns>Task that resolves to the response of the request</returns>
        public async Task<Response> RequestAsync(Request request)
        {
            request.SetAuth(AppId, Secret);
            request.SetServiceUrl(ServiceUrl);

            Response response;
            try
            {
                response = await HttpClient.MakeRequestAsync(request);
            }
            catch (Exception clientException)
            {
                throw new ApiConnectionException(
                    "Connection Error: " + request.Method + request.ConstructUrl(),
                    clientException
                );
            }
            return ProcessResponse(response);
        }

        private static HttpClient DefaultClient()
        {
            return new SystemNetHttpClient();
        }
#else
        private static HttpClient DefaultClient()
        {
            return new WebRequestClient();
        }
#endif
    }
}