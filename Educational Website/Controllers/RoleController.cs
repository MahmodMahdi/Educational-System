using BusinessLogicLayer.Services.RoleService;
using BusinessLogicLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Educational_Website.Controllers
{
	[Authorize(Roles = "Admin")]
	public class RoleController : Controller
	{
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IRoleService _roleService;
		public RoleController(RoleManager<IdentityRole> roleManager, IRoleService roleService)
		{
			_roleManager = roleManager;
			_roleService = roleService;
		}

		public async Task<IActionResult> GetRoles()
		{
			var GetRoles = await _roleManager.Roles.Where(x => x.Name != "Admin" && x.Name != "User").ToListAsync();
			return View(GetRoles);
		}
		[HttpGet]
		public IActionResult New()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> New(RoleViewModel roleViewModel)
		{
			if (ModelState.IsValid == true)
			{
				IdentityRole roleModel = new IdentityRole();
				roleModel.Name = roleViewModel.RoleName;
				IdentityResult result = await _roleManager.CreateAsync(roleModel);
				if (result.Succeeded)
				{
					TempData["successMessageForAddingRole"] = "Role Added successfully!";
					return RedirectToAction("GetRoles");
				}
				else
				{
					foreach (var item in result.Errors)
					{
						ModelState.AddModelError("", item.Description);
					}
				}
			}
			return View();
		}
		public async Task<IActionResult> Delete(IdentityRole Role)
		{
			var item = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == Role.Id);
			if (item == null) return NotFound();
			var role = await _roleManager.DeleteAsync(item!);
			TempData["successMessageForDeleteRole"] = "Role deleted successfully!";
			if (role.Succeeded)
			{
				return RedirectToAction(nameof(GetRoles));
			}
			foreach (var error in role.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
			return RedirectToAction(nameof(GetRoles));
		}
		public IActionResult Search(string searchString)
		{
			var Search = _roleService.SearchRole(searchString);
			ViewBag.Search = searchString;
			return View(Search);
		}
	}

}
