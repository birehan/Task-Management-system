using AutoMapper;
using TaskManagement.Application.Features.CheckLists.DTOs;
using TaskManagement.Application.Features.Tasks.DTOs;
using TaskManagement.Domain;

namespace TaskManagement.Application.Profiles
{
    public class MappingProfile : Profile
    {
         public MappingProfile()
         {
            CreateMap<TaskDto, Domain.Task>()
            .ReverseMap()
            .ForMember(x => x.CreatorUsername, o => o.MapFrom(s => s.Creator.UserName))
            .ForMember(x => x.CheckLists, o => o.MapFrom(s => s.CheckLists));

            CreateMap<CreateTaskDto, Domain.Task>().ReverseMap();
            CreateMap<UpdateTaskDto, Domain.Task>().ReverseMap();


            CreateMap<CheckListDto, CheckList>().ReverseMap();
            CreateMap<CreateCheckListDto, CheckList>().ReverseMap();
            CreateMap<UpdateCheckListDto, CheckList>().ReverseMap();

         }
    }
}