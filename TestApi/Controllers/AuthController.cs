using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApi.Data;
using TestApi.DTOs.Requests;
using TestApi.DTOs.Responses;
using TestApi.Models;
using TestApi.Responses;
using TestApi.Services;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly AppDbContext _db;
    private readonly PasswordHasher<User> _PasswordHasher = new();
    private readonly JwtService _jwtService;

    public AuthController(ILogger<AuthController> logger, AppDbContext db, JwtService jwtService)
    {
        _logger = logger;
        _db = db;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDetailDto>> Register(RegisterRequest request)
    {
        try
        {

            var newUser = new User();
            var hashedPassword = _PasswordHasher.HashPassword(newUser, request.Password);
            newUser.FirstName = request.FirstName;
            newUser.LastName = request.LastName;
            newUser.Email = request.Email;
            newUser.Password = hashedPassword;
            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();
            return Ok(new UserDetailDto(newUser));
        }
        catch (System.Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginRequest request)
    {
        try
        {
            if (request is null || request.Email is null || request.Password is null)
            {
                return BadRequest(ApiResponse<string>.Fail(message: "Request is not valid due to missing parameters"));
            }

            User? foundUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (foundUser is null)
            {
                return Unauthorized(ApiResponse<string>.Fail(message: "Invalid credentials."));
            }

            PasswordVerificationResult authResult = _PasswordHasher.VerifyHashedPassword(
                foundUser,
                foundUser.Password,
                request.Password
            );

            if (authResult == PasswordVerificationResult.Failed)
            {
                return Unauthorized(ApiResponse<string>.Fail(message: "Invalid credentials."));
            }
            var tokenDto = new TokenDto(_jwtService.GenerateJWT(foundUser));
            return Ok(ApiResponse<TokenDto>.Success(data: tokenDto));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<string>.Fail(message: "Something went wrong on the server."));
        }

    }

}