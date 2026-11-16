using MediatR;
using Microsoft.AspNetCore.Mvc;
using StoryCollabApi.CQRS;
using StoryCollabApi.Models;

namespace StoryCollabApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<StoryUser>>> GetUsers()
        {
            var users = await _mediator.Send(new GetUsersQuery());
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] AddUserCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid user data.");
            }

            var user = await _mediator.Send(command);
            return Ok(user);
        }
    }
}