using AutoMapper;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.CourseRepo;
using Educational_Website.ViewModels;
namespace BusinessLogicLayer.Services.CourseService
{
	public class CourseService : ICourseService
	{
		private readonly ICourseRepository _courseRepository;
		private readonly IMapper _mapper;

		public CourseService(ICourseRepository courseRepository, IMapper mapper)
		{
			_courseRepository = courseRepository;
			_mapper = mapper;
		}

		public async Task<List<Course>> GetCoursePerDepartmentAsync(int deptID)
		{
			return await _courseRepository.GetCoursePerDepartmentAsync(deptID);
		}

		public async Task<List<Course>> GetAllAsync()
		{
			return await _courseRepository.GetAllAsync();
		}

		public async Task<Course> GetCourseAsync(int? id)
		{
			return await _courseRepository.GetCourseAsync(id);
		}

		public async Task AddCourseAsync(CourseViewModel courseVM)
		{
			var course = _mapper.Map<Course>(courseVM);
			await _courseRepository.AddCourseAsync(course);
			await _courseRepository.SaveChangesAsync();
		}

		public async Task UpdateCourseAsync(CourseViewModel courseVM)
		{
			var course = await _courseRepository.GetCourseAsync(courseVM.Id);
			_mapper.Map(courseVM, course);
			await _courseRepository.UpdateCourseAsync(course);
			await _courseRepository.SaveChangesAsync();
		}

		public async Task DeleteCourseAsync(int id)
		{
			await _courseRepository.DeleteCourseAsync(id);
			await _courseRepository.SaveChangesAsync();
		}

		public async Task<List<Course>> SearchCourseAsync(string SearchString)
		{
			return await _courseRepository.SearchCourseAsync(SearchString);
		}

	}
}
