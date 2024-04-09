using AutoMapper;
using BusinessLogicLayer.ViewModels;
using DataAccessLayer.Authentication;
using DataAccessLayer.Entities;
using Educational_Website.ViewModels;

namespace Educational_Website.Helper.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<CourseViewModel, Course>().ReverseMap();
			CreateMap<DepartmentViewModel, Department>().ReverseMap();
			CreateMap<InstructorViewModel, Instructor>().ReverseMap();
			CreateMap<TraineeViewModel, Trainee>().ReverseMap();
			CreateMap<CourseResultViewModel, CourseResult>().ReverseMap();

			CreateMap<RegisterUserViewModel, ApplicationUser>()
				.ForMember(dest => dest.FirstName, op => op.MapFrom(src => src.FirstName))
				.ForMember(dest => dest.LastName, op => op.MapFrom(src => src.LastName))
				.ForMember(dest => dest.BirthDate, op => op.MapFrom(src => src.BirthDate))
				.ForMember(dest => dest.Gender, op => op.MapFrom(src => src.Gender))
				.ForMember(dest => dest.PhoneNumber, op => op.MapFrom(src => src.PhoneNumber))
				.ForMember(dest => dest.UserName, op => op.MapFrom(src => src.Email))
				.ForMember(dest => dest.Email, op => op.MapFrom(src => src.Email))
				.ForMember(dest => dest.PasswordHash, op => op.MapFrom(src => src.Password));
		}
	}
}
