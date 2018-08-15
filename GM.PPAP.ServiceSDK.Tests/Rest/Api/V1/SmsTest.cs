using System;
using System.Collections.Generic;
using System.Text;
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
    public class SmsTest : BaseTestFixture
    {
        public SmsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void TestCreateRequest()
        {
            var gmServiceRestClient = Substitute.For<IGmServiceRestClient>();
            var request = new Request(
                HttpMethod.Post,
                GM.PPAP.ServiceSDK.Rest.Domain.Api,
                "/v1/sms"
            );
            request.AddPostParam("To", Serialize(new Types.PhoneNumber("+886912345678")));
            gmServiceRestClient.AccountSid.Returns("ACXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            gmServiceRestClient.Request(request).Throws(new ApiException("Server Error, no content"));

            //Expected Exception to be thrown for 500
            Should.Throw<ApiException>(() =>
                SmsResource.Create(new Types.PhoneNumber("+886912345678"), client: gmServiceRestClient));

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
                    "{\"account_sid\": \"ACaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa\",\"api_version\": \"v1\",\"body\": \"O Slash: \\u00d8, PoP: \\ud83d\\udca9\",\"date_created\": \"Thu, 30 Jul 2015 20:12:31 +0000\",\"date_sent\": \"Thu, 30 Jul 2015 20:12:33 +0000\",\"date_updated\": \"Thu, 30 Jul 2015 20:12:33 +0000\",\"direction\": \"outbound-api\",\"error_code\": null,\"error_message\": null,\"from\": \"+14155552345\",\"messaging_service_sid\": \"MGaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa\",\"num_media\": \"0\",\"num_segments\": \"1\",\"price\": \"-0.00750\",\"price_unit\": \"USD\",\"sid\": \"SMaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa\",\"status\": \"sent\",\"subresource_uris\": {\"media\": \"/2010-04-01/Accounts/ACaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/Messages/SMaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/Media.json\"},\"to\": \"+14155552345\",\"uri\": \"/2010-04-01/Accounts/ACaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/Messages/SMaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa.json\"}"
                ));

            var response = SmsResource.Create(new Types.PhoneNumber("+886912345678"), client: gmServiceRestClient);
            response.ShouldNotBeNull();
        }
    }
}
