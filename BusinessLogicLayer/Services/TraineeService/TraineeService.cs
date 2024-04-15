using AutoMapper;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitofWork;
using Educational_Website.ViewModels;
namespace BusinessLogicLayer.Services.TraineeService
{
	public class TraineeService : ITraineeService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public TraineeService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<List<Trainee>> GetAllAsync()
		{
			return await _unitOfWork.Trainee.GetAllAsync();
		}

		public async Task<Trainee> GetTraineeAsync(int id)
		{
			return await _unitOfWork.Trainee.GetTraineeAsync(id);
		}

		public async Task AddTraineeAsync(TraineeViewModel traineeVM)
		{
			var trainee = _mapper.Map<Trainee>(traineeVM);
			await _unitOfWork.Trainee.AddTraineeAsync(trainee);
			await _unitOfWork.CompleteAsync();
		}

		public async Task UpdateTraineeAsync(TraineeViewModel traineeVM)
		{
			var trainee = await _unitOfWork.Trainee.GetTraineeAsync(traineeVM.Id);
			_mapper.Map(traineeVM, trainee);
			await _unitOfWork.Trainee.UpdateTraineeAsync(trainee);
			await _unitOfWork.CompleteAsync();
		}

		public async Task DeleteTraineeAsync(int id)
		{
			await _unitOfWork.Trainee.DeleteTraineeAsync(id);
			await _unitOfWork.CompleteAsync();
		}

		public async Task<List<Trainee>> SearchTraineeAsync(string SearchString)
		{
			return await _unitOfWork.Trainee.SearchTraineeAsync(SearchString);
		}
	}
}
