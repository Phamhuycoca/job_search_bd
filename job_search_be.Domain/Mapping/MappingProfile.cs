using AutoMapper;
using job_search_be.Domain.Dto.Auth;
using job_search_be.Domain.Dto.User;
using job_search_be.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //User
            CreateMap<User, UserDto>().ReverseMap();


            //Auth
            CreateMap<User,LoginDto>().ReverseMap();

        }
    }
}
