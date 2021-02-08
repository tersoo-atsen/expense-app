using Microsoft.EntityFrameworkCore;

namespace Expenses.Data.Access.DAL
{
  public class ExpensesAppDbContext : DbContext
  {
    public ExpensesAppDbContext(DbContextOptions<ExpensesAppDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      var mappings = MappingsHelper.GetMainMappings();

      foreach (var mapping in mappings)
      {
        mapping.Visit(modelBuilder);
      }
    }
  }
}