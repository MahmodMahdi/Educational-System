using AutoMapper;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
namespace DataAccessLayer.Repositories.CourseRepo
{
	public class CourseRepository : ICourseRepository
	{
		private readonly ApplicationDbContext context;
		private readonly IMapper _mapper;

		public CourseRepository(ApplicationDbContext db, IMapper mapper)
		{
			context = db;
			_mapper = mapper;
		}

		public async Task<List<Course>> GetCoursePerDepartmentAsync(int deptID)
		{
			var courses = await context.Courses
				.Where(c => c.dept_id == deptID)
				.ToListAsync();
			return courses;
		}

		public async Task<List<Course>> GetAllAsync()
		{
			var courses = await context.Courses
				.OrderBy(c => c.Name)
				.ToListAsync();
			return courses;
		}

		public async Task<Course> GetCourseAsync(int? id)
		{
			var course = await context.Courses.FirstOrDefaultAsync(u => u.Id == id);
			return course!;
		}

		public async Task AddCourseAsync(Course course)
		{
			await context.Courses.AddAsync(course);
			context.SaveChanges();
		}

		public async Task UpdateCourseAsync(Course course)
		{
			var oldCourse = await context.Courses.FirstOrDefaultAsync(c => c.Id == course.Id);
			if (oldCourse != null)
			{
				_mapper.Map<Course>(course);
			}
			context.SaveChanges();
		}

		public async Task DeleteCourseAsync(int id)
		{
			var course = await context.Courses.FirstOrDefaultAsync(c => c.Id == id);
			if (course is not null)
				context.Courses.Remove(course);
			context.SaveChanges();
		}

		public async Task<List<Course>> SearchCourseAsync(string SearchString)
		{
			var item = await context.Courses
				.Include(x => x.department)
				.Where(x => x.Name!.StartsWith(SearchString))
				.ToListAsync();
			return item;
		}
		public async Task<int> SaveChangesAsync()
		{
			return await context.SaveChangesAsync();
		}
	}
}
