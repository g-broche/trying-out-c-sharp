using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    private static List<User> _users = [];
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
    public ActionResult<UserDetailDto> Register(RegisterRequest request)
    {
        try
        {

            var newUser = new User();
            var hashedPassword = _PasswordHasher.HashPassword(newUser, request.Password);
            newUser.FirstName = request.FirstName;
            newUser.LastName = request.LastName;
            newUser.Email = request.Email;
            newUser.Password = hashedPassword;
            _users.Add(newUser);
            return Ok(new UserDetailDto(newUser));
        }
        catch (System.Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpPost("login")]
    public ActionResult<string> Login(LoginRequest request)
    {
        try
        {
            Console.WriteLine(">>> Incomming request email : " + request.Email);
            Console.WriteLine(">>> Incomming request password : " + request.Password);
            if (request is null || request.Email is null || request.Password is null)
            {
                return BadRequest(ApiResponse<string>.Fail(message: "Request is not valid due to missing parameters"));
            }

            _users.ForEach((user) =>
            {
                Console.WriteLine(">>> existing user in list : " + user.Email);
            });

            User? foundUser = _users.Find((it) => it.Email == request.Email);

            if (foundUser is null)
            {
                Console.WriteLine(">>> no matching user found");
                return Unauthorized(ApiResponse<string>.Fail(message: "Invalid credentials."));
            }

            PasswordVerificationResult authResult = _PasswordHasher.VerifyHashedPassword(
                foundUser,
                foundUser.Password,
                request.Password
            );

            if (authResult == PasswordVerificationResult.Failed)
            {
                Console.WriteLine(">>> password is not matching");
                return Unauthorized(ApiResponse<string>.Fail(message: "Invalid credentials."));
            }
            var tokenDto = new TokenDto(_jwtService.GenerateJWT(foundUser));
            Console.WriteLine(">>> created token : " + tokenDto.Token);
            return Ok(ApiResponse<TokenDto>.Success(data: tokenDto));
        }
        catch (Exception ex)
        {
            Console.WriteLine(">>> exception : " + ex.Message);
            return StatusCode(500, ApiResponse<string>.Fail(message: "Something went wrong on the server."));
        }

    }

}