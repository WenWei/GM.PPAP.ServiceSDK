using System;
using System.Net;
using GM.PPAP.ServiceSDK.Clients;
using GM.PPAP.ServiceSDK.Exceptions;
using GM.PPAP.ServiceSDK.Http;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace GM.PPAP.ServiceSDK.Tests.Client
{
    public class GmServiceClientTest : BaseTestFixture
    {
        private HttpClient client;

        public GmServiceClientTest(ITestOutputHelper output) : base(output)
        {
            client = Substitute.For<HttpClient>();
        }

        [Fact]
        public void TestValidSslCert()
        {
            client.MakeRequest(Arg.Any<Request>()).Returns(new Response(HttpStatusCode.OK, "OK"));
            GmServiceRestClient.ValidateSslCertificate(client);
        }

        [Fact]
        public void TestCantConnect()
        {
            // Exception type doesn't matter, just needs to match in IsInstanceOf below.
            client.MakeRequest(Arg.Any<Request>()).Throws(new InvalidOperationException());

            //Should have failed ssl verification
            var e = Should.Throw<CertificateValidationException>(() => { GmServiceRestClient.ValidateSslCertificate(client); });

            e.GetBaseException().ShouldBeOfType<InvalidOperationException>();
            e.Message.ShouldBe("Connection to api.twilio.com:8443 failed");
            e.Response.ShouldBeNull();
            e.Request.ShouldNotBeNull();
        }

        [Fact]
        public void TestNotOkResponse()
        {
            client.MakeRequest(Arg.Any<Request>()).Returns(new Response(HttpStatusCode.SwitchingProtocols, "NOTOK"));

            //Should have failed ssl verification
            var e = Should.Throw<CertificateValidationException>(() => GmServiceRestClient.ValidateSslCertificate(client));
            e.Message.ShouldBe("Unexpected response from certificate endpoint");
            e.Response.ShouldNotBeNull();
            e.Request.ShouldNotBeNull();
        }
    }
}
