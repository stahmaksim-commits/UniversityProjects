using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitwiseClone.Application;
using SplitwiseClone.Application.Services;
using SplitwiseClone.Application.Validators;
using SplitwiseClone.Domain;

namespace SplitwiseClone.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseService _expenseService;
    private readonly IValidator<CreateEqualSplitExpenseRequest> _validator;
    private readonly IRepository<Expense> _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ExpensesController(
        IExpenseService expenseService,
        IValidator<CreateEqualSplitExpenseRequest> validator,
        IRepository<Expense> expenseRepository,
        IUnitOfWork unitOfWork)
    {
        _expenseService = expenseService;
        _validator = validator;
        _expenseRepository = expenseRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var expenses = await _expenseRepository.GetAllAsync();
        return Ok(expenses);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateEqualSplitExpenseRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => new
            {
                e.PropertyName,
                e.ErrorMessage
            }));
        }

        var expense = await _expenseService.CreateEqualSplitExpenseAsync(
            request.Description,
            request.TotalCost,
            request.PaidByUserId,
            request.ParticipantUserIds,
            request.GroupId,
            request.IsPayment,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAll), new { id = expense.Id }, expense);
    }
}