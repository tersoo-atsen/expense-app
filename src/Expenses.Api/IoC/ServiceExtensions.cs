using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Expenses.Data.Access.DAL;
using Expenses.Helpers;
using Expenses.Maps;
using Expenses.Queries;
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
      AddQueries(services);
      ConfigureAuth(services);
      ConfigureAutoMapper(services);
    }

    private static void ConfigureAuth(IServiceCollection services)
    {
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddScoped<ITokenBuilder, TokenBuilder>();
      services.AddScoped<ISecurityContext, SecurityContext>();
    }

    private static void ConfigureAutoMapper(IServiceCollection services)
    {
      var mapperConfig = AutoMapperConfigurator.Configure();
      var mapper = mapperConfig.CreateMapper();
      services.AddSingleton(x => mapper);
      services.AddTransient<IAutoMapper, AutoMapperAdapter>();
    }

    private static void AddUow(IServiceCollection services, IConfiguration configuration)
    {
      var connectionString = configuration.GetConnectionString("Default");

      services.AddDbContext<ExpensesAppDbContext>(options => options.UseSqlServer(connectionString));
      services.AddScoped<IUnitOfWork>(ctx => new EFUnitOfWork(ctx.GetRequiredService<ExpensesAppDbContext>()));
      services.AddScoped<IActionTransactionHelper, ActionTransactionHelper>();
      services.AddScoped<Api.Filters.UnitOfWorkFilterAttribute>();
    }

    private static void AddQueries(IServiceCollection services)
    {
      var exampleProcessorType = typeof(UsersQueryProcessor);
      var types = (from t in exampleProcessorType.GetTypeInfo().Assembly.GetTypes()
                   where t.Namespace == exampleProcessorType.Namespace
                         && t.GetTypeInfo().IsClass
                         && t.GetTypeInfo().GetCustomAttribute<CompilerGeneratedAttribute>() == null
                   select t).ToArray();

      foreach (var type in types)
      {
        var interfaceQ = type.GetTypeInfo().GetInterfaces().First();
        services.AddScoped(interfaceQ, type);
      }
    }
  }
}
