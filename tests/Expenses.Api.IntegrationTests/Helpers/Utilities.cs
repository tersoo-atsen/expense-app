using Expenses.Data.Access.Constants;
using Expenses.Data.Access.DAL;
using Expenses.Data.Access.Helpers;
using Expenses.Data.Models;
using System;
using System.Collections.Generic;

namespace Expenses.Api.IntegrationTests.Helpers
{
  public static class Utilities
  {
    public static void InitializeDbForTests(ExpensesAppDbContext context)
    {
      SeedUsers(context);
      SeedRoles(context);
      SeedExpenses(context);
    }

    public static void ReinitializeDbForTests(ExpensesAppDbContext context)
    {
      var expenseSet = context.Set<Expense>();
      var roleSet = context.Set<Role>();
      var userRoleSet = context.Set<UserRole>();
      var userSet = context.Set<User>();

      expenseSet.RemoveRange(expenseSet);
      roleSet.RemoveRange(roleSet);
      userRoleSet.RemoveRange(userRoleSet);
      userSet.RemoveRange(userSet);

      InitializeDbForTests(context);
    }

    public static void SeedExpenses(ExpensesAppDbContext context)
    {
      var expenseSet = context.Set<Expense>();
      var expenses = new List<Expense>()
      {
        new Expense(){ 
          Date = DateTime.Now,
          Amount = 1500,
          Comment = "Comment1",
          Description = "Description1",
          UserId = 1,
          IsDeleted = false
        },
        new Expense(){
          Date = DateTime.Now,
          Amount = 2500,
          Comment = "Comment2",
          Description = "Description2",
          UserId = 2,
          IsDeleted = false
        }
      };
      expenseSet.AddRange(expenses);
      context.SaveChanges();
    }

    public static void SeedUsers(ExpensesAppDbContext context)
    {
      var userSet = context.Set<User>();
      var users = new List<User>()
      {
        new User {
          FirstName = "admin",
          LastName = "admin",
          Password = "admin".WithBCrypt(),
          Username = "admin",
          IsDeleted = false
        },
        new User {
          FirstName = "Henry",
          LastName = "Atsen",
          Password = "AppPass123".WithBCrypt(),
          Username = "henryatsen",
          IsDeleted = false
        }
      };

      userSet.AddRange(users);
      context.SaveChanges();
    }

    public static void SeedRoles(ExpensesAppDbContext context)
    {
      var roleSet = context.Set<Role>();
      var userRoleSet = context.Set<UserRole>();
      var userSet = context.Set<User>();

      roleSet.AddRange(new List<Role>()
      {
        new Role { Name = Roles.Manager },
        new Role { Name = Roles.Administrator }
      });

      userRoleSet.AddRange(
        new List<UserRole>()
        {
          new UserRole
          {
            User = userSet.Find(1),
            Role = roleSet.Find(1)
          },
          new UserRole {
            User = userSet.Find(2),
            Role = roleSet.Find(2)
          }
        }
      );

      context.SaveChanges();
    }
  }
}