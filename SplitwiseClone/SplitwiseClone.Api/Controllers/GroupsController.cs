using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitwiseClone.Application;
using SplitwiseClone.Application.Services;
using SplitwiseClone.Domain;

namespace SplitwiseClone.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    private readonly IRepository<Group> _groupRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBalanceService _balanceService;

    public GroupsController(
        IRepository<Group> groupRepository,
        IUnitOfWork unitOfWork,
        IBalanceService balanceService)
    {
        _groupRepository = groupRepository;
        _unitOfWork = unitOfWork;
        _balanceService = balanceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var groups = await _groupRepository.GetAllAsync();
        return Ok(groups);
    }

    [HttpGet("{id:guid}/balances")]
    public async Task<IActionResult> GetBalances(Guid id, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetByIdAsync(id);
        if (group is null)
            throw new KeyNotFoundException($"Group {id} not found.");

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var currentUserId) ||
            !group.Members.Any(m => m.UserId == currentUserId))
        {
            throw new UnauthorizedAccessException("User is not a member of this group.");
        }

        var balances = await _balanceService.GetGroupBalancesAsync(id, cancellationToken);
        return Ok(balances);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Group group)
    {
        await _groupRepository.AddAsync(group);
        await _unitOfWork.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = group.Id }, group);
    }
}