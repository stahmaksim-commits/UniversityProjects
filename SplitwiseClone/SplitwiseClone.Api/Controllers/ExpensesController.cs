using Microsoft.AspNetCore.Mvc;
using SplitwiseClone.Application;
using SplitwiseClone.Domain;

namespace SplitwiseClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly IRepository<Expense> _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ExpensesController(IRepository<Expense> expenseRepository, IUnitOfWork unitOfWork)
    {
        _expenseRepository = expenseRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var expenses = await _expenseRepository.GetAllAsync();
        return Ok(expenses);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Expense expense)
    {
        await _expenseRepository.AddAsync(expense);
        await _unitOfWork.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = expense.Id }, expense);
    }
}
