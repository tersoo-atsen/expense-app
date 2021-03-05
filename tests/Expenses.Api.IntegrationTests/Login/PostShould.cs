using System;
using System.Threading.Tasks;
using Expenses.Api.IntegrationTests.Fixtures;
using Expenses.Api.IntegrationTests.Helpers;
using Expenses.Api.Models.Authenticate;
using Expenses.Api.Models.Users;
using FluentAssertions;
using Xunit;

namespace Expenses.Api.IntegrationTests.Login
{
  [Collection("ApiCollection")]
  public class PostShould : IntegrationTest
  {
    private readonly HttpClientWrapper _wrappedClient;
    private Random _random;

    public PostShould(ApiWebApplicationFactory fixture) : base(fixture)
    {
      _random = new Random();
      _wrappedClient = new HttpClientWrapper(_client);
    }

    [Fact]
    public async Task AutheticateAdmin()
    {
      var username = "admin";
      var password = "admin";
      var result = await Autheticate(username, password);

      result.User.Username.Should().Be(username);
    }

    public async Task<UserWithTokenModel> Autheticate(string username, string password)
    {
      var response = await _wrappedClient
        .PostAsync<UserWithTokenModel>("api/auth/login", new LoginModel
        {
          Username = username,
          Password = password
        });

      return response;
    }

    [Fact]
    public async Task<UserModel> RegisterNewUser()
    {
      var requestItem = new RegisterModel
      {
        Username = "TU_" + _random.Next(),
        Password = _random.Next().ToString(),
        LastName = _random.Next().ToString(),
        FirstName = _random.Next().ToString()
      };

      var createdUser = await _wrappedClient.PostAsync<UserModel>("api/auth/register", requestItem);

      createdUser.Roles.Should().BeEmpty();
      createdUser.Username.Should().Be(requestItem.Username);
      createdUser.LastName.Should().Be(requestItem.LastName);
      createdUser.FirstName.Should().Be(requestItem.FirstName);

      return createdUser;
    }

    //[Fact]
    //public async Task ChangeUserPassword()
    //{
    //  var requestItem = new RegisterModel
    //  {
    //    Username = "TU_" + _random.Next(),
    //    Password = _random.Next().ToString(),
    //    LastName = _random.Next().ToString(),
    //    FirstName = _random.Next().ToString()
    //  };

    //  await _wrappedClient.PostAsync<UserModel>("api/auth/register", requestItem);

    //  var newClient = new HttpClientWrapper(_client);

    //  var newPassword = _random.Next().ToString();
    //  await newClient.PostAsync($"api/auth/reset", new ChangeUserPasswordModel
    //  {
    //    Password = newPassword
    //  });

    //  await Autheticate(requestItem.Username, newPassword);
    //}
  }
}