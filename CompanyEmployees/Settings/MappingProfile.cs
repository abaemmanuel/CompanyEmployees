using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Settings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<Company, CompanyDto>().ForMember(c => c.FullAddress, opt => opt.MapFrom(c => string.Join(' ', c.Address, c.Country)));

            CreateMap<Company, CompanyDto>().ForCtorParam("FullAddress", opt => opt.MapFrom(c => string.Join(' ', c.Address, c.Country)));
        }
    }
}
