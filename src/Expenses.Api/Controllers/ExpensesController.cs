using System.Linq;
using System.Threading.Tasks;
using Expenses.Api.Models.Expenses;
using Expenses.Data.Models;
using Expenses.Filters;
using Expenses.Maps;
using Expenses.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.Api.Controllers
{
  [Route("api/[controller]")]
  public class ExpensesController : Controller
  {
    private readonly IExpensesQueryProcessor _query;
    private readonly IAutoMapper _mapper;

    public ExpensesController(IExpensesQueryProcessor query, IAutoMapper mapper)
    {
      _query = query;
      _mapper = mapper;
    }

    [HttpGet]
    public IQueryable<ExpenseModel> Get()
    {
      var result = _query.Get();
      var models = _mapper.Map<Expense, ExpenseModel>(result);
      return models;
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      var item = _query.Get(id);
      if (item == null) return NotFound();
      var model = _mapper.Map<ExpenseModel>(item);
      return Ok(model);
    }

    [HttpPost]
    [ValidateModel]
    public async Task<ExpenseModel> Post([FromBody] CreateExpenseModel requestModel)
    {
      var item = await _query.Create(requestModel);
      var model = _mapper.Map<ExpenseModel>(item);
      return model;
    }

    [HttpPut("{id}")]
    [ValidateModel]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateExpenseModel requestModel)
    {
      var item = await _query.Update(id, requestModel);
      if (item == null) return NotFound();
      var model = _mapper.Map<ExpenseModel>(item);
      return Ok(model);
    }

    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
      await _query.Delete(id);
    }
  }
}