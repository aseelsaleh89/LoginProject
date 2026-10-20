
using LoginApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MenuController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<MenuItem>> GetMenu()
    {
        var menu = new List<MenuItem>
        {
            new MenuItem
            {
                Id = 1,
                Title = "My Profile",
                Description = "View your profile"
            },
            new MenuItem
            {
                Id = 2,
                Title = "Tasks",
                Description = "View your tasks"
            },
            new MenuItem
            {
                Id = 3,
                Title = "Settings",
                Description = "Manage settings"
            }
        };

        return Ok(menu);
    }
}
