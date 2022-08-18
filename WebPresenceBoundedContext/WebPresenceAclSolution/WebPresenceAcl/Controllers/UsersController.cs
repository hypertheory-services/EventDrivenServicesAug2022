using Confluent.Kafka;

using Dapr;

using Hypertheory.Events;

using Microsoft.AspNetCore.Mvc;

using WebPresenceAcl.Producers;

using WebPresenceMessages.Users;

namespace WebPresenceAcl.Controllers;

[ApiController]
public class UsersController : ControllerBase
{

    private readonly DomainUserOnboardedEventProducer _producer;

    public UsersController(DomainUserOnboardedEventProducer producer)
    {
        _producer = producer;
    }

    // when there is an internal new user created, publish it as a domain user onboarded

    [Topic("webpresence-acl-dev", "webpresence-internal-user-created")]
    [HttpPost]
    [Route("/webpresence/acl/map-new-user-to-domain")]
    public async Task<ActionResult> MapOnboard(NewUserCreated request)
    {
        
        var messageToPublish = new Hypertheory.Events.UserOnboarded
        {
           Email = request.Email,
           FirstName = request.FirstName,
           LastName = request.LastName,
           Source = "web-presence",
           UserId = request.UserId,
        };
        // Step 2???
        var message = new Message<Null, UserOnboarded>
        {
            Value = messageToPublish
        };

        await _producer.ProduceAsync("hypertheory-events-useronboarded", message);

        // Step 3 Profit!
        return BadRequest();
    }
}
