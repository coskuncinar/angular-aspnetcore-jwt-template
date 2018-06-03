using AutoMapper;

namespace Website.API.Models.Register
{
    public class RegisterViewModelMapper : Profile
    {
        public RegisterViewModelMapper()
        {
            CreateMap<RegisterViewModel, Website.Model.User>()
                .ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email))
                .ForMember(au => au.FirstName, map => map.MapFrom(vm => vm.FirstName))
                .ForMember(au => au.LastName, map => map.MapFrom(vm => vm.LastName));
        }
    }
}
