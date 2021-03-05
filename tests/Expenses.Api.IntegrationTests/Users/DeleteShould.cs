using System;
using System.Threading.Tasks;
using Expenses.Api.IntegrationTests.Fixtures;
using Xunit;

namespace Expenses.Api.IntegrationTests.Users
{
  [Collection("ApiCollection")]
  public class DeleteShould : IntegrationTest
  {
    private readonly ApiWebApplicationFactory _fixture;
    public DeleteShould(ApiWebApplicationFactory fixture) : base(fixture)
    {
      _fixture = fixture;
    }

    [Fact]
    public async Task DeleteExistingItem()
    {
      var item = await new Login.PostShould(_fixture).RegisterNewUser();

      var response = await _client.DeleteAsync(new Uri($"api/Users/{item.Id}", UriKind.Relative));
      response.EnsureSuccessStatusCode();
    }
  }
}