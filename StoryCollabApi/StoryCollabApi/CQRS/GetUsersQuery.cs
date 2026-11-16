using MediatR;
using StoryCollabApi.Models;
using StoryCollabApi.Repositories;

namespace StoryCollabApi.CQRS
{
    public class GetUsersQuery : IRequest<List<StoryUser>> { }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<StoryUser>>
    {
        private readonly IUserRepository _users;

        public GetUsersQueryHandler(IUserRepository users)
        {
            _users = users;
        }

        public async Task<List<StoryUser>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await _users.GetAllUsersAsync();
        }
    }
}