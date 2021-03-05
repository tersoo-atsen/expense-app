using System;
using System.Threading.Tasks;
using Expenses.Api.IntegrationTests.Fixtures;
using Expenses.Api.IntegrationTests.Helpers;
using Expenses.Api.Models.Users;
using FluentAssertions;
using Xunit;

namespace Expenses.Api.IntegrationTests.Users
{
  [Collection("ApiCollection")]
  public class PostShould : IntegrationTest
  {
    private Random _random;
    private readonly HttpClientWrapper _wrappedClient;
    private readonly ApiWebApplicationFactory _fixture;

    public PostShould(ApiWebApplicationFactory fixture) : base(fixture)
    {
      _random = new Random();
      _wrappedClient = new HttpClientWrapper(_client);
      _fixture = fixture;
    }

    [Fact]
    public async Task ChangePassword()
    {
      var newUser = await new Login.PostShould(_fixture).RegisterNewUser();
      var newPassword = _random.Next().ToString();
      await _wrappedClient.PostAsync($"api/Users/{newUser.Id}/password", new ChangeUserPasswordModel
      {
        Password = newPassword
      });

      //Should be able to login
      var loginedUser = await new Login.PostShould(_fixture).Autheticate(newUser.Username, newPassword);
      loginedUser.User.Username.Should().Be(newUser.Username);
    }
  }
}