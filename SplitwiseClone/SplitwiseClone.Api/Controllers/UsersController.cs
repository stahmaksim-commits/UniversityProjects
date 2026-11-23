using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitwiseClone.Application;
using SplitwiseClone.Domain;

namespace SplitwiseClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UsersController(IRepository<ApplicationUser> userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userRepository.GetAllAsync();
        return Ok(users);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] ApplicationUser user)
    {
        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = user.Id }, user);
    }
}