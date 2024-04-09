using DataAccessLayer.Authentication;
using DataAccessLayer.Repositories.UserRepo;

namespace BusinessLogicLayer.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<List<ApplicationUser>> SearchUserAsync(string SearchString)
        {
            var user = await _userRepository.SearchUserAsync(SearchString);
            return user;
        }
    }
}
