using DataAccessLayer.Authentication;
namespace BusinessLogicLayer.Services.UserService

{
    public interface IUserService
    {
        public Task<List<ApplicationUser>> SearchUserAsync(string SearchString);
    }
}
