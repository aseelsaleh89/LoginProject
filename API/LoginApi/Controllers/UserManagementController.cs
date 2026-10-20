
using System.Net;
using System.Security.Cryptography;
using System.Text;
using LoginApi.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoginApi.Controllers;

[ApiController]
[Route("api/admin/users")]
public class UserManagementController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public UserManagementController(
        AuthService authService,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        _authService = authService;
        _configuration = configuration;
        _environment = environment;
    }

    [HttpPost]
    public IActionResult CreateUser(
        [FromBody] CreateUserRequest request)
    {
       
        if (!_environment.IsDevelopment())
            return NotFound();

        var remoteIp = HttpContext.Connection.RemoteIpAddress;

        if (remoteIp is null || !IPAddress.IsLoopback(remoteIp))
            return NotFound();

        var configuredKey =
            _configuration["Admin:UserCreationKey"];

        var suppliedKey =
            Request.Headers["X-Admin-Key"].ToString();

        if (string.IsNullOrWhiteSpace(configuredKey) ||
            string.IsNullOrWhiteSpace(suppliedKey))
            return Unauthorized();

        var expectedBytes = Encoding.UTF8.GetBytes(configuredKey);
        var suppliedBytes = Encoding.UTF8.GetBytes(suppliedKey);

        if (expectedBytes.Length != suppliedBytes.Length ||
            !CryptographicOperations.FixedTimeEquals(
                expectedBytes, suppliedBytes))
            return Unauthorized();

        if (string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password) ||
            request.Password.Length < 8)
        {
            return BadRequest(new
            {
                message = "Username and email are required. Password must be at least 8 characters."
            });
        }

        var created = _authService.CreateUser(
            request.Username.Trim(),
            request.Email.Trim(),
            request.Password);

        if (!created)
        {
            return Conflict(new
            {
                message = "This username already exists."
            });
        }

        return Created(
            $"/api/admin/users/{Uri.EscapeDataString(request.Username)}",
            new
            {
                success = true,
                username = request.Username,
                message = "User created successfully."
            });
    }
}

public class CreateUserRequest
{
    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
