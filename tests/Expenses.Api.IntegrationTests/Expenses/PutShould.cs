using System;
using System.Threading.Tasks;
using Expenses.Api.IntegrationTests.Fixtures;
using Expenses.Api.IntegrationTests.Helpers;
using Expenses.Api.Models.Expenses;
using FluentAssertions;
using Xunit;

namespace Expenses.Api.IntegrationTests.Expenses
{
  [Collection("ApiCollection")]
  public class PutShould : IntegrationTest
  {
    private readonly HttpClientWrapper _wrappedClient;
    private readonly Random _random;
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
      var item = await new PostShould(_fixture).CreateNew();

      var requestItem = new UpdateExpenseModel
      {
        Date = DateTime.Now,
        Description = _random.Next().ToString(),
        Amount = _random.Next(),
        Comment = _random.Next().ToString()
      };

      await _wrappedClient.PutAsync<ExpenseModel>($"api/Expenses/{item.Id}", requestItem);

      var updatedItem = await GetItemShould.GetById(_client, item.Id);

      updatedItem.Date.Should().Be(requestItem.Date);
      updatedItem.Description.Should().Be(requestItem.Description);

      updatedItem.Amount.Should().Be(requestItem.Amount);
      updatedItem.Comment.Should().Contain(requestItem.Comment);
    }
  }
}