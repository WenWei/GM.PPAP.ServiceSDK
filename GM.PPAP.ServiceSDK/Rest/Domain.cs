using GM.PPAP.ServiceSDK.Types;

namespace GM.PPAP.ServiceSDK.Rest
{
    public sealed class Domain : StringEnum
    {
        private Domain(string value) : base(value) { }
        public Domain() { }
        public static implicit operator Domain(string value)
        {
            return new Domain(value);
        }

        public static readonly Domain Api = new Domain("api");
        public static readonly Domain Sms = new Domain("sms");
        public static readonly Domain Email = new Domain("email");
    }
}
