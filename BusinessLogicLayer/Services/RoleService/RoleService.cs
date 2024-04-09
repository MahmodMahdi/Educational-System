using DataAccessLayer.Repositories.RoleRepo;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogicLayer.Services.RoleService
{
	public class RoleService : IRoleService
	{
		private readonly IRoleRepository _roleRepository;

		public RoleService(IRoleRepository roleRepository)
		{
			_roleRepository = roleRepository;
		}
		public List<IdentityRole> SearchRole(string SearchString)
		{
			var result = _roleRepository.SearchRole(SearchString);
			return result;
		}
	}
}
