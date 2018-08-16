using System.Security.Authentication;
using GM.PPAP.ServiceSDK.Clients;

namespace GM.PPAP.ServiceSDK
{
    public static class GmService
    {
        private static string _appId;
        private static string _secret;
        private static IGmServiceRestClient _serviceRestClient;
        public static string ConfigFileName { get; private set; }

        public static void Init(string appId, string secret)
        {
            SetAppId(appId);
            SetSecret(secret);
        }

        public static void SetConfigFile(string configFileName)
        {
            ConfigFileName = configFileName;
        }

        private static void SetAppId(string appId)
        {
            if (appId == null) throw new AuthenticationException($"{nameof(appId)} can not be null");
            if (appId != _appId) Invalidate();

            _appId = appId;
        }

        private static void SetSecret(string secret)
        {
            if (secret == null) throw new AuthenticationException($"{nameof(secret)} can not be null");
            if (secret != _secret) Invalidate();

            _secret = secret;
        }

        public static IGmServiceRestClient GetServiceRestClient()
        {
            if (_serviceRestClient != null)
                return _serviceRestClient;

            if (_appId == null || _secret == null)
                throw new AuthenticationException("ServiceRestClient was used before AppId and Secret were set, please call GmServiceClient.init()");

            _serviceRestClient = new GmServiceRestClient(_appId, _secret);
            return _serviceRestClient;
        }

        public static void SetRestClient(IGmServiceRestClient serviceRestClient)
        {
            _serviceRestClient = serviceRestClient;
        }

        private static void Invalidate()
        {
            _serviceRestClient = null;
        }
    }
}
