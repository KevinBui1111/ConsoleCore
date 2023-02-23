using System.Net.Http;
using IdentityModel.Client;

namespace ConsoleCore
{
    public class TestOAuth2
    {
        internal static void test()
        {
            var client = new HttpClient();

            var responseTask = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = "https://sso.vn00c1.vn.infra/auth/realms/hci/protocol/openid-connect/token",
                ClientId = "PCS_User",
                ClientSecret = "PCS_User"
            });

            var token = responseTask.Result;

            var resTask = client.GetStringAsync(
                "https://coma.vn00c1.vn.infra/rest/v11/contracts?projection=CONTRACT_COMMODITY_DEFAULT&contractCode=4105169691");
            var res = resTask.Result;
        }
    }
}