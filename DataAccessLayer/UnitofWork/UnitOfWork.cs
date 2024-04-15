using DataAccessLayer.Data;
using DataAccessLayer.Repositories.CourseRepo;
using DataAccessLayer.Repositories.DepartmentRepo;
using DataAccessLayer.Repositories.InstructorRepo;
using DataAccessLayer.Repositories.TraineeCoursesResultsRepo;
using DataAccessLayer.Repositories.TraineeRepo;

namespace DataAccessLayer.UnitofWork
{
	public class UnitOfWork : IUnitOfWork
	{
		public readonly ApplicationDbContext _context;
		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
			Department = new DepartmentRepository(_context);
			Instructor = new InstructorRepository(_context);
			Trainee = new TraineeRepository(_context);
			Course = new CourseRepository(_context);
			courseResult = new CourseResultRepository(_context);
		}
		public IDepartmentRepository Department { get; private set; }
		public IInstructorRepository Instructor { get; private set; }

		public ITraineeRepository Trainee { get; private set; }

		public ICourseRepository Course { get; private set; }

		public ICourseResultRepository courseResult { get; private set; }

		public Task<int> CompleteAsync()
		{
			return _context.SaveChangesAsync();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
