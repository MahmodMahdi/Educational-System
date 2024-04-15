using AutoMapper;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitofWork;
using Educational_Website.ViewModels;
namespace BusinessLogicLayer.Services.DepartmentService
{
	public class DepartmentService : IDepartmentService
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		public DepartmentService(IMapper mapper, IUnitOfWork unitOfWork)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<List<Department>> GetDepartmentsAsync()
		{
			return await _unitOfWork.Department.GetDepartmentsAsync();
		}

		public async Task<Department> GetDepartmentAsync(int? id)
		{
			return await _unitOfWork.Department.GetDepartmentAsync(id);
		}

		public async Task AddDepartmentAsync(DepartmentViewModel departmentVM)
		{
			//Department department = new()
			//{
			//    Name = departmentVM.Name,
			//    DeptManager = departmentVM.DeptManager
			//};
			var department = _mapper.Map<Department>(departmentVM);
			await _unitOfWork.Department.AddDepartmentAsync(department);
			await _unitOfWork.CompleteAsync();
		}

		public async Task UpdateDepartmentAsync(DepartmentViewModel departmentVM)
		{
			var department = await _unitOfWork.Department.GetDepartmentAsync(departmentVM.Id);
			//department.Id = departmentVM.Id;
			//department.Name = departmentVM.Name;
			//department.DeptManager = departmentVM.DeptManager;
			_mapper.Map(departmentVM, department);
			await _unitOfWork.Department.UpdateDepartmentAsync(department);
			await _unitOfWork.CompleteAsync();
		}

		public async Task DeleteDepartmentAsync(int id)
		{
			await _unitOfWork.Department.DeleteDepartmentAsync(id);
			await _unitOfWork.CompleteAsync();
		}

		public async Task<List<Department>> SearchDepartmentAsync(string SearchString)
		{
			return await _unitOfWork.Department.SearchDepartmentAsync(SearchString);
		}

	}
}
