using System.Net.Http;
using System.Threading.Tasks;
using Expenses.Api.IntegrationTests.Fixtures;
using Expenses.Api.Models.Common;
using Expenses.Api.Models.Users;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Expenses.Api.IntegrationTests.Users
{
  [Collection("ApiCollection")]
  public class GetListShould : IntegrationTest
  {
    public GetListShould(ApiWebApplicationFactory fixture) : base(fixture)
    { }

    public static async Task<DataResult<UserModel>> Get(HttpClient client)
    {
      var response = await client.GetAsync($"api/Users?skip={0}&take={20}");
      response.EnsureSuccessStatusCode();
      var responseText = await response.Content.ReadAsStringAsync();
      var items = JsonConvert.DeserializeObject<DataResult<UserModel>>(responseText);
      return items;
    }

    [Fact]
    public async Task ReturnAnyList()
    {
      var items = await Get(_client);
      items.Should().NotBeNull();
      items.Data.Should().NotBeEmpty();
    }
  }
}
