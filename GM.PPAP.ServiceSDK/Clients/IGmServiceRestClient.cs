using GM.PPAP.ServiceSDK.Http;

namespace GM.PPAP.ServiceSDK.Clients
{
    public interface IGmServiceRestClient
    {
        string AccountSid { get; }
        HttpClient HttpClient { get; }
        Response Request(Request request);
    }
}