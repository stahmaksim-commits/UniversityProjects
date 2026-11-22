using Microsoft.AspNetCore.Mvc;
using SplitwiseClone.Application;
using SplitwiseClone.Domain;

namespace SplitwiseClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    private readonly IRepository<Group> _groupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GroupsController(IRepository<Group> groupRepository, IUnitOfWork unitOfWork)
    {
        _groupRepository = groupRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var groups = await _groupRepository.GetAllAsync();
        return Ok(groups);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Group group)
    {
        await _groupRepository.AddAsync(group);
        await _unitOfWork.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = group.Id }, group);
    }
}
