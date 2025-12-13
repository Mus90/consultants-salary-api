using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConsultantsSalary.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SecuredController : ControllerBase
{
    [HttpGet("ping")]
    [Authorize]
    public IActionResult Ping() => Ok(new { message = "pong (authorized)" });

    [HttpGet("manager-ping")]
    [Authorize(Roles = "Manager")]
    public IActionResult ManagerPing() => Ok(new { message = "pong (manager authorized)" });
}
