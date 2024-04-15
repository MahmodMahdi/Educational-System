using DataAccessLayer.Repositories.CourseRepo;
using DataAccessLayer.Repositories.DepartmentRepo;
using DataAccessLayer.Repositories.InstructorRepo;
using DataAccessLayer.Repositories.TraineeCoursesResultsRepo;
using DataAccessLayer.Repositories.TraineeRepo;

namespace DataAccessLayer.UnitofWork
{
	public interface IUnitOfWork : IDisposable
	{
		IDepartmentRepository Department { get; }
		IInstructorRepository Instructor { get; }
		ITraineeRepository Trainee { get; }
		ICourseRepository Course { get; }
		ICourseResultRepository courseResult { get; }
		Task<int> CompleteAsync();
		public new void Dispose();
	}
}
