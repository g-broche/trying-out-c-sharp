using Microsoft.AspNetCore.Mvc;

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
    public TestResponse GetTest()
    {
        return new TestResponse { Value = "test successful" };
    }
}
