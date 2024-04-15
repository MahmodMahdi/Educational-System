using AutoMapper;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitofWork;
using Educational_Website.ViewModels;
namespace BusinessLogicLayer.Services.TraineeCoursesResultsService
{
	public class CourseResultService : ICourseResultService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CourseResultService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<List<CourseResult>> GetResultsByIdAsync(int id)
		{
			return await _unitOfWork.courseResult.GetResultsByIdAsync(id);
		}

		public async Task<List<CourseResult>> GetAllAsync()
		{
			return await _unitOfWork.courseResult.GetAllAsync();
		}

		public async Task<CourseResult> GetCourseResultAsync(int id)
		{
			return await _unitOfWork.courseResult.GetCourseResultAsync(id);
		}

		public async Task AddCourseResultAsync(CourseResultViewModel courseResultVM)
		{
			var courseResult = _mapper.Map<CourseResult>(courseResultVM);
			await _unitOfWork.courseResult.AddCourseResultAsync(courseResult);
			await _unitOfWork.CompleteAsync();
		}

		public async Task UpdateCourseResultAsync(CourseResultViewModel courseResultVM)
		{
			var courseResult = await _unitOfWork.courseResult.GetCourseResultAsync(courseResultVM.Id);
			_mapper.Map(courseResultVM, courseResult);
			await _unitOfWork.courseResult.UpdateCourseResultAsync(courseResult);
			await _unitOfWork.CompleteAsync();
		}

		public async Task DeleteCourseResultAsync(int id)
		{
			await _unitOfWork.courseResult.DeleteCourseResultAsync(id);
			await _unitOfWork.CompleteAsync();
		}

		public async Task<List<CourseResult>> SearchCourseResultAsync(string SearchString)
		{
			return await _unitOfWork.courseResult.SearchCourseResultAsync(SearchString);
		}

	}
}
