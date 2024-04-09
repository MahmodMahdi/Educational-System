using DataAccessLayer.Authentication;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<ApplicationUser>> SearchUserAsync(string SearchString)
        {
            var item = await _context.users.Where(x => x.FirstName!.StartsWith(SearchString) || x.LastName!.StartsWith(SearchString)).ToListAsync();
            return item!;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
