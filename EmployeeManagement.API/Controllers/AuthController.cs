using EmployeeManagement.Application.Auth;
using EmployeeManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _auth.LoginAsync(request);

        var message = string.IsNullOrEmpty(result.Token) ? "Invalid credentials" : "Login success";

        return Ok(new
        {
            success = true,
            message,
            data = result
        });
    }
}