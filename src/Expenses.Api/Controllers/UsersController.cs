using System.Linq;
using System.Threading.Tasks;
using Expenses.Api.Models.Users;
using Expenses.Data.Access.Constants;
using Expenses.Data.Models;
using Expenses.Filters;
using Expenses.Maps;
using Expenses.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.Server.RestAPI
{
  [Route("api/[controller]")]
  [Authorize(Roles = Roles.AdministratorOrManager)]
  public class UsersController : Controller
  {
    private readonly IUsersQueryProcessor _query;
    private readonly IAutoMapper _mapper;

    public UsersController(IUsersQueryProcessor query, IAutoMapper mapper)
    {
      _query = query;
      _mapper = mapper;
    }

    [HttpGet]
    [QueryableResult]
    public IQueryable<UserModel> Get()
    {
      var result = _query.Get();
      var models = _mapper.Map<User, UserModel>(result);
      return models;
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      var item = _query.Get(id);
      if (item == null)
      {
        return NotFound();
      }
      var model = _mapper.Map<UserModel>(item);
      return Ok(model);
    }

    [HttpPost("{id}/password")]
    [ValidateModel]
    public async Task ChangePassword(int id, [FromBody] ChangeUserPasswordModel requestModel)
    {
      await _query.ChangePassword(id, requestModel);
    }

    [HttpPut("{id}")]
    [ValidateModel]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateUserModel requestModel)
    {
      var item = await _query.Update(id, requestModel);
      if (item == null)
      {
        return NotFound();
      }
      var model = _mapper.Map<UserModel>(item);
      return Ok(model);
    }

    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
      await _query.Delete(id);
    }
  }
}