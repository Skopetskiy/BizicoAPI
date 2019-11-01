using Database.Models;
using Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bizico_Project.Mappings
{
    public class ApplicationProfile : AutoMapper.Profile
    {
        public ApplicationProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Request, RequestDto>().ReverseMap();
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<Profile, ProfileDto>().ReverseMap();
        }
    }
}
