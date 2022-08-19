using Confluent.Kafka;

using Dapr;

using Hypertheory.Events;

using Microsoft.AspNetCore.Mvc;

using WebPresenceAcl.Producers;

using WebPresenceMessages.Enrollments;
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
    [Topic("webpresence-acl-dev", "webpresence-internal-enrollment-request-created")]
    [HttpPost("/webpresence/acl/man-enrollment-request-to-domain")]
    public ActionResult MapIt([FromServices] DomainEnrollmentRequestEventProducer producer, EnrollmentCreated request)
    {
        var messageToPublish = new Hypertheory.Events.EnrollmentRequested
        {
            OfferingId = request.OfferingId,
            StudentId = request.UserId,
            Originator = "web-presence"
        };
        var message = new Message<Null, EnrollmentRequested>
        {
            Value = messageToPublish
        };

        producer.Produce("hypertheory-events-enrollment-requested", message, HandleDeliveryReport);

        return Ok();
    }



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

        _producer.Produce("hypertheory-events-useronboarded", message, HandleDeliveryReport);

        // Step 3 Profit!
        return Ok();
    }

    private void HandleDeliveryReport<T>(DeliveryReport<Null, T> report)
    {
        // do your fancy stuff "we are a REAL business here maybe log something" 
    }
}
