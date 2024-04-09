using Microsoft.AspNetCore.Identity;

namespace BusinessLogicLayer.Services.RoleService
{
	public interface IRoleService
	{
		public List<IdentityRole> SearchRole(string SearchString);
	}
}
