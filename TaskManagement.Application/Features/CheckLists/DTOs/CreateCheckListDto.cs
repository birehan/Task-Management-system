
namespace TaskManagement.Application.Features.CheckLists.DTOs
{
    public class CreateCheckListDto : ICheckListDto
    {
     
    public string Title { get; set; }
    public string Description { get; set; }

    public bool Status { get; set; }

    public int TaskId { get; set; }

    }
}