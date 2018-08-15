using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace GM.PPAP.ServiceSDK.Tests
{
    public class BaseTestFixture: IDisposable
    {
        protected readonly ITestOutputHelper output;

        public BaseTestFixture(ITestOutputHelper output)
        {
            this.output = output;
        }

        public string Serialize(object obj) => obj.ToString();
        public string Serialize(DateTime date) => date.ToString("yyyy-MM-dd");
        public string Serialize(bool boolean) => boolean.ToString().ToLower();
        public string Serialize(Uri url) => url.AbsoluteUri.TrimEnd('/');

        public void Dispose()
        {
        }
    }
}
