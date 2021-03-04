using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Expenses.Api.IntegrationTests.Fixtures;
using Expenses.Api.Models.Common;
using Expenses.Api.Models.Expenses;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Expenses.Api.IntegrationTests.Expenses
{
  [Collection("ApiCollection")]
  public class GetListShould : IntegrationTest
  {
    public GetListShould(ApiWebApplicationFactory fixture) : base(fixture)
    { }

    public static async Task<List<DataResult<ExpenseModel>>> Get(HttpClient client)
    {
      var response = await client.GetAsync($"api/Expenses");
      response.EnsureSuccessStatusCode();
      var responseText = await response.Content.ReadAsStringAsync();
      var items = JsonConvert.DeserializeObject<List<DataResult<ExpenseModel>>>(responseText);
      return items;
    }

    [Fact]
    public async Task ReturnAnyList()
    {
      var items = await Get(_client);
      items.Should().NotBeNull();
    }
  }
}
