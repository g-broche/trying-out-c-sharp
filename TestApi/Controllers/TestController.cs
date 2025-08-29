using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApi.Data;
using TestApi.DTOs;
using TestApi.Responses;

namespace TestApi.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;
    private readonly AppDbContext _db;

    public TestController(ILogger<TestController> logger, AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpGet()]
    public ActionResult<ApiResponse<PictureDto[]>> GetTest()
    {
        PictureDto[] pictures = [
                new("Paul Jarvis", "Forest retreat", "https://fastly.picsum.photos/id/11/2500/1667.jpg?hmac=xxjFJtAPgshYkysU_aqx2sZir-kIOjNR9vx0te7GycQ"),
                new("Paul Jarvis", "Beach", "https://fastly.picsum.photos/id/13/2500/1667.jpg?hmac=SoX9UoHhN8HyklRA4A3vcCWJMVtiBXUg0W4ljWTor7s"),
                new("Jerry Adney", "Forest river", "https://fastly.picsum.photos/id/28/4928/3264.jpg?hmac=GnYF-RnBUg44PFfU5pcw_Qs0ReOyStdnZ8MtQWJqTfA")
            ];
        var responseData = ApiResponse<object>.Success(pictures, "Operation was successful");
        return Ok(responseData);
    }

    [HttpGet("db")]
    public async Task<IActionResult> GetDbTest()
    {
        try
        {
            // Simple test query: SELECT 1
            await _db.Database.ExecuteSqlRawAsync("SELECT 1");

            // Optionally, you can also count users as an extra check
            // var userCount = await _db.Users.CountAsync();

            return Ok(new { Message = "Database connection successful" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Database connection failed", Error = ex.Message });
        }
    }
}
