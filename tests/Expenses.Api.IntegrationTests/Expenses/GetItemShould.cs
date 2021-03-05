using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Expenses.Api.IntegrationTests.Fixtures;
using Expenses.Api.Models.Expenses;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Expenses.Api.IntegrationTests.Expenses
{
  [Collection("ApiCollection")]
  public class GetItemShould : IntegrationTest
  {
    private Random _random;
    private readonly ApiWebApplicationFactory _fixture;

    public GetItemShould(ApiWebApplicationFactory fixture) : base(fixture)
    {
      _random = new Random();
      _fixture = fixture;
    }

    [Fact]
    public async Task ReturnItemById()
    {
      var item = await new PostShould(_fixture).CreateNew();
      var result = await GetById(_client, item.Id);
      result.Should().NotBeNull();
    }

    public static async Task<ExpenseModel> GetById(HttpClient client, int id)
    {
      var response = await client.GetAsync(new Uri($"api/Expenses/{id}", UriKind.Relative));
      response.EnsureSuccessStatusCode();
      var result = await response.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<ExpenseModel>(result);
    }

    [Fact]
    public async Task ShouldReturn404StatusIfNotFound()
    {
      var response = await _client.GetAsync(new Uri($"api/Expenses/-1", UriKind.Relative));
      response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
  }
}