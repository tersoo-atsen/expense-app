using Expenses.Api.IntegrationTests.Helpers;
using Expenses.Api.Models.Authenticate;
using Expenses.Api.Models.Users;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http;
using Xunit;

namespace Expenses.Api.IntegrationTests.Fixtures
{
  public abstract class IntegrationTest : IClassFixture<ApiWebApplicationFactory>
  {
    public const string Username = "admin";
    public const string Password = "admin";
    protected readonly ApiWebApplicationFactory _factory;
    protected readonly HttpClient _client;
    protected readonly string _accessToken;

    public IntegrationTest(ApiWebApplicationFactory factory)
    {
      _factory = factory;
      _client = GetAuthenticatedClient(Username, Password);
    }

    public HttpClient GetAuthenticatedClient(string username, string password)
    {
      var client = _factory
        .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

      var response = client.PostAsync("/api/auth/login", new JsonContent(new LoginModel { Password = password, Username = username })).Result;

      response.EnsureSuccessStatusCode();

      var data = JsonConvert.DeserializeObject<UserWithTokenModel>(response.Content.ReadAsStringAsync().Result);
      client.DefaultRequestHeaders.Add("Authorization", "Bearer " + data.Token);

      return client;
    }
  }
}
