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
            CreateMap<TaskDto, Domain.Task>().ReverseMap();
            CreateMap<CreateTaskDto, Domain.Task>().ReverseMap();
            CreateMap<UpdateTaskDto, Domain.Task>().ReverseMap();


            CreateMap<CheckListDto, CheckList>().ReverseMap();
            CreateMap<CreateCheckListDto, CheckList>().ReverseMap();
            CreateMap<UpdateCheckListDto, CheckList>().ReverseMap();

         }
    }
}