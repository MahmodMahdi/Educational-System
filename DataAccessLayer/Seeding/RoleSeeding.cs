using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Seeding
{
	public static class RoleSeeding
	{
		public static void SeedAsync(RoleManager<IdentityRole> roleManager)
		{
			var roles = roleManager.Roles.CountAsync().GetAwaiter().GetResult();
			if (roles <= 0)
			{
				roleManager.CreateAsync(new IdentityRole()
				{
					Name = "Admin"
				}).GetAwaiter().GetResult();
				roleManager.CreateAsync(new IdentityRole()
				{
					Name = "User"
				}).GetAwaiter().GetResult();
			}
		}
	}
}
