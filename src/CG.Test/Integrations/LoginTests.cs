using CG.Core.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace CG.Tests.Integrations;

public class LoginTests(WebApplicationFactory<API.Startup> factory) : IClassFixture<WebApplicationFactory<API.Startup>>
{
    [Theory]
    [InlineData("/api/v1/Login/login")]
    public async Task Get_Security_Token(string url)
    {
        var loginModel = new LoginModel()
        {
            Email = "crea",
            Password = "crea"
        };
        HttpContent contentPost = new StringContent(JsonSerializer.Serialize(loginModel), Encoding.UTF8, "application/json");
        var client = factory.CreateClient();
        var response = await client.PostAsync(url, contentPost);

        Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
    }
}
