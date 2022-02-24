using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Data.Models;
using Web.Services.DTO.StudentDto;

namespace Web.API.MapConfig
{
    public class MapConfiguration : Profile
    {
        public MapConfiguration()
        {
            CreateMap<Student, StudentCreationDto>().ReverseMap();
        }
    }
}
