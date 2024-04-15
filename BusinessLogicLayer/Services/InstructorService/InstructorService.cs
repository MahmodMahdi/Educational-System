using AutoMapper;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitofWork;
using Educational_Website.ViewModels;
namespace BusinessLogicLayer.Services.InstructorService
{
	public class InstructorService : IInstructorService
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		public InstructorService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<List<Instructor>> GetAllAsync()
		{
			return await _unitOfWork.Instructor.GetAllAsync();
		}

		public async Task<Instructor> GetInstructorAsync(int id)
		{
			return await _unitOfWork.Instructor.GetInstructorAsync(id);
		}

		public async Task AddInstructorAsync(InstructorViewModel instructorVM)
		{
			var instructor = _mapper.Map<Instructor>(instructorVM);
			await _unitOfWork.Instructor.AddInstructorAsync(instructor);
			await _unitOfWork.CompleteAsync();
		}

		public async Task UpdateInstructorAsync(InstructorViewModel instructorVM)
		{
			var instructor = await _unitOfWork.Instructor.GetInstructorAsync(instructorVM.Id);
			_mapper.Map(instructorVM, instructor);
			await _unitOfWork.Instructor.UpdateInstructorAsync(instructor);
			await _unitOfWork.CompleteAsync();
		}

		public async Task DeleteInstructorAsync(int id)
		{
			await _unitOfWork.Instructor.DeleteInstructorAsync(id);
			await _unitOfWork.CompleteAsync();
		}

		public async Task<List<Instructor>> SearchInstructorAsync(string SearchString)
		{
			return await _unitOfWork.Instructor.SearchInstructorAsync(SearchString);
		}
	}
}
