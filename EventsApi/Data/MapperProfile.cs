using AutoMapper;
using EventsApi.Core.Dtos;
using EventsApi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsApi.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CodeEvent, CodeEventDto>().ReverseMap();
            CreateMap<Lecture, LectureDto>().ReverseMap();
            CreateMap<Lecture, LectureCreateDto>().ReverseMap();
        }
    }
}
