using System;

namespace Expenses.Common.Exceptions
{
  public class NotFoundException : Exception
  {
    public NotFoundException(string message) : base(message)
    { }
  }
}