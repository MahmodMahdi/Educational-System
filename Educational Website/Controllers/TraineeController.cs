using AutoMapper;
using BusinessLogicLayer.Services.DepartmentService;
using BusinessLogicLayer.Services.TraineeCoursesResultsService;
using BusinessLogicLayer.Services.TraineeService;
using Educational_Website.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Educational_Website.Controllers
{
	public class TraineeController : Controller
	{
		private readonly ITraineeService _traineeService;
		private readonly IDepartmentService _departmentService;
		private readonly ICourseResultService _traineeCoursesResultsService;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _webHost;
		public TraineeController(ITraineeService traineeService,
			IDepartmentService departmentService,
			ICourseResultService traineeCoursesResultsService,
			IMapper mapper,
			IWebHostEnvironment webHost)
		{
			_traineeService = traineeService;
			_departmentService = departmentService;
			_traineeCoursesResultsService = traineeCoursesResultsService;
			_mapper = mapper;
			_webHost = webHost;
		}
		[Authorize]
		public async Task<IActionResult> Index(int pageNo = 1)
		{
			var ItemsList = await _traineeService.GetAllAsync();
			// Pagination
			int NoOfRecordsPerPage = 10;
			int NoOfPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ItemsList.Count) / Convert.ToDouble(NoOfRecordsPerPage)));
			int NoOfRecordsToSkip = (pageNo - 1) * NoOfRecordsPerPage;
			ViewBag.PageNo = pageNo;
			ViewBag.NoOfPages = NoOfPage;
			ItemsList = ItemsList.Skip(NoOfRecordsToSkip).Take(NoOfRecordsPerPage).ToList();
			return View(ItemsList);
		}
		[Authorize]
		public async Task<IActionResult> Details(int id)
		{
			var trainee = await _traineeService.GetTraineeAsync(id);
			var TraineeVM = _mapper.Map<TraineeViewModel>(trainee);
			ViewBag.DeptName = await _departmentService.GetDepartmentAsync(trainee.dept_id);
			return View(TraineeVM);
		}
		[HttpGet]
		[Authorize(Roles = "Admin,Instructor")]
		public async Task<IActionResult> Add()
		{
			ViewBag.DeptDropDownList = await _departmentService.GetDepartmentsAsync();
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin,Instructor")]
		public async Task<IActionResult> Add(TraineeViewModel traineeVM)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					ViewBag.DeptDropDownList = await _departmentService.GetDepartmentsAsync();
					return View();
				}
				UploadImage(traineeVM);
				await _traineeService.AddTraineeAsync(traineeVM);
				TempData["successMessageForAddingTrainee"] = "Trainee Added successfully!";
			}
			catch
			{
				return View();
			}
			return RedirectToAction(nameof(Index));
		}
		[Authorize(Roles = "Admin,Instructor")]
		public async Task<IActionResult> Delete(int id)
		{
			string oldFileName = (await _traineeService.GetTraineeAsync(id)).ImageUrl!;
			string fullOldPath = Path.Combine(_webHost.WebRootPath, oldFileName);
			System.IO.File.Delete(fullOldPath);
			await _traineeService.DeleteTraineeAsync(id);
			TempData["successMessageForDeleteTrainee"] = "Trainee deleted successfully!";
			return RedirectToAction(nameof(Index));
		}
		[HttpGet]
		[Authorize(Roles = "Admin,Instructor")]
		public async Task<IActionResult> Edit(int id)
		{
			var traineeToEdit = await _traineeService.GetTraineeAsync(id);
			var trainee = _mapper.Map<TraineeViewModel>(traineeToEdit);
			ViewBag.DeptDropDownList = await _departmentService.GetDepartmentsAsync();
			return View(trainee);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin,Instructor")]
		public async Task<IActionResult> Edit(TraineeViewModel traineeVM)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.DeptDropDownList = await _departmentService.GetDepartmentsAsync();
				return View();
			}
			EditImage(traineeVM);
			await _traineeService.UpdateTraineeAsync(traineeVM);
			TempData["successMessageForEditTrainee"] = "Trainee Updated successfully!";
			return RedirectToAction(nameof(Index));
		}
		[Authorize]
		public async Task<IActionResult> Search(string searchString)
		{
			var Search = await _traineeService.SearchTraineeAsync(searchString);
			return View(Search);
		}
		private void UploadImage(TraineeViewModel traineeVM)
		{
			traineeVM.ImageUrl = "TraineesPhotos/" + Guid.NewGuid().ToString();
			traineeVM.ImageUrl += traineeVM.File!.FileName;
			string fullPath = Path.Combine(_webHost.WebRootPath, traineeVM.ImageUrl);
			using (FileStream fs = new FileStream(fullPath, FileMode.Create))
			{
				traineeVM.File?.CopyTo(fs);
			};
		}

		private void EditImage(TraineeViewModel traineeVM)
		{
			#region Update Image
			var OldUrl = traineeVM.ImageUrl;
			var UrlRoot = _webHost.WebRootPath;
			var path = $"{UrlRoot}{OldUrl}";
			traineeVM.ImageUrl = "TraineesPhotos/" + Guid.NewGuid().ToString();
			traineeVM.ImageUrl += traineeVM.File!.FileName;
			string fullPath = Path.Combine(_webHost.WebRootPath, traineeVM.ImageUrl);
			using (FileStream fs = new FileStream(fullPath, FileMode.Create))
			{
				traineeVM.File?.CopyTo(fs);
			};
			if (OldUrl != null)
			{
				System.IO.File.Delete(path);
			}

			#endregion
		}

	}
}
