using AutoMapper;

namespace Website.API.Models.User
{
    public class UserViewModelMapper : Profile
    {
        public UserViewModelMapper()
        {
            CreateMap<Website.Model.User, User.UserViewModel>()
                .ForMember(au => au.UserName, map => map.MapFrom(vm => vm.UserName))
                .ForMember(au => au.FirstName, map => map.MapFrom(vm => vm.FirstName))
                .ForMember(au => au.LastName, map => map.MapFrom(vm => vm.LastName));
        }
    }
}
