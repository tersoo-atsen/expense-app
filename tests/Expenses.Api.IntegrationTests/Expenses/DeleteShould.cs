using System;
using System.Threading.Tasks;
using Expenses.Api.IntegrationTests.Fixtures;
using Xunit;

namespace Expenses.Api.IntegrationTests.Expenses
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
      var item = await new PostShould(_fixture).CreateNew();

      var response = await _client.DeleteAsync(new Uri($"api/Expenses/{item.Id}", UriKind.Relative));
      response.EnsureSuccessStatusCode();
    }
  }
}