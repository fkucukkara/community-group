using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;

namespace CG.Tests.Integrations;

public class PersonTests(WebApplicationFactory<API.Startup> factory) : IClassFixture<WebApplicationFactory<API.Startup>>
{
    [Theory]
    [InlineData("/api/v1/CommunityGroup")]
    public async Task Get_Person_UnAuthorizedException(string url)
    {  
        var client = factory.CreateClient();
        var response = await client.GetAsync(url);

        Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
    }
}
