using System;
using System.Linq;
using Expenses.Api.IntegrationTests.Helpers;
using Expenses.Data.Access.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Expenses.Api.IntegrationTests.Fixtures
{
  public class ApiWebApplicationFactory : WebApplicationFactory<Startup>
  {
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
      builder.ConfigureServices(services =>
      {
        var descriptor = services
          .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ExpensesAppDbContext>));

        services.Remove(descriptor);

        services.AddDbContext<ExpensesAppDbContext>(options =>
        {
          options.UseInMemoryDatabase("InMemoryDbForTesting");
        });

        var sp = services.BuildServiceProvider();

        using (var scope = sp.CreateScope())
        {
          var scopedServices = scope.ServiceProvider;
          var db = scopedServices.GetRequiredService<ExpensesAppDbContext>();
          var logger = scopedServices.GetRequiredService<ILogger<ApiWebApplicationFactory>>();

          db.Database.EnsureCreated();

          try
          {
            Utilities.InitializeDbForTests(db);
          }
          catch (Exception ex)
          {
            logger.LogError(ex, "An error occurred seeding the " +
                "database with test messages. Error: {Message}", ex.Message);
          }
        }
      });
    }
  }
}