using Microsoft.AspNetCore.Mvc;
using TestApi.Responses;

namespace TestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{

    private readonly ILogger<TestController> _logger;

    public TestController(ILogger<TestController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetTest")]
    public ActionResult<ApiResponse<object>> GetTest()
    {
        var testData = new { Date = "2025-08-28", Weather = "cloudy" };
        var responseData = ApiResponse<object>.Success(testData, "Operation was successful");
        return Ok(responseData);
    }
}
