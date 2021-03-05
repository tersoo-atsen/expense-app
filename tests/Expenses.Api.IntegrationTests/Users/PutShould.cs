using System;
using System.Net.Http;
using System.Threading.Tasks;
using Expenses.Api.IntegrationTests.Fixtures;
using Expenses.Api.IntegrationTests.Helpers;
using Expenses.Api.Models.Users;
using Expenses.Data.Access.Constants;
using FluentAssertions;
using Xunit;

namespace Expenses.Api.IntegrationTests.Users
{
  [Collection("ApiCollection")]
  public class PutShould : IntegrationTest
  {
    private readonly HttpClientWrapper _wrappedClient;
    private Random _random;
    private readonly ApiWebApplicationFactory _fixture;

    public PutShould(ApiWebApplicationFactory fixture) : base(fixture)
    {
      _wrappedClient = new HttpClientWrapper(_client);
      _random = new Random();
      _fixture = fixture;
    }

    [Fact]
    public async Task UpdateExistingItem()
    {
      var item = await new Login.PostShould(_fixture).RegisterNewUser();

      var requestItem = new UpdateUserModel
      {
        Username = "TU_Update_" + _random.Next(),
        FirstName = _random.Next().ToString(),
        LastName = _random.Next().ToString(),
        Roles = new[] { Roles.Manager }
      };

      await _wrappedClient.PutAsync<UserModel>($"api/Users/{item.Id}", requestItem);

      var updatedUser = await GetItemShould.GetById(_wrappedClient.Client, item.Id);

      updatedUser.FirstName.Should().Be(requestItem.FirstName);
      updatedUser.LastName.Should().Be(requestItem.LastName);

      updatedUser.Roles.Should().HaveCount(1);
      updatedUser.Roles.Should().Contain(Roles.Manager);
    }
  }
}