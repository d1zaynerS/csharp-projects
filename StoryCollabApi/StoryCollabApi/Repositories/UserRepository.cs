using Microsoft.EntityFrameworkCore;
using StoryCollabApi.Data;
using StoryCollabApi.Models;

namespace StoryCollabApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<StoryUser> CreateUserAsync(StoryUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            int rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<List<StoryUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<StoryUser?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> UpdateUserAsync(StoryUser user)
        {
            _context.Users.Update(user);
            int rows = await _context.SaveChangesAsync();
            return rows > 0;
        }
    }
}