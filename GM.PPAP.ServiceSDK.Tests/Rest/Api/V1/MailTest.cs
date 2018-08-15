using GM.PPAP.ServiceSDK.Clients;
using GM.PPAP.ServiceSDK.Exceptions;
using GM.PPAP.ServiceSDK.Http;
using GM.PPAP.ServiceSDK.Rest.Api.V1;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace GM.PPAP.ServiceSDK.Tests.Rest.Api.V1
{
    public class MailTest: BaseTestFixture
    {
        public MailTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void TestCreteRequest()
        {
            var gmServiceRestClient = Substitute.For<IGmServiceRestClient>();
            var request = new Request(
                HttpMethod.Post,
                GM.PPAP.ServiceSDK.Rest.Domain.Api,
                "/v1/mail"
            );
            request.AddPostParam("To", Serialize(new Types.MailAddress("alger.chen23@gmail.com")));
            request.AddPostParam("Subject", Serialize("UnitTest"));
            request.AddPostParam("Body", Serialize("<h1>UnitTest</h1> <span style=\"color: red\">body</span>"));
            request.AddPostParam("IsHtml", Serialize(true));
            gmServiceRestClient.AccountSid.Returns("ACXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            gmServiceRestClient.Request(request).Throws(new ApiException("Server Error, no content"));

            //Expected Exception to be thrown for 500
            Should.Throw<ApiException>(() =>
                MailResource.Create(
                    new Types.MailAddress("alger.chen23@gmail.com"),
                    "UnitTest",
                    "<h1>UnitTest</h1> <span style=\"color: red\">body</span>",
                    true,
                    client: gmServiceRestClient));

            gmServiceRestClient.Received().Request(request);
        }

        [Fact]
        public void TestCreateResponse()
        {
            var gmServiceRestClient = Substitute.For<IGmServiceRestClient>();
            gmServiceRestClient.AccountSid.Returns("ACXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            gmServiceRestClient.Request(Arg.Any<Request>())
                .Returns(new Response(
                    System.Net.HttpStatusCode.Created,
                    "{\"account_sid\": \"ACaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa\",\"api_version\": \"v1\",\"status\": \"success\"}"
                ));

            var response = MailResource.Create(new Types.MailAddress("alger.chen23@gmail.com"),
                "UnitTest",
                "<h1>UnitTest</h1> <span style=\"color: red\">body</span>",
                true,
                client: gmServiceRestClient);
            response.ShouldNotBeNull();
        }
    }
}
