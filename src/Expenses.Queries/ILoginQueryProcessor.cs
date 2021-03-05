using System.Threading.Tasks;
using Expenses.Api.Models.Authenticate;
using Expenses.Api.Models.Users;
using Expenses.Data.Models;
using Expenses.Queries.Models;

namespace Expenses.Queries
{
    public interface ILoginQueryProcessor
    {
        UserWithToken Authenticate(string username, string password);
        Task<User> Register(RegisterModel model);
        Task ChangePassword(ChangeUserPasswordModel requestModel);
    }
}