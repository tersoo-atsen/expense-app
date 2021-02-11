using Expenses.Data.Access.DAL;
using Expenses.Helpers;
using Expenses.Security;
using Expenses.Security.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expenses.IoC
{
  public static class ServiceExtensions
  {
    public static void Setup(IServiceCollection services, IConfiguration configuration)
    {
      AddUow(services, configuration);
      ConfigureAuth(services);
    }

    private static void ConfigureAuth(IServiceCollection services)
    {
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddScoped<ITokenBuilder, TokenBuilder>();
      services.AddScoped<ISecurityContext, SecurityContext>();
    }

    private static void AddUow(IServiceCollection services, IConfiguration configuration)
    {
      var connectionString = configuration.GetConnectionString("Default");

      services.AddDbContext<ExpensesAppDbContext>(options => options.UseSqlServer(connectionString));
      services.AddScoped<IUnitOfWork>(ctx => new EFUnitOfWork(ctx.GetRequiredService<ExpensesAppDbContext>()));
      services.AddScoped<IActionTransactionHelper, ActionTransactionHelper>();
      services.AddScoped<Api.Filters.UnitOfWorkFilterAttribute>();
    }
  }
}
