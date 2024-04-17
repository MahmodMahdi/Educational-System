using AutoMapper;
using BusinessLogicLayer.Services.CourseService;
using BusinessLogicLayer.Services.TraineeCoursesResultsService;
using BusinessLogicLayer.Services.TraineeService;
using Educational_Website.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Educational_Website.Controllers
{
    public class CourseResultController : Controller
    {
        private readonly ICourseResultService _courseResultService;
        private readonly ICourseService _courseService;
        private readonly ITraineeService _traineeService;
        private readonly IMapper _mapper;
        public CourseResultController(ICourseResultService courseResultService,
            ICourseService courseService,
            ITraineeService traineeService,
            IMapper mapper)
        {
            _courseResultService = courseResultService;
            _courseService = courseService;
            _traineeService = traineeService;
            _mapper = mapper;
        }
        [Authorize]
        public async Task<IActionResult> Index(int pageNo = 1)
        {
            var ItemsList = await _courseResultService.GetAllAsync();

            // Pagination
            int NoOfRecordsPerPage = 10;
            int NoOfPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ItemsList.Count) / Convert.ToDouble(NoOfRecordsPerPage)));
            int NoOfRecordsToSkip = (pageNo - 1) * NoOfRecordsPerPage;
            ViewBag.PageNo = pageNo;
            ViewBag.NoOfPages = NoOfPage;
            ItemsList = ItemsList.Skip(NoOfRecordsToSkip).Take(NoOfRecordsPerPage).ToList();
            return View(ItemsList);
        }
        //	[Authorize]
        [HttpGet]
        public async Task<IActionResult> Results(int id)
        {
            var ItemsList = await _courseResultService.GetCourseResultAsync(id);
            ViewBag.TraineeName = await _traineeService.GetAllAsync();
            ViewBag.CourseName = await _courseService.GetAllAsync();
            return View(ItemsList);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Add()
        {
            await Helper();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Add(CourseResultViewModel CourseResultVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await Helper();
                    return View();
                }
                await _courseResultService.AddCourseResultAsync(CourseResultVM);
                TempData["successMessageForAddingResult"] = "Result Added successfully!";
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
            await _courseResultService.DeleteCourseResultAsync(id);
            TempData["successMessageForDeleteResult"] = "Result deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Edit(int id)
        {
            var TraineeCourses = await _courseResultService.GetCourseResultAsync(id);
            var CourseResultVM = _mapper.Map<CourseResultViewModel>(TraineeCourses);
            await Helper();
            return View(CourseResultVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Edit(CourseResultViewModel courseResultVM)
        {
            if (!ModelState.IsValid)
            {
                await Helper();
                return View();
            }
            await _courseResultService.UpdateCourseResultAsync(courseResultVM);
            TempData["successMessageForEditResult"] = "Result Updated successfully!";
            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        public async Task<IActionResult> Search(string searchString)
        {
            var Search = await _courseResultService.SearchCourseResultAsync(searchString);
            return View(Search);
        }
        private async Task Helper()
        {
            ViewBag.TraineeName = await _traineeService.GetAllAsync();
            ViewBag.CourseName = await _courseService.GetAllAsync();

        }
    }
}
