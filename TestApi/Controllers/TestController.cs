using Microsoft.AspNetCore.Mvc;
using TestApi.DTOs;
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
}
