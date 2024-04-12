using AutoMapper;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
namespace DataAccessLayer.Repositories.TraineeCoursesResultsRepo
{
	public class CourseResultRepository : ICourseResultRepository
	{
		private readonly ApplicationDbContext context;
		private readonly IMapper _mapper;
		public CourseResultRepository(ApplicationDbContext db, IMapper mapper)
		{
			context = db;
			_mapper = mapper;
		}

		public async Task<List<CourseResult>> GetResultsByIdAsync(int id)
		{
			var result = await context.CourseResult
				.Include(x => x.Course)
				.Include(x => x.Trainee)
				.Where(x => x.trainee_id == id)
				.ToListAsync();
			return result;
		}

		public async Task<List<CourseResult>> GetAllAsync()
		{
			var results = await context.CourseResult
				.Include(x => x.Course)
				.Include(x => x.Trainee)
				.OrderBy(x => x.Trainee!.Name)
				.ToListAsync();
			return results;
		}

		public async Task<CourseResult> GetCourseResultAsync(int id)
		{
			var result = await context.CourseResult
				.Include(x => x.Course)
				.Include(x => x.Trainee)
				.FirstOrDefaultAsync(u => u.Id == id);
			return result!;
		}

		public async Task AddCourseResultAsync(CourseResult courseResult)
		{
			await context.CourseResult.AddAsync(courseResult);
			await context.SaveChangesAsync();
		}

		public async Task UpdateCourseResultAsync(CourseResult courseResult)
		{
			var result = await context.CourseResult.FirstOrDefaultAsync(c => c.Id == courseResult.Id);
			if (result != null)
			{
				_mapper.Map<CourseResult>(courseResult);
			}
			await context.SaveChangesAsync();
		}

		public async Task DeleteCourseResultAsync(int id)
		{
			var result = await context.CourseResult.FirstOrDefaultAsync(c => c.Id == id);
			if (result is not null)
				context.CourseResult.Remove(result);
			await context.SaveChangesAsync();
		}

		public async Task<List<CourseResult>> SearchCourseResultAsync(string SearchString)
		{
			var item = await context.CourseResult
				.Include(x => x.Trainee)
				.Include(x => x.Course)
				.Where(x => x.Trainee!.Name!.StartsWith(SearchString) || x.Course!.Name!.StartsWith(SearchString))
				.ToListAsync();
			return item!;
		}

		public async Task<int> SaveChangesAsync()
		{
			return await context.SaveChangesAsync();
		}
	}
}
