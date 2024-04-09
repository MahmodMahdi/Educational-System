using DataAccessLayer.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Seeding
{
	public static class UserSeeding
	{
		public static void SeedAsync(UserManager<ApplicationUser> userManager)
		{
			var Users = userManager.Users.CountAsync().GetAwaiter().GetResult();
			if (Users <= 0)
			{
				var Admin = new ApplicationUser()
				{
					UserName = "Mahmoud@gmail.com",
					Email = "Mahmoud@gmail.com",
					FirstName = "Mahmoud",
					LastName = "Amin",
					PhoneNumber = "01212345654",
					Address = "Tanta",
					EmailConfirmed = true,
					PhoneNumberConfirmed = true,
				};
				userManager.CreateAsync(Admin, "Mahmoud123321").GetAwaiter().GetResult();
				userManager.AddToRoleAsync(Admin, "Admin").GetAwaiter().GetResult();
			}
		}
	}
}
