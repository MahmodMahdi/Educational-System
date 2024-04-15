using AutoMapper;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitofWork;
using Educational_Website.ViewModels;
namespace BusinessLogicLayer.Services.CourseService
{
	public class CourseService : ICourseService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CourseService(IMapper mapper, IUnitOfWork unitOfWork)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<List<Course>> GetCoursePerDepartmentAsync(int deptID)
		{
			return await _unitOfWork.Course.GetCoursePerDepartmentAsync(deptID);
		}

		public async Task<List<Course>> GetAllAsync()
		{
			return await _unitOfWork.Course.GetAllAsync();
		}

		public async Task<Course> GetCourseAsync(int? id)
		{
			return await _unitOfWork.Course.GetCourseAsync(id);
		}

		public async Task AddCourseAsync(CourseViewModel courseVM)
		{
			var course = _mapper.Map<Course>(courseVM);
			await _unitOfWork.Course.AddCourseAsync(course);
			await _unitOfWork.CompleteAsync();
		}

		public async Task UpdateCourseAsync(CourseViewModel courseVM)
		{
			var course = await _unitOfWork.Course.GetCourseAsync(courseVM.Id);
			_mapper.Map(courseVM, course);
			await _unitOfWork.Course.UpdateCourseAsync(course);
			await _unitOfWork.CompleteAsync();
		}

		public async Task DeleteCourseAsync(int id)
		{
			await _unitOfWork.Course.DeleteCourseAsync(id);
			await _unitOfWork.CompleteAsync();
		}

		public async Task<List<Course>> SearchCourseAsync(string SearchString)
		{
			return await _unitOfWork.Course.SearchCourseAsync(SearchString);
		}

	}
}
