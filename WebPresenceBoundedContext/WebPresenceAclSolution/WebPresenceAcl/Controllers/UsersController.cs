using Dapr;

using Microsoft.AspNetCore.Mvc;

using WebPresenceMessages.Users;

namespace WebPresenceAcl.Controllers;

[ApiController]
public class UsersController : ControllerBase
{

    // when there is an internal new user created, publish it as a domain user onboarded

    [Topic("webpresence-acl-dev", "webpresence-internal-user-created")]
    [HttpPost]
    [Route("/webpresence/acl/map-new-user-to-domain")]
    public async Task<ActionResult> MapOnboard(NewUserCreated request)
    {
        
        return BadRequest();
    }
}
