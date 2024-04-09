using AutoMapper;
using BusinessLogicLayer.Services.CourseService;
using BusinessLogicLayer.Services.DepartmentService;
using Educational_Website.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Educational_Website.Controllers
{
	public class CourseController : Controller
	{
		private readonly ICourseService _courseService;
		private readonly IDepartmentService _departmentService;
		private readonly IMapper _mapper;
		public CourseController(ICourseService courseService, IDepartmentService departmentService, IMapper mapper)
		{
			_courseService = courseService;
			_departmentService = departmentService;
			_mapper = mapper;
		}
		public IActionResult CheckGradeAsync(int MinDegree, int Grade)
		{
			if (MinDegree <= Grade)
			{
				return Json(true);
			}
			return Json(false);
		}
		public async Task<IActionResult> GetCoursePerDepartment(int deptID)
		{
			var courses = await _courseService.GetCoursePerDepartmentAsync(deptID);
			return Json(courses);
		}
		[Authorize]
		public async Task<IActionResult> Index(int pageNo = 1)
		{
			var GetAllCourses = await _courseService.GetAllAsync();
			// Pagination
			int NoOfRecordsPerPage = 10;
			int NoOfPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(GetAllCourses.Count) / Convert.ToDouble(NoOfRecordsPerPage)));
			int NoOfRecordsToSkip = (pageNo - 1) * NoOfRecordsPerPage;
			ViewBag.PageNo = pageNo;
			ViewBag.NoOfPages = NoOfPage;
			GetAllCourses = GetAllCourses.Skip(NoOfRecordsToSkip).Take(NoOfRecordsPerPage).ToList();

			return View(GetAllCourses);
		}
		[Authorize]
		public async Task<IActionResult> Details(int id)
		{
			var course = await _courseService.GetCourseAsync(id);
			var courseVM = _mapper.Map<CourseViewModel>(course);
			ViewBag.DeptName = await _departmentService.GetDepartmentAsync(course.dept_id);
			return View(courseVM);
		}
		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Add()
		{
			ViewBag.DeptDropDownList = await _departmentService.GetDepartmentsAsync();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Add(CourseViewModel courseVM)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					ViewBag.DeptDropDownList = await _departmentService.GetDepartmentsAsync();
					//TempData["errorMessage"] = "Model state is invalid";
					return View();
				}
				await _courseService.AddCourseAsync(courseVM);
				TempData["successMessageForAddingCourse"] = "Course Added successfully!";
			}
			catch
			{
				//TempData["errorMessage"] = "Model state is invalid";
				return View();
			}
			return RedirectToAction(nameof(Index));
		}
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			await _courseService.DeleteCourseAsync(id);
			TempData["successMessageForDeleteCourse"] = "Course deleted successfully!";
			return RedirectToAction(nameof(Index));
		}
		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int id)
		{
			var course = await _courseService.GetCourseAsync(id);
			ViewBag.DeptDropDownList = await _departmentService.GetDepartmentsAsync();
			var courseVM = _mapper.Map<CourseViewModel>(course);
			return View(courseVM);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(CourseViewModel courseVM)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.DeptDropDownList = await _departmentService.GetDepartmentsAsync();
				return View();
			}
			await _courseService.UpdateCourseAsync(courseVM);
			TempData["successMessageForEditCourse"] = "Course Updated successfully!";
			return RedirectToAction(nameof(Index));
		}
		[Authorize]
		public async Task<IActionResult> Search(string searchString)
		{
			var Search = await _courseService.SearchCourseAsync(searchString);
			ViewBag.Search = searchString;
			return View(Search);
		}
	}
}
