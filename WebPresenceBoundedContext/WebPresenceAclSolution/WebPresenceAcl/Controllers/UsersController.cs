using Microsoft.AspNetCore.Mvc;

namespace WebPresenceAcl.Controllers;

[ApiController]
public class UsersController : ControllerBase
{

    // when there is an internal new user created, publish it as a domain user onboarded

    [HttpPost]
    [Route("/webpresence/acl/map-new-user-to-domain")]
    public async Task<ActionResult> MapOnboard()
    {
        return BadRequest();
    }
}
