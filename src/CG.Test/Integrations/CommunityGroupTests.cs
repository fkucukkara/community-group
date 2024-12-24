using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;

namespace CG.Tests.Integrations;

public class PersonTests : IClassFixture<WebApplicationFactory<API.Startup>>
{
    private readonly WebApplicationFactory<API.Startup> _factory;

    public PersonTests(WebApplicationFactory<API.Startup> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/api/v1/CommunityGroup")]
    public async Task Get_Person_UnAuthorizedException(string url)
    {  
        var client = _factory.CreateClient();
        var response = await client.GetAsync(url);

        Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
    }
}
