
using LoginApi.BLL.Services;
using LoginApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoginApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly TokenService _tokenService;

    public AuthController(
        AuthService authService,
        TokenService tokenService)
    {
        _authService = authService;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public ActionResult<LoginResponse> Login(
        [FromBody] LoginRequest request)
    {
        if (!_authService.ValidateCredentials(
            request.Username,
            request.Password))
        {
            return Unauthorized(new LoginResponse
            {
                Success = false,
                Message = "Invalid username or password"
            });
        }

        var token = _tokenService.CreateToken(
            request.Username
        );

        return Ok(new LoginResponse
        {
            Success = true,
            Message = "Login successful",
            Username = request.Username,
            Token = token
        });
    }
}
