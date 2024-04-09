using AutoMapper;
using BusinessLogicLayer.Services.CourseService;
using BusinessLogicLayer.Services.DepartmentService;
using BusinessLogicLayer.Services.InstructorService;
using Educational_Website.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Educational_Website.Controllers
{
	public class InstructorController : Controller
	{
		private readonly IInstructorService _instructorService;
		private readonly IDepartmentService _departmentService;
		private readonly ICourseService _courseService;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _webHost;

		public InstructorController(IInstructorService instructorService,
			IDepartmentService departmentService,
			ICourseService courseService,
			IMapper mapper,
			IWebHostEnvironment webHost)
		{
			_instructorService = instructorService;
			_departmentService = departmentService;
			_courseService = courseService;
			_mapper = mapper;
			_webHost = webHost;
		}
		[Authorize]
		public async Task<IActionResult> Index(int pageNo = 1)
		{
			var GetInstructors = await _instructorService.GetAllAsync();
			// Pagination
			int NoOfRecordsPerPage = 10;
			int NoOfPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(GetInstructors.Count) / Convert.ToDouble(NoOfRecordsPerPage)));
			int NoOfRecordsToSkip = (pageNo - 1) * NoOfRecordsPerPage;
			ViewBag.PageNo = pageNo;
			ViewBag.NoOfPages = NoOfPage;
			GetInstructors = GetInstructors.Skip(NoOfRecordsToSkip).Take(NoOfRecordsPerPage).ToList();
			return View(GetInstructors);
		}
		[Authorize]
		public async Task<IActionResult> Details(int id)
		{
			var instructor = await _instructorService.GetInstructorAsync(id);
			var instructorVM = _mapper.Map<InstructorViewModel>(instructor);
			ViewBag.DeptName = await _departmentService.GetDepartmentAsync(instructorVM.dept_id);
			ViewBag.CourseName = await _courseService.GetCourseAsync(instructorVM.crs_id);
			return View(instructorVM);
		}
		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Add()
		{
			await Helper();
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Add(InstructorViewModel instructorVM)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					await Helper();
					return View();
				}
				UploadImage(instructorVM);
				await _instructorService.AddInstructorAsync(instructorVM);
				TempData["successMessageForAddingInstructor"] = "Instructor Added successfully!";
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
			string oldFileName = (await _instructorService.GetInstructorAsync(id)).ImageUrl!;
			string fullOldPath = Path.Combine(_webHost.WebRootPath, oldFileName);
			System.IO.File.Delete(fullOldPath);
			await _instructorService.DeleteInstructorAsync(id);
			TempData["successMessageForDeleteInstructor"] = "Instructor deleted successfully!";
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int id)
		{
			var instructor = await _instructorService.GetInstructorAsync(id);
			var InstructorVM = _mapper.Map<InstructorViewModel>(instructor);
			await Helper();
			return View(InstructorVM);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(InstructorViewModel instructorVM)
		{
			if (!ModelState.IsValid)
			{
				await Helper();
				return View();
			}
			EditImage(instructorVM);
			await _instructorService.UpdateInstructorAsync(instructorVM);
			TempData["successMessageForEditInstructor"] = "Instructor Updated successfully!";
			return RedirectToAction(nameof(Index));
		}
		[Authorize]
		public async Task<IActionResult> Search(string searchString)
		{
			var Search = await _instructorService.SearchInstructorAsync(searchString);
			return View(Search);
		}
		private async Task Helper()
		{
			ViewBag.DeptName = await _departmentService.GetDepartmentsAsync();
			ViewBag.CourseName = await _courseService.GetAllAsync();
		}
		private void UploadImage(InstructorViewModel instructorVM)
		{
			instructorVM.ImageUrl = "InstructorsPhotos/" + Guid.NewGuid().ToString();
			instructorVM.ImageUrl += instructorVM.File!.FileName;
			string fullPath = Path.Combine(_webHost.WebRootPath, instructorVM.ImageUrl);
			using (FileStream fs = new FileStream(fullPath, FileMode.Create))
			{
				instructorVM.File?.CopyTo(fs);
			};
		}
		private void EditImage(InstructorViewModel instructorVM)
		{
			var OldUrl = instructorVM.ImageUrl;
			var UrlRoot = _webHost.WebRootPath;
			var path = $"{UrlRoot}{OldUrl}";
			instructorVM.ImageUrl = "InstructorsPhotos/" + Guid.NewGuid().ToString();
			instructorVM.ImageUrl += instructorVM.File!.FileName;
			string fullPath = Path.Combine(_webHost.WebRootPath, instructorVM.ImageUrl);
			using (FileStream fs = new FileStream(fullPath, FileMode.Create))
			{
				instructorVM.File?.CopyTo(fs);
			};
			if (OldUrl != null)
			{
				System.IO.File.Delete(path);
			}

		}
	}
}
