using AutoMapper;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.TraineeRepo;
using Educational_Website.ViewModels;
namespace BusinessLogicLayer.Services.TraineeService
{
	public class TraineeService : ITraineeService
	{
		private readonly ITraineeRepository _traineeRepository;
		private readonly IMapper _mapper;

		public TraineeService(ITraineeRepository traineeRepository, IMapper mapper)
		{
			_traineeRepository = traineeRepository;
			_mapper = mapper;
		}

		public async Task<List<Trainee>> GetAllAsync()
		{
			return await _traineeRepository.GetAllAsync();
		}

		public async Task<Trainee> GetTraineeAsync(int id)
		{
			return await _traineeRepository.GetTraineeAsync(id);
		}

		public async Task AddTraineeAsync(TraineeViewModel traineeVM)
		{
			var trainee = _mapper.Map<Trainee>(traineeVM);
			await _traineeRepository.AddTraineeAsync(trainee);
			await _traineeRepository.SaveChangesAsync();
		}

		public async Task UpdateTraineeAsync(TraineeViewModel traineeVM)
		{
			var trainee = await _traineeRepository.GetTraineeAsync(traineeVM.Id);
			_mapper.Map(traineeVM, trainee);
			await _traineeRepository.UpdateTraineeAsync(trainee);
			await _traineeRepository.SaveChangesAsync();
		}

		public async Task DeleteTraineeAsync(int id)
		{
			await _traineeRepository.DeleteTraineeAsync(id);
			await _traineeRepository.SaveChangesAsync();
		}

		public async Task<List<Trainee>> SearchTraineeAsync(string SearchString)
		{
			return await _traineeRepository.SearchTraineeAsync(SearchString);
		}
	}
}
