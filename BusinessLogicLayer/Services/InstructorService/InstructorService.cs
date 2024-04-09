using AutoMapper;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.InstructorRepo;
using Educational_Website.ViewModels;
namespace BusinessLogicLayer.Services.InstructorService
{
	public class InstructorService : IInstructorService
	{
		private readonly IInstructorRepository _instructorRepository;
		private readonly IMapper _mapper;

		public InstructorService(IInstructorRepository instructorRepository, IMapper mapper)
		{
			_instructorRepository = instructorRepository;
			_mapper = mapper;
		}

		public async Task<List<Instructor>> GetAllAsync()
		{
			return await _instructorRepository.GetAllAsync();
		}

		public async Task<Instructor> GetInstructorAsync(int id)
		{
			return await _instructorRepository.GetInstructorAsync(id);
		}

		public async Task AddInstructorAsync(InstructorViewModel instructorVM)
		{
			var instructor = _mapper.Map<Instructor>(instructorVM);
			await _instructorRepository.AddInstructorAsync(instructor);
			await _instructorRepository.SaveChangesAsync();
		}

		public async Task UpdateInstructorAsync(InstructorViewModel instructorVM)
		{
			var instructor = await _instructorRepository.GetInstructorAsync(instructorVM.Id);
			_mapper.Map(instructorVM, instructor);
			await _instructorRepository.UpdateInstructorAsync(instructor);
			await _instructorRepository.SaveChangesAsync();
		}

		public async Task DeleteInstructorAsync(int id)
		{
			await _instructorRepository.DeleteInstructorAsync(id);
			await _instructorRepository.SaveChangesAsync();
		}

		public async Task<List<Instructor>> SearchInstructorAsync(string SearchString)
		{
			return await _instructorRepository.SearchInstructorAsync(SearchString);
		}
	}
}
