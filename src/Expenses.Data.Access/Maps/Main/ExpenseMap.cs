using Expenses.Data.Access.Maps.Common;
using Expenses.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Data.Access.Maps.Main
{
  public class ExpenseMap : IMap
  {
    public void Visit(ModelBuilder builder)
    {
      builder.Entity<Expense>().Property(e => e.Amount).HasColumnType("decimal");
      builder.Entity<Expense>().ToTable("Expenses").HasKey(x => x.Id);
    }
  }
}