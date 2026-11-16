using MediatR;
using StoryCollabApi.Models;
using StoryCollabApi.Repositories;

namespace StoryCollabApi.CQRS
{
    public class AddUserCommand : IRequest<StoryUser>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, StoryUser>
    {
        private readonly IUserRepository _userRepository;

        public AddUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<StoryUser> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var user = new StoryUser
            {
                Name = request.Name,
                Email = request.Email
            };

            await _userRepository.CreateUserAsync(user);
            return user;
        }
    }
}