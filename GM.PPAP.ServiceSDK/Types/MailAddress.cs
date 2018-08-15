namespace GM.PPAP.ServiceSDK.Types
{
    /// <summary>
    /// Mail address endpoint
    /// </summary>
    public class MailAddress: IEndpoint
    {
        private readonly string _address;
        private readonly string _displayName;

        public MailAddress(string address)
        {
            _address = address;
        }

        public MailAddress(string address, string displayName)
        {
            _address = address;
            _displayName = displayName;
        }

        public override string ToString()
        {
            if (_displayName == null) return _address;
            return $"\"{_displayName}\"<{_address}>";
        }
    }
}
