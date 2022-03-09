using AutoMapper;
using KonOgren.Domain.Model;
using KonOgren.Infrastructure.ViewModel;

namespace KonOgren.Infrastructure.AutoMappers
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<User, LoginViewModel>();
        }
    }
}
