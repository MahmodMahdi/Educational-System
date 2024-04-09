using AutoMapper;
using BusinessLogicLayer.Services.DepartmentService;
using BusinessLogicLayer.Services.TraineeService;
using Educational_Website.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Educational_Website.Controllers
{
	public class DepartmentController : Controller
	{
		private readonly IDepartmentService _departmentService;
		private readonly ITraineeService _traineeService;
		private readonly IMapper _mapper;
		public DepartmentController(IDepartmentService departmentService,
									ITraineeService traineeService, IMapper mapper)
		{
			_departmentService = departmentService;
			_traineeService = traineeService;
			_mapper = mapper;
		}
		[Authorize]
		public async Task<IActionResult> Index(int pageNo = 1)
		{
			var GetDepartments = await _departmentService.GetDepartmentsAsync();
			// Pagination
			int NoOfRecordsPerPage = 5;
			int NoOfPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(GetDepartments.Count) / Convert.ToDouble(NoOfRecordsPerPage)));
			int NoOfRecordsToSkip = (pageNo - 1) * NoOfRecordsPerPage;
			ViewBag.PageNo = pageNo;
			ViewBag.NoOfPages = NoOfPage;
			GetDepartments = GetDepartments.Skip(NoOfRecordsToSkip).Take(NoOfRecordsPerPage).ToList();
			return View(GetDepartments);
		}
		[Authorize]
		public async Task<IActionResult> Details(int id)
		{
			var department = await _departmentService.GetDepartmentAsync(id);
			var DepartmentVM = _mapper.Map<DepartmentViewModel>(department);
			return View(DepartmentVM);
		}
		[HttpGet]
		[Authorize(Roles = "Admin")]
		public IActionResult Add()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Add(DepartmentViewModel departmentVM)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return View();
				}
				await _departmentService.AddDepartmentAsync(departmentVM);
				TempData["successMessageForAddingDepartment"] = "Department Added successfully!";
			}
			catch
			{
				return View();
			}
			return RedirectToAction(nameof(Index));
		}
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			await _departmentService.DeleteDepartmentAsync(id);
			TempData["successMessageForDeleteDepartment"] = "Department deleted successfully!";
			return RedirectToAction(nameof(Index));
		}
		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int id)
		{
			var department = await _departmentService.GetDepartmentAsync(id);
			var departmentVM = _mapper.Map<DepartmentViewModel>(department);
			return View(departmentVM);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(DepartmentViewModel departmentVM)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			await _departmentService.UpdateDepartmentAsync(departmentVM);
			TempData["successMessageForEditDepartment"] = "Department Updated successfully!";
			return RedirectToAction(nameof(Index));
		}
		[Authorize]
		public async Task<IActionResult> Search(string searchString)
		{
			var Search = await _departmentService.SearchDepartmentAsync(searchString);
			return View(Search);
		}
	}
}
