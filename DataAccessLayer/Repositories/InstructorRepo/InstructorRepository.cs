using AutoMapper;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
namespace DataAccessLayer.Repositories.InstructorRepo
{
	public class InstructorRepository : IInstructorRepository
	{
		private readonly ApplicationDbContext context;
		private readonly IMapper _mapper;

		public InstructorRepository(ApplicationDbContext db, IMapper mapper)
		{
			context = db;
			_mapper = mapper;
		}

		public async Task<List<Instructor>> GetAllAsync()
		{
			var instructors = await context.Instructors
				.OrderBy(i => i.Name)
				.ToListAsync();
			return instructors;
		}

		public async Task<Instructor> GetInstructorAsync(int id)
		{
			var instructor = await context.Instructors
				.Include(x => x.department)
				.Include(x => x.course)
				.FirstOrDefaultAsync(u => u.Id == id);
			return instructor!;
		}

		public async Task AddInstructorAsync(Instructor instructor)
		{
			await context.Instructors.AddAsync(instructor);
			await context.SaveChangesAsync();
		}

		public async Task UpdateInstructorAsync(Instructor instructor)
		{
			var oldInstructor = await context.Instructors.FirstOrDefaultAsync(c => c.Id == instructor.Id);
			if (oldInstructor != null)
			{
				_mapper.Map<Instructor>(instructor);
			}
			await context.SaveChangesAsync();
		}

		public async Task DeleteInstructorAsync(int id)
		{
			var instructor = await context.Instructors.FirstOrDefaultAsync(c => c.Id == id);
			if (instructor is not null)
				context.Instructors.Remove(instructor);

			await context.SaveChangesAsync();
		}

		public async Task<List<Instructor>> SearchInstructorAsync(string SearchString)
		{
			var item = await context.Instructors
				.Include(x => x.course)
				.Include(x => x.department)
				.Where(x => x.Name.Contains(SearchString))
				.ToListAsync();
			return item;
		}

		public async Task<int> SaveChangesAsync()
		{
			return await context.SaveChangesAsync();
		}
	}
}
