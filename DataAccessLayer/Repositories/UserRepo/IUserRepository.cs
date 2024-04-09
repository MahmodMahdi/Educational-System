using DataAccessLayer.Authentication;

namespace DataAccessLayer.Repositories.UserRepo
{
    public interface IUserRepository
    {
        public Task<List<ApplicationUser>> SearchUserAsync(string SearchString);
        Task<int> SaveChangesAsync();
    }
}
