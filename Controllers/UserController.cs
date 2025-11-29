using Microsoft.AspNetCore.Mvc;
using Inventory.DTOs.User;
using Inventory.Services.Abstractions.User;


namespace Inventory.Controllers;

public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRequest(CreateUserTonerRequestDTO request)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        await _userService.CreateTonerRequest(request);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetRequestById(int id)
    {
        var request = await _userService.GetByIdAsync(id);
        if (request == null)
            return NotFound();

        return Ok(request);
    }
}