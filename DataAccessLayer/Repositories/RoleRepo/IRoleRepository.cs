using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Repositories.RoleRepo
{
	public interface IRoleRepository
	{
		public List<IdentityRole> SearchRole(string SearchString);
		Task<int> SaveChangesAsync();
	}
}
