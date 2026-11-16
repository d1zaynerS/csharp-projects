using StoryCollabApi.Models;

namespace StoryCollabApi.Repositories
{
    public interface IUserRepository
    {
        Task<List<StoryUser>> GetAllUsersAsync();
        Task<StoryUser?> GetUserByIdAsync(int id);
        Task<StoryUser> CreateUserAsync(StoryUser user);
        Task<bool> UpdateUserAsync(StoryUser user);
        Task<bool> DeleteUserAsync(int id);
    }
}