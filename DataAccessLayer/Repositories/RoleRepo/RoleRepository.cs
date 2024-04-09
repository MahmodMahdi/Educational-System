using DataAccessLayer.Data;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Repositories.RoleRepo
{
	public class RoleRepository : IRoleRepository
	{
		private readonly ApplicationDbContext _context;
		public RoleRepository(ApplicationDbContext context)
		{
			_context = context;
		}
		public List<IdentityRole> SearchRole(string SearchString)
		{
			var item = _context.Roles.Where(x => x.Name!.StartsWith(SearchString)).ToList();
			return item!;
		}
		public async Task<int> SaveChangesAsync()
		{
			return await _context.SaveChangesAsync();
		}
	}
}
